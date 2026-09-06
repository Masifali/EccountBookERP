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
using Architecture.WinApp.Helper;
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

public class EximBillOfLading : Form
{
	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private DataTable dtInvoice = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private GroupBox groupBox2;

	private TextBox txtEformNo;

	private Label label14;

	private Label label16;

	private Label label17;

	private TextBox txtNetWeight;

	private TextBox txtdeliveryterm;

	private Label label29;

	private Label label19;

	private TextBox txtNoOfContainer;

	private Label label28;

	private Label label20;

	private TextBox txtGrossWeight;

	internal UltraCombo cmbcurrencycode;

	private Label label27;

	private TextBox txtEformValue;

	internal UltraCombo cmblccontract;

	private Label label21;

	private DateTimePicker txtEformDate;

	private GroupBox groupBox1;

	private Label label42;

	private Label label41;

	private RichTextBox txtRemarks;

	private Label label3;

	private Label label33;

	private Label label39;

	internal UltraCombo cmbInvoiceNo;

	private Label label38;

	private Label label9;

	internal UltraCombo cmbImporter;

	private Label label37;

	private Label label34;

	internal UltraCombo cmbTotheOrder;

	internal UltraCombo cmbNotifyParty;

	internal UltraCombo cmbcarier;

	private Label label36;

	private TextBox txtGoodsDesc;

	private TextBox txtBookingNo;

	private Label label26;

	private Label label35;

	private TextBox txtImporter;

	internal UltraCombo cmbFreight;

	private Label label32;

	private Label label10;

	private Label label12;

	private DateTimePicker txtBookingDate;

	private Label label4;

	private TextBox txtBLNumber;

	private Label label1;

	private Label label24;

	private DateTimePicker txtBLDate;

	private TextBox txtNotifyParty;

	private Label label5;

	private Label label31;

	private TextBox txtVesselName;

	private Label label6;

	private TextBox txtVoyageNo;

	private Label label23;

	private Label label7;

	private TextBox txtTotheOrder;

	private Label label8;

	private Label label30;

	private Label label11;

	private TextBox txtFOBValue;

	private Label label22;

	internal UltraCombo comCompany;

	private Label label2;

	internal UltraCombo combranches;

	private Label label13;

	internal UltraCombo comproject;

	private Label label18;

	private Panel panel2;

	private Label label15;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private ToolStripButton btnAttachment;

	private ToolStripButton BtnShortCutkeys;

	private ToolStrip miniToolStrip;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton5;

	private TabPage tabPage2;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private Label label43;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel7;

	private GroupBox groupBox5;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Button btnShowHistory;

	private Label label44;

	private RadioButton drdocdate;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbInvoicenoHistory;

	private Panel panel5;

	private Label label45;

