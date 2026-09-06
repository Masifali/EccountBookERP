using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Import;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Import;

public class ImPurchaseOrderShipmentPackingList : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private int RecId;

	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	public DataTable dtGPGRNS = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton Print;

	private Panel panel13;

	private Panel panel18;

	private Panel panel17;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip3;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDefinePaymentTerm;

	internal GridEX grdDetails;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label16;

	private TextBox txtQtyMTon;

	private Label label13;

	internal UltraCombo combrateuom;

	private Label label10;

	internal UltraCombo combpcktype;

	private Label label2;

	internal UltraCombo combitempck;

	private Label label1;

	internal UltraCombo combitem;

	private TextBox txtExportSoNo;

	internal UltraCombo CmbFCYCode;

	private Label label20;

	private Label label6;

	private Label label21;

	private Label label11;

	private Label label3;

	private Label label4;

	internal UltraCombo CmbExporterbank;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	internal UltraCombo CmbPaymentTerms;

	internal UltraCombo CmbDeliveryTerm;

	private DateTimePicker datContractDate;

	private DateTimePicker txtDocDate;

	internal UltraCombo CmbCustomer;

	private TextBox txtFCYAmount;

	private Label label27;

	private Label label32;

	private TextBox txtNoOfContainers;

	private RichTextBox txtProductSpecification;

	private Label label12;

	private GroupBox groupBox1;

	private TextBox txtNoOfBags;

	private Label label23;

	private TextBox txtCostMTon;

	private Label label25;

	private TextBox txtAmount;

	private Label label33;

	private TextBox txtRemarks;

	private Label label34;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private Label label14;

	private Label label7;

	private Label label5;

	internal UltraCombo cmbStatus;

	private Panel panel6;

	private Button btnclose;

	private Label label18;

	private Label label40;

	private Label label37;

	private Label label36;

	private Label label35;

	private Label label19;

	private Label label38;

	private Label label48;

	private Label label47;

	private Label label46;

	private Label label45;

	private Label label42;

	private Label label41;

	private Label label49;

	private Panel panel5;

	private Label label51;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel7;

	private Label label9;

	private Label label58;

	internal UltraCombo cmbLcContractNo;

	private TextBox txtSupplierInvoiceNo;

	private Label label26;

	private Label label39;

	private TextBox txtgrosswt;

	private TextBox txtpackwt;

	private Label label29;

	internal UltraCombo cmbjoblot;

	private Label label52;

	internal UltraCombo cmbWareHouse;

	private Label label53;

	private TextBox txtcontainerNo;

	private Label label54;

	private Label label15;

	private Label label57;

	private Label label56;

	private Label label55;

	private CtrlGrdBar ctrlGrdBar2;

	private DateTimePicker datSupplierInvoiceDate;

	private Label label17;

	internal UltraCombo CmbShippingAgent;

	private Label label61;

	internal UltraCombo CmbClearingAgent;

	private Label label60;

	internal UltraCombo CmbTransporter;

	private Label label59;

	private TextBox txtTotalAmount;

	private Label label50;

	private TextBox txtExchangeRate;

	private Label label43;

	internal UltraCombo CmbHomeCurrency;

	private Label label31;

	private Label label28;

	private Label label24;

	private DateTimePicker datETDDate;

	private Label label67;

	private DateTimePicker datETADate;

	private Label label30;

	private DateTimePicker datArrivalDate;

	private Label label65;

	private DateTimePicker datBillOfLadingDate;

	private Label label44;

	private Label label8;

	private Label label66;

	private Label label64;

	private Label label22;

	private Label label63;

	private Label label62;

	private TextBox txtVoyageNo;

	private TextBox txtVessel;

	private TextBox txtTransirtDays;

	private TextBox txtBillOfLadingNo;

	private TextBox txtNetWeight;

	private TextBox txtGrossWeight;

	private ToolStripButton btnLoadAll;

	internal UltraCombo CmbOrderNo;

	public ImPurchaseOrderShipmentPackingList(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "frmImportOrderPackingDetail",
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			Print.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			((Control)(object)combitem).Enabled = false;
			((Control)(object)combpcktype).Enabled = false;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtpcktype.Columns.Add("id");
			dtpcktype.Columns.Add("Description");
			GenerateCode();
			MultiCurrencyBind();
			SupplierFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
			WareHouseFill();
			JobLotFill();
			StatusFill();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId");
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("PackWeight", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("RateUOMId");
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("HomeCurrencyAmount", typeof(double));
			dtdetail.Columns.Add("WarehouseId");
			dtdetail.Columns.Add("WarehouseName");
			dtdetail.Columns.Add("JobLotId");
			dtdetail.Columns.Add("JobLotName");
			dtdetail.Columns.Add("ContainerNo");
			dtdetail.Columns.Add("Remarks");
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			CmbCustomer.Focus();
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
			grdDetails.RootTable.Columns["ItemName"].Width = 220;
			grdDetails.RootTable.Columns["PackType"].Width = 150;
			grdDetails.RootTable.Columns["ItemId"].Visible = false;
			grdDetails.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetails.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetails.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetails.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetails.RootTable.Columns["JobLotId"].Visible = false;
			grdDetails.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PackSizeId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PackType"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Qty/M.Ton"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["NoOfBags"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Cost/M.Ton"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["HomeCurrencyAmount"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["PackWeight"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["JobLotName"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["ContainerNo"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdDetails.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Qty/M.Ton"].FormatString = "#,#.##";
			grdDetails.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,#.##";
			grdDetails.RootTable.Columns["HomeCurrencyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["HomeCurrencyAmount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["HomeCurrencyAmount"].FormatString = "#,#";
			grdDetails.RootTable.Columns["HomeCurrencyAmount"].TotalFormatString = "#,#";
			grdDetails.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["NoOfBags"].FormatString = "#,#";
			grdDetails.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Amount"].FormatString = "#,#";
			grdDetails.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Cost/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Cost/M.Ton"].FormatString = "#,#";
			grdDetails.RootTable.Columns["Cost/M.Ton"].FormatString = "#,#.##";
			grdDetails.RootTable.Columns["Cost/M.Ton"].AggregateFunction = (AggregateFunction)3;
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Width = 20;
			grdDetails.RootTable.Columns.Add("Add");
			grdDetails.RootTable.Columns["Add"].Key = "Add";
			grdDetails.RootTable.Columns["Add"].Caption = "+";
			grdDetails.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Add"].ButtonText = "+";
			grdDetails.RootTable.Columns["Add"].Width = 20;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdDetails.CurrentRow;
				dtdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			if (e.Column.Key == "Add")
			{
				dtdetail.Rows.Add(grdDetails.CurrentRow.Cells["ItemId"].Value, grdDetails.CurrentRow.Cells["ItemName"].Value, grdDetails.CurrentRow.Cells["PackTypeId"].Value, grdDetails.CurrentRow.Cells["PackType"].Value, grdDetails.CurrentRow.Cells["Qty/M.Ton"].Value, grdDetails.CurrentRow.Cells["PackSizeId"].Value, grdDetails.CurrentRow.Cells["PackSize"].Value, grdDetails.CurrentRow.Cells["NoOfBags"].Value, grdDetails.CurrentRow.Cells["PackWeight"].Value, grdDetails.CurrentRow.Cells["GrossWeight"].Value, grdDetails.CurrentRow.Cells["Cost/M.Ton"].Value, grdDetails.CurrentRow.Cells["RateUOMId"].Value, grdDetails.CurrentRow.Cells["RateUOM"].Value, grdDetails.CurrentRow.Cells["Amount"].Value, grdDetails.CurrentRow.Cells["HomeCurrencyAmount"].Value, grdDetails.CurrentRow.Cells["WarehouseId"].Value, grdDetails.CurrentRow.Cells["WarehouseName"].Value, grdDetails.CurrentRow.Cells["JobLotId"].Value, grdDetails.CurrentRow.Cells["JobLotName"].Value, grdDetails.CurrentRow.Cells["ContainerNo"].Value, grdDetails.CurrentRow.Cells["Remarks"].Value);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
			grdDetails.UpdateData();
			InsterAmountandQtyMtonInHeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtExportSoNo.Text.Trim() == string.Empty || txtExportSoNo.Text.Trim() == "0")
		{
			MessageBox.Show("Import Po No Is required");
			txtExportSoNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null)
		{
			MessageBox.Show("Supplier Is required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderNo).ActiveRow == null)
		{
			MessageBox.Show("OrderNo Is required");
			CmbOrderNo.Focus();
			return false;
		}
		if (datContractDate.Text.Trim() == string.Empty)
		{
			MessageBox.Show("OrderDate Is required");
			datContractDate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbImporterBank).ActiveRow == null)
		{
			MessageBox.Show("ImporterBank Field  Required");
			CmbImporterBank.Focus();
			return false;
		}
		if (((UltraGridBase)CmbExporterbank).ActiveRow == null)
		{
			MessageBox.Show("Exporterbank Field  Required");
			CmbExporterbank.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null)
		{
			MessageBox.Show("DeliveryTerms Is  required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerms).ActiveRow == null)
		{
			MessageBox.Show("PaymentTerms Is required");
			CmbPaymentTerms.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLoadingPort).ActiveRow == null)
		{
			MessageBox.Show("LoadingPort Is required");
			CmbLoadingPort.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDestinationPort).ActiveRow == null)
		{
			MessageBox.Show("Destination Port  Is required");
			CmbDestinationPort.Focus();
			return false;
		}
		if (((UltraGridBase)CmbFCYCode).ActiveRow == null)
		{
			MessageBox.Show("Fcy Code Is  required");
			CmbFCYCode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFCYAmount.Text) == 0.0)
		{
			MessageBox.Show("FC Amount Must Be Greater Than 0 ThankYou");
			txtFCYAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbHomeCurrency).ActiveRow == null)
		{
			MessageBox.Show("HomeCurrency field required");
			CmbHomeCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExchangeRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ExchangeRate Must Be Greater Than 0 ThankYou");
			txtExchangeRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTotalAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Must Be Greater Than 0 ThankYou");
			txtTotalAmount.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNoOfContainers.Text) > 0.0) || txtNoOfContainers.Text.Trim() == string.Empty)
		{
			MessageBox.Show("No Of Container Must Be Greater Than 0 ThankYou");
			txtNoOfContainers.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null)
		{
			MessageBox.Show("PurchaseOrder Status Is  required");
			cmbStatus.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((UltraGridBase)combitem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null)
		{
			MessageBox.Show("Pack Type field required");
			combitempck.Focus();
			return false;
		}
		if (txtQtyMTon.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQtyMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty/M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null)
		{
			MessageBox.Show("Pack Size field required");
			combitempck.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) == 0.0)
		{
			MessageBox.Show("No of Packs field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (txtgrosswt.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtgrosswt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight field required");
			txtgrosswt.Focus();
			return false;
		}
		if (txtCostMTon.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtCostMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Cost/M.Ton field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)combrateuom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM field required");
			combrateuom.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouse).ActiveRow == null)
		{
			MessageBox.Show("Warehouse field required");
			cmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbjoblot).ActiveRow == null)
		{
			MessageBox.Show("Joblot field required");
			cmbjoblot.Focus();
			return false;
		}
		if (txtcontainerNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Container No field required");
			txtcontainerNo.Focus();
			return false;
		}
		return true;
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
			int GenerateCode = ExImLcOrderPurchaseOrder.GenerateCode(new ExImLcOrderPurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 232
			});
			if (GenerateCode > 0)
			{
				txtExportSoNo.Text = GenerateCode.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
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
				DDL.BindDDLNew(dtSupplier, CmbCustomer, "Id", "CompanyName", "Customer", false);
				DDL.BindDDLNew(dtSupplier, CmbTransporter, "Id", "CompanyName", "Transporter", false);
				DDL.BindDDLNew(dtSupplier, CmbClearingAgent, "Id", "CompanyName", "ClearingAgent", false);
				DDL.BindDDLNew(dtSupplier, CmbShippingAgent, "Id", "CompanyName", "ShippingAgent", false);
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomer_TextChanged(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbCustomer.Value) <= 0)
			{
				return;
			}
			((UltraGridBase)cmbLcContractNo).DataSource = null;
			((Control)(object)cmbLcContractNo).Text = string.Empty;
			DataTable dtlcorder = ImLcOrder.GetLcOrderNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 231
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("LcOrderNo");
			if (dtlcorder.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtlcorder.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtlcorder.Rows[i]["SupCustId"]) == Conversion.ToInt(CmbCustomer.Value))
				{
					dt.Rows.Add(dtlcorder.Rows[i]["Id"], dtlcorder.Rows[i]["LcOrderNo"]);
				}
			}
			DDL.BindDDL(dt, cmbLcContractNo, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLcContractNo_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbLcContractNo).ActiveRow != null)
			{
				DataTable dt = ExImLcOrderPurchaseOrder.GetContractNoForPackingList(new ExImLcOrderPurchaseOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(cmbLcContractNo.Value)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbOrderNo, "Id", "OrderNo", "OrderNo", ZeroIndex: false);
					return;
				}
				((Control)(object)CmbOrderNo).Text = string.Empty;
				((UltraGridBase)CmbOrderNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FiledsClearOnLcorderChange()
	{
		try
		{
			datContractDate.Value = Conversion.ToDateTime((object)DateTime.Now);
			((Control)(object)CmbPaymentTerms).Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			((Control)(object)CmbExporterbank).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			((Control)(object)CmbFCYCode).Text = string.Empty;
			txtFCYAmount.Text = Conversion.ToString((object)0);
			txtProductSpecification.Text = string.Empty;
			txtSupplierInvoiceNo.Text = string.Empty;
			((Control)(object)cmbStatus).Text = string.Empty;
			grdDetails.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCurrency = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCurrency, CmbFCYCode, "Id", "CurrencyCode", "FcYCurrency", false);
				DDL.BindDDLNew(dtCurrency, CmbHomeCurrency, "Id", "CurrencyCode", "HomeCurrency", false);
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
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, CmbDeliveryTerm, "Id", "Code", "DeliveryTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, CmbPaymentTerms, "Id", "LcOrderTerm", "PaymentTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, CmbDestinationPort, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbFCYCode, "Id", "CurrencyName", "CurrencyName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BranchName");
			DataTable dtimporter = new DataTable();
			dtimporter.Columns.Add("Id");
			dtimporter.Columns.Add("BranchName");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dt.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				if (lst[i].IsHomeland == "Home Country")
				{
					dtimporter.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbExporterbank, "Id", "BranchName", "ExporterBank", ZeroIndex: false);
				DDL.BindDDL(dtimporter, CmbImporterBank, "Id", "BranchName", "ImporterBank", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, cmbWareHouse, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			DataTable dtJobLoat = CommonServices.JobLotGetAllService();
			if (dtJobLoat.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLoat, cmbjoblot, "Id", "JobLotDescription", "JobLotDescription", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable lst = CommonServices.ItemGetAllServiceBind();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, combitem, "Id", "ItemName", "Item Name", false);
			}
			((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 400;
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
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
			DDL.BindDDL(dtstatus, cmbStatus, "Id", "Status", "Order Status", ZeroIndex: false);
			((UltraGridBase)cmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPackingTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				dtpcktype.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"]);
				}
				if (dtpcktype.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtpcktype, combpcktype, "Id", "Description", "Description", false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			listUOM = UOMSchedule.Getall(obj);
			if (listUOM.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItemPackUOM = new DataTable();
			dtItemPackUOM.Columns.Add("Id");
			dtItemPackUOM.Columns.Add("UOM");
			dtItemPackUOM.Columns.Add("Equivalent");
			dtItemPackUOM.Rows.Clear();
			for (int i = 0; i < listUOM.Rows.Count; i++)
			{
				if (Conversion.ToInt(listUOM.Rows[i]["ItemId"]) == Conversion.ToInt(combitem.Value))
				{
					dtItemPackUOM.Rows.Add(listUOM.Rows[i]["Id"], listUOM.Rows[i]["UOMDescription"], listUOM.Rows[i]["Equivalent"]);
				}
				if (dtItemPackUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtItemPackUOM, combitempck, "Id", "UOM", "PackUom", ZeroIndex: false);
					DDL.BindDDL(dtItemPackUOM, combrateuom, "Id", "UOM", "RateUom", ZeroIndex: false);
					((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
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
			ExImLcOrderPurchaseOrder obj = new ExImLcOrderPurchaseOrder();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			obj.DocNo = Conversion.ToInt((object)txtExportSoNo.Text.Trim());
			obj.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value);
			obj.ExImLcOrderId = Conversion.ToInt(cmbLcContractNo.Value);
			obj.DocNoByLcOrder = Conversion.ToInt(CmbOrderNo.Value);
			obj.SupplierOrderDate = Conversion.ToDateTime((object)datContractDate.Value);
			obj.SupplierInvoiceNo = Conversion.ToString((object)txtSupplierInvoiceNo.Text.Trim());
			obj.SupplierInvoiceDate = Conversion.ToDateTime((object)datSupplierInvoiceDate.Value);
			obj.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
			obj.ExporterBankId = Conversion.ToInt(CmbExporterbank.Value);
			obj.PaymentTermId = Conversion.ToInt(CmbPaymentTerms.Value);
			obj.FcyId = Conversion.ToInt(CmbFCYCode.Value);
			obj.FcyAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.HomeCurrencyId = Conversion.ToInt(CmbHomeCurrency.Value);
			obj.ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text.Trim());
			obj.Amount = Conversion.ToDouble((object)txtTotalAmount.Text.Trim());
			obj.TransporterId = Conversion.ToInt(CmbTransporter.Value);
			obj.ClearingAgentId = Conversion.ToInt(CmbClearingAgent.Value);
			obj.ShippingAgentId = Conversion.ToInt(CmbShippingAgent.Value);
			obj.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			obj.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
			obj.Containers = Conversion.ToInt((object)txtNoOfContainers.Text);
			obj.GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			obj.NetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			obj.BillOfLadingNo = Conversion.ToString((object)txtBillOfLadingNo.Text.Trim());
			obj.BillOfLadingDate = Conversion.ToDateTime((object)datBillOfLadingDate.Value);
			obj.TransitDays = Conversion.ToInt((object)txtTransirtDays.Text.Trim());
			obj.ArrivalDate = Conversion.ToDateTime((object)datArrivalDate.Value);
			obj.Vessel = Conversion.ToString((object)txtVessel.Text.Trim());
			obj.VoyageNo = Conversion.ToString((object)txtVoyageNo.Text.Trim());
			obj.ETADate = Conversion.ToDateTime((object)datETADate.Value);
			obj.ETDDate = Conversion.ToDateTime((object)datETDDate.Value);
			obj.RemarksHeader = Conversion.ToString((object)txtProductSpecification.Text.Trim());
			obj.LcPoOrderStatus = ((Control)(object)cmbStatus).Text;
			obj.DocumentTypeId = 232;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EximLcorderPurchaseDetail = new List<ExImLcOrderPurchaseOrderDetail>();
			if (grdDetails.RowCount > 0)
			{
				double GrossWeight = 0.0;
				double NetWeight = 0.0;
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImLcOrderPurchaseOrderDetail vd = new ExImLcOrderPurchaseOrderDetail();
					if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
					{
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["PackTypeId"].Value) > 0)
						{
							vd.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
							if (Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) > 0.0)
							{
								vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
								vd.Qty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
								NetWeight += vd.NetWeight;
								if (Conversion.ToInt(r.Cells["PackSizeId"].Value) > 0)
								{
									vd.ItemUomId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
									if (Conversion.ToDouble(r.Cells["NoOfBags"].Value) > 0.0)
									{
										vd.NoOfPacks = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
										if (Conversion.ToDouble(r.Cells["GrossWeight"].Value) > 0.0)
										{
											vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
											GrossWeight += vd.GrossWeight;
											vd.PackingWeight = Conversion.ToDouble(r.Cells["PackWeight"].Value);
											if (Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value) > 0.0)
											{
												vd.ItemRate = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
												if (Conversion.ToInt(r.Cells["RateUOMId"].Value) > 0)
												{
													vd.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
													if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
													{
														vd.ItemAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
														if (Conversion.ToDouble(r.Cells["HomeCurrencyAmount"].Value) > 0.0)
														{
															vd.HomeCurrencyAmount = Conversion.ToDouble(r.Cells["HomeCurrencyAmount"].Value);
														}
														vd.SealNo = "";
														if (Conversion.ToInt(r.Cells["WarehouseId"].Value) > 0)
														{
															vd.ToWarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
															if (Conversion.ToInt(r.Cells["JobLotId"].Value) > 0)
															{
																vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
																if (Conversion.ToString(r.Cells["ContainerNo"].Value) != string.Empty)
																{
																	vd.ContainerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
																	vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
																	obj.EximLcorderPurchaseDetail.Add(vd);
																	continue;
																}
																MessageBox.Show("ContainerNo Required In Detail Grid");
																return;
															}
															MessageBox.Show("Job/Lot Required In Detail Grid");
															return;
														}
														MessageBox.Show("Warehouse Required In Detail Grid");
														return;
													}
													MessageBox.Show("Amount Required In Detail Grid");
													return;
												}
												MessageBox.Show("Cost Uom Required In Detail Grid");
												return;
											}
											MessageBox.Show("Cost M.Ton Required In Detail Grid");
											return;
										}
										MessageBox.Show("Gross Weight Required In Detail Grid");
										return;
									}
									MessageBox.Show("NoOfBags Required In Detail Grid");
									return;
								}
								MessageBox.Show("PackSize Required In Detail Grid");
								return;
							}
							MessageBox.Show("Qty/M.Ton Required In Detail Grid");
							return;
						}
						MessageBox.Show("Packing Type Required In Detail Grid");
						return;
					}
					MessageBox.Show("Item Required In Detail Grid");
					return;
				}
				obj.GrossWeight = GrossWeight;
				obj.NetWeight = NetWeight;
				ExImLcOrderPurchaseOrder.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Receod Save Successfully");
				}
				else
				{
					MessageBox.Show("Receod Update Successfully");
				}
				Reset();
				return;
			}
			throw new Exception("Grid Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			ExImLcOrderPurchaseOrder Obj = ExImLcOrderPurchaseOrder.GetByID(ID);
			if (Obj == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtExportSoNo.Text = Conversion.ToString((object)Obj.DocNo);
			txtDocDate.Value = Conversion.ToDateTime((object)Obj.DocDate);
			CmbCustomer.Value = Obj.SupplierCustomerId;
			cmbLcContractNo.Value = Conversion.ToInt((object)Obj.ExImLcOrderId);
			CmbOrderNo.Value = Conversion.ToString((object)Obj.DocNoByLcOrder);
			datContractDate.Value = Conversion.ToDateTime((object)Obj.SupplierOrderDate);
			txtSupplierInvoiceNo.Text = Conversion.ToString((object)Obj.SupplierInvoiceNo);
			datSupplierInvoiceDate.Value = Conversion.ToDateTime((object)Obj.SupplierInvoiceDate);
			CmbImporterBank.Value = Obj.ImporterBankId;
			CmbExporterbank.Value = Obj.ExporterBankId;
			CmbPaymentTerms.Value = Obj.PaymentTermId;
			CmbDeliveryTerm.Value = Obj.DeliveryTermId;
			CmbFCYCode.Value = Obj.FcyId;
			txtFCYAmount.Text = Conversion.ToString((object)Obj.FcyAmount);
			CmbHomeCurrency.Value = Obj.HomeCurrencyId;
			txtExchangeRate.Text = Conversion.ToString((object)Obj.ExchangeRate);
			txtTotalAmount.Text = Conversion.ToString((object)Obj.Amount);
			CmbTransporter.Value = Obj.TransporterId;
			CmbClearingAgent.Value = Obj.ClearingAgentId;
			CmbShippingAgent.Value = Obj.ShippingAgentId;
			CmbLoadingPort.Value = Obj.LoadingPortId;
			CmbDestinationPort.Value = Obj.DestinationPortId;
			txtNoOfContainers.Text = Conversion.ToString((object)Obj.Containers);
			txtGrossWeight.Text = Conversion.ToString((object)Obj.GrossWeight);
			txtNetWeight.Text = Conversion.ToString((object)Obj.NetWeight);
			txtBillOfLadingNo.Text = Conversion.ToString((object)Obj.BillOfLadingNo);
			datBillOfLadingDate.Value = Conversion.ToDateTime((object)Obj.BillOfLadingDate);
			txtTransirtDays.Text = Conversion.ToString((object)Obj.TransitDays);
			datArrivalDate.Value = Conversion.ToDateTime((object)Obj.ArrivalDate);
			txtVessel.Text = Conversion.ToString((object)Obj.Vessel);
			txtVoyageNo.Text = Conversion.ToString((object)Obj.VoyageNo);
			datETADate.Value = Conversion.ToDateTime((object)Obj.ETADate);
			datETDDate.Value = Conversion.ToDateTime((object)Obj.ETDDate);
			txtProductSpecification.Text = Conversion.ToString((object)Obj.RemarksHeader);
			((Control)(object)cmbStatus).Text = Conversion.ToString((object)Obj.LcPoOrderStatus);
			grdDetails.DataSource = null;
			dtdetail.Rows.Clear();
			for (int i = 0; i < Obj.EximLcorderPurchaseDetail.Count; i++)
			{
				dtdetail.Rows.Add(Obj.EximLcorderPurchaseDetail[i].ItemId, Obj.EximLcorderPurchaseDetail[i].ItemName, Obj.EximLcorderPurchaseDetail[i].PackingTypeId, Obj.EximLcorderPurchaseDetail[i].PackingType, Obj.EximLcorderPurchaseDetail[i].NetWeight / 1000.0, Obj.EximLcorderPurchaseDetail[i].ItemUomId, Obj.EximLcorderPurchaseDetail[i].ItemUOM, Obj.EximLcorderPurchaseDetail[i].NoOfPacks, Obj.EximLcorderPurchaseDetail[i].PackingWeight, Obj.EximLcorderPurchaseDetail[i].GrossWeight, Obj.EximLcorderPurchaseDetail[i].ItemRate, Obj.EximLcorderPurchaseDetail[i].RateUomId, Obj.EximLcorderPurchaseDetail[i].RateUom, Obj.EximLcorderPurchaseDetail[i].ItemAmount, Obj.EximLcorderPurchaseDetail[i].HomeCurrencyAmount, Obj.EximLcorderPurchaseDetail[i].ToWarehouseId, Obj.EximLcorderPurchaseDetail[i].WareHouseName, Obj.EximLcorderPurchaseDetail[i].JobLotId, Obj.EximLcorderPurchaseDetail[i].JobLotDescription, Obj.EximLcorderPurchaseDetail[i].ContainerNo, Obj.EximLcorderPurchaseDetail[i].RemarksDetail);
			}
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			InsterAmountandQtyMtonInHeader();
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
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

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			RecId = 0;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)cmbLcContractNo).Text = string.Empty;
			txtExportSoNo.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			txtSupplierInvoiceNo.Text = string.Empty;
			((Control)(object)CmbFCYCode).Text = string.Empty;
			txtFCYAmount.Text = string.Empty;
			((Control)(object)CmbHomeCurrency).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			((Control)(object)CmbPaymentTerms).Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbExporterbank).Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
			((Control)(object)CmbClearingAgent).Text = string.Empty;
			((Control)(object)CmbShippingAgent).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtNoOfContainers.Text = string.Empty;
			txtBillOfLadingNo.Text = string.Empty;
			txtVessel.Text = string.Empty;
			txtVoyageNo.Text = string.Empty;
			txtTransirtDays.Text = string.Empty;
			txtProductSpecification.Text = string.Empty;
			((Control)(object)cmbStatus).Text = string.Empty;
			datSupplierInvoiceDate.Value = DateTime.Now;
			datContractDate.Value = DateTime.Now;
			datArrivalDate.Value = DateTime.Now;
			datBillOfLadingDate.Value = DateTime.Now;
			datETADate.Value = DateTime.Now;
			datETDDate.Value = DateTime.Now;
			ResetDetail();
			dtdetail.Rows.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			GenerateCode();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtpackwt.Text = string.Empty;
			txtgrosswt.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((Control)(object)cmbjoblot).Text = string.Empty;
			txtcontainerNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 232;
			obj.NoOfRecords = 50;
			obj.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dt = ExImLcOrderPurchaseOrder.FormHistoryData(obj);
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSetting();
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
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DataGridHistory.RootTable.Columns["ExImLcOrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
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

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ImportReports.ExImLcOrderPurchaseOrderSlip(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 232,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "804-PurchaseOrderSlip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void txtLcOrderNo_Leave(object sender, EventArgs e)
	{
	}

	private void GenerateReport()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ImportReports.ExImLcOrderPurchaseOrderSlip(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 232,
				Id = RecId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "804-PurchaseOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
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

	private void btnImporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
	}

	private void btnExporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
	}

	private void btnDeliveryTerm_Click(object sender, EventArgs e)
	{
		new DeliveryTerms(UserAccount).Show();
	}

	private void btnPaymentTerms_Click(object sender, EventArgs e)
	{
		new PaymentTerms(UserAccount).Show();
	}

	private void btnLoadingPort_Click(object sender, EventArgs e)
	{
		new SeaPortsDefine(UserAccount).Show();
	}

	private void btnDestination_Click(object sender, EventArgs e)
	{
		new SeaPortsDefine(UserAccount).Show();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryFill();
		}
	}

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
		ReadById(RecId);
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new DefineMultiCurrency(UserAccount).Show();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		GenerateReport();
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void combitem_Leave_1(object sender, EventArgs e)
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

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				combitem.Value = item.Cells["ItemId"].Value;
				bindRateUomAndItemPackUom();
				combpcktype.Value = item.Cells["PackTypeId"].Value;
				txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				combitempck.Value = item.Cells["PackSizeId"].Value;
				txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
				txtpackwt.Text = item.Cells["PackWeight"].Value.ToString();
				txtgrosswt.Text = item.Cells["GrossWeight"].Value.ToString();
				combrateuom.Value = item.Cells["RateUOMId"].Value;
				txtCostMTon.Text = item.Cells["Cost/M.Ton"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				cmbWareHouse.Value = item.Cells["WarehouseId"].Value;
				cmbjoblot.Value = item.Cells["JobLotId"].Value;
				txtcontainerNo.Text = item.Cells["ContainerNo"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Value.ToString();
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

	private void AddtoGrid()
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(combitem.Value, ((Control)(object)combitem).Text, combpcktype.Value, ((Control)(object)combpcktype).Text, Conversion.ToDouble((object)txtQtyMTon.Text), combitempck.Value, ((Control)(object)combitempck).Text, Conversion.ToDouble((object)txtNoOfBags.Text), Conversion.ToDouble((object)txtCostMTon.Text), combrateuom.Value, ((Control)(object)combrateuom).Text, Conversion.ToDouble((object)txtAmount.Text), txtRemarks.Text);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				ResetDetail();
				combitem.Focus();
				InsterAmountandQtyMtonInHeader();
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
			MessageBox.Show("Cannot Add  on Update Existing Record");
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
			double NoofBags = 0.0;
			double OuterUom = 0.0;
			double Mton = 0.0;
			NoofBags = Conversion.ToDouble((object)txtNoOfBags.Text);
			if (((UltraGridBase)combitempck).ActiveRow != null)
			{
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value);
			}
			if (NoofBags > 0.0 && OuterUom > 0.0)
			{
				Mton = NoofBags * OuterUom / 1000.0;
				txtQtyMTon.Text = Mton.ToString();
			}
			if (Mton > 0.0)
			{
				double Grosswt = 0.0;
				double packwt = Conversion.ToDouble((object)txtpackwt.Text);
				if (packwt > 0.0)
				{
					Grosswt = Mton * 1000.0 + packwt;
					txtgrosswt.Text = Grosswt.ToString();
				}
				else
				{
					Grosswt = Mton * 1000.0;
					txtgrosswt.Text = Grosswt.ToString();
				}
			}
			else
			{
				txtQtyMTon.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemRateUom = 0.0;
			double NetWeight = 0.0;
			ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
			if (((UltraGridBase)combrateuom).ActiveRow != null)
			{
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
			}
			NetWeight = Conversion.ToDouble((object)txtQtyMTon.Text);
			if (ItemRate > 0.0 && ItemRateUom > 0.0 && NetWeight > 0.0)
			{
				double weightkgs = NetWeight * 1000.0;
				double amount = ItemRate / ItemRateUom * weightkgs;
				txtAmount.Text = amount.ToString();
			}
			else
			{
				txtAmount.Text = 0.ToString();
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
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)combitem).Text;
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = combpcktype.Value;
				dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)combpcktype).Text;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = combitempck.Value;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)combitempck).Text;
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtdetail.Rows[updateDetailIndex]["PackWeight"] = txtpackwt.Text;
				dtdetail.Rows[updateDetailIndex]["GrossWeight"] = txtgrosswt.Text;
				dtdetail.Rows[updateDetailIndex]["Cost/M.Ton"] = txtCostMTon.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = combrateuom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				dtdetail.Rows[updateDetailIndex]["WarehouseId"] = cmbWareHouse.Value;
				dtdetail.Rows[updateDetailIndex]["WarehouseName"] = ((Control)(object)cmbWareHouse).Text;
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = cmbjoblot.Value;
				dtdetail.Rows[updateDetailIndex]["JobLotName"] = ((Control)(object)cmbjoblot).Text;
				dtdetail.Rows[updateDetailIndex]["ContainerNo"] = txtcontainerNo.Text;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
				grdDetails.DataSource = dtdetail;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				combitem.Focus();
				InsterAmountandQtyMtonInHeader();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void txtpackwt_TextChanged(object sender, EventArgs e)
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

	private void InsterAmountandQtyMtonInHeader()
	{
		try
		{
			object Amount = grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2);
			object GrossWeight = grdDetails.GetTotal(grdDetails.RootTable.Columns["GrossWeight"], (AggregateFunction)2);
			double NetWeight = Conversion.ToDouble(grdDetails.GetTotal(grdDetails.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2)) * 1000.0;
			txtGrossWeight.Text = GrossWeight.ToString();
			txtNetWeight.Text = NetWeight.ToString();
			txtFCYAmount.Text = Amount.ToString();
			HomeCurrencyCaluculations();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void label26_Click(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetails).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetails).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdDetails.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void HomeCurrencyCaluculations()
	{
		try
		{
			if (Conversion.ToDouble((object)txtExchangeRate.Text.Trim()) != 0.0 && Conversion.ToDouble((object)txtFCYAmount.Text.Trim()) != 0.0)
			{
				double ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text.Trim());
				double TotalAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim()) * ExchangeRate;
				txtTotalAmount.Text = TotalAmount.ToString();
				ParporationHomeCurrencyAmount();
			}
			else
			{
				txtTotalAmount.Text = string.Empty;
				ParporationHomeCurrencyAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		HomeCurrencyCaluculations();
	}

	private void ParporationHomeCurrencyAmount()
	{
		try
		{
			grdDetails.UpdateData();
			double TotalAmount = Conversion.ToDouble((object)txtTotalAmount.Text.Trim());
			double TotalFcyAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			if (TotalAmount > 0.0 && TotalFcyAmount > 0.0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Amount = Conversion.ToDouble((object)obj.Cells["Amount"].Value.ToString());
					obj.Cells["HomeCurrencyAmount"].Value = (TotalAmount / TotalFcyAmount * Amount).ToString();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["HomeCurrencyAmount"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 232;
			obj.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dt = ExImLcOrderPurchaseOrder.FormHistoryData(obj);
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbLcContractNo).ActiveRow != null)
			{
				ExImLcOrderPurchaseOrder PIH = new ExImLcOrderPurchaseOrder();
				PIH = ExImLcOrderPurchaseOrder.GetByID(Conversion.ToInt(CmbOrderNo.Value));
				if (PIH != null)
				{
					datContractDate.Value = Conversion.ToDateTime((object)PIH.SupplierOrderDate);
					CmbPaymentTerms.Value = PIH.PaymentTermId;
					CmbDeliveryTerm.Value = PIH.DeliveryTermId;
					CmbExporterbank.Value = PIH.ExporterBankId;
					CmbImporterBank.Value = PIH.ImporterBankId;
					CmbLoadingPort.Value = PIH.LoadingPortId;
					CmbDestinationPort.Value = PIH.DestinationPortId;
					CmbFCYCode.Value = PIH.FcyId;
					txtFCYAmount.Text = Conversion.ToString((object)PIH.FcyAmount);
					CmbHomeCurrency.Value = PIH.HomeCurrencyId;
					txtExchangeRate.Text = Conversion.ToString((object)PIH.ExchangeRate);
					txtTotalAmount.Text = Conversion.ToString((object)PIH.Amount);
					txtProductSpecification.Text = PIH.RemarksHeader;
					txtNoOfContainers.Text = Conversion.ToString((object)PIH.Containers);
					grdDetails.DataSource = null;
					dtdetail.Rows.Clear();
					for (int i = 0; i < PIH.ImPurchaseOrderPackingDetailslst.Count; i++)
					{
						dtdetail.Rows.Add(PIH.ImPurchaseOrderPackingDetailslst[i].ItemId, PIH.ImPurchaseOrderPackingDetailslst[i].ItemName, PIH.ImPurchaseOrderPackingDetailslst[i].PackingTypeId, PIH.ImPurchaseOrderPackingDetailslst[i].PackingType, PIH.ImPurchaseOrderPackingDetailslst[i].Qty, PIH.ImPurchaseOrderPackingDetailslst[i].ItemUomId, PIH.ImPurchaseOrderPackingDetailslst[i].ItemUOM, PIH.ImPurchaseOrderPackingDetailslst[i].NoOfPacks, 0, PIH.ImPurchaseOrderPackingDetailslst[i].NetWeight, PIH.ImPurchaseOrderPackingDetailslst[i].ItemRate, PIH.ImPurchaseOrderPackingDetailslst[i].RateUomId, PIH.ImPurchaseOrderPackingDetailslst[i].RateUom, PIH.ImPurchaseOrderPackingDetailslst[i].ItemAmount, PIH.ImPurchaseOrderPackingDetailslst[i].HomeCurrencyAmount, "", 0, "", "", "");
					}
					grdDetails.DataSource = dtdetail;
					grdDetails.RetrieveStructure();
					grdSettings();
					InsterAmountandQtyMtonInHeader();
				}
				else
				{
					FiledsClearOnLcorderChange();
				}
			}
			else
			{
				FiledsClearOnLcorderChange();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
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
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Expected O, but got Unknown
		//IL_1264: Unknown result type (might be due to invalid IL or missing references)
		//IL_126e: Expected O, but got Unknown
		//IL_abfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_ac04: Expected O, but got Unknown
		//IL_ac11: Unknown result type (might be due to invalid IL or missing references)
		//IL_ac1b: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
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
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.ImPurchaseOrderShipmentPackingList));
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
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
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbOrderNo = new UltraCombo();
		this.CmbShippingAgent = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.CmbClearingAgent = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbTransporter = new UltraCombo();
		this.label59 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbHomeCurrency = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.datETDDate = new System.Windows.Forms.DateTimePicker();
		this.label67 = new System.Windows.Forms.Label();
		this.datETADate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.datArrivalDate = new System.Windows.Forms.DateTimePicker();
		this.label65 = new System.Windows.Forms.Label();
		this.datBillOfLadingDate = new System.Windows.Forms.DateTimePicker();
		this.label44 = new System.Windows.Forms.Label();
		this.datSupplierInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.cmbLcContractNo = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtcontainerNo = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.cmbjoblot = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.cmbWareHouse = new UltraCombo();
		this.label53 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtgrosswt = new System.Windows.Forms.TextBox();
		this.txtpackwt = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.combpcktype = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.combitempck = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.combrateuom = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtProductSpecification = new System.Windows.Forms.RichTextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtNoOfContainers = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtSupplierInvoiceNo = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtExportSoNo = new System.Windows.Forms.TextBox();
		this.CmbFCYCode = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbExporterbank = new UltraCombo();
		this.CmbImporterBank = new UltraCombo();
		this.CmbDestinationPort = new UltraCombo();
		this.CmbLoadingPort = new UltraCombo();
		this.CmbPaymentTerms = new UltraCombo();
		this.CmbDeliveryTerm = new UltraCombo();
		this.datContractDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbCustomer = new UltraCombo();
		this.txtVoyageNo = new System.Windows.Forms.TextBox();
		this.txtVessel = new System.Windows.Forms.TextBox();
		this.txtTransirtDays = new System.Windows.Forms.TextBox();
		this.txtBillOfLadingNo = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtFCYAmount = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnDefinePaymentTerm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShippingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClearingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHomeCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcContractNo).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1239, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1239, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1231, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1225, 717);
		this.panel2.TabIndex = 0;
		this.grdDetails.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDetails).BackColor = System.Drawing.Color.White;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 461);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.grdDetails).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1225, 256);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShippingAgent);
		this.panel4.Controls.Add(this.label61);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClearingAgent);
		this.panel4.Controls.Add(this.label60);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.panel4.Controls.Add(this.label59);
		this.panel4.Controls.Add(this.txtTotalAmount);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add(this.txtExchangeRate);
		this.panel4.Controls.Add(this.label43);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbHomeCurrency);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.datETDDate);
		this.panel4.Controls.Add(this.label67);
		this.panel4.Controls.Add(this.datETADate);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.datArrivalDate);
		this.panel4.Controls.Add(this.label65);
		this.panel4.Controls.Add(this.datBillOfLadingDate);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.datSupplierInvoiceDate);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.label57);
		this.panel4.Controls.Add(this.label56);
		this.panel4.Controls.Add(this.label55);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label58);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLcContractNo);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label47);
		this.panel4.Controls.Add(this.label46);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.label41);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtProductSpecification);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add(this.txtNoOfContainers);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.txtSupplierInvoiceNo);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.txtExportSoNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFCYCode);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label66);
		this.panel4.Controls.Add(this.label64);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.label63);
		this.panel4.Controls.Add(this.label62);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterbank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterBank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerms);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.panel4.Controls.Add(this.datContractDate);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel4.Controls.Add(this.txtVoyageNo);
		this.panel4.Controls.Add(this.txtVessel);
		this.panel4.Controls.Add(this.txtTransirtDays);
		this.panel4.Controls.Add(this.txtBillOfLadingNo);
		this.panel4.Controls.Add(this.txtNetWeight);
		this.panel4.Controls.Add(this.txtGrossWeight);
		this.panel4.Controls.Add(this.txtFCYAmount);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1225, 431);
		this.panel4.TabIndex = 1;
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(128, 159);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(CmbOrderNo_Leave);
		this.CmbShippingAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbShippingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShippingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShippingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShippingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.CmbShippingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShippingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShippingAgent).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShippingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShippingAgent).Location = new System.Drawing.Point(699, 100);
		((System.Windows.Forms.Control)(object)this.CmbShippingAgent).Name = "CmbShippingAgent";
		((System.Windows.Forms.Control)(object)this.CmbShippingAgent).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbShippingAgent).TabIndex = 18;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(583, 109);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(88, 15);
		this.label61.TabIndex = 591;
		this.label61.Text = "Shipping Agent";
		this.CmbClearingAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbClearingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClearingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClearingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClearingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.CmbClearingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance205).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClearingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClearingAgent).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbClearingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbClearingAgent).Location = new System.Drawing.Point(699, 70);
		((System.Windows.Forms.Control)(object)this.CmbClearingAgent).Name = "CmbClearingAgent";
		((System.Windows.Forms.Control)(object)this.CmbClearingAgent).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbClearingAgent).TabIndex = 17;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(583, 78);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(86, 15);
		this.label60.TabIndex = 591;
		this.label60.Text = "Clearing Agent";
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance213;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance218).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(699, 40);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 16;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(583, 46);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(71, 15);
		this.label59.TabIndex = 589;
		this.label59.Text = "Transporter";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(408, 243);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(165, 23);
		this.txtTotalAmount.TabIndex = 15;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(301, 247);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(49, 15);
		this.label50.TabIndex = 586;
		this.label50.Text = "Amount";
		this.txtExchangeRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(408, 216);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(165, 23);
		this.txtExchangeRate.TabIndex = 14;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(301, 220);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(85, 15);
		this.label43.TabIndex = 586;
		this.label43.Text = "Exchange Rate";
		this.CmbHomeCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbHomeCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbHomeCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbHomeCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance232).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbHomeCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Location = new System.Drawing.Point(408, 186);
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Name = "CmbHomeCurrency";
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).TabIndex = 13;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(301, 192);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(90, 15);
		this.label31.TabIndex = 585;
		this.label31.Text = "Home Currency";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(390, 45);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 8;
		this.label28.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(390, 76);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 9;
		this.label24.Text = "*";
		this.datETDDate.CustomFormat = "dd-MMM-yyyy";
		this.datETDDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datETDDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datETDDate.Location = new System.Drawing.Point(1001, 243);
		this.datETDDate.Name = "datETDDate";
		this.datETDDate.Size = new System.Drawing.Size(165, 23);
		this.datETDDate.TabIndex = 32;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(892, 247);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(55, 15);
		this.label67.TabIndex = 580;
		this.label67.Text = "ETD Date";
		this.datETADate.CustomFormat = "dd-MMM-yyyy";
		this.datETADate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datETADate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datETADate.Location = new System.Drawing.Point(1001, 216);
		this.datETADate.Name = "datETADate";
		this.datETADate.Size = new System.Drawing.Size(165, 23);
		this.datETADate.TabIndex = 31;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(892, 220);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(53, 15);
		this.label30.TabIndex = 580;
		this.label30.Text = "ETA Date";
		this.datArrivalDate.CustomFormat = "dd-MMM-yyyy";
		this.datArrivalDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datArrivalDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datArrivalDate.Location = new System.Drawing.Point(1001, 131);
		this.datArrivalDate.Name = "datArrivalDate";
		this.datArrivalDate.Size = new System.Drawing.Size(165, 23);
		this.datArrivalDate.TabIndex = 28;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(892, 135);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(73, 15);
		this.label65.TabIndex = 580;
		this.label65.Text = "Arrival Date";
		this.datBillOfLadingDate.CustomFormat = "dd-MMM-yyyy";
		this.datBillOfLadingDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datBillOfLadingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datBillOfLadingDate.Location = new System.Drawing.Point(1001, 72);
		this.datBillOfLadingDate.Name = "datBillOfLadingDate";
		this.datBillOfLadingDate.Size = new System.Drawing.Size(165, 23);
		this.datBillOfLadingDate.TabIndex = 26;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(892, 76);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(100, 15);
		this.label44.TabIndex = 580;
		this.label44.Text = "BillOfLadingDate";
		this.datSupplierInvoiceDate.CustomFormat = "dd-MMM-yyyy";
		this.datSupplierInvoiceDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datSupplierInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datSupplierInvoiceDate.Location = new System.Drawing.Point(128, 243);
		this.datSupplierInvoiceDate.Name = "datSupplierInvoiceDate";
		this.datSupplierInvoiceDate.Size = new System.Drawing.Size(165, 23);
		this.datSupplierInvoiceDate.TabIndex = 7;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 247);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(105, 15);
		this.label17.TabIndex = 580;
		this.label17.Text = "Supp Invoice Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(301, 78);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(83, 15);
		this.label57.TabIndex = 579;
		this.label57.Text = "Exporter Bank";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(301, 46);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(85, 15);
		this.label56.TabIndex = 578;
		this.label56.Text = "Importer Bank";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(583, 191);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(95, 15);
		this.label55.TabIndex = 577;
		this.label55.Text = "Destination Port";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(892, 275);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(41, 15);
		this.label15.TabIndex = 576;
		this.label15.Text = "Status";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 135);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(72, 15);
		this.label9.TabIndex = 575;
		this.label9.Text = "Contract No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(103, 134);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 17);
		this.label58.TabIndex = 3;
		this.label58.Text = "*";
		this.cmbLcContractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbLcContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLcContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLcContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance239;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance245).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbLcContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Location = new System.Drawing.Point(128, 129);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Name = "cmbLcContractNo";
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).TabIndex = 4;
		this.cmbLcContractNo.ValueChanged += new System.EventHandler(cmbLcContractNo_Leave);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Leave += new System.EventHandler(cmbLcContractNo_Leave);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(103, 75);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 1;
		this.label49.Text = "*";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(103, 162);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 4;
		this.label38.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(390, 109);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 10;
		this.label48.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(681, 191);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 21;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(388, 134);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 11;
		this.label46.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(389, 162);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 12;
		this.label45.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(939, 274);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 514;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(681, 219);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 22;
		this.label41.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(681, 134);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 19;
		this.label40.Text = "*";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(681, 162);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 17);
		this.label37.TabIndex = 20;
		this.label37.Text = "*";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(103, 191);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(14, 17);
		this.label36.TabIndex = 5;
		this.label36.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(103, 45);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 0;
		this.label35.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(103, 105);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 2;
		this.label19.Text = "*";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel6.Controls.Add(this.btnclose);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1225, 36);
		this.panel6.TabIndex = 504;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1177, 2);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 28);
		this.btnclose.TabIndex = 26;
		this.btnclose.TabStop = false;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(25, 5);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(328, 25);
		this.label18.TabIndex = 25;
		this.label18.Text = "Import Purchase Order Packing List";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance252;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(1001, 269);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 33;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(583, 163);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 15);
		this.label14.TabIndex = 501;
		this.label14.Text = "Loading Port";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(301, 106);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(83, 15);
		this.label7.TabIndex = 500;
		this.label7.Text = "Payment Term";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(583, 135);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(81, 15);
		this.label5.TabIndex = 499;
		this.label5.Text = "Delivery Term";
		this.groupBox1.Controls.Add(this.txtcontainerNo);
		this.groupBox1.Controls.Add(this.label54);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbjoblot);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.txtgrosswt);
		this.groupBox1.Controls.Add(this.txtpackwt);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtCostMTon);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add(this.txtNoOfBags);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktype);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtQtyMTon);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.groupBox1.Location = new System.Drawing.Point(5, 304);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1217, 122);
		this.groupBox1.TabIndex = 498;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.txtcontainerNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontainerNo.Location = new System.Drawing.Point(681, 88);
		this.txtcontainerNo.Name = "txtcontainerNo";
		this.txtcontainerNo.Size = new System.Drawing.Size(168, 23);
		this.txtcontainerNo.TabIndex = 12;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(681, 66);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(79, 15);
		this.label54.TabIndex = 499;
		this.label54.Text = "Container No";
		this.cmbjoblot.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Location = new System.Drawing.Point(508, 85);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Name = "cmbjoblot";
		((System.Windows.Forms.Control)(object)this.cmbjoblot).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblot).TabIndex = 11;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(513, 66);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(42, 15);
		this.label52.TabIndex = 497;
		this.label52.Text = "JobLot";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(321, 85);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 10;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(320, 66);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(68, 15);
		this.label53.TabIndex = 496;
		this.label53.Text = "WareHouse";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(966, 16);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(81, 15);
		this.label39.TabIndex = 493;
		this.label39.Text = "Gross Weight";
		this.txtgrosswt.BackColor = System.Drawing.Color.White;
		this.txtgrosswt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgrosswt.Location = new System.Drawing.Point(969, 40);
		this.txtgrosswt.Name = "txtgrosswt";
		this.txtgrosswt.ReadOnly = true;
		this.txtgrosswt.Size = new System.Drawing.Size(116, 23);
		this.txtgrosswt.TabIndex = 6;
		this.txtgrosswt.Text = "0";
		this.txtgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtpackwt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpackwt.Location = new System.Drawing.Point(848, 40);
		this.txtpackwt.Name = "txtpackwt";
		this.txtpackwt.Size = new System.Drawing.Size(116, 23);
		this.txtpackwt.TabIndex = 5;
		this.txtpackwt.Text = "0";
		this.txtpackwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtpackwt.TextChanged += new System.EventHandler(txtpackwt_TextChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(848, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(75, 15);
		this.label29.TabIndex = 491;
		this.label29.Text = "Pack Weight";
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(855, 88);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(223, 23);
		this.txtRemarks.TabIndex = 13;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1106, 90);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(67, 26);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(855, 66);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(54, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1106, 67);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(67, 26);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtCostMTon.BackColor = System.Drawing.Color.White;
		this.txtCostMTon.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(9, 88);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.ReadOnly = true;
		this.txtCostMTon.Size = new System.Drawing.Size(80, 23);
		this.txtCostMTon.TabIndex = 7;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(6, 66);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(68, 15);
		this.label25.TabIndex = 487;
		this.label25.Text = "Cost/M.Ton";
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(1080, 85);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(23, 26);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtNoOfBags.BackColor = System.Drawing.Color.White;
		this.txtNoOfBags.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(487, 40);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(120, 23);
		this.txtNoOfBags.TabIndex = 2;
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(484, 17);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(73, 15);
		this.label23.TabIndex = 485;
		this.label23.Text = "No Of Packs";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(9, 38);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(323, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 470;
		this.label1.Text = "Item Name";
		this.combpcktype.AllowNull = (DefaultableBoolean)1;
		this.combpcktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combpcktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktype).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Location = new System.Drawing.Point(332, 38);
		((System.Windows.Forms.Control)(object)this.combpcktype).Name = "combpcktype";
		((System.Windows.Forms.Control)(object)this.combpcktype).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.combpcktype).TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(333, 17);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Packing Type";
		this.txtQtyMTon.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(722, 40);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.ReadOnly = true;
		this.txtQtyMTon.Size = new System.Drawing.Size(121, 23);
		this.txtQtyMTon.TabIndex = 4;
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(722, 15);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(63, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "Qty/M.Ton";
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(611, 38);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combitempck).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(610, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(61, 15);
		this.label2.TabIndex = 474;
		this.label2.Text = "Pack Uom";
		this.txtAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.txtAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(187, 88);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(128, 23);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(187, 66);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(49, 15);
		this.label33.TabIndex = 485;
		this.label33.Text = "Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(91, 66);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 479;
		this.label13.Text = "Cost UOM";
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(94, 85);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		this.combrateuom.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 274);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 15);
		this.label12.TabIndex = 497;
		this.label12.Text = "Specification";
		this.txtProductSpecification.Location = new System.Drawing.Point(128, 269);
		this.txtProductSpecification.Name = "txtProductSpecification";
		this.txtProductSpecification.Size = new System.Drawing.Size(445, 36);
		this.txtProductSpecification.TabIndex = 21;
		this.txtProductSpecification.Text = "";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(583, 220);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(101, 15);
		this.label32.TabIndex = 495;
		this.label32.Text = "No Of Containers";
		this.txtNoOfContainers.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainers.Location = new System.Drawing.Point(699, 216);
		this.txtNoOfContainers.Name = "txtNoOfContainers";
		this.txtNoOfContainers.Size = new System.Drawing.Size(185, 23);
		this.txtNoOfContainers.TabIndex = 19;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 163);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(57, 15);
		this.label27.TabIndex = 491;
		this.label27.Text = "Order No";
		this.txtSupplierInvoiceNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierInvoiceNo.Location = new System.Drawing.Point(128, 216);
		this.txtSupplierInvoiceNo.Name = "txtSupplierInvoiceNo";
		this.txtSupplierInvoiceNo.Size = new System.Drawing.Size(165, 23);
		this.txtSupplierInvoiceNo.TabIndex = 6;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(7, 220);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(114, 15);
		this.label26.TabIndex = 489;
		this.label26.Text = "Supplier Invoice No";
		this.label26.Click += new System.EventHandler(label26_Click);
		this.txtExportSoNo.BackColor = System.Drawing.Color.White;
		this.txtExportSoNo.Enabled = false;
		this.txtExportSoNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportSoNo.Location = new System.Drawing.Point(129, 42);
		this.txtExportSoNo.Name = "txtExportSoNo";
		this.txtExportSoNo.ReadOnly = true;
		this.txtExportSoNo.Size = new System.Drawing.Size(165, 23);
		this.txtExportSoNo.TabIndex = 0;
		this.CmbFCYCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFCYCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFCYCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFCYCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFCYCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Location = new System.Drawing.Point(408, 129);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Name = "CmbFCYCode";
		this.CmbFCYCode.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(892, 192);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 457;
		this.label8.Text = "Voyage No";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(892, 163);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(41, 15);
		this.label66.TabIndex = 457;
		this.label66.Text = "Vessel";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(892, 106);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(75, 15);
		this.label64.TabIndex = 457;
		this.label64.Text = "Transit Days";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(892, 46);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(90, 15);
		this.label22.TabIndex = 457;
		this.label22.Text = "BillOfLadingNo";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(583, 275);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(64, 15);
		this.label63.TabIndex = 457;
		this.label63.Text = "NetWeight";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(583, 247);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(78, 15);
		this.label62.TabIndex = 457;
		this.label62.Text = "GrossWeight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(301, 135);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(55, 15);
		this.label20.TabIndex = 458;
		this.label20.Text = "Fcy Code";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 106);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(87, 15);
		this.label6.TabIndex = 459;
		this.label6.Text = "Supplier Name";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(301, 163);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(71, 15);
		this.label21.TabIndex = 461;
		this.label21.Text = "FCY Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 192);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(82, 15);
		this.label11.TabIndex = 462;
		this.label11.Text = "Contract Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 76);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Doc Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 15);
		this.label4.TabIndex = 464;
		this.label4.Text = "DocNo";
		this.CmbExporterbank.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Location = new System.Drawing.Point(408, 70);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Name = "CmbExporterbank";
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).TabIndex = 12;
		this.CmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(408, 40);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 11;
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(699, 186);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		this.CmbDestinationPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabStop = false;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(699, 157);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		this.CmbLoadingPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabStop = false;
		this.CmbPaymentTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Location = new System.Drawing.Point(408, 100);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Name = "CmbPaymentTerms";
		this.CmbPaymentTerms.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabStop = false;
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(699, 129);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		this.CmbDeliveryTerm.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabStop = false;
		this.datContractDate.CustomFormat = "dd-MMM-yyyy";
		this.datContractDate.Enabled = false;
		this.datContractDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datContractDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datContractDate.Location = new System.Drawing.Point(129, 188);
		this.datContractDate.Name = "datContractDate";
		this.datContractDate.Size = new System.Drawing.Size(165, 23);
		this.datContractDate.TabIndex = 6;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(129, 72);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(165, 23);
		this.txtDocDate.TabIndex = 1;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(128, 100);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 2;
		this.CmbCustomer.ValueChanged += new System.EventHandler(CmbCustomer_TextChanged);
		this.txtVoyageNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoyageNo.Location = new System.Drawing.Point(1001, 187);
		this.txtVoyageNo.Name = "txtVoyageNo";
		this.txtVoyageNo.Size = new System.Drawing.Size(165, 23);
		this.txtVoyageNo.TabIndex = 30;
		this.txtVoyageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVessel.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVessel.Location = new System.Drawing.Point(1001, 159);
		this.txtVessel.Name = "txtVessel";
		this.txtVessel.Size = new System.Drawing.Size(165, 23);
		this.txtVessel.TabIndex = 29;
		this.txtVessel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransirtDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransirtDays.Location = new System.Drawing.Point(1001, 102);
		this.txtTransirtDays.Name = "txtTransirtDays";
		this.txtTransirtDays.Size = new System.Drawing.Size(165, 23);
		this.txtTransirtDays.TabIndex = 27;
		this.txtTransirtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillOfLadingNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillOfLadingNo.Location = new System.Drawing.Point(1001, 42);
		this.txtBillOfLadingNo.Name = "txtBillOfLadingNo";
		this.txtBillOfLadingNo.Size = new System.Drawing.Size(165, 23);
		this.txtBillOfLadingNo.TabIndex = 25;
		this.txtBillOfLadingNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(699, 271);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(185, 23);
		this.txtNetWeight.TabIndex = 24;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.BackColor = System.Drawing.Color.White;
		this.txtGrossWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(699, 243);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(185, 23);
		this.txtGrossWeight.TabIndex = 23;
		this.txtGrossWeight.TabStop = false;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCYAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCYAmount.Location = new System.Drawing.Point(408, 159);
		this.txtFCYAmount.Name = "txtFCYAmount";
		this.txtFCYAmount.ReadOnly = true;
		this.txtFCYAmount.Size = new System.Drawing.Size(165, 23);
		this.txtFCYAmount.TabIndex = 15;
		this.txtFCYAmount.TabStop = false;
		this.txtFCYAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1225, 30);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1174, 5);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 47;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1225, 657);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 342;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnsave, this.btnUpdate, this.Print, this.btnAttachment, this.toolStripButton1, this.toolStripButton2, this.btnDefinePaymentTerm });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1225, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
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
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(79, 24);
		this.Print.Text = "804-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(82, 24);
		this.toolStripButton2.Text = "Delivery Term";
		this.toolStripButton2.Click += new System.EventHandler(btnDeliveryTerm_Click);
		this.btnDefinePaymentTerm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefinePaymentTerm.Name = "btnDefinePaymentTerm";
		this.btnDefinePaymentTerm.Size = new System.Drawing.Size(87, 24);
		this.btnDefinePaymentTerm.Text = "Payment Term";
		this.btnDefinePaymentTerm.Click += new System.EventHandler(btnPaymentTerms_Click);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1231, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel18);
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1225, 717);
		this.panel13.TabIndex = 0;
		this.panel18.Controls.Add(this.panel7);
		this.panel18.Controls.Add(this.panel5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 27);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1225, 690);
		this.panel18.TabIndex = 1;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1225, 657);
		this.panel7.TabIndex = 506;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel5.Controls.Add(this.label51);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1225, 33);
		this.panel5.TabIndex = 505;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(20, 3);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(323, 25);
		this.label51.TabIndex = 25;
		this.label51.Text = "Import : Purchase Contract History";
		this.panel17.Controls.Add(this.ctrlGrdBar1);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1225, 27);
		this.panel17.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1180, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 46;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1225, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_search_16;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1239, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImPurchaseOrderShipmentPackingList";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShippingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClearingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHomeCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcContractNo).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
