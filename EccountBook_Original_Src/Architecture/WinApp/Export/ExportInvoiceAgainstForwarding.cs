using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class ExportInvoiceAgainstForwarding : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int VoucherHeadId;

	private int Id;

	private bool Approved;

	public static int SupplierId;

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtPartyAddless = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public string GRNS = string.Empty;

	public bool GRNSelected;

	private UserAccount UserAccount = new UserAccount();

	private LoadExportForwarding LoadForwarding;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Panel panel4;

	private Panel panel5;

	private Label label7;

	private Label label8;

	private Label label6;

	private TextBox txtbillno;

	private DateTimePicker txtDocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grd;

	private Panel panel7;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbstockprty;

	internal UltraCombo cmbPartyName;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private TextBox txtDeliveryTerm;

	private Label label15;

	private CheckBox ChkBok;

	private Panel panel15;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnFrmRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker txtDueDate;

	private ToolStripButton btnDelete;

	private Panel panel8;

	private ToolStripLabel toolStripLabel1;

	private Panel panel3;

	internal GridEX grdDetail;

	private RichTextBox txtRemarks;

	private ToolStripButton btnSlip220A;

	private CheckBox ChkBoxSlip;

	private ToolStripButton btn220bSummary;

	private TextBox txtEmptyBagsCut;

	private Label label10;

	private Panel panel9;

	private Panel panel11;

	internal GridEX GrdPartyAddless;

	private Panel panel12;

	private Label label5;

	private Panel panel10;

	internal GridEX grdInvExp;

	private Label label1;

	private TextBox txtItemRateHeader;

	private Label label2;

	private TextBox txtRateUomHeader;

	public ExportInvoiceAgainstForwarding(UserAccount user)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			DocumentNo();
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("InvforwardingId");
			dtGrid.Columns.Add("ReferenceNo");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("PackingType");
			dtGrid.Columns.Add("PackUomId");
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("AddLessWeight", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate");
			dtGrid.Columns.Add("RateUomId");
			dtGrid.Columns.Add("RateUom", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			dtGrid.Columns.Add("BiltyNo");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("WarehouseId");
			dtGrid.Columns.Add("WarehouseName");
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("JobLot");
			dtGrid.Columns.Add("ItemDescription");
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtPartyAddless.Columns.Add("AccountId");
			dtPartyAddless.Columns.Add("Percentage");
			dtPartyAddless.Columns.Add("Qty", typeof(double));
			dtPartyAddless.Columns.Add("Rate");
			dtPartyAddless.Columns.Add("Debit", typeof(double));
			dtPartyAddless.Columns.Add("Credit", typeof(double));
			dtPartyAddless.Columns.Add("Remarks");
			AddRowInPartyAddlessGrid();
			GrdPartyAddless.RetrieveStructure();
			PartyAddlessGridSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("ItemName");
			dtInvExp.Columns.Add("Remarks");
			dtInvExp.Columns.Add("Amount", typeof(double));
			OtherItemsBind();
			SupplierNameFilll();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			ChkBoxSlip.Checked = true;
			ChkBok.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPartyAddlessGrid()
	{
		dtPartyAddless.Rows.Add(0, 0, 0, 0, 0, 0, "");
		GrdPartyAddless.DataSource = dtPartyAddless;
	}

	private void PartyAddlessGridSettings()
	{
		try
		{
			DataTable dtPartyAddlessAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,15");
			GrdPartyAddless.RootTable.Columns["Percentage"].Caption = " % ";
			GrdPartyAddless.RootTable.Columns["Qty"].Width = 70;
			GrdPartyAddless.RootTable.Columns["Credit"].Width = 90;
			GrdPartyAddless.RootTable.Columns["Rate"].Width = 90;
			GrdPartyAddless.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			GrdPartyAddless.RootTable.Columns["AccountId"].HasValueList = true;
			GrdPartyAddless.RootTable.Columns["AccountId"].LimitToList = true;
			GrdPartyAddless.RootTable.Columns["AccountId"].EditType = (EditType)4;
			GrdPartyAddless.RootTable.Columns["AccountId"].Caption = "Party Add/Less";
			GrdPartyAddless.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtPartyAddlessAc.DefaultView, "Id", "AccountTitle");
			GrdPartyAddless.RootTable.Columns["AccountId"].Width = 250;
			GrdPartyAddless.RootTable.Columns.Add("Delete");
			GrdPartyAddless.RootTable.Columns["Delete"].Key = "Delete";
			GrdPartyAddless.RootTable.Columns["Delete"].Caption = "X";
			GrdPartyAddless.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdPartyAddless.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			GrdPartyAddless.RootTable.Columns["Delete"].ButtonText = "X";
			GrdPartyAddless.RootTable.Columns["Delete"].Width = 20;
			GrdPartyAddless.RootTable.Columns.Add("Add");
			GrdPartyAddless.RootTable.Columns["Add"].Key = "Add";
			GrdPartyAddless.RootTable.Columns["Add"].Caption = "+";
			GrdPartyAddless.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdPartyAddless.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			GrdPartyAddless.RootTable.Columns["Add"].ButtonText = "+";
			GrdPartyAddless.RootTable.Columns["Add"].Width = 20;
			GrdPartyAddless.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			GrdPartyAddless.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			GrdPartyAddless.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			GrdPartyAddless.RootTable.Columns["Percentage"].FormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Debit"].FormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Credit"].FormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Rate"].FormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Percentage"].TotalFormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Debit"].TotalFormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Credit"].TotalFormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Rate"].TotalFormatString = "#,##0.##";
			GrdPartyAddless.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdPartyAddless.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			GrdPartyAddless.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			GrdPartyAddless.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			GrdPartyAddless.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdPartyAddless_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = GrdPartyAddless.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					dtPartyAddless.Rows[item.RowIndex].Delete();
					item.Delete();
					BillAmount();
				}
				if (dtPartyAddless.Rows.Count == 0)
				{
					AddRowInPartyAddlessGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPartyAddlessGrid();
			}
			GrdPartyAddless.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdPartyAddless_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdPartyAddless.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double amount = num * Rate;
				item.Cells["Credit"].Value = amount;
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				double num2 = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num2 / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
			{
				item.Cells["Debit"].Value = 0;
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
			{
				item.Cells["Credit"].Value = 0;
			}
			if (e.Column.Key == "AccountId" && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
			{
				MessageBox.Show("You Can No Select Customer Account Here....");
				item.Cells["AccountId"].Value = 0;
			}
			else
			{
				GrdPartyAddless.UpdateData();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].Visible = false;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = "#,#";
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = "#,#";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double num2 = Conversion.ToDouble((object)GrdPartyAddless.GetTotal(GrdPartyAddless.RootTable.Columns["Debit"], (AggregateFunction)2).ToString());
			double PartyAdLsCredit = Conversion.ToDouble((object)GrdPartyAddless.GetTotal(GrdPartyAddless.RootTable.Columns["Credit"], (AggregateFunction)2).ToString());
			double PartyAdlsForbill = num2 - PartyAdLsCredit;
			BillAmount = num - PartyAdlsForbill;
			txtBillAmount.Text = Math.Round(BillAmount).ToString("#,#.##");
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbPartyName).ActiveRow == null)
		{
			MessageBox.Show("Party Name is Required");
			cmbPartyName.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No is Required");
			txtdocno.Focus();
			return false;
		}
		return true;
	}

	private void SupplierNameFilll()
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
				DDL.BindDDLNew(dtSupplier, cmbPartyName, "Id", "CompanyName", "Customer", false);
				((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[1].Width = 480;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOtherItem = InventoryItemsOther.GetAll(new InventoryItemsOther
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			for (int i = 0; i < dtOtherItem.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(dtOtherItem.Rows[i]["Id"], dtOtherItem.Rows[i]["OtherItemName"], "", 0);
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 208,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 208);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsuppliername_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(cmbPartyName.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(cmbPartyName.Value));
				if (dr.Length != 0)
				{
					txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DuedaysCalculates()
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				txtDueDate.Value = txtDocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				txtDueDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DuedaysCalculates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DuedaysCalculates();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			Id = 0;
			VoucherHeadId = 0;
			Approved = false;
			txtDocDate.Focus();
			txtEmptyBagsCut.Text = string.Empty;
			txtItemRateHeader.Text = string.Empty;
			txtRateUomHeader.Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtDeliveryTerm.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtDueDate.Value = DateTime.Now;
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			cmbPartyName.Focus();
			((Control)(object)cmbPartyName).Enabled = true;
			((Control)(object)cmbPartyName).Text = string.Empty;
			txtBillAmount.Text = "0";
			OtherItemReset();
			dtPartyAddless.Clear();
			GrdPartyAddless.ClearStructure();
			AddRowInPartyAddlessGrid();
			GrdPartyAddless.RetrieveStructure();
			PartyAddlessGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemReset()
	{
		try
		{
			if (grdInvExp.GetRows().Length != 0)
			{
				GridEXRow[] rows = grdInvExp.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Amount"].Value = 0;
					obj.Cells["Remarks"].Value = "";
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 208);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportVoucher104()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.VoucherSlipForInventoryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 208,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dt, "104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReportVoucher103();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipDetail_Click(object sender, EventArgs e)
	{
		try
		{
			GeneratereportSlip("220-InvRptPurchaseBillSupplierRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratereportSlip(string ReportName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			if (UserAccount.CompName.ToString() == string.Empty)
			{
				rpt.RptPerameter("@CompanyName", (string)null);
			}
			else
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@BranchesId", (string)null);
			rpt.RptPerameter("@FromDate", (string)null);
			rpt.RptPerameter("@ToDate", (string)null);
			rpt.RptPerameter("@FromDocNo", (string)null);
			rpt.RptPerameter("@ToDocNo", (string)null);
			rpt.RptPerameter("@SupplierCustomerId", (string)null);
			rpt.RptPerameter("@Id", Id.ToString());
			rpt.RptPerameter("@PihId", Id.ToString());
			rpt.ShowReport(ReportName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn104Voucher_Click(object sender, EventArgs e)
	{
		GenerateReportVoucher104();
	}

	private void btn220bSummary_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceDirectSlip_294("319A-ExportSaleInvoiceSlip.rpt", Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip220A_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoiceDirectSlip_294("319-ExportSaleInvoiceSlip.rpt", Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
	}

	private void BillProportion()
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			if (Id == 0)
			{
				throw new Exception("Record Not Update because Record Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Expected O, but got Unknown
		//IL_0a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvSaleInvoice obj = new InvSaleInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdInvExp.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against Expense First");
				}
			}
			obj.DocDate = txtDocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.DocumentTypeId = 208;
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.RemarksHeader = Conversion.ToString((object)txtRemarks.Text.Trim());
			obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			obj.DueDate = Conversion.ToDateTime((object)txtDueDate.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbPartyName.Value);
			obj.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.SupplierInvoiceDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			obj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			if (grd.RowCount > 0)
			{
				int LineId = 1;
				rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
					detail.LineId = LineId;
					detail.InvForwardingDetailId = Conversion.ToInt(r3.Cells["Id"].Value);
					detail.InvForwardingId = Conversion.ToInt(r3.Cells["InvforwardingId"].Value);
					detail.ReferenceNo = Conversion.ToString(r3.Cells["ReferenceNo"].Value);
					detail.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
					detail.GpNo = Conversion.ToInt(r3.Cells["GpNo"].Value);
					if (Conversion.ToInt(r3.Cells["ItemId"].Value) != 0)
					{
						detail.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
						detail.RemarksDetail = Conversion.ToString(r3.Cells["ItemDescription"].Value);
						detail.WarehouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
						detail.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
						detail.CropYear = Conversion.ToString(r3.Cells["CropYear"].Value);
						detail.PackingTypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
						detail.ItemUOMId = Conversion.ToInt(r3.Cells["PackUOMId"].Value);
						detail.ItemQty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
						detail.NetBillWeight = Conversion.ToDouble(r3.Cells["NetBillWeight"].Value);
						detail.AdLsWeight = Conversion.ToDouble(r3.Cells["AddLessWeight"].Value);
						detail.GrossWeight = Conversion.ToDouble(r3.Cells["GrossWeight"].Value);
						detail.NetStockWeight = detail.NetBillWeight;
						detail.GpDate = DateTime.Now;
						if (Conversion.ToDouble(r3.Cells["ItemRate"].Value) != 0.0)
						{
							detail.ItemRate = Conversion.ToDouble(r3.Cells["ItemRate"].Value);
							if (Conversion.ToDouble((object)r3.Cells["RateUom"].Text) != 0.0 && Conversion.ToString((object)r3.Cells["RateUom"].Text) != string.Empty)
							{
								DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
								if (dt.Rows.Count > 0)
								{
									DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r3.Cells["RateUom"].Value) + "'");
									if (dr.Length == 0)
									{
										MessageBox.Show("this RateUom not define please check");
										return;
									}
									detail.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
									detail.RateUOM = Conversion.ToDouble(r3.Cells["RateUOM"].Value);
								}
								detail.ItemAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
								detail.BillAmount = Conversion.ToDouble(r3.Cells["ItemAmount"].Value);
								obj.InvSaleInvoiceDetaillist.Add(detail);
								LineId++;
								continue;
							}
							MessageBox.Show("Rate Uom Feild Required In Detail Grid");
							return;
						}
						MessageBox.Show("Item Rate Feild Required In Detail Grid");
						return;
					}
					MessageBox.Show("Item Name Feild Required In Detail Grid");
					return;
				}
				rows = GrdPartyAddless.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r.Cells["Credit"].Value);
						obj.InvSaleInvoiceJournalList.Add(PJ);
					}
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
					attach.ScreenName = base.Name;
					obj.AttachmentsList.Add(attach);
				}
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					obj.DeleteAttachmentsList = new List<DMSAttachments>();
					for (int j = 0; j < AT.RemovedAttachmentListInUpdateCase.Count; j++)
					{
						DMSAttachments delete = new DMSAttachments();
						delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[j].Path);
						delete.ScreenName = base.Name;
						obj.DeleteAttachmentsList.Add(delete);
					}
				}
				int sucess = InvSaleInvoice.Save(obj);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(Id, base.Name, 208, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
				}
				Reset();
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 208);
				}
				if (ChkBoxSlip.Checked)
				{
					Reporting rpt = new Reporting();
					if (UserAccount.CompName.ToString() == string.Empty)
					{
						rpt.RptPerameter("CompanyName", (string)null);
					}
					else
					{
						rpt.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					}
					rpt.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
					rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
					rpt.RptPerameter("@Id", sucess.ToString());
					rpt.RptPerameter("@PihId", sucess.ToString());
					rpt.ShowReport("319-ExportSaleInvoiceSlip.rpt");
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters EI = new ReportsParameters();
			EI.OrganizationId = UserAccount.OrganizationId;
			EI.CompanyId = UserAccount.CompanyId;
			EI.DocumentTypeId = 208;
			EI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!EI.CanViewAllRecord)
			{
				EI.EntryUser = UserAccount.ID;
			}
			EI.NoOfRecords = NoOfRecords;
			EI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			DataTable dthistory = InvSaleInvoice.FormHistory(EI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtHis = new DataTable();
				dtHis.Columns.Add("RecordNo");
				dtHis.Columns.Add("Id");
				dtHis.Columns.Add("VoucherHeadId");
				dtHis.Columns.Add("DocNo");
				dtHis.Columns.Add("DocDate");
				dtHis.Columns.Add("CustomerName");
				dtHis.Columns.Add("DueDate");
				dtHis.Columns.Add("ManualBillNo");
				dtHis.Columns.Add("SupplierCustomerId");
				dtHis.Columns.Add("RemarksHeader");
				dtHis.Columns.Add("BillAmount", typeof(double));
				dtHis.Columns.Add("EntryDate");
				dtHis.Columns.Add("EntryUser");
				dtHis.Columns.Add("NoOfAttachments");
				dtHis.Clear();
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dtHis.Rows.Add(dthistory.Rows[i]["RecordNo"], dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["CustomerName"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["BillAmount"], Conversion.ToDateTime(dthistory.Rows[i]["EntryDate"]).ToShortDateString(), dthistory.Rows[i]["UserName"], dthistory.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtHis;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
			}
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
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Print";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Print";
			grdHistory.RootTable.Columns["View"].Width = 50;
			grdHistory.RootTable.Columns.Add("Detail");
			grdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdHistory.RootTable.Columns["Detail"].Width = 50;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Width = 80;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns["CustomerName"].Width = 150;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = 150;
			grdHistory.RootTable.Columns["Voucher"].Position = 0;
			grdHistory.RootTable.Columns["View"].Position = 1;
			grdHistory.RootTable.Columns["PartySlip"].Position = 2;
			grdHistory.RootTable.Columns["Detail"].Position = 3;
			grdHistory.RootTable.Columns["Edit"].Position = 4;
			grdHistory.FrozenColumns = 5;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
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

	private void closeHistory_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		GetDetailGrdByHeadId();
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ForwardingId");
			dtdetail.Columns.Add("ForwardingDocNo");
			dtdetail.Columns.Add("ReferenceNo");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("AddLessWeight", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("WareHouse");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("Description");
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].InvForwardingId, obj.InvSaleInvoiceDetaillist[i].ForwardingDocNo, obj.InvSaleInvoiceDetaillist[i].ReferenceNo, obj.InvSaleInvoiceDetaillist[i].ItemId, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].PackTypeDesc, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].AdLsWeight, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].RateUOM, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].CropYear, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].RemarksDetail);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["ForwardingId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["AddLessWeight"].Caption = "Add Less";
			grdDetail.RootTable.Columns["ItemName"].Width = 150;
			grdDetail.RootTable.Columns["ForwardingDocNo"].Width = 120;
			grdDetail.RootTable.Columns["ReferenceNo"].Width = 90;
			grdDetail.RootTable.Columns["ItemUom"].Width = 70;
			grdDetail.RootTable.Columns["RateUom"].Width = 70;
			grdDetail.RootTable.Columns["ItemRate"].Width = 70;
			grdDetail.RootTable.Columns["ItemQty"].Width = 70;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = 90;
			grdDetail.RootTable.Columns["AddLessWeight"].Width = 70;
			grdDetail.RootTable.Columns["GrossWeight"].Width = 90;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(Id);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), 208);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoiceDirectSlip_294("319-ExportSaleInvoiceSlip.rpt", Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "PartySlip")
			{
				CommonServices.SaleInvoiceDirectSlip_294("319A-ExportSaleInvoiceSlip.rpt", Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Detail")
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById_Update(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById_Update(int ID)
	{
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			Id = Conversion.ToInt((object)ID);
			InvSaleInvoice ExpInv = InvSaleInvoice.GetByID(Id);
			txtdocno.Text = ExpInv.DocNo.ToString();
			txtDocDate.Value = ExpInv.DocDate;
			tabControl1.SelectedIndex = 0;
			cmbPartyName.Value = ExpInv.SupplierCustomerId;
			((Control)(object)cmbPartyName).Enabled = false;
			cmbstockprty.Value = ExpInv.StockPartyId;
			txtbillno.Text = ExpInv.ManualBillNo;
			txtRemarks.Text = Conversion.ToString((object)ExpInv.RemarksHeader);
			txtBillAmount.Text = ExpInv.BillAmount.ToString();
			txtDueDate.Value = Conversion.ToDateTime((object)ExpInv.DueDate);
			txtDueDays.Text = ExpInv.DueDays.ToString();
			Approved = ExpInv.IsApproved;
			AttachmentsValues = ExpInv.AttachmentsValues;
			CustomAttachmentsValues = ExpInv.CustomAttachmentsValues;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int j = 0; j < ExpInv.InvSaleInvoiceDetaillist.Count; j++)
			{
				dtGrid.Rows.Add(ExpInv.InvSaleInvoiceDetaillist[j].Id, ExpInv.InvSaleInvoiceDetaillist[j].InvForwardingId, ExpInv.InvSaleInvoiceDetaillist[j].ReferenceNo, ExpInv.InvSaleInvoiceDetaillist[j].ItemId, ExpInv.InvSaleInvoiceDetaillist[j].ItemName, ExpInv.InvSaleInvoiceDetaillist[j].PackingTypeId, ExpInv.InvSaleInvoiceDetaillist[j].PackTypeDesc, ExpInv.InvSaleInvoiceDetaillist[j].ItemUOMId, ExpInv.InvSaleInvoiceDetaillist[j].UOMCodeItem, ExpInv.InvSaleInvoiceDetaillist[j].ItemQty, ExpInv.InvSaleInvoiceDetaillist[j].NetBillWeight, ExpInv.InvSaleInvoiceDetaillist[j].AdLsWeight, ExpInv.InvSaleInvoiceDetaillist[j].GrossWeight, ExpInv.InvSaleInvoiceDetaillist[j].ItemRate, ExpInv.InvSaleInvoiceDetaillist[j].UomScheduleIdRate, ExpInv.InvSaleInvoiceDetaillist[j].RateUOM, ExpInv.InvSaleInvoiceDetaillist[j].ItemAmount, ExpInv.InvSaleInvoiceDetaillist[j].GpNo, ExpInv.InvSaleInvoiceDetaillist[j].VehicleNo, 0, ExpInv.InvSaleInvoiceDetaillist[j].CropYear, ExpInv.InvSaleInvoiceDetaillist[j].WarehouseId, ExpInv.InvSaleInvoiceDetaillist[j].WareHouseName, ExpInv.InvSaleInvoiceDetaillist[j].JobLotId, ExpInv.InvSaleInvoiceDetaillist[j].JobLotDescription, ExpInv.InvSaleInvoiceDetaillist[j].RemarksDetail);
			}
			dtPartyAddless.Rows.Clear();
			GrdPartyAddless.ClearStructure();
			for (int i = 0; i < ExpInv.InvSaleInvoiceJournalList.Count; i++)
			{
				dtPartyAddless.Rows.Add(ExpInv.InvSaleInvoiceJournalList[i].ChartofAccountId, ExpInv.InvSaleInvoiceJournalList[i].JvPrcnt, ExpInv.InvSaleInvoiceJournalList[i].JvQty, ExpInv.InvSaleInvoiceJournalList[i].JvRate, ExpInv.InvSaleInvoiceJournalList[i].JvDebit, ExpInv.InvSaleInvoiceJournalList[i].JvCredit, ExpInv.InvSaleInvoiceJournalList[i].JvRemarks);
			}
			GrdPartyAddless.DataSource = dtPartyAddless;
			GrdPartyAddless.RetrieveStructure();
			PartyAddlessGridSettings();
			txtItemRateHeader.Text = Conversion.ToDouble(grdDetail.CurrentRow.Cells["ItemRate"].Value).ToString();
			txtRateUomHeader.Text = Conversion.ToDouble(grdDetail.CurrentRow.Cells["RateUom"].Value).ToString();
			tabControl1.SelectedIndex = 0;
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.ActionId = 2;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
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
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDocDate.Focus();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				toolStripButton3_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells[1].Value.ToString());
				ReadById_Update(Id);
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdInvExp).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.C)
			{
				_ = tabControl1.SelectedIndex;
			}
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				LoadForwarding = new LoadExportForwarding(UserAccount);
				LoadForwarding.ShowDialog();
				LoadForwarding.BringToFront();
				LoadInGridDetail();
			}
			else
			{
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridAgainstForwarding(string IDs)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 206;
			obj.Ids = IDs;
			DataTable DtForWarding = new DataTable();
			if (obj.Ids != string.Empty)
			{
				DtForWarding = ExImForwarding.forwardingPendingForSaleInvoice(obj);
			}
			if (DtForWarding.Rows.Count > 0)
			{
				cmbPartyName.Value = DtForWarding.Rows[0]["SupplierCustomerId"];
				((Control)(object)cmbPartyName).Enabled = false;
				for (int i = 0; i < DtForWarding.Rows.Count; i++)
				{
					dtGrid.Rows.Add(DtForWarding.Rows[i]["Id"], DtForWarding.Rows[i]["ExImForwardingId"], DtForWarding.Rows[i]["ReferenceNo"], DtForWarding.Rows[i]["ItemId"], DtForWarding.Rows[i]["ItemName"], DtForWarding.Rows[i]["PackingMaterialId"], DtForWarding.Rows[i]["PackingType"], DtForWarding.Rows[i]["UOMScheduleIdOuter"], DtForWarding.Rows[i]["PackUOM"], DtForWarding.Rows[i]["Qty"], DtForWarding.Rows[i]["NetWeight"], DtForWarding.Rows[i]["AdLsWeight"], DtForWarding.Rows[i]["GrossWeight"], 0, 0, 40, 0, DtForWarding.Rows[i]["GpNo"], DtForWarding.Rows[i]["VehicleNo"], DtForWarding.Rows[i]["BiltyNo"], DtForWarding.Rows[i]["CropYear"], DtForWarding.Rows[i]["WarehouseId"], DtForWarding.Rows[i]["WareHouseName"], DtForWarding.Rows[i]["LotJobId"], DtForWarding.Rows[i]["JobLotDescription"], DtForWarding.Rows[i]["ItemDescription"]);
				}
			}
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
			dtGrid.Rows.Clear();
			string ForwarWadingIds = "";
			for (int i = 0; i < LoadForwarding.dtSupply.Rows.Count; i++)
			{
				ForwarWadingIds = ForwarWadingIds + "," + Conversion.ToInt((object)LoadForwarding.dtSupply.Rows[i]["Id"].ToString());
			}
			LoadDataDetailGridAgainstForwarding(ForwarWadingIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			BillProportion();
			BillAmount();
			ForwarWadingIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "ItemRate" || e.Column.Key == "RateUom" || e.Column.Key == "AddLessWeight") && item != null && (int)item.RowType == 0)
			{
				string obj = item.Cells["GrossWeight"].Value.ToString();
				string AdLsWeight = item.Cells["AddLessWeight"].Value.ToString();
				double NetBillWeight = Conversion.ToDouble((object)obj) + Conversion.ToDouble((object)AdLsWeight);
				item.Cells["NetBillWeight"].Value = NetBillWeight;
				string EquivalentPoRate = item.Cells["RateUom"].Text.ToString();
				string ItemRate = item.Cells["ItemRate"].Value.ToString();
				double ItemAmount = Conversion.ToDouble((object)item.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
				item.Cells["ItemAmount"].Value = ItemAmount;
			}
			grd.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (e.KeyChar.ToString() == "RateUom" || e.KeyChar.ToString() == "ItemRate")
			{
				CommonServices.OnlytextdecimelFunction(sender, e);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["InvForwardingId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["AddLessWeight"].Caption = "AddLess";
			grd.RootTable.Columns["ItemName"].Width = 180;
			grd.RootTable.Columns["WarehouseName"].Width = 120;
			grd.RootTable.Columns["RateUom"].Width = 70;
			grd.RootTable.Columns["PackUom"].Width = 70;
			grd.RootTable.Columns["ItemQty"].Width = 70;
			grd.RootTable.Columns["ItemRate"].Width = 70;
			grd.RootTable.Columns["GrossWeight"].Width = 90;
			grd.RootTable.Columns["NetBillWeight"].Width = 95;
			grd.RootTable.Columns["GpNo"].Width = 60;
			grd.RootTable.Columns["VehicleNo"].Width = 85;
			grd.RootTable.Columns["BiltyNo"].Width = 80;
			grd.RootTable.Columns["CropYear"].Width = 80;
			grd.RootTable.Columns["AddLessWeight"].Width = 70;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AddLessWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].FormatString = "0.00;(0.00);0";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferenceNo"].EditType = (EditType)0;
			grd.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			grd.RootTable.Columns["GpNo"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["InvForwardingId"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLessWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)1;
			grd.RootTable.Columns["RateUom"].EditType = (EditType)1;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			ctrlGrdBar1_Load(null, null);
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			GetAll(50);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFilll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				grd.UpdateData();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double ItemRateHeader = Conversion.ToDouble((object)txtItemRateHeader.Text);
					obj.Cells["ItemRate"].Value = Conversion.ToString((object)ItemRateHeader);
					string GrossWeight = obj.Cells["GrossWeight"].Value.ToString();
					string AdLsWeight = obj.Cells["AddLessWeight"].Value.ToString();
					double NetBillWeight = Conversion.ToDouble((object)GrossWeight) + Conversion.ToDouble((object)AdLsWeight);
					obj.Cells["NetBillWeight"].Value = NetBillWeight;
					string EquivalentPoRate = obj.Cells["RateUom"].Text.ToString();
					string ItemRate = obj.Cells["ItemRate"].Value.ToString();
					double ItemAmount = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
					obj.Cells["ItemAmount"].Value = ItemAmount;
					obj.EndEdit();
				}
				grd.UpdateData();
			}
			BillAmount();
			PartyAddlessGridAmountAgainstPrcnt();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyAddlessGridAmountAgainstPrcnt()
	{
		try
		{
			GridEXRow item = GrdPartyAddless.CurrentRow;
			if (Conversion.ToInt((object)item.Cells["Percentage"].Value.ToString()) != 0)
			{
				if (Math.Abs(Conversion.ToInt(item.Cells["Percentage"].Value)) > 100)
				{
					item.Cells["Percentage"].Value = 0;
					throw new Exception("Percentage mustbe less than 100");
				}
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			GrdPartyAddless.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
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

	private void textBox1_TextChanged_1(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				grd.UpdateData();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double RateUomHeader = Conversion.ToDouble((object)txtRateUomHeader.Text);
					obj.Cells["RateUom"].Value = Conversion.ToString((object)RateUomHeader);
					string GrossWeight = obj.Cells["GrossWeight"].Value.ToString();
					string AdLsWeight = obj.Cells["AddLessWeight"].Value.ToString();
					double NetBillWeight = Conversion.ToDouble((object)GrossWeight) + Conversion.ToDouble((object)AdLsWeight);
					obj.Cells["NetBillWeight"].Value = NetBillWeight;
					string EquivalentPoRate = obj.Cells["RateUom"].Text.ToString();
					string ItemRate = obj.Cells["ItemRate"].Value.ToString();
					double ItemAmount = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
					obj.Cells["ItemAmount"].Value = ItemAmount;
					obj.EndEdit();
				}
				grd.UpdateData();
			}
			BillAmount();
			PartyAddlessGridAmountAgainstPrcnt();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportInvoiceAgainstForwarding_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c2: Expected O, but got Unknown
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Expected O, but got Unknown
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Expected O, but got Unknown
		//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Expected O, but got Unknown
		//IL_3174: Unknown result type (might be due to invalid IL or missing references)
		//IL_317e: Expected O, but got Unknown
		//IL_31a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ac: Expected O, but got Unknown
		GridEXLayout GrdPartyAddless_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExportInvoiceAgainstForwarding));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdPartyAddless = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.txtRateUomHeader = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtItemRateHeader = new System.Windows.Forms.TextBox();
		this.txtEmptyBagsCut = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.ChkBoxSlip = new System.Windows.Forms.CheckBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtDueDate = new System.Windows.Forms.DateTimePicker();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbstockprty = new UltraCombo();
		this.cmbPartyName = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlip220A = new System.Windows.Forms.ToolStripButton();
		this.btn220bSummary = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdPartyAddless).BeginInit();
		this.panel12.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1145, 692);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1145, 692);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1137, 666);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 121);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1131, 542);
		this.panel5.TabIndex = 3;
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 324);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1131, 218);
		this.panel8.TabIndex = 4;
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1131, 218);
		this.panel9.TabIndex = 1;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.GrdPartyAddless);
		this.panel11.Controls.Add(this.panel12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(828, 218);
		this.panel11.TabIndex = 2;
		((ScrollableBase)this.GrdPartyAddless).BackColor = System.Drawing.Color.White;
		this.GrdPartyAddless.ColumnAutoResize = true;
		GrdPartyAddless_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdPartyAddless.DesignTimeLayout = GrdPartyAddless_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdPartyAddless.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).Name = "GrdPartyAddless";
		((ScrollableBase)this.GrdPartyAddless).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).Size = new System.Drawing.Size(828, 192);
		((System.Windows.Forms.Control)(object)this.GrdPartyAddless).TabIndex = 0;
		this.GrdPartyAddless.TotalRow = (InheritableBoolean)1;
		this.GrdPartyAddless.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdPartyAddless).VisualStyle = (VisualStyle)4;
		this.GrdPartyAddless.CellUpdated += new ColumnActionEventHandler(GrdPartyAddless_CellUpdated);
		this.GrdPartyAddless.ColumnButtonClick += new ColumnActionEventHandler(GrdPartyAddless_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label5);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(828, 26);
		this.panel12.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(213, 20);
		this.label5.TabIndex = 1;
		this.label5.Text = "Debit Or Credit Against Party";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel10.Location = new System.Drawing.Point(834, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(297, 218);
		this.panel10.TabIndex = 1;
		this.panel10.Visible = false;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(297, 218);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabStop = false;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1131, 324);
		this.panel7.TabIndex = 2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1131, 324);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grd).KeyPress += new System.Windows.Forms.KeyPressEventHandler(grd_KeyPress);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtRateUomHeader);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtItemRateHeader);
		this.panel4.Controls.Add(this.txtEmptyBagsCut);
		this.panel4.Controls.Add(this.txtBillAmount);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.ChkBoxSlip);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.txtDueDays);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.txtDueDate);
		this.panel4.Controls.Add(this.ChkBok);
		this.panel4.Controls.Add(this.txtDeliveryTerm);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyName);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtbillno);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1131, 90);
		this.panel4.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(664, 66);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 37;
		this.label2.Text = "Rate Uom";
		this.txtRateUomHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateUomHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateUomHeader.Location = new System.Drawing.Point(774, 61);
		this.txtRateUomHeader.Name = "txtRateUomHeader";
		this.txtRateUomHeader.Size = new System.Drawing.Size(116, 22);
		this.txtRateUomHeader.TabIndex = 36;
		this.txtRateUomHeader.TextChanged += new System.EventHandler(textBox1_TextChanged_1);
		this.txtRateUomHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress_1);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(664, 41);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 35;
		this.label1.Text = "Item Rate";
		this.txtItemRateHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRateHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRateHeader.Location = new System.Drawing.Point(774, 36);
		this.txtItemRateHeader.Name = "txtItemRateHeader";
		this.txtItemRateHeader.Size = new System.Drawing.Size(116, 22);
		this.txtItemRateHeader.TabIndex = 5;
		this.txtItemRateHeader.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.txtItemRateHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.txtEmptyBagsCut.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsCut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsCut.Location = new System.Drawing.Point(866, 144);
		this.txtEmptyBagsCut.Name = "txtEmptyBagsCut";
		this.txtEmptyBagsCut.Size = new System.Drawing.Size(83, 22);
		this.txtEmptyBagsCut.TabIndex = 6;
		this.txtEmptyBagsCut.Visible = false;
		this.txtBillAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Enabled = false;
		this.txtBillAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(774, 8);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(116, 25);
		this.txtBillAmount.TabIndex = 21;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(662, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(106, 17);
		this.label3.TabIndex = 17;
		this.label3.Text = "Invoice Amount";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(863, 125);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(82, 13);
		this.label10.TabIndex = 34;
		this.label10.Text = "Empty Bags Cut";
		this.label10.Visible = false;
		this.ChkBoxSlip.AutoSize = true;
		this.ChkBoxSlip.Checked = true;
		this.ChkBoxSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBoxSlip.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBoxSlip.Location = new System.Drawing.Point(895, 62);
		this.ChkBoxSlip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxSlip.Name = "ChkBoxSlip";
		this.ChkBoxSlip.Size = new System.Drawing.Size(74, 19);
		this.ChkBoxSlip.TabIndex = 7;
		this.ChkBoxSlip.Text = "Slip Print";
		this.ChkBoxSlip.UseVisualStyleBackColor = true;
		this.txtRemarks.Location = new System.Drawing.Point(377, 35);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(281, 48);
		this.txtRemarks.TabIndex = 4;
		this.txtRemarks.Text = "";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.txtDueDays.Location = new System.Drawing.Point(102, 35);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(64, 22);
		this.txtDueDays.TabIndex = 1;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(6, 39);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(98, 15);
		this.label22.TabIndex = 32;
		this.label22.Text = "Due Days && Date";
		this.txtDueDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDueDate.Enabled = false;
		this.txtDueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.txtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDate.Location = new System.Drawing.Point(169, 35);
		this.txtDueDate.Name = "txtDueDate";
		this.txtDueDate.Size = new System.Drawing.Size(129, 22);
		this.txtDueDate.TabIndex = 3;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Checked = true;
		this.ChkBok.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBok.Location = new System.Drawing.Point(895, 39);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(98, 19);
		this.ChkBok.TabIndex = 6;
		this.ChkBok.Text = "Voucher Print";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.txtDeliveryTerm.BackColor = System.Drawing.Color.White;
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(952, 144);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.ReadOnly = true;
		this.txtDeliveryTerm.Size = new System.Drawing.Size(109, 22);
		this.txtDeliveryTerm.TabIndex = 7;
		this.txtDeliveryTerm.TabStop = false;
		this.txtDeliveryTerm.Visible = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(949, 125);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(72, 13);
		this.label15.TabIndex = 28;
		this.label15.Text = "Delivery Term";
		this.label15.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1358, 11);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(44, 26);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1358, 40);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(44, 26);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1351, 99);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(52, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.cmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Location = new System.Drawing.Point(377, 7);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Name = "cmbPartyName";
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Size = new System.Drawing.Size(281, 24);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).TabIndex = 3;
		this.cmbPartyName.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(305, 12);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Party Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(305, 39);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 20;
		this.label4.Text = "Remarks";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1348, 74);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(6, 65);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual Bill No";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(102, 61);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(196, 22);
		this.txtbillno.TabIndex = 2;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(169, 8);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(129, 22);
		this.txtDocDate.TabIndex = 0;
		this.txtDocDate.TabStop = false;
		this.txtDocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(102, 8);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(64, 22);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 12);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1131, 28);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1082, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.toolStripLabel1, this.toolStripSeparator1, this.btnNew, this.btnUpdate, this.btnSave, this.toolStripSeparator, this.btnDelete, this.btnAttachment, this.btnFrmRefresh, this.toolStripButton3,
			this.btnPrint, this.btnSlip220A, this.btn220bSummary
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1131, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(156, 24);
		this.toolStripLabel1.Text = "Export Invoice         ";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(99, 24);
		this.toolStripButton3.Text = "Load &Farwarding";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(97, 24);
		this.btnPrint.Text = "103-&Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSlip220A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip220A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip220A.Name = "btnSlip220A";
		this.btnSlip220A.Size = new System.Drawing.Size(76, 24);
		this.btnSlip220A.Text = "319- Slip";
		this.btnSlip220A.Click += new System.EventHandler(btnSlip220A_Click);
		this.btn220bSummary.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn220bSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn220bSummary.Name = "btn220bSummary";
		this.btn220bSummary.Size = new System.Drawing.Size(81, 24);
		this.btn220bSummary.Text = "319A-Slip";
		this.btn220bSummary.Click += new System.EventHandler(btn220bSummary_Click);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1137, 666);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 459);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1131, 204);
		this.panel3.TabIndex = 31;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1131, 204);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 65);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1131, 394);
		this.panel15.TabIndex = 30;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1131, 394);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 44;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 31);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1131, 34);
		this.panel14.TabIndex = 29;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(14, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(192, 24);
		this.label9.TabIndex = 24;
		this.label9.Text = "Export Invoice History";
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1131, 28);
		this.panel6.TabIndex = 28;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1081, -2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1131, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1145, 692);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ExportInvoiceAgainstForwarding";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExportInvoiceAgainstForwarding_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdPartyAddless).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
