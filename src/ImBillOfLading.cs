// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
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

public class ImBillOfLading : Form
{
	private int RecId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	internal UltraCombo comCompany;

	private Label label2;

	internal UltraCombo combranches;

	private Label label13;

	internal UltraCombo comproject;

	private Label label18;

	internal UltraCombo cmbPoNo;

	private Label label3;

	private TextBox txtBLNumber;

	private Label label4;

	private TextBox txtVoyageNo;

	private Label label6;

	private TextBox txtVesselName;

	private Label label5;

	private Label label1;

	private DateTimePicker txtBLDate;

	private TextBox txtNetWeight;

	private Label label29;

	private TextBox txtNoOfContainer;

	private Label label28;

	private TextBox txtGrossWeight;

	private Label label27;

	private TextBox txtGoodsDesc;

	private Label label26;

	private TextBox txtImporter;

	private Label label24;

	private TextBox txtNotifyParty;

	private Label label23;

	private TextBox txtTotheOrder;

	private Label label22;

	private Label label21;

	private Label label19;

	private Label label14;

	private TextBox txtRemarks;

	private Label label12;

	private TextBox txtFOBValue;

	private Label label11;

	private Label label10;

	private DateTimePicker txtBookingDate;

	private TextBox txtBookingNo;

	private Label label9;

	private Label label8;

	private Label label7;

	internal UltraCombo cmbTotheOrder;

	internal UltraCombo cmbImporter;

	internal UltraCombo cmbNotifyParty;

	private Panel panel5;

	private Panel panel7;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton5;

	internal GridEX DataGridHistory;

	internal UltraCombo cmbFreight;

	internal UltraCombo cmbcarier;

	internal UltraCombo cmblccontract;

	internal UltraCombo cmbcurrencycode;

	private Label label32;

	private Label label31;

	private Label label30;

	private TextBox txtdeliveryterm;

	private Label label25;

	private Label label40;

	private Label label39;

	private Label label38;

	private Label label37;

	private Label label36;

	private Label label35;

	private Label label34;

	private Label label33;

	private Panel panel8;

	private Label label15;

	private Label label16;

