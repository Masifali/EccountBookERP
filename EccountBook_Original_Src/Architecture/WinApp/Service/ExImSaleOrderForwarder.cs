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

namespace Architecture.WinApp.Service;

public class ExImSaleOrderForwarder : Form
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

	internal GridEX grdhistory;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Label label11;

	internal UltraCombo CmbBillToParty;

	private Label label9;

	internal UltraCombo CmbContractNo;

	private Label label8;

	internal UltraCombo CmbInvoiceNo;

	private Label label7;

	internal UltraCombo CmbForwarder;

	private Label label16;

	private TextBox txtNetWeight;

	private Label label15;

	private TextBox txtNoOfContainers;

	private Label label14;

	private Label label13;

	private TextBox txtremarksheader;

	private Label label12;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button BtnAdd;

	private Label label20;

	internal UltraCombo CmbItemName;

	private TextBox txtQty;

	private Label label1;

	private TextBox txtDescription;

	private Label label18;

	internal GridEX grdhistorydetail;

	private TextBox txtAmount;

	private Label label2;

	private Label label3;

	internal UltraCombo CmbCurrency;

	private TextBox txtChargesRate;

	private Label label17;

	private Label label19;

	internal UltraCombo CmbUom;

	private TextBox txtExchangeRate;

	private Label label21;

	private Label label4;

	private DateTimePicker txtValidUpto;

	private Label label10;

	private DateTimePicker txtEffectiveDate;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	internal UltraCombo CmbTaxType;

	private Label label24;

	private TextBox txtTaxAmount;

	private Label label23;

	private TextBox txtTaxPercent;

	private Label label22;

	private Label label25;

	private TextBox txtTotalAmount;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel5;

	private Label label26;

	private CheckBox ChkPrint;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnShortcutkeys;

	private Panel panel6;

	private Label label5;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private Button btnShow;

	private Label label6;

	private Label label27;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label label31;

	internal UltraCombo CmbInvoiceNoHistory;

	internal UltraCombo CmbBillToPartyHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label32;

	private Panel panel12;

	private Label label33;

	private Panel panel11;

	public ExImSaleOrderForwarder(UserAccount pUserAccount)
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
				DDL.BindDDLNew(dtsuppliercustomer, CmbForwarder, "Id", "CompanyName", "Service Provider", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNo()
	{
		try
		{
			DataTable dtinvoice = CommonServices.GetExportInvoiceNo();
			if (dtinvoice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtinvoice, CmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbeximinvoiceno_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dtinvoice = ExImInvoice.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Ids = "204,209",
				Id = Conversion.ToInt(CmbInvoiceNo.Value)
			});
			if (dtinvoice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtinvoice, CmbContractNo, "LcOrderNoId", "LcOrderNo", "LcOrder No", false);
				DDL.BindDDLNew(dtinvoice, CmbLoadingPort, "LoadingPortId", "LoadingPort", "Loading Port", false);
				DDL.BindDDLNew(dtinvoice, CmbDestinationPort, "DestinationPortId", "DestinationPort", "Destination Port", false);
				txtNoOfContainers.Text = dtinvoice.Rows[0]["NoOfContainers"].ToString();
				txtNetWeight.Text = dtinvoice.Rows[0]["NetWeight"].ToString();
				((UltraGridBase)CmbContractNo).Rows[0].Activate();
				((UltraGridBase)CmbLoadingPort).Rows[0].Activate();
				((UltraGridBase)CmbDestinationPort).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LcContract()
	{
	}

	private void Shippingline()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsupcust = ItemPricingSupplierCustomer.GetSupplierCustomerFromItemPricingSupplierCustomer(new ItemPricingSupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsupcust.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupcust, CmbBillToParty, "SupplierCustomerId", "CompanyName", "Shippingline", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
				DDL.BindDDLNew(dtitem, CmbItemName, "Id", "ItemName", "Item", false);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
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
			if (((Control)(object)CmbItemName).Text != null)
			{
				DataTable dtCOMMUOM = UOMSchedule.SearchByObject(new UOMSchedule
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					ItemId = Conversion.ToInt(CmbItemName.Value)
				});
				if (dtCOMMUOM.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtCOMMUOM, CmbUom, "Id", "Equivalent", "Rate UOM", false);
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
				DDL.BindDDLNew(curr, CmbCurrency, "Id", "CurrencyCode", "FcyCode", false);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			if (((Control)(object)CmbBillToParty).Text != null)
			{
				DataTable dtdetail = ItemPricingSupplierCustomer.ReadDetailByOrganizationCompanyIdNSupplierId(new ItemPricingSupplierCustomer
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbBillToParty.Value)
				});
				table.Clear();
				for (int i = 0; i < dtdetail.Rows.Count; i++)
				{
					table.Rows.Add(dtdetail.Rows[i]["ItemId"], dtdetail.Rows[i]["ItemName"], string.Empty, dtdetail.Rows[i]["ChargesRate"], dtdetail.Rows[i]["UomId"], dtdetail.Rows[i]["UomName"], dtdetail.Rows[i]["CurrencyId"], dtdetail.Rows[i]["CurrencyName"], dtdetail.Rows[i]["ExhangeRate"], 0, 0, dtdetail.Rows[i]["Amount"], 0, "", 0, 0, 0, Convert.ToDateTime(dtdetail.Rows[i]["EffectedDateD"]).ToString("dd-MMM-yyyy"), Convert.ToDateTime(dtdetail.Rows[i]["ValidUpTo"]).ToString("dd-MMM-yyyy"));
				}
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

	private void PortFrom()
	{
	}

	private void PortTo()
	{
	}

	private void CombTaxtypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtTax = TaxesTypes.GetForComboBind(new TaxesTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtTax.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTax, CmbTaxType, "Id", "TaxName", "Tax Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)CmbForwarder).Text == "")
		{
			MessageBox.Show("Please Select Forwarder");
			CmbForwarder.Focus();
			return false;
		}
		if (((Control)(object)CmbBillToParty).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Shipping Line");
			CmbBillToParty.Focus();
			return false;
		}
		if (txtNoOfContainers.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter No Of Containers");
			txtNoOfContainers.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Net Weight");
			txtNetWeight.Focus();
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
			CmbBillToParty.Focus();
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
			table.Columns.Add("EffectiveDate");
			table.Columns.Add("ValidUpTo");
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
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
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
				labheader.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
				labheader.InvoiceNo = Conversion.ToString((object)((Control)(object)CmbInvoiceNo).Text.Trim());
				labheader.SupplierCustomerId = Conversion.ToInt(CmbBillToParty.Value);
				labheader.ShippingLineId = Conversion.ToInt(CmbForwarder.Value);
				labheader.LcContractId = Conversion.ToInt(CmbContractNo.Value);
				labheader.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
				labheader.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
				labheader.ReamarksHeader = Conversion.ToString((object)txtremarksheader.Text.Trim());
				labheader.NoofContainers = Conversion.ToInt((object)txtNoOfContainers.Text.Trim());
				labheader.NetBillWeight = Conversion.ToInt((object)txtNetWeight.Text.Trim());
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
											labdetail.EffectedDateD = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
											labdetail.ValidUpTo = Conversion.ToDateTime(r.Cells["ValidUpTo"].Value);
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
			CmbBillToParty.Focus();
			((Control)(object)CmbForwarder).Text = "";
			((Control)(object)CmbInvoiceNo).Text = "";
			((Control)(object)CmbContractNo).Text = "";
			((Control)(object)CmbBillToParty).Text = "";
			txtremarksheader.Text = "";
			((Control)(object)CmbLoadingPort).Text = "";
			((Control)(object)CmbDestinationPort).Text = "";
			txtNoOfContainers.Text = "";
			txtNetWeight.Text = "";
			((Control)(object)CmbItemName).Text = "";
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
			txtDescription.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtChargesRate.Text = string.Empty;
			((Control)(object)CmbUom).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			((Control)(object)CmbCurrency).Text = string.Empty;
			((Control)(object)CmbUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
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
				CmbBillToParty.Value = Conversion.ToInt((object)po.SupplierCustomerId);
				CmbForwarder.Value = Conversion.ToInt((object)po.ShippingLineId);
				txtremarksheader.Text = Conversion.ToString((object)po.ReamarksHeader);
				CmbInvoiceNo.Value = Conversion.ToInt((object)po.InvoiceId);
				table.Clear();
				for (int i = 0; i < po.ExImSaleOrderForwarderDetail.Count; i++)
				{
					table.Rows.Add(po.ExImSaleOrderForwarderDetail[i].ItemId, po.ExImSaleOrderForwarderDetail[i].ItemName, po.ExImSaleOrderForwarderDetail[i].Description, po.ExImSaleOrderForwarderDetail[i].ChargesRate, po.ExImSaleOrderForwarderDetail[i].UomId, po.ExImSaleOrderForwarderDetail[i].UomName, po.ExImSaleOrderForwarderDetail[i].CurrencyId, po.ExImSaleOrderForwarderDetail[i].CurrencyName, po.ExImSaleOrderForwarderDetail[i].ExhangeRate, po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].ChargesRate * po.ExImSaleOrderForwarderDetail[i].Qty, po.ExImSaleOrderForwarderDetail[i].Amount, po.ExImSaleOrderForwarderDetail[i].TaxNameId, po.ExImSaleOrderForwarderDetail[i].TaxName, po.ExImSaleOrderForwarderDetail[i].TaxPercent, po.ExImSaleOrderForwarderDetail[i].TaxAmount, po.ExImSaleOrderForwarderDetail[i].TotalAmount, po.ExImSaleOrderForwarderDetail[i].EffectedDateD.ToString("dd-MMM-yyyy"), po.ExImSaleOrderForwarderDetail[i].ValidUpTo.ToString("dd-MMM-yyyy"));
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
			if (((Control)(object)CmbItemName).Text.Trim() == "")
			{
				MessageBox.Show("Please Select item");
				CmbItemName.Focus();
				return;
			}
			if (txtChargesRate.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter ChargesRate");
				txtChargesRate.Focus();
				return;
			}
			if (((Control)(object)CmbUom).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Uom");
				CmbUom.Focus();
				return;
			}
			if (((Control)(object)CmbCurrency).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Currency");
				CmbCurrency.Focus();
				return;
			}
			if (txtQty.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Qty");
				txtQty.Focus();
				return;
			}
			if (txtAmount.Text.Trim() == "")
			{
				MessageBox.Show("Please Check Amount");
				txtAmount.Focus();
				return;
			}
			double ChargesRate = 0.0;
			if (txtChargesRate.Text != string.Empty)
			{
				ChargesRate = Conversion.ToDouble((object)txtChargesRate.Text.Trim());
			}
			double Qty = 0.0;
			if (txtQty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim());
			}
			table.Rows.Add(CmbItemName.Value, ((Control)(object)CmbItemName).Text.Trim(), txtDescription.Text.Trim(), txtChargesRate.Text.Trim(), CmbUom.Value, ((Control)(object)CmbUom).Text, CmbCurrency.Value, ((Control)(object)CmbCurrency).Text.Trim(), txtExchangeRate.Text.Trim(), txtQty.Text.Trim(), ChargesRate * Qty, txtAmount.Text.Trim(), CmbTaxType.Value, ((Control)(object)CmbTaxType).Text, txtTaxPercent.Text, txtTaxAmount.Text, txtTotalAmount.Text, txtEffectiveDate.Value.ToString("dd-MMM-yyyy"), txtValidUpto.Value.ToString("dd-MMM-yyyy"));
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
				txtDescription.Text = item.Cells["Description"].Value.ToString();
				txtQty.Text = item.Cells["Qty"].Value.ToString();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				txtChargesRate.Text = item.Cells["ChargesRate"].Value.ToString();
				CmbUom.Value = item.Cells["UomId"].Value.ToString();
				CmbCurrency.Value = item.Cells["CurrencyId"].Value.ToString();
				txtExchangeRate.Text = item.Cells["ExchangeRate"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				CmbTaxType.Value = item.Cells["TaxNameId"].Value.ToString();
				((Control)(object)CmbTaxType).Text = item.Cells["TaxName"].Value.ToString();
				txtTaxPercent.Text = item.Cells["TaxPercent"].Value.ToString();
				txtTaxAmount.Text = item.Cells["TaxAmount"].Value.ToString();
				txtTotalAmount.Text = item.Cells["TotalAmount"].Value.ToString();
				txtEffectiveDate.Text = item.Cells["EffectiveDate"].Value.ToString();
				txtValidUpto.Text = item.Cells["ValidUpTo"].Value.ToString();
				CmbItemName.Focus();
				BtnAdd.Visible = false;
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
			table.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
			table.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemName).Text;
			table.Rows[updateDetailIndex]["Description"] = txtDescription.Text;
			table.Rows[updateDetailIndex]["ChargesRate"] = txtChargesRate.Text;
			double num = Conversion.ToDouble((object)txtChargesRate.Text);
			Qty = Conversion.ToDouble((object)txtQty.Text);
			double Dollars = num * Qty;
			table.Rows[updateDetailIndex]["UomId"] = CmbUom.Value;
			table.Rows[updateDetailIndex]["Uom"] = ((Control)(object)CmbUom).Text;
			table.Rows[updateDetailIndex]["Currency"] = ((Control)(object)CmbCurrency).Text;
			table.Rows[updateDetailIndex]["ExchangeRate"] = txtExchangeRate.Text;
			table.Rows[updateDetailIndex]["Qty"] = txtQty.Text;
			table.Rows[updateDetailIndex]["TotalDollar"] = Dollars;
			table.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
			table.Rows[updateDetailIndex]["TaxNameId"] = CmbTaxType.Value;
			table.Rows[updateDetailIndex]["TaxName"] = ((Control)(object)CmbTaxType).Text;
			table.Rows[updateDetailIndex]["TaxPercent"] = txtTaxPercent.Text;
			table.Rows[updateDetailIndex]["TaxAmount"] = txtTaxAmount.Text;
			table.Rows[updateDetailIndex]["TotalAmount"] = txtTotalAmount.Text;
			table.Rows[updateDetailIndex]["EffectiveDate"] = txtEffectiveDate.Value.ToString("dd-MMM-yyyy");
			table.Rows[updateDetailIndex]["ValidUpTo"] = txtValidUpto.Value.ToString("dd-MMM-yyyy");
			grddetailadd.DataSource = table;
			grddetailadd.RetrieveStructure();
			gridsetting();
			resetdetail();
			BtnAdd.Visible = true;
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
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
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
				labheader.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
				labheader.InvoiceNo = Conversion.ToString((object)((Control)(object)CmbInvoiceNo).Text.Trim());
				labheader.SupplierCustomerId = Conversion.ToInt(CmbBillToParty.Value);
				labheader.ShippingLineId = Conversion.ToInt(CmbForwarder.Value);
				labheader.LcContractId = Conversion.ToInt(CmbContractNo.Value);
				labheader.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
				labheader.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
				labheader.ReamarksHeader = Conversion.ToString((object)txtremarksheader.Text.Trim());
				labheader.NoofContainers = Conversion.ToInt((object)txtNoOfContainers.Text.Trim());
				labheader.NetBillWeight = Conversion.ToInt((object)txtNetWeight.Text.Trim());
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
											labdetail.EffectedDateD = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
											labdetail.ValidUpTo = Conversion.ToDateTime(r.Cells["ValidUpTo"].Value);
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
			if (!(txtExchangeRate.Text.Trim() != string.Empty) || !(txtChargesRate.Text.Trim() != string.Empty) || !(((Control)(object)CmbUom).Text.Trim() != "0") || !(txtQty.Text.Trim() != string.Empty))
			{
				return;
			}
			exchagerate = Conversion.ToDouble((object)txtExchangeRate.Text.Trim());
			chargesrate = Conversion.ToDouble((object)txtChargesRate.Text.Trim());
			equivalent = Conversion.ToInt((object)((Control)(object)CmbUom).Text.Trim());
			qty = Conversion.ToDouble((object)txtQty.Text);
			TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
			if (exchagerate == 0.0)
			{
				TotalAmount = chargesrate * (double)equivalent * qty;
				txtAmount.Text = TotalAmount.ToString();
				if (((Control)(object)CmbTaxType).Text.ToString() != string.Empty)
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
				txtAmount.Text = TotalAmount.ToString();
				if (((Control)(object)CmbTaxType).Text.ToString() != string.Empty)
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
			taxSchedule.TaxNameId = Conversion.ToInt(CmbTaxType.Value);
			if (TaxScheduleMain.ReadTaxSchedule(taxSchedule) == null)
			{
				txtTaxPercent.Text = "0";
			}
			else
			{
				DataTable detail = TaxScheduleMain.ReadTaxSchedule(taxSchedule);
				txtTaxPercent.Text = detail.Rows[0]["TaxPercent"].ToString();
				double taxamount = Conversion.ToDouble((object)txtAmount.Text) * Conversion.ToDouble((object)txtTaxPercent.Text) / 100.0;
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Expected O, but got Unknown
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.ExImSaleOrderForwarder));
		GridEXLayout grddetailadd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistorydetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
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
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
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
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.grddetailadd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdhistorydetail = new GridEX();
		this.grdhistory = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.BtnAdd = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.txtChargesRate = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbTaxType = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbUom = new UltraCombo();
		this.CmbCurrency = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtValidUpto = new System.Windows.Forms.DateTimePicker();
		this.txtEffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbDestinationPort = new UltraCombo();
		this.CmbLoadingPort = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtNoOfContainers = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtremarksheader = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbBillToParty = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbContractNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbForwarder = new UltraCombo();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.CmbBillToPartyHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label32 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).BeginInit();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrency).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbForwarder).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyHistory).BeginInit();
		this.toolStrip2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		base.SuspendLayout();
		this.btn.BackColor = System.Drawing.Color.White;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.btnattachment, this.btnprint, this.btnShortcutkeys });
		this.btn.Location = new System.Drawing.Point(0, 0);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(920, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
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
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(61, 24);
		this.btnprint.Text = "&Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnShortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeys.Name = "btnShortcutkeys";
		this.btnShortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutkeys.Text = "ShortCut Keys";
		this.grddetailadd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grddetailadd).BackColor = System.Drawing.Color.White;
		grddetailadd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailadd.DesignTimeLayout = grddetailadd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailadd.FilterMode = (FilterMode)2;
		this.grddetailadd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailadd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Location = new System.Drawing.Point(3, 289);
		((System.Windows.Forms.Control)(object)this.grddetailadd).Name = "grddetailadd";
		((ScrollableBase)this.grddetailadd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailadd).Size = new System.Drawing.Size(920, 240);
		((System.Windows.Forms.Control)(object)this.grddetailadd).TabIndex = 0;
		this.grddetailadd.TotalRow = (InheritableBoolean)1;
		this.grddetailadd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailadd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grddetailadd).DoubleClick += new System.EventHandler(grddetailadd_DoubleClick);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabHistory.Controls.Add(this.panel9);
		this.tabHistory.Controls.Add(this.panel11);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Controls.Add(this.panel12);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistorydetail);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(926, 532);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdhistorydetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistorydetail).BackColor = System.Drawing.Color.White;
		grdhistorydetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistorydetail.DesignTimeLayout = grdhistorydetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistorydetail.FilterMode = (FilterMode)2;
		this.grdhistorydetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistorydetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Location = new System.Drawing.Point(3, 379);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Name = "grdhistorydetail";
		((ScrollableBase)this.grdhistorydetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Size = new System.Drawing.Size(920, 150);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).TabIndex = 3;
		this.grdhistorydetail.TotalRow = (InheritableBoolean)1;
		this.grdhistorydetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistorydetail).VisualStyle = (VisualStyle)4;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(3, 160);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(920, 193);
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
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(934, 561);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailadd);
		this.tabForm.Controls.Add(this.panel6);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(926, 532);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 265);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(920, 24);
		this.panel6.TabIndex = 57;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(877, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grddetailadd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 55;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel1.Controls.Add(this.BtnAdd);
		this.panel1.Controls.Add(this.label25);
		this.panel1.Controls.Add(this.txtChargesRate);
		this.panel1.Controls.Add(this.label24);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel1.Controls.Add(this.txtTotalAmount);
		this.panel1.Controls.Add(this.label20);
		this.panel1.Controls.Add(this.txtTaxAmount);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Controls.Add(this.btnUpdateDetail);
		this.panel1.Controls.Add(this.txtTaxPercent);
		this.panel1.Controls.Add(this.btnCancelUpdateDetial);
		this.panel1.Controls.Add(this.label22);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.panel1.Controls.Add(this.txtQty);
		this.panel1.Controls.Add(this.label18);
		this.panel1.Controls.Add(this.txtDescription);
		this.panel1.Controls.Add(this.txtAmount);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.txtExchangeRate);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUom);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrency);
		this.panel1.Controls.Add(this.label19);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label17);
		this.panel1.Controls.Add(this.txtValidUpto);
		this.panel1.Controls.Add(this.txtEffectiveDate);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 171);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(920, 94);
		this.panel1.TabIndex = 38;
		this.BtnAdd.BackColor = System.Drawing.Color.Teal;
		this.BtnAdd.FlatAppearance.BorderSize = 0;
		this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnAdd.ForeColor = System.Drawing.Color.White;
		this.BtnAdd.Location = new System.Drawing.Point(740, 67);
		this.BtnAdd.Name = "BtnAdd";
		this.BtnAdd.Size = new System.Drawing.Size(40, 23);
		this.BtnAdd.TabIndex = 25;
		this.BtnAdd.Text = "+";
		this.BtnAdd.UseVisualStyleBackColor = false;
		this.BtnAdd.Click += new System.EventHandler(btnplus_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(378, 50);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(80, 15);
		this.label25.TabIndex = 96;
		this.label25.Text = "Total Amount";
		this.txtChargesRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtChargesRate.Location = new System.Drawing.Point(426, 23);
		this.txtChargesRate.Name = "txtChargesRate";
		this.txtChargesRate.Size = new System.Drawing.Size(80, 23);
		this.txtChargesRate.TabIndex = 13;
		this.txtChargesRate.TextChanged += new System.EventHandler(txtchargesrate_TextChanged);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(294, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(72, 15);
		this.label24.TabIndex = 96;
		this.label24.Text = "Tax Amount";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance240;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(4, 21);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(cmbitem_Leave);
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(378, 67);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(105, 23);
		this.txtTotalAmount.TabIndex = 22;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(1, 5);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(32, 15);
		this.label20.TabIndex = 66;
		this.label20.Text = "Item";
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(294, 67);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(83, 23);
		this.txtTaxAmount.TabIndex = 21;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(222, 50);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(35, 15);
		this.label23.TabIndex = 94;
		this.label23.Text = "Tax%";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(740, 67);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 23);
		this.btnUpdateDetail.TabIndex = 26;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(222, 67);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(70, 23);
		this.txtTaxPercent.TabIndex = 20;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(812, 67);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 23);
		this.btnCancelUpdateDetial.TabIndex = 27;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(102, 50);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(53, 15);
		this.label22.TabIndex = 92;
		this.label22.Text = "Tax Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(808, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(29, 15);
		this.label1.TabIndex = 72;
		this.label1.Text = "QTY";
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(105, 65);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 19;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(combtaxtype_Leave);
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(808, 23);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(80, 23);
		this.txtQty.TabIndex = 17;
		this.txtQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(233, 5);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(68, 15);
		this.label18.TabIndex = 76;
		this.label18.Text = "Description";
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDescription.Location = new System.Drawing.Point(236, 23);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(189, 23);
		this.txtDescription.TabIndex = 12;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(4, 67);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(100, 23);
		this.txtAmount.TabIndex = 18;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(737, 5);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(45, 15);
		this.label21.TabIndex = 84;
		this.label21.Text = "Ex Rate";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(4, 50);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 90;
		this.label2.Text = "Amount";
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(737, 23);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(70, 23);
		this.txtExchangeRate.TabIndex = 16;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtexrate_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(610, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 15);
		this.label3.TabIndex = 89;
		this.label3.Text = "Currency";
		this.CmbUom.AllowNull = (DefaultableBoolean)1;
		this.CmbUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUom).Location = new System.Drawing.Point(508, 21);
		((System.Windows.Forms.Control)(object)this.CmbUom).Name = "CmbUom";
		((System.Windows.Forms.Control)(object)this.CmbUom).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbUom).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbUom).TextChanged += new System.EventHandler(cmbuom_TextChanged);
		this.CmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrency).Location = new System.Drawing.Point(610, 21);
		((System.Windows.Forms.Control)(object)this.CmbCurrency).Name = "CmbCurrency";
		((System.Windows.Forms.Control)(object)this.CmbCurrency).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrency).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbCurrency).Leave += new System.EventHandler(cmbcurrency_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(508, 5);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 85;
		this.label19.Text = "UOM";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(610, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 15);
		this.label4.TabIndex = 88;
		this.label4.Text = "Valid Up To";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(426, 5);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(75, 15);
		this.label17.TabIndex = 86;
		this.label17.Text = "Charges Rate";
		this.txtValidUpto.CustomFormat = "dd-MMM-yy";
		this.txtValidUpto.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtValidUpto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtValidUpto.Location = new System.Drawing.Point(610, 67);
		this.txtValidUpto.Name = "txtValidUpto";
		this.txtValidUpto.Size = new System.Drawing.Size(125, 22);
		this.txtValidUpto.TabIndex = 24;
		this.txtEffectiveDate.CustomFormat = "dd-MMM-yy";
		this.txtEffectiveDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectiveDate.Location = new System.Drawing.Point(484, 67);
		this.txtEffectiveDate.Name = "txtEffectiveDate";
		this.txtEffectiveDate.Size = new System.Drawing.Size(125, 22);
		this.txtEffectiveDate.TabIndex = 23;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(484, 50);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 87;
		this.label10.Text = "Effective Date";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbForwarder);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.txtNetWeight);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.txtNoOfContainers);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.txtremarksheader);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToParty);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 54);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(920, 117);
		this.panel7.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 58;
		this.label5.Text = "Doc No && Data";
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance135).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(416, 60);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		this.CmbDestinationPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 8;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(717, 4);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		this.CmbLoadingPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 7;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(620, 66);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(68, 15);
		this.label16.TabIndex = 57;
		this.label16.Text = "Net Weight";
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(717, 62);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(200, 23);
		this.txtNetWeight.TabIndex = 10;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(620, 38);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(96, 15);
		this.label15.TabIndex = 55;
		this.label15.Text = "No of Containers";
		this.txtNoOfContainers.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfContainers.Location = new System.Drawing.Point(717, 34);
		this.txtNoOfContainers.Name = "txtNoOfContainers";
		this.txtNoOfContainers.ReadOnly = true;
		this.txtNoOfContainers.Size = new System.Drawing.Size(200, 23);
		this.txtNoOfContainers.TabIndex = 9;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(322, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(93, 15);
		this.label14.TabIndex = 53;
		this.label14.Text = "Destination Port";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(5, 92);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 51;
		this.label13.Text = "Remarks";
		this.txtremarksheader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksheader.Location = new System.Drawing.Point(107, 88);
		this.txtremarksheader.Name = "txtremarksheader";
		this.txtremarksheader.Size = new System.Drawing.Size(810, 23);
		this.txtremarksheader.TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(620, 10);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(75, 15);
		this.label12.TabIndex = 49;
		this.label12.Text = "Loading Port";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 38);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(77, 15);
		this.label11.TabIndex = 47;
		this.label11.Text = "Bill To / Party";
		this.CmbBillToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBillToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Location = new System.Drawing.Point(107, 32);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Name = "CmbBillToParty";
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Leave += new System.EventHandler(cmbshippingline_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(322, 38);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 45;
		this.label9.Text = "Lc / Contract";
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(416, 32);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		this.CmbContractNo.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(322, 10);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 15);
		this.label8.TabIndex = 43;
		this.label8.Text = "Invoice No";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance218).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(416, 4);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 3;
		this.CmbInvoiceNo.ValueChanged += new System.EventHandler(cmbeximinvoiceno_Leave);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Leave += new System.EventHandler(cmbeximinvoiceno_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 66);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(103, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "Forwarder / Agent";
		this.CmbForwarder.AllowNull = (DefaultableBoolean)1;
		this.CmbForwarder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbForwarder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbForwarder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbForwarder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbForwarder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbForwarder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbForwarder).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbForwarder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbForwarder).Location = new System.Drawing.Point(107, 60);
		((System.Windows.Forms.Control)(object)this.CmbForwarder).Name = "CmbForwarder";
		((System.Windows.Forms.Control)(object)this.CmbForwarder).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbForwarder).TabIndex = 5;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(188, 6);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(130, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(107, 6);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 23);
		this.txtdocno.TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add(this.ChkPrint);
		this.panel5.Controls.Add(this.btn);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(920, 51);
		this.panel5.TabIndex = 56;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(3, 29);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(206, 19);
		this.label26.TabIndex = 0;
		this.label26.Text = "Purchase Order Services";
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Checked = true;
		this.ChkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(769, 28);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(148, 20);
		this.ChkPrint.TabIndex = 9;
		this.ChkPrint.TabStop = false;
		this.ChkPrint.Text = "Preview On Save";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.RadNotReffered);
		this.groupBox3.Controls.Add(this.RadReffered);
		this.groupBox3.Controls.Add(this.RadAll);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1042, 73);
		this.groupBox3.TabIndex = 68;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(937, 42);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 92;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(937, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 91;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(856, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 89;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(856, 42);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 90;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(751, 48);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 9;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadNotReffered.Visible = false;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(751, 30);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 8;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadReffered.Visible = false;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(751, 12);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 7;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadAll.Visible = false;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(688, 42);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(382, 47);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(65, 15);
		this.label6.TabIndex = 88;
		this.label6.Text = "Invoice No";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(382, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(77, 15);
		this.label27.TabIndex = 87;
		this.label27.Text = "Bill To / Party";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(213, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(64, 15);
		this.label28.TabIndex = 86;
		this.label28.Text = "To Doc No";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(213, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 85;
		this.label29.Text = "From Doc No";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(6, 47);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(48, 15);
		this.label30.TabIndex = 84;
		this.label30.Text = "To Date";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(6, 19);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(63, 15);
		this.label31.TabIndex = 83;
		this.label31.Text = "From Date";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(468, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.CmbBillToPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Location = new System.Drawing.Point(468, 13);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Name = "CmbBillToPartyHistory";
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(298, 43);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(298, 15);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(70, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(70, 15);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(920, 27);
		this.toolStrip2.TabIndex = 71;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Controls.Add(this.label32);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 134);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(920, 26);
		this.panel9.TabIndex = 69;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(880, 2);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 2;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.Teal;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label32.Location = new System.Drawing.Point(6, 3);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(142, 19);
		this.label32.TabIndex = 1;
		this.label32.Text = "Filtered Records";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label33);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(3, 353);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(920, 26);
		this.panel12.TabIndex = 70;
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.Teal;
		this.label33.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label33.Location = new System.Drawing.Point(4, 4);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(211, 17);
		this.label33.TabIndex = 1;
		this.label33.Text = "Detail Of Above Selected Row";
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 30);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(920, 104);
		this.panel11.TabIndex = 72;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(934, 561);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ExImSaleOrderForwarder";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExImSaleOrderForwarder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExImSaleOrderForwarder_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailadd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrency).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbForwarder).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyHistory).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
