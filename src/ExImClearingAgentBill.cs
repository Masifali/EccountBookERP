using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Service;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Service;
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
namespace Architecture.WinApp.Service
{

public class ExImClearingAgentBill : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable otherchargestable = new DataTable();

	private DataTable dtAccount = new DataTable();

	private DataTable Taxtable = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dt = new DataTable();

	private DataTable dttaxrate = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public bool UpdateMode;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int RecId;

	private int updateDetailIndex;

	private int success;

	private int voucherheadid;

	private int billtocustomerid;

	private int TaxGlAcId;

	private IContainer components;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grddetailadd;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private Label label6;

	private DateTimePicker txtdocdate;

	private Label label5;

	private TextBox txtdocno;

	private Label label11;

	internal UltraCombo CmbRefrenceParty;

	private Label label9;

	internal UltraCombo cmblccontract;

	private Label label8;

	internal UltraCombo cmbeximinvoiceno;

	private Label label7;

	internal UltraCombo cmbbillTo;

	private Label label16;

	private TextBox txtnetweight;

	private Label label15;

	private TextBox txtnoofcontainer;

	private Label label14;

	private Label label13;

	private TextBox txtcontainersno;

	private Label label12;

	private Panel panel2;

	private Label label3;

	private TextBox txtrefbillno;

	private Label Forwarder;

	internal UltraCombo cmborderno;

	private Label label10;

	private TextBox txtusdheader;

	internal GridEX grdtax;

	internal GridEX grdothercharges;

	private TextBox txtgrandtotal;

	private Label label24;

	private TextBox txtotherchargesamount;

	private Label label23;

	private TextBox txttaxamount;

	private Label label22;

	private TextBox txtitemamount;

	private Panel panel3;

	private Panel panel9;

	private Label label21;

	private Label label26;

	private Label label25;

	private Label label27;

	private TextBox txtduedays;

	private Label label28;

	private DateTimePicker txtduedate;

	private TextBox txtamount;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbcurrency;

	private TextBox txtchargesrate;

	private Label label19;

	private Label label20;

	internal UltraCombo cmbuom;

	private TextBox txtexrate;

	private Label label29;

	private TextBox txtdescription;

	private Label label30;

	private TextBox txtqty;

	private Label label31;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label32;

	internal UltraCombo cmbitem;

	internal UltraCombo cmbdestinationport;

	internal UltraCombo cmbloadingport;

	private Panel panel5;

	private ToolStrip btn;

	private ToolStripLabel toolStripLabel1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnVoucher;

	private Label label4;

	private Label label17;

	private TextBox txtTotalAmount;

	private Label label18;

	private TextBox txtTaxPercent;

	private Label label33;

	internal UltraCombo combtaxtype;

	private Label label34;

	private DateTimePicker txtvalidupto;

	private Label label35;

	private DateTimePicker txteffectivedatedetail;

	private TextBox textTaxAmount;

	private CheckBox ChkPost;

