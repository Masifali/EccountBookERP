using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Service;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Service;
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

public class ItemPricingSupplierExport : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private DataTable dt = new DataTable();

	private DataTable dataTablegrhistorydetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private DataTable grdtable = new DataTable();

	private DataTable tblhistory = new DataTable();

	private DataTable getbysuppliertable = new DataTable();

	private int updateDetailIndex;

	private IContainer components;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnprint;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnNew;

	private ToolStrip btn;

	private ToolStripButton btnattachment;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grddetailadd;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private TextBox txtremarks;

	private Label label5;

	private Label label1;

	internal UltraCombo cmbsupcust;

	private Label label3;

	internal UltraCombo cmbitem;

	private Label label6;

	internal UltraCombo cmbuom;

	private TextBox txtexrate;

	private Label label4;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private ToolStripLabel toolStripLabel1;

	internal GridEX grdrecordbysup;

	private TextBox txtchargesrate;

	private Label label8;

	private Label label9;

	private DateTimePicker txteffectivedatedetail;

	private Panel panel3;

	private Panel panel2;

	private Label label10;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Label label11;

	private Label label14;

	private DateTimePicker txtvalidupto;

	private Label label13;

	private DateTimePicker txtdocdatedetail;

	private TextBox txtsupplierrefno;

	private Label label12;

	private Panel panel6;

	internal GridEX grdhistorydetail;

	private Panel panel5;

	private Label label15;

	internal UltraCombo cmbcurrency;

	private TextBox txtamount;

	private Label label7;

	public ItemPricingSupplierExport(UserAccount pUserAccount)
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			int Code = ItemPricingSupplierCustomer.GenerateCode(new ItemPricingSupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
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

	private void SupplierCustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsuppliercustomer = SupplierCustomer.ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsuppliercustomer.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsuppliercustomer, cmbsupcust, "Id", "CompanyName", "Service Provider", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupcust_Leave(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbsupcust).Text != null)
			{
				new List<ItemPricingSupplierCustomer>();
				DataTable dtgetdetail = ItemPricingSupplierCustomer.ReadDetailByOrganizationCompanyIdNSupplierId(new ItemPricingSupplierCustomer
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(cmbsupcust.Value)
				});
				getbysuppliertable.Clear();
				for (int i = 0; i < dtgetdetail.Rows.Count; i++)
				{
					getbysuppliertable.Rows.Add(dtgetdetail.Rows[i]["DocDate"], dtgetdetail.Rows[i]["ItemName"], dtgetdetail.Rows[i]["ChargesRate"], dtgetdetail.Rows[i]["CurrencyName"], dtgetdetail.Rows[i]["ExhangeRate"], dtgetdetail.Rows[i]["Amount"]);
				}
				grdrecordbysup.DataSource = getbysuppliertable;
				grdrecordbysup.RetrieveStructure();
				GetBysupplierId();
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
				DDL.BindDDLNew(dtitem, cmbitem, "Id", "ItemName", "Item", false);
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception)
		{
			throw;
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
			DataTable curr = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (curr.Rows.Count > 0)
			{
				DDL.BindDDLNew(curr, cmbcurrency, "Id", "CurrencyCode", "FcyCode", false);
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbitem).Text != null)
			{
				DataTable dtCOMMUOM = UOMSchedule.SearchByObject(new UOMSchedule
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					ItemId = Conversion.ToInt(cmbitem.Value)
				});
				if (dtCOMMUOM.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtCOMMUOM, cmbuom, "Id", "Equivalent", "Rate UOM", false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbsupcust).ActiveRow == null)
		{
			MessageBox.Show("Please Select SupplierCustomer");
			cmbsupcust.Focus();
			return false;
		}
		return true;
	}

	private bool formvalidationDetail()
	{
		if (((UltraGridBase)cmbitem).ActiveRow == null)
		{
			MessageBox.Show("Please Select item");
			cmbitem.Focus();
			return false;
		}
		if (txtchargesrate.Text.Trim() == "" || Conversion.ToDouble((object)txtchargesrate.Text) == 0.0 || !(Conversion.ToDouble((object)txtchargesrate.Text) > 0.0))
		{
			MessageBox.Show("Please Enter ChargesRate");
			txtchargesrate.Focus();
			return false;
		}
		if (((UltraGridBase)cmbuom).ActiveRow == null)
		{
			MessageBox.Show("Please Select Uom");
			cmbuom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcurrency).ActiveRow == null)
		{
			MessageBox.Show("Please Select FcyCode");
			cmbcurrency.Focus();
			return false;
		}
		if (txtamount.Text.Trim() == "" || !(Conversion.ToDouble((object)txtamount.Text) > 0.0) || Conversion.ToDouble((object)txtamount.Text) == 0.0)
		{
			MessageBox.Show("Please Check Amount");
			txtamount.Focus();
			return false;
		}
		return true;
	}

	private void ItemPricingSupplierExport_Load(object sender, EventArgs e)
	{
		try
		{
			cmbsupcust.Focus();
			GenerateCode();
			Currency();
			SupplierCustomer();
			item();
			Currency();
			table.Columns.Add("SupplierRefNo");
			table.Columns.Add("DocDate");
			table.Columns.Add("ItemId");
			table.Columns.Add("Item");
			table.Columns.Add("ChargesRate");
			table.Columns.Add("UomId");
			table.Columns.Add("Uom");
			table.Columns.Add("CurrencyId");
			table.Columns.Add("Currency");
			table.Columns.Add("ExchangeRate");
			table.Columns.Add("Amount");
			table.Columns.Add("EffectiveDate");
			table.Columns.Add("ValidUpTo");
			getbysuppliertable.Columns.Add("DocDate");
			getbysuppliertable.Columns.Add("Item");
			getbysuppliertable.Columns.Add("ChargesRate");
			getbysuppliertable.Columns.Add("Currency");
			getbysuppliertable.Columns.Add("ExchangeRate");
			getbysuppliertable.Columns.Add("Amount");
			grdtable.Columns.Add("Supplier");
			grdtable.Columns.Add("Item");
			grdtable.Columns.Add("USD");
			grdtable.Columns.Add("Rate");
			grdtable.Columns.Add("RateUom");
			grdtable.Columns.Add("ValidUpTo");
			dataTablegrhistorydetail.Columns.Add("SupplierRefNo");
			dataTablegrhistorydetail.Columns.Add("DocDate");
			dataTablegrhistorydetail.Columns.Add("ItemId");
			dataTablegrhistorydetail.Columns.Add("Item");
			dataTablegrhistorydetail.Columns.Add("ChargesRate");
			dataTablegrhistorydetail.Columns.Add("Uom");
			dataTablegrhistorydetail.Columns.Add("Currency");
			dataTablegrhistorydetail.Columns.Add("ExchangeRate");
			dataTablegrhistorydetail.Columns.Add("Amount");
			dataTablegrhistorydetail.Columns.Add("EffectiveDate");
			dataTablegrhistorydetail.Columns.Add("ValidUpTo");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grddetailadd.RowCount > 0)
			{
				ItemPricingSupplierCustomer labheader = new ItemPricingSupplierCustomer();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					labheader.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				labheader.EntryDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.ModifyDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.ApprovedDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.DocTypeId = 152;
				labheader.EntryUser = UserAccount.ID;
				labheader.OrganizationId = UserAccount.OrganizationId;
				labheader.CompanyId = UserAccount.CompanyId;
				labheader.EffectedFromH = Conversion.ToDateTime((object)DateTime.Now);
				labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				labheader.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
				labheader.SupplierCustomerId = Conversion.ToInt(cmbsupcust.Value);
				labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
				labheader.ItemPricingDetail = new List<ItemPricingSupplierCustomerDetail>();
				GridEXRow[] rows = grddetailadd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ItemPricingSupplierCustomerDetail labdetail = new ItemPricingSupplierCustomerDetail();
					labdetail.SupplierRefNo = Conversion.ToString(r.Cells["SupplierRefNo"].Value);
					labdetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					labdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (labdetail.ItemId == 0)
					{
						MessageBox.Show("ItemName Field Required");
						return;
					}
					labdetail.ChargesRate = Conversion.ToDouble(r.Cells["ChargesRate"].Value);
					if (labdetail.ChargesRate == 0.0)
					{
						MessageBox.Show("ChargesRate Field Required");
						return;
					}
					labdetail.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
					if (labdetail.UomId == 0)
					{
						MessageBox.Show("Uom Field Required");
						return;
					}
					labdetail.CurrencyId = Conversion.ToInt(r.Cells["CurrencyId"].Value);
					if (labdetail.CurrencyId == 0)
					{
						MessageBox.Show("Currency Field Required");
						return;
					}
					labdetail.ExhangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
					labdetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					if (labdetail.Amount == 0.0)
					{
						MessageBox.Show("Amount Field Required");
						return;
					}
					labdetail.EffectedDateD = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
					labdetail.ValidUpTo = Conversion.ToDateTime(r.Cells["ValidUpTo"].Value);
					labheader.ItemPricingDetail.Add(labdetail);
				}
				ItemPricingSupplierCustomer.Save(labheader);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
				reset();
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
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

	private void reset()
	{
		RecId = 0;
		cmbsupcust.Focus();
		GenerateCode();
		table.Clear();
		grdtable.Clear();
		btnSave.Visible = true;
		btnUpdate.Visible = false;
		txtremarks.Text = "";
		((Control)(object)cmbsupcust).Text = "";
		txtsupplierrefno.Text = string.Empty;
		txtchargesrate.Text = string.Empty;
		txtexrate.Text = string.Empty;
		((Control)(object)cmbcurrency).Text = string.Empty;
		((Control)(object)cmbuom).Text = string.Empty;
	}

	private void ResetDetail()
	{
		txtsupplierrefno.Text = string.Empty;
		((Control)(object)cmbitem).Text = string.Empty;
		txtchargesrate.Text = string.Empty;
		((Control)(object)cmbuom).Text = string.Empty;
		txtexrate.Text = string.Empty;
		((Control)(object)cmbcurrency).Text = string.Empty;
		((Control)(object)cmbuom).Text = string.Empty;
		txtamount.Text = string.Empty;
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ItemPricingSupplierCustomer po = new ItemPricingSupplierCustomer();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
			po = ItemPricingSupplierCustomer.GetByID(RecId);
			txtdocdate.Value = Conversion.ToDateTime((object)po.DocDate);
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			((Control)(object)cmbsupcust).Text = Conversion.ToString((object)po.SupplierCustomerId);
			txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
			table.Clear();
			for (int i = 0; i < po.ItemPricingDetail.Count; i++)
			{
				table.Rows.Add(po.ItemPricingDetail[i].SupplierRefNo, po.ItemPricingDetail[i].DocDate, po.ItemPricingDetail[i].ItemId, po.ItemPricingDetail[i].ItemName, po.ItemPricingDetail[i].ChargesRate, po.ItemPricingDetail[i].UomId, po.ItemPricingDetail[i].UomName, po.ItemPricingDetail[i].CurrencyId, po.ItemPricingDetail[i].CurrencyName, po.ItemPricingDetail[i].ExhangeRate, po.ItemPricingDetail[i].Amount, po.ItemPricingDetail[i].EffectedDateD, po.ItemPricingDetail[i].ValidUpTo);
			}
			grddetailadd.DataSource = table;
			grddetailadd.RetrieveStructure();
			tabControl1.SelectedIndex = 0;
			gridsetting();
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
			grddetailadd.RootTable.Columns["CurrencyId"].Visible = false;
			grddetailadd.RootTable.Columns["SupplierRefNo"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)5;
			grddetailadd.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["ChargesRate"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Uom"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Currency"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grddetailadd.RootTable.Columns["EffectiveDate"].FilterEditType = (FilterEditType)5;
			grddetailadd.RootTable.Columns["ValidUpTo"].FilterEditType = (FilterEditType)5;
			grddetailadd.RootTable.Columns["SupplierRefNo"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Item"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ChargesRate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Uom"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Currency"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["EffectiveDate"].EditType = (EditType)0;
			grddetailadd.RootTable.Columns["ValidUpTo"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBysupplierId()
	{
		try
		{
			grdrecordbysup.RootTable.Columns["DocDate"].Width = 90;
			grdrecordbysup.RootTable.Columns["Item"].Width = 250;
			grdrecordbysup.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)5;
			grdrecordbysup.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdrecordbysup.RootTable.Columns["ChargesRate"].FilterEditType = (FilterEditType)1;
			grdrecordbysup.RootTable.Columns["Currency"].FilterEditType = (FilterEditType)1;
			grdrecordbysup.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grdrecordbysup.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdrecordbysup.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdrecordbysup.RootTable.Columns["Item"].EditType = (EditType)0;
			grdrecordbysup.RootTable.Columns["ChargesRate"].EditType = (EditType)0;
			grdrecordbysup.RootTable.Columns["Currency"].EditType = (EditType)0;
			grdrecordbysup.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grdrecordbysup.RootTable.Columns["Amount"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocNo"].Width = 70;
			grdhistory.RootTable.Columns["DocDate"].Width = 150;
			grdhistory.RootTable.Columns["ServiceProvider"].Width = 250;
			grdhistory.RootTable.Columns["Remarks"].Width = 250;
			grdhistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)5;
			grdhistory.RootTable.Columns["ServiceProvider"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["ServiceProvider"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["Remarks"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGrid()
	{
		try
		{
			grdhistorydetail.RootTable.Columns["ItemId"].Visible = false;
			grdhistorydetail.RootTable.Columns["SupplierRefNo"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)5;
			grdhistorydetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["ChargesRate"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Uom"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Currency"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["EffectiveDate"].FilterEditType = (FilterEditType)5;
			grdhistorydetail.RootTable.Columns["ValidUpTo"].FilterEditType = (FilterEditType)5;
			grdhistorydetail.RootTable.Columns["SupplierRefNo"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ChargesRate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Currency"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["EffectiveDate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ValidUpTo"].EditType = (EditType)0;
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
			if (formvalidationDetail())
			{
				table.Rows.Add(txtsupplierrefno.Text, Convert.ToDateTime(txtdocdatedetail.Value).ToString("dd-MMM-yyyy"), cmbitem.Value, ((Control)(object)cmbitem).Text.Trim(), txtchargesrate.Text.Trim(), cmbuom.Value, ((Control)(object)cmbuom).Text, cmbcurrency.Value, ((Control)(object)cmbcurrency).Text.Trim(), txtexrate.Text.Trim(), txtamount.Text.Trim(), Convert.ToDateTime(txteffectivedatedetail.Value).ToString("dd-MMM-yyyy"), Convert.ToDateTime(txtvalidupto.Value).ToString("dd-MMM-yyyy"));
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
				ResetDetail();
			}
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
				txtsupplierrefno.Text = item.Cells["SupplierRefNo"].Value.ToString();
				txtdocdate.Text = item.Cells["DocDate"].Value.ToString();
				((Control)(object)cmbitem).Text = item.Cells["Item"].Value.ToString();
				txtchargesrate.Text = item.Cells["ChargesRate"].Value.ToString();
				((Control)(object)cmbuom).Text = item.Cells["Uom"].Value.ToString();
				((Control)(object)cmbcurrency).Text = item.Cells["Currency"].Value.ToString();
				txtexrate.Text = item.Cells["ExchangeRate"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				txteffectivedatedetail.Text = item.Cells["EffectiveDate"].Value.ToString();
				txtvalidupto.Text = item.Cells["ValidUpTo"].Value.ToString();
				cmbitem.Focus();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
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
			if (formvalidationDetail())
			{
				table.Rows[updateDetailIndex]["SupplierRefNo"] = txtsupplierrefno.Text;
				table.Rows[updateDetailIndex]["DocDate"] = txtdocdatedetail.Value;
				table.Rows[updateDetailIndex]["ItemId"] = cmbitem.Value;
				table.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbitem).Text;
				table.Rows[updateDetailIndex]["ChargesRate"] = txtchargesrate.Text;
				table.Rows[updateDetailIndex]["UomId"] = cmbuom.Value;
				table.Rows[updateDetailIndex]["Uom"] = ((Control)(object)cmbuom).Text;
				table.Rows[updateDetailIndex]["Currency"] = ((Control)(object)cmbcurrency).Text;
				table.Rows[updateDetailIndex]["ExchangeRate"] = txtexrate.Text;
				table.Rows[updateDetailIndex]["Amount"] = txtamount.Text;
				table.Rows[updateDetailIndex]["EffectiveDate"] = txteffectivedatedetail.Value;
				table.Rows[updateDetailIndex]["ValidUpTo"] = txtvalidupto.Value;
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
				ResetDetail();
				txtsupplierrefno.Focus();
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
			}
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
		DataTable dthistoryheader = ItemPricingSupplierCustomer.FormHistoryData(new ItemPricingSupplierCustomer
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId
		});
		DataTable tblhistory = new DataTable();
		tblhistory.Columns.Add("Id");
		tblhistory.Columns.Add("DocNo");
		tblhistory.Columns.Add("DocDate");
		tblhistory.Columns.Add("ServiceProvider");
		tblhistory.Columns.Add("Remarks");
		tblhistory.Clear();
		for (int i = 0; i < dthistoryheader.Rows.Count; i++)
		{
			tblhistory.Rows.Add(dthistoryheader.Rows[i]["Id"], dthistoryheader.Rows[i]["DocNo"], Convert.ToDateTime(dthistoryheader.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthistoryheader.Rows[i]["CompanyName"], dthistoryheader.Rows[i]["RemarksHeader"]);
		}
		grdhistory.DataSource = tblhistory;
		grdhistory.RetrieveStructure();
		HistoryGridSetting();
	}

	private void GridFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		new List<ItemPricingSupplierCustomer>();
		new ItemPricingSupplierCustomer
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId
		};
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
		ResetDetail();
	}

	private void totalamount()
	{
		try
		{
			double chargesrate = 0.0;
			double exchagerate = 0.0;
			int equivalent = 0;
			double TotalAmount = 0.0;
			if (txtexrate.Text.Trim() != string.Empty && txtchargesrate.Text.Trim() != string.Empty && ((Control)(object)cmbuom).Text.Trim() != "0")
			{
				exchagerate = Conversion.ToDouble((object)txtexrate.Text.Trim());
				chargesrate = Conversion.ToDouble((object)txtchargesrate.Text.Trim());
				equivalent = Conversion.ToInt((object)((Control)(object)cmbuom).Text.Trim());
				if (exchagerate == 0.0)
				{
					txtamount.Text = (chargesrate * (double)equivalent).ToString();
				}
				else
				{
					txtamount.Text = (exchagerate * chargesrate * (double)equivalent).ToString();
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

	private void cmbcurrency_TextChanged(object sender, EventArgs e)
	{
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

	private void grdhistory_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			if (grdhistory.SelectedItems.Count <= 0)
			{
				return;
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				ItemPricingSupplierCustomer po = new ItemPricingSupplierCustomer();
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
				po = ItemPricingSupplierCustomer.GetByID(RecId);
				dataTablegrhistorydetail.Clear();
				for (int i = 0; i < po.ItemPricingDetail.Count; i++)
				{
					dataTablegrhistorydetail.Rows.Add(po.ItemPricingDetail[i].SupplierRefNo, Convert.ToDateTime(po.ItemPricingDetail[i].DocDate).ToString("dd-MMM-yyyy"), po.ItemPricingDetail[i].ItemId, po.ItemPricingDetail[i].ItemName, po.ItemPricingDetail[i].ChargesRate, po.ItemPricingDetail[i].UomName, po.ItemPricingDetail[i].CurrencyName, po.ItemPricingDetail[i].ExhangeRate, po.ItemPricingDetail[i].Amount, Convert.ToDateTime(po.ItemPricingDetail[i].EffectedDateD).ToString("dd-MMM-yyyy"), Convert.ToDateTime(po.ItemPricingDetail[i].ValidUpTo).ToString("dd-MMM-yyyy"));
				}
				grdhistorydetail.DataSource = dataTablegrhistorydetail;
				grdhistorydetail.RetrieveStructure();
				HistoryDetailGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			historygrid();
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
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
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		GridEXLayout grddetailadd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistorydetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdrecordbysup_DesignTimeLayout = new GridEXLayout();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.grddetailadd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdhistorydetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdrecordbysup = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtvalidupto = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.txtdocdatedetail = new System.Windows.Forms.DateTimePicker();
		this.txtsupplierrefno = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.txteffectivedatedetail = new System.Windows.Forms.DateTimePicker();
		this.txtchargesrate = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbuom = new UltraCombo();
		this.txtexrate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbsupcust = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdrecordbysup).BeginInit();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupcust).BeginInit();
		base.SuspendLayout();
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btn.BackColor = System.Drawing.Color.White;
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.btnprint, this.btnattachment, this.toolStripSeparator });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1356, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(254, 24);
		this.toolStripLabel1.Text = "Customer Pricing Agreement        ";
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
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
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
		((System.Windows.Forms.Control)(object)this.grddetailadd).Size = new System.Drawing.Size(896, 380);
		((System.Windows.Forms.Control)(object)this.grddetailadd).TabIndex = 0;
		this.grddetailadd.TotalRow = (InheritableBoolean)1;
		this.grddetailadd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailadd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grddetailadd).DoubleClick += new System.EventHandler(grddetailadd_DoubleClick);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1362, 596);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1356, 590);
		this.panel4.TabIndex = 1;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistorydetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 326);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1356, 264);
		this.panel6.TabIndex = 5;
		this.grdhistorydetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistorydetail).BackColor = System.Drawing.Color.White;
		grdhistorydetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistorydetail.DesignTimeLayout = grdhistorydetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistorydetail.FilterMode = (FilterMode)2;
		this.grdhistorydetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistorydetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Name = "grdhistorydetail";
		((ScrollableBase)this.grdhistorydetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Size = new System.Drawing.Size(1356, 264);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).TabIndex = 3;
		this.grdhistorydetail.TotalRow = (InheritableBoolean)1;
		this.grdhistorydetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistorydetail).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1356, 326);
		this.panel5.TabIndex = 4;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1356, 326);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdhistory).Click += new System.EventHandler(grdhistory_Click);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 622);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1362, 596);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 213);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1356, 380);
		this.panel1.TabIndex = 38;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdrecordbysup);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(896, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(460, 380);
		this.panel3.TabIndex = 1;
		this.grdrecordbysup.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdrecordbysup).BackColor = System.Drawing.Color.White;
		grdrecordbysup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdrecordbysup.DesignTimeLayout = grdrecordbysup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdrecordbysup.FilterMode = (FilterMode)2;
		this.grdrecordbysup.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdrecordbysup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).Name = "grdrecordbysup";
		((ScrollableBase)this.grdrecordbysup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).Size = new System.Drawing.Size(460, 380);
		((System.Windows.Forms.Control)(object)this.grdrecordbysup).TabIndex = 0;
		this.grdrecordbysup.TotalRow = (InheritableBoolean)1;
		this.grdrecordbysup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdrecordbysup).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailadd);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(896, 380);
		this.panel2.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.txtamount);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.txtvalidupto);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.txtdocdatedetail);
		this.panel7.Controls.Add(this.txtsupplierrefno);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.txteffectivedatedetail);
		this.panel7.Controls.Add(this.txtchargesrate);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.btnCancelUpdateDetial);
		this.panel7.Controls.Add(this.btnUpdateDetail);
		this.panel7.Controls.Add(this.btnplus);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.panel7.Controls.Add(this.txtexrate);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupcust);
		this.panel7.Controls.Add(this.txtremarks);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1356, 183);
		this.panel7.TabIndex = 0;
		this.txtamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(948, 97);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(96, 25);
		this.txtamount.TabIndex = 11;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(946, 77);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 17);
		this.label7.TabIndex = 42;
		this.label7.Text = "Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label15.Location = new System.Drawing.Point(709, 77);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(61, 17);
		this.label15.TabIndex = 40;
		this.label15.Text = "Fcy Code";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(709, 96);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TextChanged += new System.EventHandler(cmbcurrency_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(1166, 78);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 17);
		this.label14.TabIndex = 38;
		this.label14.Text = "Valid Up To";
		this.txtvalidupto.CustomFormat = "dd-MMM-yy";
		this.txtvalidupto.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvalidupto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtvalidupto.Location = new System.Drawing.Point(1172, 98);
		this.txtvalidupto.Name = "txtvalidupto";
		this.txtvalidupto.Size = new System.Drawing.Size(117, 25);
		this.txtvalidupto.TabIndex = 13;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(127, 78);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 17);
		this.label13.TabIndex = 36;
		this.label13.Text = "Doc Date";
		this.txtdocdatedetail.CustomFormat = "dd-MMM-yy";
		this.txtdocdatedetail.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdatedetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdatedetail.Location = new System.Drawing.Point(130, 97);
		this.txtdocdatedetail.Name = "txtdocdatedetail";
		this.txtdocdatedetail.Size = new System.Drawing.Size(117, 25);
		this.txtdocdatedetail.TabIndex = 5;
		this.txtsupplierrefno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtsupplierrefno.Location = new System.Drawing.Point(17, 97);
		this.txtsupplierrefno.Name = "txtsupplierrefno";
		this.txtsupplierrefno.Size = new System.Drawing.Size(111, 25);
		this.txtsupplierrefno.TabIndex = 4;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(14, 78);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(91, 17);
		this.label12.TabIndex = 35;
		this.label12.Text = "Supplier Ref #";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(154, 37);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(138, 25);
		this.txtdocno.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(152, 17);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 17);
		this.label11.TabIndex = 33;
		this.label11.Text = "Doc No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(13, 17);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(62, 17);
		this.label10.TabIndex = 30;
		this.label10.Text = "Doc Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(16, 37);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(132, 25);
		this.txtdocdate.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(1046, 78);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(87, 17);
		this.label9.TabIndex = 28;
		this.label9.Text = "Effective Date";
		this.txteffectivedatedetail.CustomFormat = "dd-MMM-yy";
		this.txteffectivedatedetail.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txteffectivedatedetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txteffectivedatedetail.Location = new System.Drawing.Point(1049, 97);
		this.txteffectivedatedetail.Name = "txteffectivedatedetail";
		this.txteffectivedatedetail.Size = new System.Drawing.Size(117, 25);
		this.txteffectivedatedetail.TabIndex = 12;
		this.txtchargesrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtchargesrate.Location = new System.Drawing.Point(487, 97);
		this.txtchargesrate.Name = "txtchargesrate";
		this.txtchargesrate.Size = new System.Drawing.Size(95, 25);
		this.txtchargesrate.TabIndex = 7;
		this.txtchargesrate.TextChanged += new System.EventHandler(txtchargesrate_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(484, 78);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(86, 17);
		this.label8.TabIndex = 27;
		this.label8.Text = "Charges Rate";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1234, 142);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(55, 26);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1172, 142);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 26);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(1113, 142);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(53, 26);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(589, 77);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(39, 17);
		this.label6.TabIndex = 21;
		this.label6.Text = "UOM";
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(586, 96);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbuom).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.txtexrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtexrate.Location = new System.Drawing.Point(848, 96);
		this.txtexrate.Name = "txtexrate";
		this.txtexrate.Size = new System.Drawing.Size(96, 25);
		this.txtexrate.TabIndex = 10;
		this.txtexrate.TextChanged += new System.EventHandler(txtexrate_TextChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(848, 78);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 17);
		this.label4.TabIndex = 18;
		this.label4.Text = "Ex Rate";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(247, 78);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(33, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Item";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(250, 96);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(298, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(102, 17);
		this.label1.TabIndex = 13;
		this.label1.Text = "Service Provider";
		this.cmbsupcust.AllowNull = (DefaultableBoolean)1;
		this.cmbsupcust.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupcust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupcust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupcust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupcust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupcust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupcust).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupcust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupcust).Location = new System.Drawing.Point(298, 37);
		((System.Windows.Forms.Control)(object)this.cmbsupcust).Name = "cmbsupcust";
		((System.Windows.Forms.Control)(object)this.cmbsupcust).Size = new System.Drawing.Size(300, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupcust).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbsupcust).Leave += new System.EventHandler(cmbsupcust_Leave);
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(604, 37);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(582, 25);
		this.txtremarks.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(601, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 17);
		this.label5.TabIndex = 0;
		this.label5.Text = "Remarks ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 622);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ItemPricingSupplierExport";
		this.Text = "Eccountbook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ItemPricingSupplierExport_Load);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdrecordbysup).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupcust).EndInit();
		base.ResumeLayout(false);
	}
}

}
