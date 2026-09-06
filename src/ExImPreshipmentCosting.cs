using System;
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

public class ExImPreshipmentCosting : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private DataTable dt = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private bool Confiquration;

	private int RecId;

	private int HeaderId;

	private DataTable grdtable = new DataTable();

	private DataTable dtgrdhistory = new DataTable();

	private int updateDetailIndex;

	private IContainer components;

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

	private ToolStripLabel toolStripLabel1;

	private Panel panel7;

	private Label label6;

	private DateTimePicker txtdocdate;

	private Label label5;

	private TextBox txtdocno;

	private Label label16;

	private TextBox txtnetweight;

	private Label label15;

	private TextBox txtnoofcontainer;

	private Label label14;

	private Label label13;

	private TextBox txtremarksheader;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label20;

	internal UltraCombo cmbitem;

	private TextBox txtqty;

	private Label label1;

	private TextBox txtdescription;

	private Label label18;

	private Panel panel3;

	internal GridEX grdhistorydetail;

	private Panel panel2;

	private TextBox txtamount;

	private Label label2;

	private Label label3;

	internal UltraCombo cmbcurrency;

	private TextBox txtchargesrate;

	private Label label17;

	private Label label19;

	internal UltraCombo cmbuom;

	private TextBox txtexrate;

	private Label label21;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo combtaxtype;

	private Label label24;

	private TextBox txtTaxAmount;

	private Label label23;

	private TextBox txtTaxPercent;

	private Label label22;

	private Label label25;

	private TextBox txtTotalAmount;

	private CtrlGrdBar ctrlGrdBar2;

	public ExImPreshipmentCosting(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImSaleOrderForwarder code = new ExImSaleOrderForwarder();
			code.OrganizationId = UserAccount.OrganizationId;
			code.CompanyId = UserAccount.CompanyId;
			txtdocno.Text = ExImSaleOrderForwarder.GenerateCode(code).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomer()
	{
	}

	private void InvoiceNo()
	{
	}

	private void cmbeximinvoiceno_Leave(object sender, EventArgs e)
	{
	}

	private void LcContract()
	{
	}

	private void Shippingline()
	{
	}

	public void item()
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

	public void Currency()
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

	private void cmbcurrency_Leave(object sender, EventArgs e)
	{
	}

	private void cmbshippingline_Leave(object sender, EventArgs e)
	{
	}

	private void PortFrom()
	{
	}

	private void PortTo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				CountryId = 0
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Destination Port", false);
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

	private bool formvalidation()
	{
		if (txtnoofcontainer.Text.Trim() == "" && !(Conversion.ToDouble((object)txtnoofcontainer.Text) > 0.0))
		{
			MessageBox.Show("Please Enter No Of Containers");
			txtnoofcontainer.Focus();
			return false;
		}
		if (txtnetweight.Text.Trim() == "" && !(Conversion.ToDouble((object)txtnetweight.Text) > 0.0))
		{
			MessageBox.Show("Please Enter Net Weight");
			txtnetweight.Focus();
			return false;
		}
		return true;
	}

	private void ExImSaleOrderForwarder_Load(object sender, EventArgs e)
	{
		try
		{
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			cmbDestinationPort.Focus();
			GenerateCode();
			PortFrom();
			PortTo();
			Currency();
			SupplierCustomer();
			Shippingline();
			LcContract();
			InvoiceNo();
			item();
			CombTaxtypeFill();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grddetailadd.RowCount > 0)
			{
				ExImSaleOrderForwarder labheader = new ExImSaleOrderForwarder();
				labheader.EntryDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.ModifyDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.EntryUser = UserAccount.ID;
				labheader.OrganizationId = UserAccount.OrganizationId;
				labheader.DocumentTypeId = 153;
				labheader.CompanyId = UserAccount.CompanyId;
				labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				labheader.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
				labheader.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
				labheader.ReamarksHeader = Conversion.ToString((object)txtremarksheader.Text.Trim());
				labheader.NoofContainers = Conversion.ToInt((object)txtnoofcontainer.Text.Trim());
				labheader.NetBillWeight = Conversion.ToInt((object)txtnetweight.Text.Trim());
				labheader.ExImSaleOrderForwarderDetail = new List<ExImSaleOrderForwarderDetail>();
				GridEXRow[] rows = grddetailadd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImSaleOrderForwarderDetail labdetail = new ExImSaleOrderForwarderDetail();
					labdetail.DocDate = Conversion.ToDateTime((object)DateTime.Now);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0)
					{
						labdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["ChargesRate"].Value) != 0)
						{
							labdetail.ChargesRate = Conversion.ToDouble((object)r.Cells["ChargesRate"].Text);
							if (Conversion.ToInt(r.Cells["UomId"].Value) != 0)
							{
								labdetail.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
								if (Conversion.ToInt(r.Cells["CurrencyId"].Value) != 0)
								{
									labdetail.CurrencyId = Conversion.ToInt(r.Cells["CurrencyId"].Value);
									if (Conversion.ToInt(r.Cells["Qty"].Value) != 0)
									{
										labdetail.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
										if (Conversion.ToInt(r.Cells["Amount"].Value) != 0)
										{
											labdetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
											labdetail.Description = Conversion.ToString((object)r.Cells["Description"].Text);
											labdetail.ExhangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
											labdetail.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
											labdetail.TaxPercent = Conversion.ToSingle(r.Cells["TaxPercent"].Value);
											labdetail.TaxAmount = Conversion.ToSingle(r.Cells["TaxAmount"].Value);
											labdetail.TotalAmount = Conversion.ToSingle(r.Cells["TotalAmount"].Value);
											labheader.ExImSaleOrderForwarderDetail.Add(labdetail);
											continue;
										}
										MessageBox.Show("Amount In Grid Is Required");
										return;
									}
									MessageBox.Show("Qty In Grid Is Required");
									return;
								}
								MessageBox.Show("Currency In Grid Is Required");
								return;
							}
							MessageBox.Show("UOM In Grid Is Required");
							return;
						}
						MessageBox.Show("Charges Rate In Grid Is Required");
						return;
					}
					MessageBox.Show("Item Required In Grid Is Required");
					return;
				}
				ExImSaleOrderForwarder.Save(labheader);
				MessageBox.Show("Save Successfully");
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

	private void reset()
	{
		try
		{
			GenerateCode();
			table.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			cmbDestinationPort.Focus();
			txtremarksheader.Text = "";
			((Control)(object)cmbDestinationPort).Text = "";
			txtnoofcontainer.Text = "";
			txtnetweight.Text = "";
			((Control)(object)cmbitem).Text = "";
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
			((Control)(object)combtaxtype).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImSaleOrderForwarder po = new ExImSaleOrderForwarder();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
			po = ExImSaleOrderForwarder.GetByID(RecId);
			if (po != null)
			{
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				txtdocdate.Value = Conversion.ToDateTime((object)po.DocDate);
				txtremarksheader.Text = Conversion.ToString((object)po.ReamarksHeader);
				cmbDestinationPort.Value = Conversion.ToInt((object)po.DestinationPortId);
				txtnetweight.Text = Conversion.ToString((object)po.NetBillWeight);
				txtnoofcontainer.Text = Conversion.ToString((object)po.NoofContainers);
				table.Clear();
				for (int i = 0; i < po.ExImSaleOrderForwarderDetail.Count; i++)
				{
					table.Rows.Add(po.ExImSaleOrderForwarderDetail[i].ItemId, po.ExImSaleOrderForwarderDetail[i].ItemName, po.ExImSaleOrderForwarderDetail[i].Description, po.ExImSaleOrderForwarderDetail[i].ChargesRate, po.ExImSaleOrderForwarderDetail[i].UomId, po.ExImSaleOrderForwarderDetail[i].UomName, po.ExImSaleOrderForwarderDetail[i].CurrencyId, po.ExImSaleOrderForwarderDetail[i].CurrencyName, po.ExImSaleOrderForwarderDetail[i].ExhangeRate, po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].ChargesRate * po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].Amount, po.ExImSaleOrderForwarderDetail[i].TaxNameId, po.ExImSaleOrderForwarderDetail[i].TaxName, po.ExImSaleOrderForwarderDetail[i].TaxPercent, po.ExImSaleOrderForwarderDetail[i].TaxAmount, po.ExImSaleOrderForwarderDetail[i].TotalAmount);
				}
				grddetailadd.DataSource = table;
				grddetailadd.RetrieveStructure();
				gridsetting();
				tabControl1.SelectedIndex = 0;
				btnUpdate.Visible = true;
			}
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
			if (((Control)(object)cmbitem).Text.Trim() == "")
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
			if (((Control)(object)cmbuom).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Uom");
				cmbuom.Focus();
				return;
			}
			if (((Control)(object)cmbcurrency).Text.Trim() == "")
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
			double ChargesRate = 0.0;
			if (txtchargesrate.Text != string.Empty)
			{
				ChargesRate = Conversion.ToDouble((object)txtchargesrate.Text.Trim());
			}
			double Qty = 0.0;
			if (txtqty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			}
			table.Rows.Add(cmbitem.Value, ((Control)(object)cmbitem).Text.Trim(), txtdescription.Text.Trim(), txtchargesrate.Text.Trim(), cmbuom.Value, ((Control)(object)cmbuom).Text, cmbcurrency.Value, ((Control)(object)cmbcurrency).Text.Trim(), txtexrate.Text.Trim(), txtqty.Text.Trim(), ChargesRate * Qty, txtamount.Text.Trim(), combtaxtype.Value, ((Control)(object)combtaxtype).Text, txtTaxPercent.Text, txtTaxAmount.Text, txtTotalAmount.Text);
			grddetailadd.DataSource = table;
			grddetailadd.RetrieveStructure();
			gridsetting();
			resetdetail();
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
				((Control)(object)combtaxtype).Text = item.Cells["TaxName"].Value.ToString();
				txtTaxPercent.Text = item.Cells["TaxPercent"].Value.ToString();
				txtTaxAmount.Text = item.Cells["TaxAmount"].Value.ToString();
				txtTotalAmount.Text = item.Cells["TotalAmount"].Value.ToString();
				cmbitem.Focus();
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
			double Qty = 0.0;
			table.Rows[updateDetailIndex]["ItemId"] = cmbitem.Value;
			table.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbitem).Text;
			table.Rows[updateDetailIndex]["Description"] = txtdescription.Text;
			table.Rows[updateDetailIndex]["ChargesRate"] = txtchargesrate.Text;
			double num = Conversion.ToDouble((object)txtchargesrate.Text);
			Qty = Conversion.ToDouble((object)txtqty.Text);
			double Dollars = num * Qty;
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
			table.Rows[updateDetailIndex]["TaxAmount"] = txtTaxAmount.Text;
			table.Rows[updateDetailIndex]["TotalAmount"] = txtTotalAmount.Text;
			grddetailadd.DataSource = table;
			grddetailadd.RetrieveStructure();
			gridsetting();
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
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorysetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["InvoiceNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["NoofContainers"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["NetWeight"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["NoofContainers"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["NetWeight"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetailsetting()
	{
		try
		{
			grdhistorydetail.RootTable.Columns["ItemId"].Visible = false;
			grdhistorydetail.RootTable.Columns["UomId"].Visible = false;
			grdhistorydetail.RootTable.Columns["TaxNameId"].Visible = false;
			grdhistorydetail.RootTable.Columns["CurrencyId"].Visible = false;
			grdhistorydetail.RootTable.Columns["Description"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["ChargesRate"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Uom"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Currency"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["TotalDollar"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["TaxNameId"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["TaxPercent"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["TaxAmount"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["TotalAmount"].FilterEditType = (FilterEditType)1;
			grdhistorydetail.RootTable.Columns["EffectiveDate"].FilterEditType = (FilterEditType)5;
			grdhistorydetail.RootTable.Columns["ValidUpTo"].FilterEditType = (FilterEditType)5;
			grdhistorydetail.RootTable.Columns["Description"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ChargesRate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Currency"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["TotalDollar"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["TaxNameId"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["TaxPercent"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["EffectiveDate"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ValidUpTo"].EditType = (EditType)0;
			grdhistorydetail.RootTable.Columns["ChargesRate"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["TotalDollar"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["ChargesRate"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["Qty"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["TotalDollar"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["Amount"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["TaxAmount"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["ChargesRate"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["TotalDollar"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grddetailadd.RowCount > 0)
			{
				ExImSaleOrderForwarder labheader = new ExImSaleOrderForwarder();
				labheader.Id = Conversion.ToInt((object)RecId);
				labheader.EntryDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.ModifyDate = Conversion.ToDateTime((object)DateTime.Now);
				labheader.ModifyUser = UserAccount.ID;
				labheader.OrganizationId = UserAccount.OrganizationId;
				labheader.DocumentTypeId = 153;
				labheader.CompanyId = UserAccount.CompanyId;
				labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				labheader.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
				labheader.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
				labheader.ReamarksHeader = Conversion.ToString((object)txtremarksheader.Text.Trim());
				labheader.NoofContainers = Conversion.ToInt((object)txtnoofcontainer.Text.Trim());
				labheader.NetBillWeight = Conversion.ToInt((object)txtnetweight.Text.Trim());
				labheader.ExImSaleOrderForwarderDetail = new List<ExImSaleOrderForwarderDetail>();
				GridEXRow[] rows = grddetailadd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImSaleOrderForwarderDetail labdetail = new ExImSaleOrderForwarderDetail();
					labdetail.DocDate = Conversion.ToDateTime((object)DateTime.Now);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0)
					{
						labdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["ChargesRate"].Value) != 0)
						{
							labdetail.ChargesRate = Conversion.ToDouble((object)r.Cells["ChargesRate"].Text);
							if (Conversion.ToInt(r.Cells["UomId"].Value) != 0)
							{
								labdetail.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
								if (Conversion.ToInt(r.Cells["CurrencyId"].Value) != 0)
								{
									labdetail.CurrencyId = Conversion.ToInt(r.Cells["CurrencyId"].Value);
									if (Conversion.ToInt(r.Cells["Qty"].Value) != 0)
									{
										labdetail.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
										if (Conversion.ToInt(r.Cells["Amount"].Value) != 0)
										{
											labdetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
											labdetail.Description = Conversion.ToString((object)r.Cells["Description"].Text);
											labdetail.ExhangeRate = Conversion.ToDouble(r.Cells["ExchangeRate"].Value);
											labdetail.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
											labdetail.TaxPercent = Conversion.ToSingle(r.Cells["TaxPercent"].Value);
											labdetail.TaxAmount = Conversion.ToSingle(r.Cells["TaxAmount"].Value);
											labdetail.TotalAmount = Conversion.ToSingle(r.Cells["TotalAmount"].Value);
											labheader.ExImSaleOrderForwarderDetail.Add(labdetail);
											continue;
										}
										MessageBox.Show("Amount In Grid Is Required");
										return;
									}
									MessageBox.Show("Qty In Grid Is Required");
									return;
								}
								MessageBox.Show("Currency In Grid Is Required");
								return;
							}
							MessageBox.Show("UOM In Grid Is Required");
							return;
						}
						MessageBox.Show("Charges Rate In Grid Is Required");
						return;
					}
					MessageBox.Show("Item Required In Grid Is Required");
					return;
				}
				ExImSaleOrderForwarder.Save(labheader);
				MessageBox.Show("Update Successfully");
				reset();
				table.Clear();
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

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtgrdform = ExImSaleOrderForwarder.FormHistoryData(new ExImSaleOrderForwarder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtgrdform.Rows.Count > 0)
			{
				grdhistory.DataSource = dtgrdform;
				grdhistory.RetrieveStructure();
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
			table.Clear();
			grdtable.Clear();
			dtgrdhistory.Clear();
			reset();
			resetdetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
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
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			ExImSaleOrderForwarder po = new ExImSaleOrderForwarder();
			po = ExImSaleOrderForwarder.GetByID(Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value));
			if (po != null)
			{
				DataTable dtgrdhistorydetail = new DataTable();
				dtgrdhistorydetail.Columns.Add("ItemId");
				dtgrdhistorydetail.Columns.Add("Item");
				dtgrdhistorydetail.Columns.Add("Description");
				dtgrdhistorydetail.Columns.Add("ChargesRate", typeof(double));
				dtgrdhistorydetail.Columns.Add("UomId");
				dtgrdhistorydetail.Columns.Add("Uom");
				dtgrdhistorydetail.Columns.Add("CurrencyId");
				dtgrdhistorydetail.Columns.Add("Currency");
				dtgrdhistorydetail.Columns.Add("ExchangeRate");
				dtgrdhistorydetail.Columns.Add("Qty", typeof(double));
				dtgrdhistorydetail.Columns.Add("TotalDollar", typeof(double));
				dtgrdhistorydetail.Columns.Add("Amount", typeof(double));
				dtgrdhistorydetail.Columns.Add("TaxNameId");
				dtgrdhistorydetail.Columns.Add("TaxName");
				dtgrdhistorydetail.Columns.Add("TaxPercent");
				dtgrdhistorydetail.Columns.Add("TaxAmount", typeof(double));
				dtgrdhistorydetail.Columns.Add("TotalAmount", typeof(double));
				dtgrdhistorydetail.Columns.Add("EffectiveDate");
				dtgrdhistorydetail.Columns.Add("ValidUpTo");
				dtgrdhistorydetail.Clear();
				for (int i = 0; i < po.ExImSaleOrderForwarderDetail.Count; i++)
				{
					dtgrdhistorydetail.Rows.Add(po.ExImSaleOrderForwarderDetail[i].ItemId, po.ExImSaleOrderForwarderDetail[i].ItemName, po.ExImSaleOrderForwarderDetail[i].Description, po.ExImSaleOrderForwarderDetail[i].ChargesRate, po.ExImSaleOrderForwarderDetail[i].UomId, po.ExImSaleOrderForwarderDetail[i].UomName, po.ExImSaleOrderForwarderDetail[i].CurrencyId, po.ExImSaleOrderForwarderDetail[i].CurrencyName, po.ExImSaleOrderForwarderDetail[i].ExhangeRate, po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].ChargesRate * po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].Amount, po.ExImSaleOrderForwarderDetail[i].TaxNameId, po.ExImSaleOrderForwarderDetail[i].TaxName, po.ExImSaleOrderForwarderDetail[i].TaxPercent, po.ExImSaleOrderForwarderDetail[i].TaxAmount, po.ExImSaleOrderForwarderDetail[i].TotalAmount, po.ExImSaleOrderForwarderDetail[i].EffectedDateD.ToString("dd-MMM-yyyy"), po.ExImSaleOrderForwarderDetail[i].ValidUpTo.ToString("dd-MMM-yyyy"));
				}
				grdhistorydetail.DataSource = dtgrdhistorydetail;
				grdhistorydetail.RetrieveStructure();
				gridhistorydetailsetting();
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
				DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
				txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
				double taxamount = Conversion.ToDouble((object)txtamount.Text) * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0;
				txtTaxAmount.Text = taxamount.ToString();
			}
			totalamount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImSaleOrderForwarder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryFill();
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
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		GridEXLayout grddetailadd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistorydetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
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
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdhistorydetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.combtaxtype = new UltraCombo();
		this.cmbDestinationPort = new UltraCombo();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbcurrency = new UltraCombo();
		this.txtchargesrate = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbuom = new UltraCombo();
		this.txtexrate = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtnetweight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtnoofcontainer = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtremarksheader = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		base.SuspendLayout();
		this.btn.BackColor = System.Drawing.Color.Azure;
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.btnprint, this.btnattachment });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1356, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(168, 24);
		this.toolStripLabel1.Text = "Pre Shipment Costing";
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
		((System.Windows.Forms.Control)(object)this.grddetailadd).Size = new System.Drawing.Size(1356, 307);
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
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1356, 590);
		this.panel4.TabIndex = 1;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistorydetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 346);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1356, 244);
		this.panel3.TabIndex = 5;
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
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Size = new System.Drawing.Size(1356, 244);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).TabIndex = 3;
		this.grdhistorydetail.TotalRow = (InheritableBoolean)1;
		this.grdhistorydetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistorydetail).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1356, 346);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1356, 346);
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
		this.tabForm.Controls.Add(this.ctrlGrdBar2);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1362, 596);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailadd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 286);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1356, 307);
		this.panel1.TabIndex = 38;
		this.panel7.BackColor = System.Drawing.Color.Azure;
		this.panel7.Controls.Add(this.label25);
		this.panel7.Controls.Add(this.label24);
		this.panel7.Controls.Add(this.txtTotalAmount);
		this.panel7.Controls.Add(this.txtTaxAmount);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Controls.Add(this.txtTaxPercent);
		this.panel7.Controls.Add(this.label22);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.combtaxtype);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel7.Controls.Add(this.txtamount);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.panel7.Controls.Add(this.txtchargesrate);
		this.panel7.Controls.Add(this.label17);
		this.panel7.Controls.Add(this.label19);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.panel7.Controls.Add(this.txtexrate);
		this.panel7.Controls.Add(this.label21);
		this.panel7.Controls.Add(this.txtdescription);
		this.panel7.Controls.Add(this.label18);
		this.panel7.Controls.Add(this.txtqty);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.btnCancelUpdateDetial);
		this.panel7.Controls.Add(this.btnUpdateDetail);
		this.panel7.Controls.Add(this.btnplus);
		this.panel7.Controls.Add(this.label20);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.txtnetweight);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.txtnoofcontainer);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.txtremarksheader);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1356, 256);
		this.panel7.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(331, 202);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(79, 15);
		this.label25.TabIndex = 96;
		this.label25.Text = "Total Amount";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(233, 202);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(72, 15);
		this.label24.TabIndex = 96;
		this.label24.Text = "Tax Amount";
		this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(333, 223);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(114, 25);
		this.txtTotalAmount.TabIndex = 22;
		this.txtTaxAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(236, 223);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(95, 25);
		this.txtTaxAmount.TabIndex = 21;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(154, 203);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(38, 15);
		this.label23.TabIndex = 94;
		this.label23.Text = "Tax%";
		this.txtTaxPercent.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(157, 223);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(76, 25);
		this.txtTaxPercent.TabIndex = 20;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(16, 203);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(56, 15);
		this.label22.TabIndex = 92;
		this.label22.Text = "Tax Type";
		this.combtaxtype.AllowNull = (DefaultableBoolean)1;
		this.combtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Location = new System.Drawing.Point(19, 222);
		((System.Windows.Forms.Control)(object)this.combtaxtype).Name = "combtaxtype";
		((System.Windows.Forms.Control)(object)this.combtaxtype).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.combtaxtype).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.combtaxtype).Leave += new System.EventHandler(combtaxtype_Leave);
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(144, 42);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(209, 27);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 8;
		this.txtamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(942, 165);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(96, 25);
		this.txtamount.TabIndex = 18;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(940, 145);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 17);
		this.label2.TabIndex = 90;
		this.label2.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(621, 145);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 17);
		this.label3.TabIndex = 89;
		this.label3.Text = "Currency";
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(621, 164);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Leave += new System.EventHandler(cmbcurrency_Leave);
		this.txtchargesrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtchargesrate.Location = new System.Drawing.Point(399, 165);
		this.txtchargesrate.Name = "txtchargesrate";
		this.txtchargesrate.Size = new System.Drawing.Size(95, 25);
		this.txtchargesrate.TabIndex = 13;
		this.txtchargesrate.TextChanged += new System.EventHandler(txtchargesrate_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(396, 145);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(86, 17);
		this.label17.TabIndex = 86;
		this.label17.Text = "Charges Rate";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(501, 145);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(39, 17);
		this.label19.TabIndex = 85;
		this.label19.Text = "UOM";
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(498, 164);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.cmbuom).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.txtexrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtexrate.Location = new System.Drawing.Point(760, 165);
		this.txtexrate.Name = "txtexrate";
		this.txtexrate.Size = new System.Drawing.Size(96, 25);
		this.txtexrate.TabIndex = 16;
		this.txtexrate.TextChanged += new System.EventHandler(txtexrate_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label21.Location = new System.Drawing.Point(760, 145);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(51, 17);
		this.label21.TabIndex = 84;
		this.label21.Text = "Ex Rate";
		this.txtdescription.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdescription.Location = new System.Drawing.Point(259, 165);
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(135, 25);
		this.txtdescription.TabIndex = 12;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label18.Location = new System.Drawing.Point(256, 138);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(74, 17);
		this.label18.TabIndex = 76;
		this.label18.Text = "Description";
		this.txtqty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(862, 165);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(74, 25);
		this.txtqty.TabIndex = 17;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(860, 145);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(31, 17);
		this.label1.TabIndex = 72;
		this.label1.Text = "QTY";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(562, 223);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(55, 26);
		this.btnCancelUpdateDetial.TabIndex = 27;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(509, 223);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 26);
		this.btnUpdateDetail.TabIndex = 26;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(453, 223);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(53, 26);
		this.btnplus.TabIndex = 25;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label20.Location = new System.Drawing.Point(16, 139);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(33, 17);
		this.label20.TabIndex = 66;
		this.label20.Text = "Item";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(19, 164);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label16.Location = new System.Drawing.Point(706, 46);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(73, 17);
		this.label16.TabIndex = 57;
		this.label16.Text = "Net Weight";
		this.txtnetweight.Location = new System.Drawing.Point(829, 42);
		this.txtnetweight.Name = "txtnetweight";
		this.txtnetweight.Size = new System.Drawing.Size(209, 25);
		this.txtnetweight.TabIndex = 10;
		this.txtnetweight.Text = "0";
		this.txtnetweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label15.Location = new System.Drawing.Point(378, 46);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(108, 17);
		this.label15.TabIndex = 55;
		this.label15.Text = "No of Containers";
		this.txtnoofcontainer.Location = new System.Drawing.Point(487, 44);
		this.txtnoofcontainer.Name = "txtnoofcontainer";
		this.txtnoofcontainer.Size = new System.Drawing.Size(209, 25);
		this.txtnoofcontainer.TabIndex = 9;
		this.txtnoofcontainer.Text = "0";
		this.txtnoofcontainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(20, 45);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(101, 17);
		this.label14.TabIndex = 53;
		this.label14.Text = "Destination Port";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(16, 76);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 17);
		this.label13.TabIndex = 51;
		this.label13.Text = "Remarks";
		this.txtremarksheader.Location = new System.Drawing.Point(144, 73);
		this.txtremarksheader.Name = "txtremarksheader";
		this.txtremarksheader.Size = new System.Drawing.Size(552, 25);
		this.txtremarksheader.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(376, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 17);
		this.label6.TabIndex = 38;
		this.label6.Text = "Doc Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(487, 16);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(209, 25);
		this.txtdocdate.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(22, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(53, 17);
		this.label5.TabIndex = 37;
		this.label5.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.Enabled = false;
		this.txtdocno.Location = new System.Drawing.Point(144, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(209, 25);
		this.txtdocno.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1308, 4);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grddetailadd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar2.TabIndex = 55;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 622);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "ExImPreshipmentCosting";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExImSaleOrderForwarder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExImSaleOrderForwarder_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).EndInit();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		base.ResumeLayout(false);
	}
}

}