	public ExImClearingAgentBill(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			int docno = ExImClearingAgentBill.GenerateCode(new ExImClearingAgentBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocTypeId = 154
			});
			if (docno > 0)
			{
				txtdocno.Text = docno.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOrder = ExImSaleOrderForwarder.ReadByOrganizationCompanyIdNOrderId(new ExImSaleOrderForwarder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtOrder.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtOrder, cmborderno, "Id", "OrderNo", "OrderNo", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Shippingline()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsupcust = SupplierCustomer.ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsupcust.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupcust, CmbRefrenceParty, "Id", "CompanyName", "ReferenceParty", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void item()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtitem = Item.ReadAllForItemsForServices(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbitem, "Id", "ItemName", "Item", ZeroIndex: false);
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbitem).Text != null)
			{
				DataTable dtCOMMUOM = UOMSchedule.SearchByObject(new UOMSchedule
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = int.Parse(cmbitem.Value.ToString())
				});
				if (dtCOMMUOM.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtCOMMUOM, cmbuom, "Id", "Equivalent", "Rate UOM ", false);
					((UltraGridBase)cmbuom).DisplayLayout.Bands[0].Columns[1].Width = 250;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Currency()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtcurrency = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcurrency, cmbcurrency, "Id", "CurrencyCode", "FcyCode", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmborderno_Leave(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		try
		{
			Conversion.ToInt(cmborderno.Value);
			_ = 0;
			DataTable dtheader = ExImSaleOrderForwarder.ReadByOrganizationCompanyIdNOrderId(new ExImSaleOrderForwarder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmborderno.Value)
			});
			if (dtheader.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtheader, cmbbillTo, "SupplierCustomerId", "BillToParty", "BillToParty", false);
				DDL.BindDDLNew(dtheader, cmbeximinvoiceno, "InvoiceId", "InvoiceNo", "InvoiceNo", false);
				DDL.BindDDLNew(dtheader, cmbdestinationport, "DestinationPortId", "DestinationPort", "DestinationPort", false);
				DDL.BindDDLNew(dtheader, cmbloadingport, "LoadingPortId", "LoadingPort", "LoadingPort", false);
				DDL.BindDDLNew(dtheader, cmblccontract, "LcContractId", "LcOrderNo", "LcContractNo", false);
				txtnoofcontainer.Text = Conversion.ToString(dtheader.Rows[0]["NoOfContainers"]);
				txtnetweight.Text = Conversion.ToString(dtheader.Rows[0]["NetWeight"]);
				((UltraGridBase)cmbbillTo).Rows[0].Activate();
				((UltraGridBase)cmbeximinvoiceno).Rows[0].Activate();
				((UltraGridBase)cmbdestinationport).Rows[0].Activate();
				((UltraGridBase)cmbloadingport).Rows[0].Activate();
				((UltraGridBase)cmblccontract).Rows[0].Activate();
			}
			if (RecId > 0)
			{
				return;
			}
			ExImSaleOrderForwarder po = new ExImSaleOrderForwarder();
			po = ExImSaleOrderForwarder.GetByID(Conversion.ToInt(cmborderno.Value));
			if (po != null)
			{
				table.Rows.Clear();
				for (int i = 0; i < po.ExImSaleOrderForwarderDetail.Count; i++)
				{
					table.Rows.Add(po.ExImSaleOrderForwarderDetail[i].ItemId, po.ExImSaleOrderForwarderDetail[i].ItemName, po.ExImSaleOrderForwarderDetail[i].Description, po.ExImSaleOrderForwarderDetail[i].ChargesRate, po.ExImSaleOrderForwarderDetail[i].UomId, po.ExImSaleOrderForwarderDetail[i].UomName, po.ExImSaleOrderForwarderDetail[i].CurrencyId, po.ExImSaleOrderForwarderDetail[i].CurrencyName, po.ExImSaleOrderForwarderDetail[i].ExhangeRate, po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].ChargesRate * po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].Amount, po.ExImSaleOrderForwarderDetail[i].TaxNameId, po.ExImSaleOrderForwarderDetail[i].TaxName, po.ExImSaleOrderForwarderDetail[i].TaxPercent, po.ExImSaleOrderForwarderDetail[i].TaxAmount, po.ExImSaleOrderForwarderDetail[i].TotalAmount, po.ExImSaleOrderForwarderDetail[i].EffectedDateD.ToString("dd-MMM-yyyy"), po.ExImSaleOrderForwarderDetail[i].ValidUpTo.ToString("dd-MMM-yyyy"));
				}
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
				Caluculations();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombTaxtypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtTax = global::Architecture.BLL.Inventory.TaxesTypes.GetForComboBind(new global::Architecture.Model.Inventory.TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtTax.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTax, combtaxtype, "Id", "TaxName", "Tax Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImClearingAgentBill_Load(object sender, EventArgs e)
	{
		try
		{
			GenerateCode();
			OrderNo();
			AccountsFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			Currency();
			Shippingline();
			item();
			CombTaxtypeFill();
			txtdocno.Focus();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("ItemId");
			table.Columns.Add("Item");
			table.Columns.Add("Description");
			table.Columns.Add("ChargesRate", typeof(double));
			table.Columns.Add("UomId");
			table.Columns.Add("Uom");
			table.Columns.Add("CurrencyId");
			table.Columns.Add("Currency");
			table.Columns.Add("ExchangeRate");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("TotalDollar", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("TaxNameId");
			table.Columns.Add("TaxName");
			table.Columns.Add("TaxPercent");
			table.Columns.Add("TaxAmount", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("EffectiveDate");
			table.Columns.Add("ValidUpTo");
			otherchargestable.Columns.Add("AccountTitle");
			otherchargestable.Columns.Add("Remarks");
			otherchargestable.Columns.Add("Qty");
			otherchargestable.Columns.Add("Rate");
			otherchargestable.Columns.Add("Amount", typeof(double));
			Taxtable.Columns.Add("AccountTitle");
			Taxtable.Columns.Add("TaxName");
			Taxtable.Columns.Add("Rate");
			Taxtable.Columns.Add("TaxAmount", typeof(double));
			AddRowInothercharges();
			AddRowInTaxTable();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtdocno.Text == "" || txtdocno.Text == "0" || txtdocno.Text == null)
		{
			MessageBox.Show("Please Check DocNo");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbbillTo).ActiveRow == null)
		{
			MessageBox.Show("Please select Bill To");
			cmbbillTo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRefrenceParty).ActiveRow == null)
		{
			MessageBox.Show("Please select Consignee");
			CmbRefrenceParty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbloadingport).ActiveRow == null)
		{
			MessageBox.Show("Please select Loading Port");
			cmbloadingport.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdestinationport).ActiveRow == null)
		{
			MessageBox.Show("Please select Destination Port");
			cmbdestinationport.Focus();
			return false;
		}
		return true;
	}

	private void reset()
	{
		try
		{
			RecId = 0;
			voucherheadid = 0;
			GenerateCode();
			table.Clear();
			Taxtable.Clear();
			otherchargestable.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)cmbbillTo).Text = "";
			((Control)(object)cmbeximinvoiceno).Text = "";
			((Control)(object)cmblccontract).Text = "";
			((Control)(object)CmbRefrenceParty).Text = "";
			((Control)(object)cmbloadingport).Text = "";
			((Control)(object)cmbdestinationport).Text = "";
			txtcontainersno.Text = "";
			txtnoofcontainer.Text = "";
			txtnetweight.Text = "";
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmborderno).Text = "";
			txtrefbillno.Text = "";
			txtduedays.Text = "";
			txtusdheader.Text = "";
			txttaxamount.Text = "";
			txtitemamount.Text = "";
			txtotherchargesamount.Text = "";
			txtgrandtotal.Text = "";
			AddRowInothercharges();
			AddRowInTaxTable();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			table.Clear();
			Taxtable.Clear();
			otherchargestable.Clear();
			txtdocno.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetdetail()
	{
		try
		{
			txtdescription.Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			txtchargesrate.Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtexrate.Text = string.Empty;
			((Control)(object)cmbcurrency).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtamount.Text = string.Empty;
			txtqty.Text = string.Empty;
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
			MessageBox.Show("Only update Record Not Insert Thank You :)");
			((Control)(object)cmbitem).Text = string.Empty;
			txtdescription.Text = string.Empty;
			txtexrate.Text = string.Empty;
			txtqty.Text = string.Empty;
			((Control)(object)cmbcurrency).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtamount.Text = string.Empty;
			txtchargesrate.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailadd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grddetailadd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				txtdescription.Text = item.Cells["Description"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				cmbitem.Value = item.Cells["ItemId"].Value.ToString();
				txtchargesrate.Text = item.Cells["ChargesRate"].Value.ToString();
				cmbuom.Value = item.Cells["UomId"].Value.ToString();
				cmbcurrency.Value = item.Cells["CurrencyId"].Value.ToString();
				txtexrate.Text = item.Cells["ExchangeRate"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				combtaxtype.Value = item.Cells["TaxNameId"].Value.ToString();
				txtTaxPercent.Text = item.Cells["TaxPercent"].Value.ToString();
				textTaxAmount.Text = item.Cells["TaxAmount"].Value.ToString();
				txtTotalAmount.Text = item.Cells["TotalAmount"].Value.ToString();
				txteffectivedatedetail.Text = item.Cells["EffectiveDate"].Value.ToString();
				txtvalidupto.Text = item.Cells["ValidUpTo"].Value.ToString();
				cmbitem.Focus();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				cmbitem.Focus();
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
			if (((UltraGridBase)cmbitem).ActiveRow == null)
			{
				MessageBox.Show("Please Select item");
				cmbitem.Focus();
				return;
			}
			if (txtchargesrate.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter ChargesRate");
				txtchargesrate.Focus();
				return;
			}
			if (((UltraGridBase)cmbuom).ActiveRow == null)
			{
				MessageBox.Show("Please Select Uom");
				cmbuom.Focus();
				return;
			}
			if (((UltraGridBase)cmbcurrency).ActiveRow == null)
			{
				MessageBox.Show("Please Select Currency");
				cmbcurrency.Focus();
				return;
			}
			if (txtqty.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Qty");
				txtqty.Focus();
				return;
			}
			if (txtamount.Text.Trim() == "")
			{
				MessageBox.Show("Please Check Amount");
				txtamount.Focus();
				return;
			}
			double Qty = 0.0;
			double num = Conversion.ToDouble((object)txtchargesrate.Text);
			Qty = Conversion.ToDouble((object)txtqty.Text);
			double Dollars = num * Qty;
			table.Rows[updateDetailIndex]["ItemId"] = cmbitem.Value;
			table.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbitem).Text;
			table.Rows[updateDetailIndex]["Description"] = txtdescription.Text;
			table.Rows[updateDetailIndex]["ChargesRate"] = txtchargesrate.Text;
			table.Rows[updateDetailIndex]["UomId"] = cmbuom.Value;
			table.Rows[updateDetailIndex]["Uom"] = ((Control)(object)cmbuom).Text;
			table.Rows[updateDetailIndex]["Currency"] = ((Control)(object)cmbcurrency).Text;
			table.Rows[updateDetailIndex]["ExchangeRate"] = txtexrate.Text;
			table.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
			table.Rows[updateDetailIndex]["TotalDollar"] = Dollars;
			table.Rows[updateDetailIndex]["Amount"] = txtamount.Text;
			table.Rows[updateDetailIndex]["TaxNameId"] = combtaxtype.Value;
			table.Rows[updateDetailIndex]["TaxName"] = ((Control)(object)combtaxtype).Text;
			table.Rows[updateDetailIndex]["TaxPercent"] = txtTaxPercent.Text;
			table.Rows[updateDetailIndex]["TaxAmount"] = textTaxAmount.Text;
			table.Rows[updateDetailIndex]["TotalAmount"] = txtTotalAmount.Text;
			table.Rows[updateDetailIndex]["EffectiveDate"] = txteffectivedatedetail.Value.ToString("dd-MMM-yyyy");
			table.Rows[updateDetailIndex]["ValidUpTo"] = txtvalidupto.Value.ToString("dd-MMM-yyyy");
			resetdetail();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Caluculations();
			cmbitem.Focus();
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
			resetdetail();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grddetailadd.RootTable.Columns["ItemId"].Visible = false;
			grddetailadd.RootTable.Columns["UomId"].Visible = false;
			grddetailadd.RootTable.Columns["TaxNameId"].Visible = false;
			grddetailadd.RootTable.Columns["CurrencyId"].Visible = false;
			grddetailadd.RootTable.Columns["Description"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["ChargesRate"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Uom"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Currency"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["TotalDollar"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["TaxNameId"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["TaxPercent"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["TaxAmount"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["TotalAmount"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["EffectiveDate"].FilterEditType = (FilterEditType)5;
			grddetailadd.RootTable.Columns["ValidUpTo"].FilterEditType = (FilterEditType)5;
			grddetailadd.RootTable.Columns["Description"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Item"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ChargesRate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Uom"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Currency"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["TotalDollar"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["TaxNameId"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["TaxPercent"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["EffectiveDate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ValidUpTo"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ChargesRate"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["TotalDollar"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grddetailadd.RootTable.Columns["ChargesRate"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["Qty"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["TotalDollar"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["Amount"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["TaxAmount"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			grddetailadd.RootTable.Columns["ChargesRate"].TextAlignment = (TextAlignment)3;
			grddetailadd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grddetailadd.RootTable.Columns["TotalDollar"].TextAlignment = (TextAlignment)3;
			grddetailadd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetailadd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grddetailadd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
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
			dtAccount = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridotherchargessetting()
	{
		try
		{
			grdothercharges.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdothercharges.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdothercharges.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdothercharges.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdothercharges.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdothercharges.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)0;
			grdothercharges.RootTable.Columns["AccountTitle"].HasValueList = true;
			grdothercharges.RootTable.Columns["AccountTitle"].LimitToList = true;
			grdothercharges.RootTable.Columns["AccountTitle"].EditType = (EditType)4;
			grdothercharges.RootTable.Columns["AccountTitle"].Caption = "AccountTitle";
			grdothercharges.RootTable.Columns["AccountTitle"].ValueList.PopulateValueList((IEnumerable)dtAccount.DefaultView, "Id", "AccountTitle");
			grdothercharges.RootTable.Columns["AccountTitle"].Width = 250;
			grdothercharges.RootTable.Columns["Qty"].Width = 60;
			grdothercharges.RootTable.Columns["Rate"].Width = 60;
			grdothercharges.RootTable.Columns["Remarks"].Width = 150;
			grdothercharges.RootTable.Columns.Add("X");
			grdothercharges.RootTable.Columns["X"].Key = "X";
			grdothercharges.RootTable.Columns["X"].Caption = "X";
			grdothercharges.RootTable.Columns["X"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdothercharges.RootTable.Columns["X"].ButtonStyle = (ButtonStyle)5;
			grdothercharges.RootTable.Columns["X"].ButtonText = "X";
			grdothercharges.RootTable.Columns["X"].Width = 20;
			grdothercharges.RootTable.Columns.Add("+");
			grdothercharges.RootTable.Columns["+"].Key = "+";
			grdothercharges.RootTable.Columns["+"].Caption = "+";
			grdothercharges.RootTable.Columns["+"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdothercharges.RootTable.Columns["+"].ButtonStyle = (ButtonStyle)5;
			grdothercharges.RootTable.Columns["+"].ButtonText = "+";
			grdothercharges.RootTable.Columns["+"].Width = 20;
			grdothercharges.RootTable.Columns["+"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridTaxsetting()
	{
	}

	private void AddRowInothercharges()
	{
		try
		{
			otherchargestable.Rows.Add("", "", 0, 0, 0);
			grdothercharges.DataSource = otherchargestable;
			grdothercharges.RetrieveStructure();
			gridotherchargessetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInTaxTable()
	{
		try
		{
			Taxtable.Rows.Add("", "", 0, 0);
			grdtax.DataSource = Taxtable;
			grdtax.RetrieveStructure();
			gridTaxsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdothercharges_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "X")
			{
				GridEXRow item = grdothercharges.CurrentRow;
				otherchargestable.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (otherchargestable.Rows.Count == 0)
				{
					AddRowInothercharges();
				}
			}
			if (e.Column.Key == "+")
			{
				AddRowInothercharges();
			}
			grdothercharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdtax_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "X")
			{
				GridEXRow item = grdtax.CurrentRow;
				Taxtable.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (Taxtable.Rows.Count == 0)
				{
					AddRowInTaxTable();
				}
			}
			if (e.Column.Key == "+")
			{
				AddRowInTaxTable();
			}
			grdtax.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Expected O, but got Unknown
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			ExImClearingAgentBill billHeader = new ExImClearingAgentBill();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				billHeader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grddetailadd.RowCount > 0)
			{
				billHeader.EntryUser = UserAccount.ID;
				billHeader.OrganizationId = UserAccount.OrganizationId;
				billHeader.CompanyId = UserAccount.CompanyId;
				billHeader.DocTypeId = 154;
				billHeader.DueDate = Conversion.ToDateTime((object)txtduedate.Value);
				billHeader.DueDays = Conversion.ToInt((object)txtduedays.Text);
				billHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				billHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
				billHeader.ServiceOrderId = Conversion.ToInt(cmborderno.Value);
				billHeader.ForwarderId = Conversion.ToInt(CmbRefrenceParty.Value);
				billHeader.ExportInvoiceId = Conversion.ToInt(cmbeximinvoiceno.Value);
				billHeader.LcContractId = Conversion.ToInt(cmblccontract.Value);
				billHeader.RefBillNo = Conversion.ToString((object)txtrefbillno.Text);
				billHeader.SupCustId = Conversion.ToInt(cmbbillTo.Value);
				billHeader.USDRate = Conversion.ToInt((object)txtusdheader.Text);
				billHeader.TaxAmountTotal = Conversion.ToInt((object)txttaxamount.Text);
				billHeader.TotalAmount = Conversion.ToInt((object)txtitemamount.Text);
				billHeader.OtherChargesTotal = Conversion.ToInt((object)txtotherchargesamount.Text);
				billHeader.GrandTotal = Conversion.ToInt((object)txtgrandtotal.Text);
				billHeader.NoOfContainer = Conversion.ToInt((object)txtnoofcontainer.Text);
				billHeader.NetBillWeight = Conversion.ToDouble((object)txtnetweight.Text);
				billHeader.LoadingPortId = Conversion.ToInt(cmbloadingport.Value);
				billHeader.DestinationPortId = Conversion.ToInt(cmbdestinationport.Value);
				billHeader.ContainerNo = Conversion.ToString((object)txtcontainersno.Text);
				billHeader.BillDetail = new List<ExImClearingAgentBillDetail>();
				billHeader.OtherChargesDetail = new List<ExImClearingAgentBillOtherChargesDetail>();
				billHeader.TaxDetail = new List<ExImClearingAgentBillTaxDetail>();
				GridEXRow[] rows = grddetailadd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					ExImClearingAgentBillDetail labdetail = new ExImClearingAgentBillDetail();
					labdetail.DocDate = Conversion.ToDateTime((object)DateTime.Now);
					labdetail.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					if (labdetail.ItemId == 0)
					{
						MessageBox.Show("ItemName Field Required");
						return;
					}
					labdetail.ChargesRate = Conversion.ToDouble(r3.Cells["ChargesRate"].Value);
					if (labdetail.ChargesRate == 0.0)
					{
						MessageBox.Show("ChargesRate Field Required");
						return;
					}
					labdetail.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					if (labdetail.Amount == 0.0)
					{
						MessageBox.Show("Amount Field Required");
						return;
					}
					labdetail.Description = Conversion.ToString((object)r3.Cells["Description"].Text);
					labdetail.UomId = Conversion.ToInt(r3.Cells["UomId"].Value);
					if (labdetail.UomId == 0)
					{
						MessageBox.Show("PackUom Field Required");
						return;
					}
					labdetail.CurrencyId = Conversion.ToInt(r3.Cells["CurrencyId"].Value);
					if (labdetail.CurrencyId == 0)
					{
						MessageBox.Show("Currency Field Required");
						return;
					}
					labdetail.ExhangeRate = Conversion.ToDouble(r3.Cells["ExchangeRate"].Value);
					if (labdetail.ExhangeRate == 0.0)
					{
						MessageBox.Show("ExhangeRate Field Required");
						return;
					}
					labdetail.Qty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					if (labdetail.Qty == 0.0)
					{
						MessageBox.Show("Qty Field Required");
						return;
					}
					labdetail.TaxNameId = Conversion.ToInt(r3.Cells["TaxNameId"].Value);
					labdetail.TaxPercent = Conversion.ToSingle(r3.Cells["TaxPercent"].Value);
					labdetail.TaxAmount = Conversion.ToSingle(r3.Cells["TaxAmount"].Value);
					labdetail.TotalAmount = Conversion.ToSingle(r3.Cells["TotalAmount"].Value);
					labdetail.EffectedDateD = Conversion.ToDateTime((object)DateTime.Now);
					labdetail.ValidUpTo = Conversion.ToDateTime((object)DateTime.Now);
					billHeader.BillDetail.Add(labdetail);
				}
				if (grdothercharges.RowCount > 0)
				{
					rows = grdothercharges.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						ExImClearingAgentBillOtherChargesDetail othercharges = new ExImClearingAgentBillOtherChargesDetail();
						if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0)
						{
							if (Conversion.ToInt(r2.Cells["AccountTitle"].Value) == 0)
							{
								throw new Exception("AccountTitile Field Required");
							}
							othercharges.ChartOfAccountId = Conversion.ToInt(r2.Cells["AccountTitle"].Value);
							othercharges.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
							othercharges.Rate = Conversion.ToInt(r2.Cells["Rate"].Value);
							othercharges.Amount = Conversion.ToInt(r2.Cells["Amount"].Value);
							othercharges.OtherCharges = Conversion.ToString((object)r2.Cells["Remarks"].Text);
							billHeader.OtherChargesDetail.Add(othercharges);
						}
					}
				}
				if (grdtax.RowCount > 0)
				{
					rows = grdtax.GetRows();
					foreach (GridEXRow r in rows)
					{
						ExImClearingAgentBillTaxDetail taxdetail = new ExImClearingAgentBillTaxDetail();
						if (Conversion.ToInt(r.Cells["AccountTitle"].Value) != 0)
						{
							taxdetail.ChartOfAccountId = Conversion.ToInt(r.Cells["AccountTitle"].Value);
						}
						if (Conversion.ToInt(r.Cells["TaxName"].Value) != 0)
						{
							taxdetail.TaxNameId = Conversion.ToInt(r.Cells["TaxName"].Value);
							taxdetail.TaxName = Conversion.ToString((object)r.Cells["TaxName"].Text);
						}
						taxdetail.TaxRate = Conversion.ToInt(r.Cells["Rate"].Value);
						if (r.Cells["TaxAmount"].Text != string.Empty)
						{
							taxdetail.TaxAmount = Conversion.ToInt(r.Cells["TaxAmount"].Value);
						}
						billHeader.TaxDetail.Add(taxdetail);
					}
				}
				ExImClearingAgentBill.Save(billHeader);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
				reset();
				resetdetail();
				return;
			}
			throw new Exception("Grid Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void historygrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable grdform = ExImClearingAgentBill.FormHistoryData(new ExImClearingAgentBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (grdform.Rows.Count > 0)
			{
				grdhistory.DataSource = grdform;
				grdhistory.RetrieveStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Caluculations()
	{
		try
		{
			double ItemAmount = 0.0;
			double OtherChargesAmount = 0.0;
			double TaxAmount = 0.0;
			double grandtottal = 0.0;
			if (grddetailadd.GetTotal(grddetailadd.RootTable.Columns["TotalAmount"], (AggregateFunction)2) != null)
			{
				ItemAmount = double.Parse(grddetailadd.GetTotal(grddetailadd.RootTable.Columns["TotalAmount"], (AggregateFunction)2).ToString());
				txtitemamount.Text = ItemAmount.ToString();
			}
			if (grdothercharges.GetTotal(grdothercharges.RootTable.Columns["Amount"], (AggregateFunction)2) != null)
			{
				OtherChargesAmount = double.Parse(grdothercharges.GetTotal(grdothercharges.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				txtotherchargesamount.Text = OtherChargesAmount.ToString();
			}
			if (grdtax.GetTotal(grdtax.RootTable.Columns["TaxAmount"], (AggregateFunction)2) != null)
			{
				TaxAmount = double.Parse(grdtax.GetTotal(grdtax.RootTable.Columns["TaxAmount"], (AggregateFunction)2).ToString());
				txttaxamount.Text = TaxAmount.ToString();
			}
			grandtottal = ItemAmount + OtherChargesAmount + TaxAmount;
			txtgrandtotal.Text = grandtottal.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailadd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grddetailadd.UpdateData();
			Caluculations();
			grddetailadd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdothercharges_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdothercharges.UpdateData();
			GridEXRow item = grdothercharges.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			Caluculations();
			grdothercharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById_Update(int ID)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)ID);
			ExImClearingAgentBill po = new ExImClearingAgentBill();
			po = ExImClearingAgentBill.GetByID(RecId);
			if (po != null)
			{
				btnSave.Visible = false;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				txtdocdate.Value = Conversion.ToDateTime((object)po.DocDate);
				cmbbillTo.Value = Conversion.ToInt((object)po.SupCustId);
				cmborderno.Value = Conversion.ToInt((object)po.ServiceOrderId);
				txtrefbillno.Text = Conversion.ToString((object)po.RefBillNo);
				txtduedays.Text = Conversion.ToString((object)po.DueDays);
				txtduedate.Value = Conversion.ToDateTime((object)po.DueDate);
				CmbRefrenceParty.Value = Conversion.ToInt((object)po.ForwarderId);
				table.Clear();
				for (int j = 0; j < po.BillDetail.Count; j++)
				{
					table.Rows.Add(po.BillDetail[j].ItemId, po.BillDetail[j].ItemName, po.BillDetail[j].Description, po.BillDetail[j].ChargesRate, po.BillDetail[j].UomId, po.BillDetail[j].UomName, po.BillDetail[j].CurrencyId, po.BillDetail[j].CurrencyName, po.BillDetail[j].ExhangeRate, po.BillDetail[j].Qty, po.BillDetail[j].ChargesRate * po.BillDetail[j].Qty, po.BillDetail[j].Amount, po.BillDetail[j].TaxNameId, po.BillDetail[j].TaxName, po.BillDetail[j].TaxPercent, po.BillDetail[j].TaxAmount, po.BillDetail[j].TotalAmount, po.BillDetail[j].EffectedDateD.ToString("dd-MMM-yyyy"), po.BillDetail[j].ValidUpTo.ToString("dd-MMM-yyyy"));
				}
				otherchargestable.Rows.Clear();
				for (int i = 0; i < po.OtherChargesDetail.Count; i++)
				{
					otherchargestable.Rows.Add(po.OtherChargesDetail[i].ChartOfAccountId, po.OtherChargesDetail[i].OtherCharges, po.OtherChargesDetail[i].Qty, po.OtherChargesDetail[i].Rate, po.OtherChargesDetail[i].Amount);
				}
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
				grdothercharges.DataSource = otherchargestable;
				grdothercharges.RetrieveStructure();
				gridotherchargessetting();
				tabControl1.SelectedIndex = 0;
				btnUpdate.Visible = true;
				Caluculations();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void totalamount()
	{
		try
		{
			double chargesrate = 0.0;
			double exchagerate = 0.0;
			int equivalent = 0;
			double qty = 0.0;
			double TaxAmount = 0.0;
			double TotalAmount = 0.0;
			if (!(txtexrate.Text.Trim() != string.Empty) || !(txtchargesrate.Text.Trim() != string.Empty) || !(((Control)(object)cmbuom).Text.Trim() != "0") || !(txtqty.Text.Trim() != string.Empty))
			{
				return;
			}
			exchagerate = Conversion.ToDouble((object)txtexrate.Text.Trim());
			chargesrate = Conversion.ToDouble((object)txtchargesrate.Text.Trim());
			equivalent = Conversion.ToInt((object)((Control)(object)cmbuom).Text.Trim());
			qty = Conversion.ToDouble((object)txtqty.Text);
			TaxAmount = Conversion.ToDouble((object)textTaxAmount.Text);
			if (exchagerate == 0.0)
			{
				TotalAmount = chargesrate * (double)equivalent * qty;
				txtamount.Text = TotalAmount.ToString();
				if (((Control)(object)combtaxtype).Text.ToString() != string.Empty)
				{
					double TotalAmt = TotalAmount + TaxAmount;
					txtTotalAmount.Text = TotalAmt.ToString();
				}
				else
				{
					txtTotalAmount.Text = TotalAmount.ToString();
				}
			}
			else
			{
				TotalAmount = exchagerate * chargesrate * (double)equivalent * qty;
				txtamount.Text = TotalAmount.ToString();
				if (((Control)(object)combtaxtype).Text.ToString() != string.Empty)
				{
					double TotalAmt2 = TotalAmount + TaxAmount;
					txtTotalAmount.Text = TotalAmt2.ToString();
				}
				else
				{
					txtTotalAmount.Text = TotalAmount.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtchargesrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			totalamount();
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
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImClearingAgentBillDirect_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
			{
				btnUpdate_Click(null, null);
			}
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
			reset();
			resetdetail();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdtax_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			grdtax.UpdateData();
			GridEXRow item = grdtax.CurrentRow;
			if (e.Column.Key == "TaxName")
			{
				string TaxId = item.Cells["TaxName"].Value.ToString();
				TaxScheduleMain taxSchedule = new TaxScheduleMain();
				taxSchedule.OrganizationId = UserAccount.OrganizationId;
				taxSchedule.CompanyId = UserAccount.CompanyId;
				taxSchedule.EffectedDate = txtdocdate.Value;
				taxSchedule.TaxNameId = Conversion.ToInt((object)TaxId);
				if (TaxScheduleMain.ReadTaxSchedule(taxSchedule) == null)
				{
					item.Cells["Rate"].Value = "0";
				}
				else
				{
					DataTable dtdetail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
					item.Cells["Rate"].Value = Convert.ToDouble(dtdetail.Rows[0]["TaxPercent"]);
					double num = Conversion.ToDouble(grddetailadd.GetTotal(grddetailadd.RootTable.Columns["Amount"], (AggregateFunction)2));
					string TaxPercent = item.Cells["Rate"].Value.ToString();
					double Taxamount = num / 100.0 * double.Parse(TaxPercent);
					item.Cells["TaxAmount"].Value = Taxamount;
				}
			}
			grdtax.UpdateData();
			Caluculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddetailadd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grddetailadd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grddetailadd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
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

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
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
				historygrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdtax_CellEdited(object sender, ColumnActionEventArgs e)
	{
		grdtax.UpdateData();
		Caluculations();
		grdtax.UpdateData();
	}

	private void grdothercharges_CellEdited(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdothercharges.UpdateData();
			GridEXRow item = grdothercharges.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			Caluculations();
			grdothercharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combtaxtype_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			TaxScheduleMain taxSchedule = new TaxScheduleMain();
			taxSchedule.OrganizationId = UserAccount.OrganizationId;
			taxSchedule.CompanyId = UserAccount.CompanyId;
			taxSchedule.EffectedDate = txtdocdate.Value;
			taxSchedule.TaxNameId = Conversion.ToInt(combtaxtype.Value);
			if (TaxScheduleMain.ReadTaxSchedule(taxSchedule) == null)
			{
				txtTaxPercent.Text = "0";
			}
			else
			{
				DataTable dtdetail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
				txtTaxPercent.Text = Convert.ToDouble(dtdetail.Rows[0]["TaxPercent"]).ToString();
				double taxamount = Conversion.ToDouble((object)txtamount.Text) * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0;
				textTaxAmount.Text = taxamount.ToString();
			}
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ExImClearingAgentBill();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
			ReadById_Update(RecId);
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
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Expected O, but got Unknown
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Expected O, but got Unknown
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Expected O, but got Unknown
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Expected O, but got Unknown
		//IL_113c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Expected O, but got Unknown
		//IL_1153: Unknown result type (might be due to invalid IL or missing references)
		//IL_115d: Expected O, but got Unknown
		//IL_116a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1174: Expected O, but got Unknown
		//IL_12d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e3: Expected O, but got Unknown
		//IL_12f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Expected O, but got Unknown
		//IL_1307: Unknown result type (might be due to invalid IL or missing references)
		//IL_1311: Expected O, but got Unknown
		GridEXLayout grddetailadd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdtax_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdothercharges_DesignTimeLayout = new GridEXLayout();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
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
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.grddetailadd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdtax = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdothercharges = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.txtgrandtotal = new System.Windows.Forms.TextBox();
		this.txttaxamount = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtitemamount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtotherchargesamount = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ChkPost = new System.Windows.Forms.CheckBox();
		this.textTaxAmount = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.combtaxtype = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.txtvalidupto = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txteffectivedatedetail = new System.Windows.Forms.DateTimePicker();
		this.cmbdestinationport = new UltraCombo();
		this.cmbloadingport = new UltraCombo();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.txtchargesrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbuom = new UltraCombo();
		this.txtexrate = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtduedate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtusdheader = new System.Windows.Forms.TextBox();
		this.Forwarder = new System.Windows.Forms.Label();
		this.cmborderno = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtrefbillno = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtnetweight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtnoofcontainer = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtcontainersno = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbRefrenceParty = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmblccontract = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbeximinvoiceno = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbbillTo = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdtax).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdothercharges).BeginInit();
		this.panel3.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbloadingport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmborderno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefrenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbeximinvoiceno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbillTo).BeginInit();
		this.btn.SuspendLayout();
		base.SuspendLayout();
		this.grddetailadd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grddetailadd).BackColor = System.Drawing.Color.White;
		grddetailadd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailadd.DesignTimeLayout = grddetailadd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailadd.FilterMode = (FilterMode)2;
		this.grddetailadd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailadd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetailadd).Name = "grddetailadd";
		((ScrollableBase)this.grddetailadd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Size = new System.Drawing.Size(1246, 239);
		((System.Windows.Forms.Control)(object)this.grddetailadd).TabIndex = 0;
		this.grddetailadd.TotalRow = (InheritableBoolean)1;
		this.grddetailadd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailadd).VisualStyle = (VisualStyle)4;
		this.grddetailadd.CellUpdated += new ColumnActionEventHandler(grddetailadd_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grddetailadd).DoubleClick += new System.EventHandler(grddetailadd_DoubleClick);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1252, 706);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1246, 700);
		this.panel4.TabIndex = 1;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1246, 700);
		this.panel2.TabIndex = 4;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1246, 700);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1260, 732);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.ctrlGrdBar3);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1252, 706);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1198, 5);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grddetailadd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar3.TabIndex = 39;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdtax);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.txtgrandtotal);
		this.panel1.Controls.Add(this.txttaxamount);
		this.panel1.Controls.Add(this.label22);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.txtitemamount);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Controls.Add(this.txtotherchargesamount);
		this.panel1.Controls.Add(this.label24);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 306);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1246, 397);
		this.panel1.TabIndex = 0;
		this.grdtax.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdtax).BackColor = System.Drawing.Color.White;
		grdtax_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdtax.DesignTimeLayout = grdtax_DesignTimeLayout;
		this.grdtax.FilterMode = (FilterMode)2;
		this.grdtax.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdtax).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdtax.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdtax).Location = new System.Drawing.Point(1296, 261);
		((System.Windows.Forms.Control)(object)this.grdtax).Name = "grdtax";
		((ScrollableBase)this.grdtax).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdtax).Size = new System.Drawing.Size(115, 52);
		((System.Windows.Forms.Control)(object)this.grdtax).TabIndex = 0;
		this.grdtax.TotalRow = (InheritableBoolean)1;
		this.grdtax.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdtax).Visible = false;
		((ScrollableBase)this.grdtax).VisualStyle = (VisualStyle)4;
		this.grdtax.CellEdited += new ColumnActionEventHandler(grdtax_CellEdited);
		this.grdtax.CellUpdated += new ColumnActionEventHandler(grdtax_CellUpdated);
		this.grdtax.ColumnButtonClick += new ColumnActionEventHandler(grdtax_ColumnButtonClick);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdothercharges);
		this.panel5.Location = new System.Drawing.Point(3, 241);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(668, 155);
		this.panel5.TabIndex = 85;
		this.grdothercharges.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdothercharges).BackColor = System.Drawing.Color.White;
		grdothercharges_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdothercharges.DesignTimeLayout = grdothercharges_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdothercharges).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdothercharges.FilterMode = (FilterMode)2;
		this.grdothercharges.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdothercharges).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdothercharges.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdothercharges).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdothercharges).Name = "grdothercharges";
		((ScrollableBase)this.grdothercharges).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdothercharges).Size = new System.Drawing.Size(668, 155);
		((System.Windows.Forms.Control)(object)this.grdothercharges).TabIndex = 0;
		this.grdothercharges.TotalRow = (InheritableBoolean)1;
		this.grdothercharges.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdothercharges).VisualStyle = (VisualStyle)4;
		this.grdothercharges.CellEdited += new ColumnActionEventHandler(grdothercharges_CellEdited);
		this.grdothercharges.CellUpdated += new ColumnActionEventHandler(grdothercharges_CellUpdated);
		this.grdothercharges.ColumnButtonClick += new ColumnActionEventHandler(grdothercharges_ColumnButtonClick);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1246, 239);
		this.panel3.TabIndex = 84;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailadd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1246, 239);
		this.panel9.TabIndex = 85;
		this.txtgrandtotal.BackColor = System.Drawing.Color.White;
		this.txtgrandtotal.Enabled = false;
		this.txtgrandtotal.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtgrandtotal.Location = new System.Drawing.Point(843, 315);
		this.txtgrandtotal.Name = "txtgrandtotal";
		this.txtgrandtotal.ReadOnly = true;
		this.txtgrandtotal.Size = new System.Drawing.Size(157, 25);
		this.txtgrandtotal.TabIndex = 4;
		this.txtgrandtotal.WordWrap = false;
		this.txttaxamount.BackColor = System.Drawing.Color.White;
		this.txttaxamount.Enabled = false;
		this.txttaxamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txttaxamount.Location = new System.Drawing.Point(1417, 264);
		this.txttaxamount.Name = "txttaxamount";
		this.txttaxamount.ReadOnly = true;
		this.txttaxamount.Size = new System.Drawing.Size(157, 25);
		this.txttaxamount.TabIndex = 2;
		this.txttaxamount.Visible = false;
		this.txttaxamount.WordWrap = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label22.Location = new System.Drawing.Point(1417, 296);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 17);
		this.label22.TabIndex = 76;
		this.label22.Text = "Tax Amount";
		this.label22.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label23.Location = new System.Drawing.Point(695, 290);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 17);
		this.label23.TabIndex = 78;
		this.label23.Text = "Other Charges Amount";
		this.txtitemamount.BackColor = System.Drawing.Color.White;
		this.txtitemamount.Enabled = false;
		this.txtitemamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtitemamount.Location = new System.Drawing.Point(843, 258);
		this.txtitemamount.Name = "txtitemamount";
		this.txtitemamount.ReadOnly = true;
		this.txtitemamount.Size = new System.Drawing.Size(157, 25);
		this.txtitemamount.TabIndex = 1;
		this.txtitemamount.WordWrap = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label21.Location = new System.Drawing.Point(695, 261);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(36, 17);
		this.label21.TabIndex = 74;
		this.label21.Text = "Total";
		this.txtotherchargesamount.BackColor = System.Drawing.Color.White;
		this.txtotherchargesamount.Enabled = false;
		this.txtotherchargesamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtotherchargesamount.Location = new System.Drawing.Point(843, 287);
		this.txtotherchargesamount.Name = "txtotherchargesamount";
		this.txtotherchargesamount.ReadOnly = true;
		this.txtotherchargesamount.Size = new System.Drawing.Size(157, 25);
		this.txtotherchargesamount.TabIndex = 3;
		this.txtotherchargesamount.WordWrap = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label24.Location = new System.Drawing.Point(695, 318);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(76, 17);
		this.label24.TabIndex = 80;
		this.label24.Text = "Grand Total";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.ChkPost);
		this.panel7.Controls.Add(this.textTaxAmount);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label17);
		this.panel7.Controls.Add(this.txtTotalAmount);
		this.panel7.Controls.Add(this.label18);
		this.panel7.Controls.Add(this.txtTaxPercent);
		this.panel7.Controls.Add(this.label33);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.combtaxtype);
		this.panel7.Controls.Add(this.label34);
		this.panel7.Controls.Add(this.txtvalidupto);
		this.panel7.Controls.Add(this.label35);
		this.panel7.Controls.Add(this.txteffectivedatedetail);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdestinationport);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbloadingport);
		this.panel7.Controls.Add(this.txtamount);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.panel7.Controls.Add(this.txtchargesrate);
		this.panel7.Controls.Add(this.label19);
		this.panel7.Controls.Add(this.label20);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.panel7.Controls.Add(this.txtexrate);
		this.panel7.Controls.Add(this.label29);
		this.panel7.Controls.Add(this.txtdescription);
		this.panel7.Controls.Add(this.label30);
		this.panel7.Controls.Add(this.txtqty);
		this.panel7.Controls.Add(this.label31);
		this.panel7.Controls.Add(this.btnCancelUpdateDetial);
		this.panel7.Controls.Add(this.btnUpdateDetail);
		this.panel7.Controls.Add(this.btnplus);
		this.panel7.Controls.Add(this.label32);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add(this.label27);
		this.panel7.Controls.Add(this.txtduedays);
		this.panel7.Controls.Add(this.label28);
		this.panel7.Controls.Add(this.txtduedate);
		this.panel7.Controls.Add(this.label26);
		this.panel7.Controls.Add(this.label25);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.txtusdheader);
		this.panel7.Controls.Add(this.Forwarder);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmborderno);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.txtrefbillno);
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.txtnetweight);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.txtnoofcontainer);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.txtcontainersno);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefrenceParty);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmblccontract);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbillTo);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1246, 276);
		this.panel7.TabIndex = 0;
		this.ChkPost.AutoSize = true;
		this.ChkPost.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPost.Location = new System.Drawing.Point(1192, 128);
		this.ChkPost.Name = "ChkPost";
		this.ChkPost.Size = new System.Drawing.Size(48, 17);
		this.ChkPost.TabIndex = 133;
		this.ChkPost.Text = "Post";
		this.ChkPost.UseVisualStyleBackColor = true;
		this.textTaxAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.textTaxAmount.Location = new System.Drawing.Point(239, 239);
		this.textTaxAmount.Name = "textTaxAmount";
		this.textTaxAmount.ReadOnly = true;
		this.textTaxAmount.Size = new System.Drawing.Size(94, 25);
		this.textTaxAmount.TabIndex = 26;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(334, 218);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 131;
		this.label4.Text = "Total Amount";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(236, 218);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(72, 15);
		this.label17.TabIndex = 132;
		this.label17.Text = "Tax Amount";
		this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(336, 239);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(114, 25);
		this.txtTotalAmount.TabIndex = 27;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(157, 219);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(38, 15);
		this.label18.TabIndex = 130;
		this.label18.Text = "Tax%";
		this.txtTaxPercent.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(160, 239);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(76, 25);
		this.txtTaxPercent.TabIndex = 25;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(19, 219);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(56, 15);
		this.label33.TabIndex = 129;
		this.label33.Text = "Tax Type";
		this.combtaxtype.AllowNull = (DefaultableBoolean)1;
		this.combtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Location = new System.Drawing.Point(22, 238);
		((System.Windows.Forms.Control)(object)this.combtaxtype).Name = "combtaxtype";
		((System.Windows.Forms.Control)(object)this.combtaxtype).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.combtaxtype).TabIndex = 24;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Leave += new System.EventHandler(combtaxtype_Leave);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label34.Location = new System.Drawing.Point(567, 219);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(75, 17);
		this.label34.TabIndex = 128;
		this.label34.Text = "Valid Up To";
		this.txtvalidupto.CustomFormat = "dd-MMM-yy";
		this.txtvalidupto.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvalidupto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtvalidupto.Location = new System.Drawing.Point(573, 239);
		this.txtvalidupto.Name = "txtvalidupto";
		this.txtvalidupto.Size = new System.Drawing.Size(117, 25);
		this.txtvalidupto.TabIndex = 29;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label35.Location = new System.Drawing.Point(453, 219);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(87, 17);
		this.label35.TabIndex = 127;
		this.label35.Text = "Effective Date";
		this.txteffectivedatedetail.CustomFormat = "dd-MMM-yy";
		this.txteffectivedatedetail.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txteffectivedatedetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txteffectivedatedetail.Location = new System.Drawing.Point(453, 239);
		this.txteffectivedatedetail.Name = "txteffectivedatedetail";
		this.txteffectivedatedetail.Size = new System.Drawing.Size(117, 25);
		this.txteffectivedatedetail.TabIndex = 28;
		this.cmbdestinationport.AllowNull = (DefaultableBoolean)1;
		this.cmbdestinationport.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdestinationport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdestinationport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdestinationport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Location = new System.Drawing.Point(761, 91);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Name = "cmbdestinationport";
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Size = new System.Drawing.Size(209, 27);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).TabIndex = 11;
		this.cmbloadingport.AllowNull = (DefaultableBoolean)1;
		this.cmbloadingport.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbloadingport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbloadingport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbloadingport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Location = new System.Drawing.Point(442, 91);
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Name = "cmbloadingport";
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Size = new System.Drawing.Size(209, 27);
		((System.Windows.Forms.Control)(object)this.cmbloadingport).TabIndex = 10;
		this.txtamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(993, 182);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(96, 25);
		this.txtamount.TabIndex = 23;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(991, 162);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 17);
		this.label2.TabIndex = 117;
		this.label2.Text = "Amount";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(672, 162);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 17);
		this.label1.TabIndex = 116;
		this.label1.Text = "Fcy Code";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(672, 181);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 20;
		this.txtchargesrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtchargesrate.Location = new System.Drawing.Point(450, 182);
		this.txtchargesrate.Name = "txtchargesrate";
		this.txtchargesrate.Size = new System.Drawing.Size(95, 25);
		this.txtchargesrate.TabIndex = 18;
		this.txtchargesrate.TextChanged += new System.EventHandler(txtchargesrate_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(447, 162);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(86, 17);
		this.label19.TabIndex = 113;
		this.label19.Text = "Charges Rate";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label20.Location = new System.Drawing.Point(552, 162);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(39, 17);
		this.label20.TabIndex = 112;
		this.label20.Text = "UOM";
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(549, 181);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.cmbuom).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.txtexrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtexrate.Location = new System.Drawing.Point(811, 182);
		this.txtexrate.Name = "txtexrate";
		this.txtexrate.Size = new System.Drawing.Size(96, 25);
		this.txtexrate.TabIndex = 21;
		this.txtexrate.TextChanged += new System.EventHandler(txtexrate_TextChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label29.Location = new System.Drawing.Point(811, 162);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(51, 17);
		this.label29.TabIndex = 111;
		this.label29.Text = "Ex Rate";
		this.txtdescription.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdescription.Location = new System.Drawing.Point(262, 182);
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(182, 25);
		this.txtdescription.TabIndex = 17;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label30.Location = new System.Drawing.Point(259, 155);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(74, 17);
		this.label30.TabIndex = 110;
		this.label30.Text = "Description";
		this.txtqty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(913, 182);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(74, 25);
		this.txtqty.TabIndex = 22;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label31.Location = new System.Drawing.Point(911, 162);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(31, 17);
		this.label31.TabIndex = 109;
		this.label31.Text = "QTY";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(812, 239);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(55, 26);
		this.btnCancelUpdateDetial.TabIndex = 32;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(752, 239);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 26);
		this.btnUpdateDetail.TabIndex = 31;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(694, 239);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(53, 26);
		this.btnplus.TabIndex = 30;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label32.Location = new System.Drawing.Point(19, 156);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(33, 17);
		this.label32.TabIndex = 108;
		this.label32.Text = "Item";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(22, 181);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label27.Location = new System.Drawing.Point(689, 10);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 17);
		this.label27.TabIndex = 94;
		this.label27.Text = "Due Days";
		this.txtduedays.Location = new System.Drawing.Point(761, 6);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(209, 25);
		this.txtduedays.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label28.Location = new System.Drawing.Point(38, 38);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 17);
		this.label28.TabIndex = 92;
		this.label28.Text = "Due Date";
		this.txtduedate.CustomFormat = "dd-MMM-yy";
		this.txtduedate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtduedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtduedate.Location = new System.Drawing.Point(107, 34);
		this.txtduedate.Name = "txtduedate";
		this.txtduedate.Size = new System.Drawing.Size(209, 25);
		this.txtduedate.TabIndex = 3;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label26.Location = new System.Drawing.Point(0, 310);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(108, 17);
		this.label26.TabIndex = 33;
		this.label26.Text = "OTHER CHARGES";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label25.Location = new System.Drawing.Point(1321, 310);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(30, 17);
		this.label25.TabIndex = 85;
		this.label25.Text = "TAX";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(37, 125);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(63, 17);
		this.label10.TabIndex = 82;
		this.label10.Text = "USD Rate";
		this.txtusdheader.Location = new System.Drawing.Point(107, 121);
		this.txtusdheader.Name = "txtusdheader";
		this.txtusdheader.Size = new System.Drawing.Size(209, 25);
		this.txtusdheader.TabIndex = 12;
		this.Forwarder.AutoSize = true;
		this.Forwarder.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.Forwarder.Location = new System.Drawing.Point(672, 67);
		this.Forwarder.Name = "Forwarder";
		this.Forwarder.Size = new System.Drawing.Size(80, 17);
		this.Forwarder.TabIndex = 80;
		this.Forwarder.Text = "Bill To/ Party";
		this.cmborderno.AllowNull = (DefaultableBoolean)1;
		this.cmborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmborderno).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmborderno).Location = new System.Drawing.Point(761, 33);
		((System.Windows.Forms.Control)(object)this.cmborderno).Name = "cmborderno";
		((System.Windows.Forms.Control)(object)this.cmborderno).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmborderno).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmborderno).Leave += new System.EventHandler(cmborderno_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(365, 38);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 17);
		this.label3.TabIndex = 78;
		this.label3.Text = "Ref Bill No";
		this.txtrefbillno.Location = new System.Drawing.Point(442, 34);
		this.txtrefbillno.Name = "txtrefbillno";
		this.txtrefbillno.Size = new System.Drawing.Size(209, 25);
		this.txtrefbillno.TabIndex = 4;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label16.Location = new System.Drawing.Point(679, 125);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(73, 17);
		this.label16.TabIndex = 57;
		this.label16.Text = "Net Weight";
		this.txtnetweight.Location = new System.Drawing.Point(761, 121);
		this.txtnetweight.Name = "txtnetweight";
		this.txtnetweight.Size = new System.Drawing.Size(209, 25);
		this.txtnetweight.TabIndex = 14;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label15.Location = new System.Drawing.Point(324, 125);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(110, 17);
		this.label15.TabIndex = 55;
		this.label15.Text = "No Of Containers";
		this.txtnoofcontainer.Location = new System.Drawing.Point(442, 121);
		this.txtnoofcontainer.Name = "txtnoofcontainer";
		this.txtnoofcontainer.Size = new System.Drawing.Size(209, 25);
		this.txtnoofcontainer.TabIndex = 13;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(651, 96);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(101, 17);
		this.label14.TabIndex = 53;
		this.label14.Text = "Destination Port";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(976, 13);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(92, 17);
		this.label13.TabIndex = 51;
		this.label13.Text = "Containers No";
		this.txtcontainersno.Location = new System.Drawing.Point(976, 34);
		this.txtcontainersno.Multiline = true;
		this.txtcontainersno.Name = "txtcontainersno";
		this.txtcontainersno.Size = new System.Drawing.Size(209, 112);
		this.txtcontainersno.TabIndex = 15;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(351, 96);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(83, 17);
		this.label12.TabIndex = 49;
		this.label12.Text = "Loading Port";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(4, 96);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(99, 17);
		this.label11.TabIndex = 47;
		this.label11.Text = "Reference Party";
		this.CmbRefrenceParty.AllowNull = (DefaultableBoolean)1;
		this.CmbRefrenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefrenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefrenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefrenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefrenceParty).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefrenceParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefrenceParty).Location = new System.Drawing.Point(107, 91);
		((System.Windows.Forms.Control)(object)this.CmbRefrenceParty).Name = "CmbRefrenceParty";
		((System.Windows.Forms.Control)(object)this.CmbRefrenceParty).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefrenceParty).TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(353, 67);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(82, 17);
		this.label9.TabIndex = 45;
		this.label9.Text = "Lc / Contract";
		this.cmblccontract.AllowNull = (DefaultableBoolean)1;
		this.cmblccontract.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmblccontract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmblccontract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmblccontract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmblccontract).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmblccontract).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmblccontract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Location = new System.Drawing.Point(442, 62);
		((System.Windows.Forms.Control)(object)this.cmblccontract).Name = "cmblccontract";
		((System.Windows.Forms.Control)(object)this.cmblccontract).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmblccontract).TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(29, 67);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(70, 17);
		this.label8.TabIndex = 43;
		this.label8.Text = "Invoice No";
		this.cmbeximinvoiceno.AllowNull = (DefaultableBoolean)1;
		this.cmbeximinvoiceno.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbeximinvoiceno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbeximinvoiceno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance105).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbeximinvoiceno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno).Location = new System.Drawing.Point(107, 62);
		((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno).Name = "cmbeximinvoiceno";
		((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmbeximinvoiceno).TabIndex = 7;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(687, 38);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 17);
		this.label7.TabIndex = 41;
		this.label7.Text = "Order No";
		this.cmbbillTo.AllowNull = (DefaultableBoolean)1;
		this.cmbbillTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbillTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbillTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbillTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbbillTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbillTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbillTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbillTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbillTo).Location = new System.Drawing.Point(761, 62);
		((System.Windows.Forms.Control)(object)this.cmbbillTo).Name = "cmbbillTo";
		((System.Windows.Forms.Control)(object)this.cmbbillTo).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmbbillTo).TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(372, 10);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 17);
		this.label6.TabIndex = 38;
		this.label6.Text = "Doc Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(442, 6);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(209, 25);
		this.txtdocdate.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(47, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(53, 17);
		this.label5.TabIndex = 37;
		this.label5.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.Location = new System.Drawing.Point(107, 6);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(209, 25);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.btn.BackColor = System.Drawing.Color.White;
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.btnprint, this.btnattachment, this.btnVoucher });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1246, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(225, 24);
		this.toolStripLabel1.Text = "Service Bill -Against Po            ";
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(35, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(56, 24);
		this.btnprint.Text = "&Print";
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher.Name = "btnVoucher";
		this.btnVoucher.Size = new System.Drawing.Size(74, 24);
		this.btnVoucher.Text = "Voucher";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1260, 732);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ExImClearingAgentBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExImClearingAgentBill_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExImClearingAgentBillDirect_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdtax).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdothercharges).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbloadingport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmborderno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefrenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbeximinvoiceno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbillTo).EndInit();
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