	private ToolStrip toolStrip3;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	public EximBillOfLading(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void EximShipmentInfo_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 700);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtInvoice.Columns.Add("InvoiceId", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			cmbInvoiceNo.Focus();
			bindInvoiceNo();
			bindSupplier();
			TotheORderOf();
			freight();
			CarierType();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			HistoryCombosFill();
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void freight()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(2, "Freight PrePaid");
			dt.Rows.Add(1, "Freight Collect");
			DDL.BindDDL(dt, cmbFreight, "Id", "Name", "Freight Type", ZeroIndex: true);
			((UltraGridBase)cmbFreight).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CarierType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "By Sea");
			dt.Rows.Add(2, "By Air");
			dt.Rows.Add(3, "By Road");
			DDL.BindDDL(dt, cmbcarier, "Id", "Name", "Carier Type", ZeroIndex: true);
			((UltraGridBase)cmbcarier).Rows[1].Activate();
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
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetInvoiceNoForBillOfLading(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			dtInvoice.Clear();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvoiceNo"]);
				}
				if (dtInvoice.Rows.Count > 0)
				{
					DDL.BindDDL(dtInvoice, cmbInvoiceNo, "InvoiceId", "InvoiceNo", "Invoice No", ZeroIndex: true);
				}
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
				DDL.BindDDL(dtimporter, cmbTotheOrder, "Id", "BranchName", "To The Order", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindSupplier()
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
					DDL.BindDDL(dt, cmbNotifyParty, "Id", "CompanyName", "Notify party", ZeroIndex: true);
					DDL.BindDDL(dt, cmbImporter, "Id", "CompanyName", "Importer", ZeroIndex: true);
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
			int InvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			if (InvoiceId > 0)
			{
				cmbinvoiceLeaveComboFills(InvoiceId);
				ReadBookingCroByInvoiceId(InvoiceId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbinvoiceLeaveComboFills(int InvoiceId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			if (InvoiceId <= 0)
			{
				return;
			}
			DataTable dteform = ExImInvoice.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Ids = "204,211",
				Id = InvoiceId
			});
			if (dteform.Rows.Count > 0)
			{
				DataTable dtLc = new DataTable();
				dtLc.Columns.Add("LcOrderNoId");
				dtLc.Columns.Add("LcOrderNo");
				DataTable dtcurrency = new DataTable();
				dtcurrency.Columns.Add("FcurrencyId");
				dtcurrency.Columns.Add("CurrencyCode");
				txtdeliveryterm.Text = Conversion.ToString(dteform.Rows[0]["DeliveryTerm"]);
				for (int i = 0; i < dteform.Rows.Count; i++)
				{
					if (Convert.ToInt32(dteform.Rows[i]["Id"]) == InvoiceId)
					{
						dtLc.Rows.Add(dteform.Rows[i]["LcOrderNoId"], dteform.Rows[i]["LcOrderNo"]);
						dtcurrency.Rows.Add(dteform.Rows[i]["FcurrencyId"], dteform.Rows[i]["CurrencyCode"]);
					}
				}
				if (dtLc.Rows.Count > 0)
				{
					DDL.BindDDL(dtLc, cmblccontract, "LcOrderNoId", "LcOrderNo", "Lc Order No", ZeroIndex: true);
					((UltraGridBase)cmblccontract).Rows[1].Activate();
				}
				if (dtcurrency.Rows.Count > 0)
				{
					DDL.BindDDL(dtcurrency, cmbcurrencycode, "FcurrencyId", "CurrencyCode", "Currency", ZeroIndex: true);
					((UltraGridBase)cmbcurrencycode).Rows[1].Activate();
				}
				txtGrossWeight.Text = Conversion.ToString(dteform.Rows[0]["GrossWeight"]);
				txtNetWeight.Text = Conversion.ToString(dteform.Rows[0]["NetWeight"]);
				txtNoOfContainer.Text = Conversion.ToString(dteform.Rows[0]["NoOfContainers"]);
				cmbImporter.Value = Conversion.ToInt(dteform.Rows[0]["SupplierCustomerId"]);
				txtEformNo.Text = Conversion.ToString(dteform.Rows[0]["EFormNo"]);
				txtEformDate.Value = Conversion.ToDateTime(dteform.Rows[0]["EFormDate"]);
				if (Conversion.ToInt(dteform.Rows[0]["NotifyParty1"]) <= 0)
				{
					cmbNotifyParty.Value = Conversion.ToInt(dteform.Rows[0]["NotifyParty2"]);
				}
				else
				{
					cmbNotifyParty.Value = Conversion.ToInt(dteform.Rows[0]["NotifyParty1"]);
				}
			}
			else if (dteform.Rows.Count <= 0)
			{
				txtGrossWeight.Text = "";
				txtNetWeight.Text = "";
				txtNoOfContainer.Text = "";
				cmbImporter.Value = "";
				txtEformNo.Text = "";
				txtEformDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadBookingCroByInvoiceId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImExportShipingLineBooking.ReadByInvoiceId(new ExImExportShipingLineBooking
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				ExImInvoiceId = Id
			});
			if (dt.Rows.Count > 0)
			{
				txtBookingNo.Text = Conversion.ToString(dt.Rows[0]["BookingCroNo"]);
				txtBookingDate.Value = Conversion.ToDateTime(dt.Rows[0]["BookingDate"]);
				txtVesselName.Text = Conversion.ToString(dt.Rows[0]["VesselName"]);
				txtVoyageNo.Text = Conversion.ToString(dt.Rows[0]["VoyageNo"]);
			}
			else
			{
				txtBookingNo.Text = "";
				txtBookingDate.Value = Conversion.ToDateTime((object)DateTime.Now);
				txtVesselName.Text = "";
				txtVoyageNo.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbInvoiceNo).Text.Trim() == string.Empty || Conversion.ToInt(cmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("Invoice  Field Required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (txtBookingNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Booking No  Field Required");
			txtBookingNo.Focus();
			return false;
		}
		if (txtBLNumber.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Bl Number  Field Required");
			txtBLNumber.Focus();
			return false;
		}
		if (txtVoyageNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Voyage No  Field Required");
			txtVoyageNo.Focus();
			return false;
		}
		if (((Control)(object)cmbFreight).Text.Trim() == string.Empty || Conversion.ToInt(cmbFreight.Value) == 0)
		{
			MessageBox.Show("Freight Type Field Required");
			cmbFreight.Focus();
			return false;
		}
		if (((Control)(object)cmbcarier).Text.Trim() == string.Empty || Conversion.ToInt(cmbcarier.Value) == 0)
		{
			MessageBox.Show("Carrier Type  Field Required");
			cmbcarier.Focus();
			return false;
		}
		return true;
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ExImBillOfLading BillOfLading = new ExImBillOfLading();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (RecId > 0 && UpdateMode)
				{
					BillOfLading.Id = RecId;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
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
			BillOfLading.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			BillOfLading.ItemsGoodsDesc = txtGoodsDesc.Text;
			BillOfLading.Remarks = txtRemarks.Text;
			BillOfLading.EntryDate = DateTime.Now;
			BillOfLading.EntryUser = UserAccount.ID;
			BillOfLading.ModifyDate = DateTime.Now;
			BillOfLading.ModifyUser = UserAccount.ID;
			BillOfLading.ApprovedDate = DateTime.Now;
			BillOfLading.IsApproved = false;
			BillOfLading.ApprovedUser = UserAccount.ID;
			BillOfLading.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImBillOfLading>(BillOfLading, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImBillOfLading.Save(BillOfLading);
			if (!UpdateMode && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
				FormReset();
			}
			if (UpdateMode && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
				FormReset();
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
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
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = Id;
			ExImBillOfLading shippingBL = new ExImBillOfLading();
			shippingBL = ExImBillOfLading.GetByID(RecId);
			if (shippingBL != null)
			{
				((Control)(object)cmbInvoiceNo).Enabled = false;
				tabControl1.SelectedIndex = 0;
				dtInvoice.Rows.Add(shippingBL.ExImInvoiceId, shippingBL.ExImInvoiceNo);
				DDL.BindDDL(dtInvoice, cmbInvoiceNo, "InvoiceId", "InvoiceNo", "Invoice No", ZeroIndex: true);
				cmbInvoiceNo.Value = shippingBL.ExImInvoiceId;
				cmbTotheOrder.Value = shippingBL.OrderOfId;
				txtTotheOrder.Text = shippingBL.ToTheOrderOfName;
				cmbNotifyParty.Value = shippingBL.NotifyPartyId;
				txtNotifyParty.Text = shippingBL.NotifyPartyName;
				cmbImporter.Value = shippingBL.ImporterId;
				txtImporter.Text = shippingBL.ImporterName;
				txtBLNumber.Text = shippingBL.BLNumber;
				txtBLDate.Value = Conversion.ToDateTime((object)shippingBL.BLDate);
				txtVesselName.Text = shippingBL.VesselNo;
				txtVoyageNo.Text = shippingBL.VoyageNo;
				((Control)(object)cmbFreight).Text = shippingBL.FreightType;
				((Control)(object)cmbcarier).Text = shippingBL.CarierType;
				txtBookingNo.Text = shippingBL.BookingNo;
				txtBookingDate.Value = shippingBL.BookingDate;
				txtFOBValue.Text = shippingBL.FobValue.ToString();
				txtGoodsDesc.Text = shippingBL.ItemsGoodsDesc;
				txtRemarks.Text = shippingBL.Remarks;
				comCompany.Value = shippingBL.CompanyId;
				comproject.Value = shippingBL.ProjectsId;
				combranches.Value = shippingBL.BranchesId;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				UpdateMode = true;
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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

	public void FormReset()
	{
		try
		{
			((Control)(object)cmbInvoiceNo).Enabled = true;
			txtBLDate.Value = DateTime.Now;
			txtBLNumber.Text = string.Empty;
			txtBookingDate.Value = DateTime.Now;
			txtBookingNo.Text = string.Empty;
			((Control)(object)cmbcurrencycode).Text = string.Empty;
			txtEformDate.Value = DateTime.Now;
			txtEformNo.Text = string.Empty;
			txtEformValue.Text = string.Empty;
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
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			((Control)(object)cmbNotifyParty).Text = string.Empty;
			((Control)(object)cmbTotheOrder).Text = string.Empty;
			((Control)(object)cmbcarier).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			bindInvoiceNo();
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			bindInvoiceNo();
			bindSupplier();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EximBillOfLading_FormClosed(object sender, FormClosedEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			cmbInvoiceNo.Focus();
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbInvoicenoHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
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

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("InvoiceNo", typeof(string));
			DataTable dt = ExImBillOfLading.GetDataForDropDownFromBillOfLading(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbInvoicenoHistory, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
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
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoicenoHistory.Value);
			DataTable dtGrid = ExImBillOfLading.FormHistory(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EximInvoiceId", typeof(int));
				dt.Columns.Add("InvoiceNo");
				dt.Columns.Add("BookingNo");
				dt.Columns.Add("BookingDate", typeof(DateTime));
				dt.Columns.Add("BLNumber");
				dt.Columns.Add("BLDate", typeof(DateTime));
				dt.Columns.Add("VesselNo");
				dt.Columns.Add("VoyageNo");
				dt.Columns.Add("FrieghtType");
				dt.Columns.Add("CarierType");
				dt.Columns.Add("FobValue");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["EximInvoiceId"], dtGrid.Rows[i]["InvoiceNo"], dtGrid.Rows[i]["BookingNo"], Conversion.ToDateTime(dtGrid.Rows[i]["BookingDate"]).ToShortDateString(), dtGrid.Rows[i]["BLNumber"], Conversion.ToDateTime(dtGrid.Rows[i]["BLDate"]).ToShortDateString(), dtGrid.Rows[i]["VesselNo"], dtGrid.Rows[i]["VoyageNo"], dtGrid.Rows[i]["VoyageNo"], dtGrid.Rows[i]["CarierType"], dtGrid.Rows[i]["FobValue"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["EntryUser"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ModifyUser"], dtGrid.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.ColumnAutoResize = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["EximInvoiceId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 50;
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow r = DataGridHistory.CurrentRow;
		if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
		{
			ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				GenerateGrdSlip(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["EximInvoiceId"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
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
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), base.Name);
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
			GenerateSlip(Conversion.ToInt(cmbInvoiceNo.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateGrdSlip(int PrintId)
	{
		try
		{
			CommonServices.ExportInvoiceSlip_520(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		try
		{
			CommonServices.ExImBillOfLading_Slip(PrintId);
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

	private void txtNetWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFOBValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoOfContainer_KeyPress(object sender, KeyPressEventArgs e)
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

	private void EximShipmentInfo_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtBookingNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtBookingNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Booking Combo");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Up", "For Focus On Booking Number ");
			dt.Rows.Add("Ctrl+Down", "For Focus On grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					GenerateGrdSlip(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["EximInvoiceId"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), base.Name);
				}
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
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_6584: Unknown result type (might be due to invalid IL or missing references)
		//IL_658e: Expected O, but got Unknown
		//IL_659b: Unknown result type (might be due to invalid IL or missing references)
		//IL_65a5: Expected O, but got Unknown
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
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
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
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
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
		Appearance appearance157 = new Appearance();
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
		Appearance appearance168 = new Appearance();
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
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
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
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.EximBillOfLading));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtEformNo = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtdeliveryterm = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.cmbcurrencycode = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.txtEformValue = new System.Windows.Forms.TextBox();
		this.cmblccontract = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.txtEformDate = new System.Windows.Forms.DateTimePicker();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbImporter = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbTotheOrder = new UltraCombo();
		this.cmbNotifyParty = new UltraCombo();
		this.cmbcarier = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtGoodsDesc = new System.Windows.Forms.TextBox();
		this.txtBookingNo = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtImporter = new System.Windows.Forms.TextBox();
		this.cmbFreight = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtBookingDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.txtBLNumber = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtBLDate = new System.Windows.Forms.DateTimePicker();
		this.txtNotifyParty = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtVesselName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtVoyageNo = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtTotheOrder = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtFOBValue = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label44 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbInvoicenoHistory = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrencycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTotheOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcarier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoicenoHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 401);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 401);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 372);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 311);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtEformNo);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.txtNetWeight);
		this.groupBox2.Controls.Add(this.txtdeliveryterm);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.txtNoOfContainer);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.txtGrossWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrencycode);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.txtEformValue);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmblccontract);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtEformDate);
		this.groupBox2.Enabled = false;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(677, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(264, 308);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Other";
		this.txtEformNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEformNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEformNo.Location = new System.Drawing.Point(102, 78);
		this.txtEformNo.Name = "txtEformNo";
		this.txtEformNo.Size = new System.Drawing.Size(157, 23);
		this.txtEformNo.TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(7, 26);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(71, 15);
		this.label14.TabIndex = 78;
		this.label14.Text = "Contract No";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(7, 82);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(56, 15);
		this.label16.TabIndex = 82;
		this.label16.Text = "E-Form #";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(7, 110);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(74, 15);
		this.label17.TabIndex = 84;
		this.label17.Text = "E-Form Date";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(102, 250);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(157, 23);
		this.txtNetWeight.TabIndex = 8;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeight_KeyPress);
		this.txtdeliveryterm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliveryterm.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdeliveryterm.Location = new System.Drawing.Point(102, 50);
		this.txtdeliveryterm.Name = "txtdeliveryterm";
		this.txtdeliveryterm.Size = new System.Drawing.Size(157, 23);
		this.txtdeliveryterm.TabIndex = 1;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(7, 254);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(90, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Net Weight Kgs";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(7, 139);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(55, 15);
		this.label19.TabIndex = 86;
		this.label19.Text = "Fcy Code";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfContainer.Location = new System.Drawing.Point(102, 163);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(157, 23);
		this.txtNoOfContainer.TabIndex = 5;
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainer_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(7, 167);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(93, 15);
		this.label28.TabIndex = 104;
		this.label28.Text = "No Of Container";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(7, 194);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 88;
		this.label20.Text = "E-Form Value";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(102, 220);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(157, 23);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.cmbcurrencycode.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrencycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrencycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrencycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrencycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcurrencycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Location = new System.Drawing.Point(102, 133);
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Name = "cmbcurrencycode";
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrencycode).TabIndex = 4;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(7, 224);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 15);
		this.label27.TabIndex = 102;
		this.label27.Text = "Gross Weight";
		this.txtEformValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEformValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEformValue.Location = new System.Drawing.Point(102, 190);
		this.txtEformValue.Name = "txtEformValue";
		this.txtEformValue.Size = new System.Drawing.Size(157, 23);
		this.txtEformValue.TabIndex = 6;
		this.txtEformValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmblccontract.AllowNull = (DefaultableBoolean)1;
		this.cmblccontract.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmblccontract.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmblccontract.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmblccontract).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblccontract).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
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
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmblccontract).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmblccontract).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmblccontract.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmblccontract).Location = new System.Drawing.Point(102, 20);
		((System.Windows.Forms.Control)(object)this.cmblccontract).Name = "cmblccontract";
		((System.Windows.Forms.Control)(object)this.cmblccontract).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmblccontract).TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(7, 54);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(85, 15);
		this.label21.TabIndex = 90;
		this.label21.Text = "Delivery Terms";
		this.txtEformDate.CustomFormat = "dd-MMM-yy";
		this.txtEformDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEformDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEformDate.Location = new System.Drawing.Point(102, 106);
		this.txtEformDate.Name = "txtEformDate";
		this.txtEformDate.Size = new System.Drawing.Size(157, 23);
		this.txtEformDate.TabIndex = 3;
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbImporter);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTotheOrder);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcarier);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.txtGoodsDesc);
		this.groupBox1.Controls.Add(this.txtBookingNo);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtImporter);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFreight);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtBookingDate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtBLNumber);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtBLDate);
		this.groupBox1.Controls.Add(this.txtNotifyParty);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtVesselName);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtVoyageNo);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtTotheOrder);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtFOBValue);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(670, 308);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(85, 166);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 578;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(84, 193);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 577;
		this.label41.Text = "*";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(398, 218);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(266, 83);
		this.txtRemarks.TabIndex = 17;
		this.txtRemarks.Text = "";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 26);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 15);
		this.label3.TabIndex = 51;
		this.label3.Text = "Invoice No#";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(84, 25);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 569;
		this.label33.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(84, 253);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 575;
		this.label39.Text = "*";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(99, 20);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 0;
		this.cmbInvoiceNo.ValueChanged += new System.EventHandler(cmbInvoiceNo_Leave);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(84, 223);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 574;
		this.label38.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 54);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(70, 15);
		this.label9.TabIndex = 70;
		this.label9.Text = "Booking No";
		this.cmbImporter.AllowNull = (DefaultableBoolean)1;
		this.cmbImporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbImporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbImporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbImporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Location = new System.Drawing.Point(398, 133);
		((System.Windows.Forms.Control)(object)this.cmbImporter).Name = "cmbImporter";
		this.cmbImporter.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbImporter).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbImporter).TabIndex = 14;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(84, 138);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 17);
		this.label37.TabIndex = 573;
		this.label37.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(84, 53);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 570;
		this.label34.Text = "*";
		this.cmbTotheOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbTotheOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTotheOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTotheOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTotheOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTotheOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Location = new System.Drawing.Point(398, 20);
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Name = "cmbTotheOrder";
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbTotheOrder).TabIndex = 10;
		this.cmbNotifyParty.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbNotifyParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Location = new System.Drawing.Point(398, 76);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Name = "cmbNotifyParty";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).TabIndex = 12;
		this.cmbcarier.AllowNull = (DefaultableBoolean)1;
		this.cmbcarier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcarier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcarier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbcarier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcarier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbcarier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcarier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcarier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcarier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcarier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcarier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcarier).Location = new System.Drawing.Point(99, 248);
		((System.Windows.Forms.Control)(object)this.cmbcarier).Name = "cmbcarier";
		((System.Windows.Forms.Control)(object)this.cmbcarier).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbcarier).TabIndex = 8;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(84, 109);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 572;
		this.label36.Text = "*";
		this.txtGoodsDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGoodsDesc.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGoodsDesc.Location = new System.Drawing.Point(398, 190);
		this.txtGoodsDesc.Name = "txtGoodsDesc";
		this.txtGoodsDesc.Size = new System.Drawing.Size(266, 23);
		this.txtGoodsDesc.TabIndex = 16;
		this.txtBookingNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBookingNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingNo.Location = new System.Drawing.Point(99, 50);
		this.txtBookingNo.Name = "txtBookingNo";
		this.txtBookingNo.Size = new System.Drawing.Size(176, 23);
		this.txtBookingNo.TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(282, 194);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(105, 15);
		this.label26.TabIndex = 100;
		this.label26.Text = "Goods Description";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(84, 81);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 571;
		this.label35.Text = "*";
		this.txtImporter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtImporter.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtImporter.Location = new System.Drawing.Point(398, 163);
		this.txtImporter.Name = "txtImporter";
		this.txtImporter.Size = new System.Drawing.Size(266, 23);
		this.txtImporter.TabIndex = 15;
		this.cmbFreight.AllowNull = (DefaultableBoolean)1;
		this.cmbFreight.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFreight.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFreight.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbFreight).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFreight).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbFreight).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFreight).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFreight).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFreight).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFreight).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFreight.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFreight).Location = new System.Drawing.Point(99, 218);
		((System.Windows.Forms.Control)(object)this.cmbFreight).Name = "cmbFreight";
		((System.Windows.Forms.Control)(object)this.cmbFreight).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbFreight).TabIndex = 7;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(282, 167);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(115, 15);
		this.label32.TabIndex = 96;
		this.label32.Text = "Importer Comments";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 82);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 72;
		this.label10.Text = "Booking Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(282, 224);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 76;
		this.label12.Text = "Remarks";
		this.txtBookingDate.CustomFormat = "dd-MMM-yy";
		this.txtBookingDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBookingDate.Location = new System.Drawing.Point(99, 78);
		this.txtBookingDate.Name = "txtBookingDate";
		this.txtBookingDate.Size = new System.Drawing.Size(176, 23);
		this.txtBookingDate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 110);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 58;
		this.label4.Text = "B/L Number";
		this.txtBLNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBLNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBLNumber.Location = new System.Drawing.Point(99, 106);
		this.txtBLNumber.Name = "txtBLNumber";
		this.txtBLNumber.Size = new System.Drawing.Size(176, 23);
		this.txtBLNumber.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 139);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 15);
		this.label1.TabIndex = 60;
		this.label1.Text = "B/L Date";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(282, 139);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 96;
		this.label24.Text = "Importer";
		this.txtBLDate.CustomFormat = "dd-MMM-yy";
		this.txtBLDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBLDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBLDate.Location = new System.Drawing.Point(99, 135);
		this.txtBLDate.Name = "txtBLDate";
		this.txtBLDate.Size = new System.Drawing.Size(176, 23);
		this.txtBLDate.TabIndex = 4;
		this.txtNotifyParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNotifyParty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNotifyParty.Location = new System.Drawing.Point(398, 106);
		this.txtNotifyParty.Name = "txtNotifyParty";
		this.txtNotifyParty.Size = new System.Drawing.Size(266, 23);
		this.txtNotifyParty.TabIndex = 13;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 167);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 15);
		this.label5.TabIndex = 62;
		this.label5.Text = "Vessel Name";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(282, 110);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(95, 15);
		this.label31.TabIndex = 94;
		this.label31.Text = "Party Comments";
		this.txtVesselName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVesselName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVesselName.Location = new System.Drawing.Point(99, 163);
		this.txtVesselName.Name = "txtVesselName";
		this.txtVesselName.Size = new System.Drawing.Size(176, 23);
		this.txtVesselName.TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 194);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(65, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Voyage No";
		this.txtVoyageNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoyageNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoyageNo.Location = new System.Drawing.Point(99, 190);
		this.txtVoyageNo.Name = "txtVoyageNo";
		this.txtVoyageNo.Size = new System.Drawing.Size(176, 23);
		this.txtVoyageNo.TabIndex = 6;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(282, 82);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(70, 15);
		this.label23.TabIndex = 94;
		this.label23.Text = "Notify Party";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 224);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 15);
		this.label7.TabIndex = 66;
		this.label7.Text = "Freight Type";
		this.txtTotheOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotheOrder.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotheOrder.Location = new System.Drawing.Point(398, 50);
		this.txtTotheOrder.Name = "txtTotheOrder";
		this.txtTotheOrder.Size = new System.Drawing.Size(266, 23);
		this.txtTotheOrder.TabIndex = 11;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 254);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 15);
		this.label8.TabIndex = 68;
		this.label8.Text = "Carier Type";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(282, 54);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(105, 15);
		this.label30.TabIndex = 92;
		this.label30.Text = "To The Order Desc";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 282);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(59, 15);
		this.label11.TabIndex = 74;
		this.label11.Text = "Fob Value";
		this.txtFOBValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFOBValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFOBValue.Location = new System.Drawing.Point(99, 278);
		this.txtFOBValue.Name = "txtFOBValue";
		this.txtFOBValue.Size = new System.Drawing.Size(176, 23);
		this.txtFOBValue.TabIndex = 9;
		this.txtFOBValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFOBValue_KeyPress);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(282, 26);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 15);
		this.label22.TabIndex = 92;
		this.label22.Text = "To The Order";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(1034, 228);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(972, 233);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 39;
		this.label2.Text = "Company";
		this.label2.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance224).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1034, 256);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(972, 261);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 40;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1034, 284);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(972, 289);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 41;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label15);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 55);
		this.panel2.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(11, 31);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(162, 19);
		this.label15.TabIndex = 578;
		this.label15.Text = "Bill Of Lading         ";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.BtnRefresh, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.btnAttachment, this.Print, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "505-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Size = new System.Drawing.Size(956, 372);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 119);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(956, 253);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label43);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 97);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(956, 22);
		this.panel8.TabIndex = 52;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(5, 2);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(132, 18);
		this.label43.TabIndex = 22;
		this.label43.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(910, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 52);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(956, 45);
		this.panel7.TabIndex = 49;
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.label44);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(3, 2);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(950, 43);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(855, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 6;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(778, 17);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 5;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(243, 15);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 15);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateHistory.TabIndex = 0;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(640, 13);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 3;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(373, 19);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "Invoice #";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(704, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 4;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(197, 19);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 19);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbInvoicenoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoicenoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoicenoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoicenoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoicenoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoicenoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory).Location = new System.Drawing.Point(431, 13);
		((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory).Name = "CmbInvoicenoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoicenoHistory).TabIndex = 2;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(956, 52);
		this.panel5.TabIndex = 51;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(3, 30);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(180, 19);
		this.label45.TabIndex = 22;
		this.label45.Text = "Bill Of Lading History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(956, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
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
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.miniToolStrip.Location = new System.Drawing.Point(183, 3);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.miniToolStrip.Size = new System.Drawing.Size(1908, 25);
		this.miniToolStrip.TabIndex = 7;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
		this.toolStrip2.Size = new System.Drawing.Size(1908, 25);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(79, 29);
		this.toolStripButton2.Text = "&New";
		this.toolStripButton2.Visible = false;
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.toolStripButton5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(84, 29);
		this.toolStripButton5.Text = "&Print";
		this.toolStripButton5.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 401);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EximBillOfLading";
		this.Text = "Bill Of Lading";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(EximBillOfLading_FormClosed);
		base.Load += new System.EventHandler(EximShipmentInfo_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EximShipmentInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrencycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmblccontract).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTotheOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcarier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFreight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoicenoHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