	public ImBillOfLading(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void EximShipmentInfo_Load(object sender, EventArgs e)
	{
		try
		{
			cmbPoNo.Focus();
			bindInvoiceNo();
			bindSupplier();
			TotheORderOf();
			freight();
			CarierType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void freight()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(2, "Freight PrePaid");
			dt.Rows.Add(1, "Freight Collect");
			DDL.BindDDL(dt, cmbFreight, "Id", "Name", "Freight Type", ZeroIndex: false);
			((UltraGridBase)cmbFreight).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarierType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "By Sea");
			dt.Rows.Add(2, "By Air");
			dt.Rows.Add(3, "By Road");
			DDL.BindDDL(dt, cmbcarier, "Id", "Name", "Carier Type", ZeroIndex: false);
			((UltraGridBase)cmbcarier).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindInvoiceNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtInvoice = ExImLcOrderPurchaseOrder.GetPurchaseOrderNoForBillOfLading(new ExImLcOrderPurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 232
			});
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtInvoice, cmbPoNo, "Id", "OrderNo", "OrderNo", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindInvoiceNoGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtInvoice = ExImLcOrderPurchaseOrder.GetPurchaseOrderNo(new ExImLcOrderPurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 232
			});
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtInvoice, cmbPoNo, "Id", "OrderNo", "OrderNo", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotheORderOf()
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
			DataTable dtimporter = new DataTable();
			dtimporter.Columns.Add("Id");
			dtimporter.Columns.Add("BranchName");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Home Country")
				{
					dtimporter.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dtimporter.Rows.Count > 0)
			{
				DDL.BindDDL(dtimporter, cmbTotheOrder, "Id", "BranchName", "To The Order", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindSupplier()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsuppliercustomer = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsuppliercustomer.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CompanyName");
			for (int i = 0; i < dtsuppliercustomer.Rows.Count; i++)
			{
				dt.Rows.Add(dtsuppliercustomer.Rows[i]["Id"], dtsuppliercustomer.Rows[i]["CompanyName"]);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, cmbNotifyParty, "Id", "CompanyName", "Notify party", false);
					DDL.BindDDLNew(dt, cmbImporter, "Id", "CompanyName", "Importer", false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbPoNo.Value) > 0)
			{
				cmbinvoiceLeaveComboFills();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbinvoiceLeaveComboFills()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrderPurchaseOrder.GetLcContractDetailByPurchaseOrderId(new ExImLcOrderPurchaseOrder
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				Id = Conversion.ToInt(cmbPoNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmblccontract, "ExImLcOrderId", "LcOrderNo", "Lc Order No", false);
				DDL.BindDDLNew(dt, cmbcurrencycode, "FcurrencyId", "CurrencyCode", "Currency", false);
				DDL.BindDDLNew(dt, cmbImporter, "SupplierCustomerId", "Supplier", "Supplier", false);
				txtdeliveryterm.Text = Conversion.ToString(dt.Rows[0]["DeliveryTerm"]);
				txtGrossWeight.Text = Conversion.ToString(dt.Rows[0]["GrossWeight"]);
				txtNetWeight.Text = Conversion.ToString(dt.Rows[0]["Netweight"]);
				txtNoOfContainer.Text = Conversion.ToString(dt.Rows[0]["NoofContainer"]);
				((UltraGridBase)cmblccontract).Rows[0].Activate();
				((UltraGridBase)cmbImporter).Rows[0].Activate();
				((UltraGridBase)cmbcurrencycode).Rows[0].Activate();
			}
			else if (dt.Rows.Count <= 0)
			{
				MessageBox.Show("Record Not Found Against This Invoice No!");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FormReset();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ImBillOfLading BillOfLading = new ImBillOfLading();
			if (RecId > 0 && UpdateMode)
			{
				BillOfLading.Id = RecId;
			}
			if (FormValidation())
			{
				BillOfLading.CompanyId = UserAccount.CompanyId;
				BillOfLading.OrganizationId = UserAccount.OrganizationId;
				BillOfLading.OrderOfId = Conversion.ToInt(cmbTotheOrder.Value);
				BillOfLading.ToTheOrderOfName = txtTotheOrder.Text;
				BillOfLading.NotifyPartyId = Conversion.ToInt(cmbNotifyParty.Value);
				BillOfLading.NotifyPartyName = txtNotifyParty.Text;
				BillOfLading.ImporterId = Conversion.ToInt(cmbImporter.Value);
				BillOfLading.ImporterName = txtImporter.Text;
				BillOfLading.BLNumber = txtBLNumber.Text;
				BillOfLading.BLDate = txtBLDate.Value;
				BillOfLading.VesselNo = txtVesselName.Text;
				BillOfLading.VoyageNo = txtVoyageNo.Text;
				BillOfLading.FreightType = ((Control)(object)cmbFreight).Text;
				BillOfLading.CarierType = ((Control)(object)cmbcarier).Text;
				BillOfLading.BookingNo = txtBookingNo.Text;
				BillOfLading.BookingDate = txtBookingDate.Value;
				BillOfLading.FobValue = Conversion.ToDouble((object)txtFOBValue.Text);
				BillOfLading.ShipingCertNo = 0;
				BillOfLading.ShipingCertDate = DateTime.Now;
				BillOfLading.ExImLcPoId = Conversion.ToInt(cmbPoNo.Value);
				BillOfLading.ItemsGoodsDesc = txtGoodsDesc.Text;
				BillOfLading.Remarks = txtRemarks.Text;
				BillOfLading.EntryDate = DateTime.Now;
				BillOfLading.EntryUser = UserAccount.ID;
				BillOfLading.ModifyDate = DateTime.Now;
				BillOfLading.ModifyUser = UserAccount.ID;
				BillOfLading.ApprovedDate = DateTime.Now;
				BillOfLading.IsApproved = false;
				BillOfLading.ApprovedUser = UserAccount.ID;
				int success = ImBillOfLading.Save(BillOfLading);
				if (!UpdateMode && success > 0)
				{
					MessageBox.Show("Save SuccessFully");
				}
				if (UpdateMode && success > 0)
				{
					MessageBox.Show("Update SuccessFully");
				}
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbPoNo).ActiveRow == null)
		{
			MessageBox.Show("Purchase Order Field Required");
			cmbPoNo.Focus();
			return false;
		}
		if (txtBLNumber.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Bl Number  Field Required");
			txtBLNumber.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcarier).ActiveRow == null)
		{
			MessageBox.Show("Carier Type  Field Required");
			cmbcarier.Focus();
			return false;
		}
		if (((UltraGridBase)cmbFreight).ActiveRow == null)
		{
			MessageBox.Show("Freight Type Field Required");
			cmbFreight.Focus();
			return false;
		}
		if (txtBookingNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Booking No  Field Required");
			txtBookingNo.Focus();
			return false;
		}
		if (txtFOBValue.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Fob Value Field Required");
			txtFOBValue.Focus();
			return false;
		}
		if (((UltraGridBase)cmbImporter).ActiveRow == null)
		{
			MessageBox.Show("Importer Field Required");
			cmbImporter.Focus();
			return false;
		}
		return true;
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
	}

	private void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ImBillOfLading shippingBL = new ImBillOfLading();
			RecId = Id;
			shippingBL = ImBillOfLading.GetByID(RecId);
			if (shippingBL != null)
			{
				bindInvoiceNoGetAll();
				((Control)(object)cmbPoNo).Enabled = false;
				tabControl1.SelectedIndex = 0;
				cmbPoNo.Value = shippingBL.ExImLcPoId;
				cmbTotheOrder.Value = shippingBL.OrderOfId;
				txtTotheOrder.Text = Conversion.ToString((object)shippingBL.ToTheOrderOfName);
				cmbNotifyParty.Value = shippingBL.NotifyPartyId;
				txtNotifyParty.Text = Conversion.ToString((object)shippingBL.NotifyPartyName);
				cmbImporter.Value = shippingBL.ImporterId;
				txtImporter.Text = Conversion.ToString((object)shippingBL.ImporterName);
				txtBLNumber.Text = Conversion.ToString((object)shippingBL.BLNumber);
				txtBLDate.Value = Conversion.ToDateTime((object)shippingBL.BLDate);
				txtVesselName.Text = Conversion.ToString((object)shippingBL.VesselNo);
				txtVoyageNo.Text = Conversion.ToString((object)shippingBL.VoyageNo);
				((Control)(object)cmbFreight).Text = Conversion.ToString((object)shippingBL.FreightType);
				((Control)(object)cmbcarier).Text = Conversion.ToString((object)shippingBL.CarierType);
				txtBookingNo.Text = Conversion.ToString((object)shippingBL.BookingNo);
				txtBookingDate.Value = shippingBL.BookingDate;
				txtFOBValue.Text = Conversion.ToString((object)shippingBL.FobValue);
				txtGoodsDesc.Text = Conversion.ToString((object)shippingBL.ItemsGoodsDesc);
				txtRemarks.Text = Conversion.ToString((object)shippingBL.Remarks);
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbPoNo).Enabled = true;
			bindInvoiceNo();
			txtBLDate.Value = DateTime.Now;
			txtBLNumber.Text = string.Empty;
			txtBookingDate.Value = DateTime.Now;
			txtBookingNo.Text = string.Empty;
			((Control)(object)cmbcurrencycode).Text = string.Empty;
			txtFOBValue.Text = string.Empty;
			txtGoodsDesc.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtImporter.Text = string.Empty;
			((Control)(object)cmblccontract).Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtNotifyParty.Text = string.Empty;
			txtdeliveryterm.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtTotheOrder.Text = string.Empty;
			txtVesselName.Text = string.Empty;
			txtVoyageNo.Text = string.Empty;
			((Control)(object)cmbFreight).Text = string.Empty;
			((Control)(object)cmbImporter).Text = string.Empty;
			((Control)(object)cmbPoNo).Text = string.Empty;
			((Control)(object)cmbNotifyParty).Text = string.Empty;
			((Control)(object)cmbTotheOrder).Text = string.Empty;
			((Control)(object)cmbcarier).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EximShipmentInfo_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
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

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ImBillOfLading val = new ImBillOfLading
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtHistory = new DataTable();
			dtHistory = ImBillOfLading.Getall(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ExImLcPoId");
			dt.Columns.Add("OrderNo");
			dt.Columns.Add("BookingNo");
			dt.Columns.Add("BookingDate");
			dt.Columns.Add("BLNumber");
			dt.Columns.Add("BLDate");
			dt.Columns.Add("VesselNo");
			dt.Columns.Add("VoyageNo");
			dt.Columns.Add("FrieghtType");
			dt.Columns.Add("CarierType");
			dt.Columns.Add("FobValue");
			for (int i = 0; i < dtHistory.Rows.Count; i++)
			{
				dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ExImLcPoId"], dtHistory.Rows[i]["OrderNo"], Convert.ToDateTime(dtHistory.Rows[i]["BookingDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["BLNumber"], Convert.ToDateTime(dtHistory.Rows[i]["BLDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["VesselNo"], dtHistory.Rows[i]["VoyageNo"], dtHistory.Rows[i]["VoyageNo"], dtHistory.Rows[i]["CarierType"], dtHistory.Rows[i]["FobValue"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			GridSetting();
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
			BindGrid();
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ExImLcPoId"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ImBillOfLading.ExImBillOfLading_SlipAndRegister(new ReportsParameters
			{
				EximInvoiceId = Conversion.ToInt(cmbPoNo.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "505-ExImBillOfLading_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.EximInvoiceSlip_520(new ReportsParameters
				{
					OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
					CompanyId = Conversion.ToInt((object)UserAccount.CompanyId),
					EximInvoiceId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["EximInvoiceId"].Value),
					ApprovedFilter = Conversion.ToString((object)"All")
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "530-EximBillOfLadingSlip.rpt");
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
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Expected O, but got Unknown
		//IL_5e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e26: Expected O, but got Unknown
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
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
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.ImBillOfLading));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtdeliveryterm = new System.Windows.Forms.TextBox();
		this.cmbcurrencycode = new UltraCombo();
		this.cmblccontract = new UltraCombo();
		this.cmbcarier = new UltraCombo();
		this.cmbFreight = new UltraCombo();
		this.cmbTotheOrder = new UltraCombo();
		this.cmbImporter = new UltraCombo();
		this.cmbNotifyParty = new UltraCombo();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtGoodsDesc = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtImporter = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtNotifyParty = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtTotheOrder = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtFOBValue = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtBookingDate = new System.Windows.Forms.DateTimePicker();
		this.txtBookingNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtVoyageNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtVesselName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtBLDate = new System.Windows.Forms.DateTimePicker();
		this.txtBLNumber = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbPoNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.label16 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrencycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcarier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTotheOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPoNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1079, 713);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1079, 713);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1071, 687);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1065, 681);
		this.panel2.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.txtdeliveryterm);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrencycode);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmblccontract);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcarier);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFreight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTotheOrder);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbImporter);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty);
		this.panel4.Controls.Add(this.txtNetWeight);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.txtNoOfContainer);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.txtGrossWeight);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.txtGoodsDesc);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.txtImporter);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.txtNotifyParty);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.txtTotheOrder);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtFOBValue);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtBookingDate);
		this.panel4.Controls.Add(this.txtBookingNo);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtVoyageNo);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtVesselName);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtBLDate);
		this.panel4.Controls.Add(this.txtBLNumber);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPoNo);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1065, 378);
		this.panel4.TabIndex = 1;
		this.panel8.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1065, 44);
		this.panel8.TabIndex = 577;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(19, 12);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(221, 24);
		this.label15.TabIndex = 25;
		this.label15.Text = "Import :Bill Of Lading (BL)";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(64, 183);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 576;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(409, 90);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 575;
		this.label39.Text = "*";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(243, 90);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 574;
		this.label38.Text = "*";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(733, 44);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 17);
		this.label37.TabIndex = 573;
		this.label37.Text = "*";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(585, 44);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 572;
		this.label36.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(422, 45);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 571;
		this.label35.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(243, 44);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 570;
		this.label34.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(107, 44);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 569;
		this.label33.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label25.Location = new System.Drawing.Point(7, 291);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(1015, 15);
		this.label25.TabIndex = 568;
		this.label25.Text = "________________________________________________________________________________________________________________________________________________________________________";
		this.txtdeliveryterm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliveryterm.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtdeliveryterm.Location = new System.Drawing.Point(179, 331);
		this.txtdeliveryterm.Name = "txtdeliveryterm";
		this.txtdeliveryterm.Size = new System.Drawing.Size(162, 23);
		this.txtdeliveryterm.TabIndex = 19;
		this.cmbcurrencycode.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrencycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrencycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrencycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbcurrencycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Location = new System.Drawing.Point(345, 329);
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Name = "cmbcurrencycode";
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).TabIndex = 22;
		this.cmblccontract.AllowNull = (DefaultableBoolean)1;
		this.cmblccontract.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmblccontract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmblccontract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmblccontract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmblccontract).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmblccontract).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmblccontract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Location = new System.Drawing.Point(11, 329);
		((System.Windows.Forms.Control)(object)this.cmblccontract).Name = "cmblccontract";
		((System.Windows.Forms.Control)(object)this.cmblccontract).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmblccontract).TabIndex = 18;
		this.cmbcarier.AllowNull = (DefaultableBoolean)1;
		this.cmbcarier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcarier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcarier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbcarier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcarier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcarier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcarier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcarier).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbcarier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcarier).Location = new System.Drawing.Point(343, 110);
		((System.Windows.Forms.Control)(object)this.cmbcarier).Name = "cmbcarier";
		((System.Windows.Forms.Control)(object)this.cmbcarier).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbcarier).TabIndex = 8;
		this.cmbFreight.AllowNull = (DefaultableBoolean)1;
		this.cmbFreight.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFreight.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFreight.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbFreight).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFreight).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbFreight).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFreight).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFreight).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbFreight.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFreight).Location = new System.Drawing.Point(175, 110);
		((System.Windows.Forms.Control)(object)this.cmbFreight).Name = "cmbFreight";
		((System.Windows.Forms.Control)(object)this.cmbFreight).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbFreight).TabIndex = 7;
		this.cmbTotheOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbTotheOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTotheOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTotheOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbTotheOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Location = new System.Drawing.Point(679, 110);
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Name = "cmbTotheOrder";
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Size = new System.Drawing.Size(328, 26);
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).TabIndex = 10;
		this.cmbImporter.AllowNull = (DefaultableBoolean)1;
		this.cmbImporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbImporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbImporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbImporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Location = new System.Drawing.Point(11, 201);
		((System.Windows.Forms.Control)(object)this.cmbImporter).Name = "cmbImporter";
		this.cmbImporter.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.cmbImporter).TabIndex = 14;
		this.cmbNotifyParty.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbNotifyParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Location = new System.Drawing.Point(343, 156);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Name = "cmbNotifyParty";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Size = new System.Drawing.Size(330, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).TabIndex = 12;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(678, 331);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(162, 23);
		this.txtNetWeight.TabIndex = 25;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label29.Location = new System.Drawing.Point(675, 312);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(89, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Net Weight Kgs";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNoOfContainer.Location = new System.Drawing.Point(841, 331);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(162, 23);
		this.txtNoOfContainer.TabIndex = 26;
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label28.Location = new System.Drawing.Point(839, 312);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(95, 15);
		this.label28.TabIndex = 104;
		this.label28.Text = "No Of Container";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(511, 331);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(162, 23);
		this.txtGrossWeight.TabIndex = 24;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label27.Location = new System.Drawing.Point(508, 312);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(81, 15);
		this.label27.TabIndex = 102;
		this.label27.Text = "Gross Weight";
		this.txtGoodsDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGoodsDesc.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtGoodsDesc.Location = new System.Drawing.Point(679, 203);
		this.txtGoodsDesc.Name = "txtGoodsDesc";
		this.txtGoodsDesc.Size = new System.Drawing.Size(328, 23);
		this.txtGoodsDesc.TabIndex = 16;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label26.Location = new System.Drawing.Point(678, 185);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(108, 15);
		this.label26.TabIndex = 100;
		this.label26.Text = "Goods Description";
		this.txtImporter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtImporter.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtImporter.Location = new System.Drawing.Point(343, 203);
		this.txtImporter.Name = "txtImporter";
		this.txtImporter.Size = new System.Drawing.Size(330, 23);
		this.txtImporter.TabIndex = 15;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label32.Location = new System.Drawing.Point(342, 185);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(115, 15);
		this.label32.TabIndex = 96;
		this.label32.Text = "Importer Comments";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label24.Location = new System.Drawing.Point(10, 185);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(55, 15);
		this.label24.TabIndex = 96;
		this.label24.Text = "Importer";
		this.txtNotifyParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNotifyParty.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNotifyParty.Location = new System.Drawing.Point(679, 158);
		this.txtNotifyParty.Name = "txtNotifyParty";
		this.txtNotifyParty.Size = new System.Drawing.Size(328, 23);
		this.txtNotifyParty.TabIndex = 13;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label31.Location = new System.Drawing.Point(679, 140);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(131, 15);
		this.label31.TabIndex = 94;
		this.label31.Text = "Notify Party Comments";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label23.Location = new System.Drawing.Point(340, 140);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(71, 15);
		this.label23.TabIndex = 94;
		this.label23.Text = "Notify Party";
		this.txtTotheOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotheOrder.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtTotheOrder.Location = new System.Drawing.Point(10, 158);
		this.txtTotheOrder.Name = "txtTotheOrder";
		this.txtTotheOrder.Size = new System.Drawing.Size(327, 23);
		this.txtTotheOrder.TabIndex = 11;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label30.Location = new System.Drawing.Point(8, 140);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(121, 15);
		this.label30.TabIndex = 92;
		this.label30.Text = "To The Order Of Desc";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label22.Location = new System.Drawing.Point(676, 94);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 15);
		this.label22.TabIndex = 92;
		this.label22.Text = "To The Order";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label21.Location = new System.Drawing.Point(176, 312);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(87, 15);
		this.label21.TabIndex = 90;
		this.label21.Text = "Delivery Terms";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label19.Location = new System.Drawing.Point(342, 312);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(55, 15);
		this.label19.TabIndex = 86;
		this.label19.Text = "Fcy Code";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label14.Location = new System.Drawing.Point(9, 312);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(86, 15);
		this.label14.TabIndex = 78;
		this.label14.Text = "Lc Contract No";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(11, 251);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(996, 37);
		this.txtRemarks.TabIndex = 17;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label12.Location = new System.Drawing.Point(8, 232);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(54, 15);
		this.label12.TabIndex = 76;
		this.label12.Text = "Remarks";
		this.txtFOBValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFOBValue.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtFOBValue.Location = new System.Drawing.Point(511, 112);
		this.txtFOBValue.Name = "txtFOBValue";
		this.txtFOBValue.Size = new System.Drawing.Size(162, 23);
		this.txtFOBValue.TabIndex = 9;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label11.Location = new System.Drawing.Point(508, 94);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 74;
		this.label11.Text = "Fob Value";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label10.Location = new System.Drawing.Point(344, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 72;
		this.label10.Text = "Booking Date";
		this.txtBookingDate.CustomFormat = "dd-MMM-yy";
		this.txtBookingDate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtBookingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBookingDate.Location = new System.Drawing.Point(343, 64);
		this.txtBookingDate.Name = "txtBookingDate";
		this.txtBookingDate.Size = new System.Drawing.Size(162, 23);
		this.txtBookingDate.TabIndex = 2;
		this.txtBookingNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBookingNo.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtBookingNo.Location = new System.Drawing.Point(175, 64);
		this.txtBookingNo.Name = "txtBookingNo";
		this.txtBookingNo.Size = new System.Drawing.Size(162, 23);
		this.txtBookingNo.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label9.Location = new System.Drawing.Point(176, 47);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(69, 15);
		this.label9.TabIndex = 70;
		this.label9.Text = "Booking No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label8.Location = new System.Drawing.Point(340, 94);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 15);
		this.label8.TabIndex = 68;
		this.label8.Text = "Carier Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label7.Location = new System.Drawing.Point(172, 94);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 15);
		this.label7.TabIndex = 66;
		this.label7.Text = "Freight Type";
		this.txtVoyageNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoyageNo.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtVoyageNo.Location = new System.Drawing.Point(10, 112);
		this.txtVoyageNo.Name = "txtVoyageNo";
		this.txtVoyageNo.Size = new System.Drawing.Size(162, 23);
		this.txtVoyageNo.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label6.Location = new System.Drawing.Point(7, 94);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Voyage No";
		this.txtVesselName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVesselName.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtVesselName.Location = new System.Drawing.Point(845, 64);
		this.txtVesselName.Name = "txtVesselName";
		this.txtVesselName.Size = new System.Drawing.Size(162, 23);
		this.txtVesselName.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label5.Location = new System.Drawing.Point(846, 47);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 62;
		this.label5.Text = "Vessel Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label1.Location = new System.Drawing.Point(680, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 60;
		this.label1.Text = "B/L Date";
		this.txtBLDate.CustomFormat = "dd-MMM-yy";
		this.txtBLDate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtBLDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBLDate.Location = new System.Drawing.Point(679, 64);
		this.txtBLDate.Name = "txtBLDate";
		this.txtBLDate.Size = new System.Drawing.Size(162, 23);
		this.txtBLDate.TabIndex = 4;
		this.txtBLNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBLNumber.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtBLNumber.Location = new System.Drawing.Point(511, 64);
		this.txtBLNumber.Name = "txtBLNumber";
		this.txtBLNumber.Size = new System.Drawing.Size(162, 23);
		this.txtBLNumber.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label4.Location = new System.Drawing.Point(512, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 15);
		this.label4.TabIndex = 58;
		this.label4.Text = "B/L Number";
		this.cmbPoNo.AllowNull = (DefaultableBoolean)1;
		this.cmbPoNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPoNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPoNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPoNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbPoNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPoNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPoNo).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbPoNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPoNo).Location = new System.Drawing.Point(7, 62);
		((System.Windows.Forms.Control)(object)this.cmbPoNo).Name = "cmbPoNo";
		((System.Windows.Forms.Control)(object)this.cmbPoNo).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbPoNo).TabIndex = 0;
		this.cmbPoNo.ValueChanged += new System.EventHandler(cmbInvoiceNo_Leave);
		((System.Windows.Forms.Control)(object)this.cmbPoNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label3.Location = new System.Drawing.Point(7, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(99, 15);
		this.label3.TabIndex = 51;
		this.label3.Text = "Purchase Order#";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(1247, 10);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(157, 22);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1178, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 17);
		this.label2.TabIndex = 39;
		this.label2.Text = "Company";
		this.label2.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1247, 38);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(157, 22);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1194, 43);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(47, 17);
		this.label13.TabIndex = 40;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1247, 66);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(157, 22);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1193, 71);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(48, 17);
		this.label18.TabIndex = 41;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1065, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1065, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
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
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1071, 687);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1065, 681);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 31);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1065, 650);
		this.panel7.TabIndex = 1;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistory.ColumnAutoResize = true;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1065, 650);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1065, 31);
		this.panel6.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton1, this.toolStripButton2, this.toolStripSeparator1, this.toolStripButton5 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1065, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Font = new System.Drawing.Font("Times New Roman", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(107, 24);
		this.toolStripButton1.Text = "Bill Of Lading";
		this.toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&New";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton5.Text = "&Print";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(571, 90);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 578;
		this.label16.Text = "*";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1079, 713);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImBillOfLading";
		this.Text = "Bill Of Lading";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EximShipmentInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EximShipmentInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrencycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcarier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTotheOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPoNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
