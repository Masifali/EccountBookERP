using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Purchase;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Paddy_Purchase;

public class frmPurchaseInvoiceForGate : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtGPGRNS = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int GRNID;

	private int VoucherHeadId;

	private int Id;

	private int POID;

	private bool Approved;

	public static int SupplierId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private frmLoadGRNForPaddyPurchase LoadGRN;

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

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Panel panel4;

	private Panel panel5;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label6;

	private TextBox txtbillno;

	private Label label5;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel12;

	internal GridEX grd;

	private Panel panel7;

	private Panel panel10;

	internal GridEX grdInvExp;

	private Panel panel9;

	internal GridEX grdFreight;

	private TextBox txtBillAmount;

	private Label label3;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

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

	private DateTimePicker DueDate;

	private Label label16;

	private ToolStripButton btnDelete;

	internal UltraCombo cmbcommtype;

	internal UltraCombo cmbcommuom;

	private Panel panel8;

	private Panel panel16;

	internal GridEX grdEmptyBags;

	private Panel panel13;

	internal GridEX grdGLedger;

	private ToolStripLabel toolStripLabel1;

	private Panel panel3;

	internal GridEX grdDetail;

	private RichTextBox txtRemarks;

	private RichTextBox txtcommremarks;

	private ToolStripButton btnSlip220A;

	private CheckBox ChkBoxParty;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnRegister;

	private Label label1;

	private TextBox txtOrderNo;

	private Label label2;

	private TextBox txtDeliveryTerm;

	public frmPurchaseInvoiceForGate(UserAccount user)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceForGate_Load(object sender, EventArgs e)
	{
		try
		{
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
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("AccountTitle");
			AccountsFill();
			OtherItemsBind();
			dtGPGRNS.Columns.Add("Id");
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("PurchaseOrderId");
			dtGrid.Columns.Add("InvGrnDetailId");
			dtGrid.Columns.Add("InvGrnId");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("CropYear");
			dtGrid.Columns.Add("ItemUomId");
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("ScaleKart", typeof(double));
			dtGrid.Columns.Add("EbTotal", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("StockWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate");
			dtGrid.Columns.Add("ItemRateUOMId");
			dtGrid.Columns.Add("RateUom", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("ItemNetAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			dtGrid.Columns.Add("JobLotId");
			dtGrid.Columns.Add("WarehouseId");
			dtGrid.Columns.Add("WareHouseName");
			dtGrid.Columns.Add("PackingTypeId");
			dtGrid.Columns.Add("LabNo");
			dtGrid.Columns.Add("GpNo");
			dtGrid.Columns.Add("VehicleNo");
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage");
			dtGrdGL.Columns.Add("Qty");
			dtGrdGL.Columns.Add("Rate");
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty");
			dtInvExp.Columns.Add("Rate");
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Columns.Add("OrderId");
			dtFreight.Columns.Add("InvGrnId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Percentage");
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtEmptyBags.Columns.Add("OrderId");
			dtEmptyBags.Columns.Add("TypeId");
			dtEmptyBags.Columns.Add("Type");
			dtEmptyBags.Columns.Add("InvGrnId");
			dtEmptyBags.Columns.Add("ItemId");
			dtEmptyBags.Columns.Add("ItemName");
			dtEmptyBags.Columns.Add("PurchaseQty", typeof(double));
			dtEmptyBags.Columns.Add("Rate");
			dtEmptyBags.Columns.Add("Amount", typeof(double));
			dtEmptyBags.Columns.Add("Remarks");
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			SupplierNameFilll();
			DocumentNo();
			CommissionTypeFill();
			CommissionUOMFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBok.Checked = true;
			ChkBoxParty.Checked = true;
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
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Other Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["OrderId"].Visible = false;
			grdFreight.RootTable.Columns["Debit"].Visible = false;
			grdFreight.RootTable.Columns["InvGrnId"].Visible = false;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].Caption = "AccountTitle";
			grdFreight.RootTable.Columns["Percentage"].Caption = "%";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Charge To Product";
			grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdFreight.RootTable.Columns["Transporter"].Width = 150;
			grdFreight.RootTable.Columns["Remarks"].Width = 120;
			grdFreight.RootTable.Columns["Freight"].Width = 80;
			grdFreight.RootTable.Columns["Percentage"].Width = 50;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = "#,#";
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = "#,#";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Supplier Add/Less";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
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
			grd.RootTable.Columns["InvGrnDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemUomId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["ItemRateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["InvGrnId"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ScaleKart"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ScaleKart"].FormatString = "#,##0.##";
			grd.RootTable.Columns["EbTotal"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["RateUom"].FormatString = "0;(0);0";
			grd.RootTable.Columns["ItemAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].FormatString = "0.00;(0.00);0";
			grd.RootTable.Columns["ItemNetAmount"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Freights"].FormatString = "0,0";
			grd.RootTable.Columns["Expense"].FormatString = "0,0";
			grd.RootTable.Columns["Commission"].FormatString = "0,0";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ScaleKart"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemNetAmount"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Commission"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Freights"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ScaleKart"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(POID, GRNID, 0, 0, 0, 0, 0, 0, "");
		grdFreight.DataSource = dtFreight;
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			grdEmptyBags.RootTable.Columns["OrderId"].Visible = false;
			grdEmptyBags.RootTable.Columns["TypeId"].Visible = false;
			grdEmptyBags.RootTable.Columns["InvGrnId"].Visible = false;
			grdEmptyBags.RootTable.Columns["ItemId"].Visible = false;
			grdEmptyBags.RootTable.Columns["ItemName"].Width = 180;
			grdEmptyBags.RootTable.Columns["Remarks"].Width = 150;
			grdEmptyBags.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["PurchaseQty"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["TypeId"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdEmptyBags.RootTable.Columns["PurchaseQty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
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

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdFreight.CurrentRow;
				dtFreight.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Freight"].Value = Qty * Rate;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Freight"].Value = Math.Round(TotlaPercentage);
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
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

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Qty * Rate;
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
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
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
			{
				item.Cells["Credit"].Value = 0;
				MessageBox.Show("Debit Side is aleady added");
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
			{
				item.Cells["Debit"].Value = 0;
				MessageBox.Show("Credit Side is aleady added");
			}
			if (e.Column.Key == "AccountId" && Conversion.ToInt((object)txtSupplierGLId.Text) == Conversion.ToInt(item.Cells["AccountId"].Value))
			{
				MessageBox.Show("Supplier Account Not select");
				item.Cells["AccountId"].Value = 0;
			}
			else
			{
				grdGLedger.UpdateData();
				BillAmount();
				LedgerProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "ItemRate" || e.Column.Key == "RateCut" || e.Column.Key == "RateUom") && item != null && (int)item.RowType == 0)
			{
				string RateCut = item.Cells["RateCut"].Value.ToString();
				string obj = item.Cells["GrossWeight"].Value.ToString();
				string EbTotal = item.Cells["EbTotal"].Value.ToString();
				double NetBillWeight = 0.0;
				NetBillWeight = Conversion.ToDouble((object)obj) - Conversion.ToDouble((object)EbTotal);
				item.Cells["NetBillWeight"].Value = NetBillWeight.ToString();
				string RateUom = item.Cells["RateUom"].Text.ToString();
				double RateCutAmount = Conversion.ToDouble((object)NetBillWeight) / Conversion.ToDouble((object)RateUom) * Conversion.ToDouble((object)RateCut);
				item.Cells["RateCutAmount"].Value = RateCutAmount;
				string ItemRate = item.Cells["ItemRate"].Value.ToString();
				double ItemAmount = Conversion.ToDouble((object)item.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)RateUom) * Conversion.ToDouble((object)ItemRate);
				item.Cells["ItemAmount"].Value = ItemAmount - RateCutAmount;
			}
			grd.UpdateData();
			BillAmount();
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
				ReadById(Id);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), 166);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.PurchaseInvoicettSlip_220A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
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

	private void GetDetailGrdByHeadId()
	{
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("Unloading");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("ItemUOM");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("EBTotalWt", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			InvPurchaseInvoice obj = new InvPurchaseInvoice();
			obj = InvPurchaseInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj.invPurchaseInvoiceDetailList.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.invPurchaseInvoiceDetailList.Count; i++)
				{
					dtdetail.Rows.Add(obj.invPurchaseInvoiceDetailList[i].GpNo, obj.invPurchaseInvoiceDetailList[i].VehicleNo, obj.invPurchaseInvoiceDetailList[i].WareHouseName, obj.invPurchaseInvoiceDetailList[i].ItemName, obj.invPurchaseInvoiceDetailList[i].PackTypeDesc, obj.invPurchaseInvoiceDetailList[i].UOMCodeItem, obj.invPurchaseInvoiceDetailList[i].CropYear, obj.invPurchaseInvoiceDetailList[i].JobLotDescription, obj.invPurchaseInvoiceDetailList[i].ItemQty, obj.invPurchaseInvoiceDetailList[i].GrossWeight, obj.invPurchaseInvoiceDetailList[i].EBTotalWt, obj.invPurchaseInvoiceDetailList[i].NetBillWeight, obj.invPurchaseInvoiceDetailList[i].ItemRate, obj.invPurchaseInvoiceDetailList[i].EquivalentPoRate, obj.invPurchaseInvoiceDetailList[i].ItemAmount, obj.invPurchaseInvoiceDetailList[i].CommissionAmount, obj.invPurchaseInvoiceDetailList[i].ExpenseAmount, obj.invPurchaseInvoiceDetailList[i].FreightAmount);
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
		catch (Exception)
		{
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["ItemName"].Width = 200;
			grdDetail.RootTable.Columns["Unloading"].Width = 150;
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
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmptyBags.UpdateData();
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (e.Column.Key == "Rate" && item != null && (int)item.RowType == 0 && item.Cells["PurchaseQty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["PurchaseQty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdEmptyBags.UpdateData();
			BillAmount();
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
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double EmptyBagsAmount = 0.0;
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterDebit = 0.0;
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt(cmbsuppliername.Value) > 0 && int.Parse(r3.Cells["AccountId"].Value.ToString()) > 0)
				{
					JournalDebit += Conversion.ToDouble((object)r3.Cells["Debit"].Value.ToString());
					JournalCredit += Conversion.ToDouble((object)r3.Cells["Credit"].Value.ToString());
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["Transporter"].Value) > 0 && txtSupplierGLId.Text.ToString() == r2.Cells["Transporter"].Value.ToString())
				{
					TransporterDebit += Conversion.ToDouble((object)r2.Cells["Debit"].Value.ToString());
					TransporterCredit += Conversion.ToDouble((object)r2.Cells["Freight"].Value.ToString());
				}
			}
			if (grdEmptyBags.GetRows().Count() > 0)
			{
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["TypeId"].Value) == 1)
					{
						EmptyBagsAmount += Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString());
					}
				}
			}
			BillAmount = ItemAmount + ExpAmount + EmptyBagsAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount = BillAmount + TransporterCredit - TransporterDebit;
			if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(cmbcommagent.Value))
			{
				if (txtcommrate.Text != string.Empty)
				{
					BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
				}
				else
				{
					txtcommamount.Text = "0";
				}
			}
			txtBillAmount.Text = Math.Round(BillAmount).ToString("0,0");
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbsuppliername).ActiveRow == null)
		{
			MessageBox.Show("Supplier Name is Required");
			cmbsuppliername.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtDueDays.Text.Trim() == string.Empty || txtDueDays.Text.Trim() == "0" || txtDueDays.Text.Trim() == null)
		{
			MessageBox.Show("DueDays Field is Required");
			txtDueDays.Focus();
			return false;
		}
		return true;
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbsuppliername, "Id", "CompanyName", "Supplier Name", false);
				DDL.BindDDLNew(dtSupplier, cmbcommagent, "Id", "CompanyName", "Commission Agent Name", false);
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)cmbcommagent).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
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
			DataTable dtcoa = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtcoa.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcoa.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 15)
				{
					dtAccountlst.Rows.Add(dtcoa.Rows[i]["Id"], dtcoa.Rows[i]["AccountTitle"]);
				}
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
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Uom");
			dt.Rows.Add(1, "1");
			dt.Rows.Add(2, "5");
			dt.Rows.Add(3, "10");
			dt.Rows.Add(4, "25");
			dt.Rows.Add(5, "40");
			dt.Rows.Add(6, "50");
			dt.Rows.Add(7, "60");
			dt.Rows.Add(8, "65");
			dt.Rows.Add(9, "80");
			dt.Rows.Add(10, "100");
			DDL.BindDDL(dt, cmbcommuom, "Id", "Uom", "UOM", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		try
		{
			int Code = CommonServices.PurchaseInvoiceGenerateCode(166);
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
				return;
			}
			throw new Exception("Max Number Not Found");
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
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, 166);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Flat")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					txtcommamount.Text = CommRate3.ToString("0,0");
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Percent")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
						txtcommamount.Text = Math.Round(CommAmt2).ToString("0,0");
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)cmbcommtype).Text == "Comm Weight")
				{
					double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)cmbcommuom).Text.Trim());
						double CommAmt = num / UOM * CommRate;
						txtcommamount.Text = Math.Round(CommAmt).ToString("0,0");
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		CommissionProportion();
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 166);
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

	private void LoadDataDetailGridAgainstGP(string IDs, int OrderId = 0)
	{
		try
		{
			DataTable invGrn = new DataTable();
			if (OrderId > 0)
			{
				POID = OrderId;
				invGrn = InvPurchaseInvoice.GrnLoadForPaddyPurchaseByOrder_GrnId(Conversion.ToInt((object)IDs), OrderId);
			}
			else
			{
				invGrn = InvPurchaseInvoice.GrnLoadForPurchaseInvoice(IDs);
			}
			if (invGrn.Rows.Count > 0)
			{
				cmbsuppliername.Value = invGrn.Rows[0]["SupplierCustomerId"];
				cmbcommagent.Value = invGrn.Rows[0]["BrokerAgentSupCustId"];
				((Control)(object)cmbcommtype).Text = string.Empty;
				((Control)(object)cmbcommtype).Text = invGrn.Rows[0]["CommissionType"].ToString();
				txtcommrate.Text = invGrn.Rows[0]["CommRate"].ToString();
				((Control)(object)cmbcommuom).Text = string.Empty;
				((Control)(object)cmbcommuom).Text = invGrn.Rows[0]["UomScheduleIdCmRate"].ToString();
				txtcommamount.Text = invGrn.Rows[0]["CommAmount"].ToString();
				txtcommremarks.Text = invGrn.Rows[0]["CommissionRemarks"].ToString();
				txtRemarks.Text = invGrn.Rows[0]["RemarksHeader"].ToString();
				txtDueDays.Text = invGrn.Rows[0]["OrderDueDays"].ToString();
				GRNID = Conversion.ToInt(invGrn.Rows[0]["InvGrnId"]);
				txtDeliveryTerm.Text = Conversion.ToString(invGrn.Rows[0]["DeliveryTerm"]);
				if (OrderId > 0)
				{
					((Control)(object)cmbsuppliername).Enabled = false;
					txtOrderNo.Text = Conversion.ToString(invGrn.Rows[0]["OrderNo"]);
				}
				else
				{
					((Control)(object)cmbsuppliername).Enabled = true;
					txtOrderNo.Text = "0";
				}
				for (int i = 0; i < invGrn.Rows.Count; i++)
				{
					dtGrid.Rows.Add(0, invGrn.Rows[i]["PurchaseOrderId"], invGrn.Rows[i]["InvGrnDetailId"], invGrn.Rows[i]["InvGrnId"], invGrn.Rows[i]["ItemId"], invGrn.Rows[i]["ItemName"], invGrn.Rows[i]["CropYear"], invGrn.Rows[i]["ItemUOMId"], invGrn.Rows[i]["UOMCodeItem"], invGrn.Rows[i]["ItemQty"], invGrn.Rows[i]["GrossWeight"], invGrn.Rows[i]["ScaleKart"], invGrn.Rows[i]["EBTotalWt"], invGrn.Rows[i]["NetBillWeight"], invGrn.Rows[i]["NetStockWeight"], invGrn.Rows[i]["ItemRate"], invGrn.Rows[i]["UomScheduleIdRate"], Conversion.ToDouble(invGrn.Rows[i]["EquivalentPoRate"]), invGrn.Rows[i]["ItemAmount"], 0, 0, 0, 0, invGrn.Rows[i]["JobLotId"], invGrn.Rows[i]["WarehouseId"], invGrn.Rows[i]["WareHouseName"], invGrn.Rows[i]["PackingTypeId"], invGrn.Rows[i]["LabAnalisysNo"], invGrn.Rows[i]["GpNo"], invGrn.Rows[i]["VehicleNo"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadFreightData(string IDs)
	{
		try
		{
			string DeliveryTerm = txtDeliveryTerm.Text.Trim();
			DataTable item = InvPurchaseInvoice.GetTransporterAndFreightFromGrn(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (Conversion.ToInt(item.Rows[i]["CarriageAmount"]) > 0)
				{
					string Remarks = "";
					if (Conversion.ToInt(item.Rows[i]["GpNo"]) > 0)
					{
						Remarks = Remarks + "GpNo: " + Conversion.ToString(item.Rows[i]["GpNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["VehicleNo"]) != string.Empty)
					{
						Remarks = Remarks + "   VehicleNo:  " + Conversion.ToString(item.Rows[i]["VehicleNo"]);
					}
					if (Conversion.ToString(item.Rows[i]["BiltyNo"]) != string.Empty)
					{
						Remarks = Remarks + "   BiltyNo: " + Conversion.ToString(item.Rows[i]["BiltyNo"]);
					}
					switch (DeliveryTerm)
					{
					case "Load":
					case "Load & PartyWeight":
					case "Load & FactoryWeight":
						dtFreight.Rows.Add(POID, item.Rows[i]["MainId"], item.Rows[i]["Transporter"], 0, 0, 0, item.Rows[i]["CarriageAmount"], 0, Remarks);
						break;
					default:
						dtGrdGL.Rows.Add(item.Rows[i]["Transporter"], Remarks, 0, 0, 0, 0, item.Rows[i]["CarriageAmount"]);
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadEmptyBagsData(string IDs, int OrderId = 0)
	{
		try
		{
			DataTable item = InvPurchaseInvoice.GetEmptyBagsFromGrn(IDs, OrderId);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (Conversion.ToInt(item.Rows[i]["PurchaseQty"]) > 0)
				{
					dtEmptyBags.Rows.Add(item.Rows[i]["PurchaseOrderId"], item.Rows[i]["TypeId"], item.Rows[i]["EmptyBagsType"], item.Rows[i]["InvGrnId"], item.Rows[i]["ItemId"], item.Rows[i]["ItemName"], item.Rows[i]["PurchaseQty"], item.Rows[i]["Rate"], Conversion.ToDouble(item.Rows[i]["PurchaseQty"]) * Conversion.ToDouble(item.Rows[i]["Rate"]), item.Rows[i]["Remarks"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadExpData(string IDs)
	{
		try
		{
			DataTable item = InvPurchaseInvoice.GetItemQtyAndItemRateFromPO(IDs);
			if (item.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < item.Rows.Count; i++)
			{
				if (Conversion.ToInt(item.Rows[i]["BagPrice"]) > 0)
				{
					dtInvExp.Rows.Add(0, item.Rows[i]["ItemQty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["ItemQty"]) * Conversion.ToDouble(item.Rows[i]["BagPrice"]), 0);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LoadInGridDetail(int GrnIdByDocNo = 0, int InvoicetypeIdbygrndocno = 0)
	{
		try
		{
			dtGrid.Rows.Clear();
			string GrnIds = "";
			int OrderId = 0;
			for (int i = 0; i < LoadGRN.dtGRNS.Rows.Count; i++)
			{
				GrnIds = Conversion.ToString(LoadGRN.dtGRNS.Rows[i][0]);
				OrderId = Conversion.ToInt((object)LoadGRN.dtGRNS.Rows[i][3].ToString());
			}
			if (OrderId > 0)
			{
				LoadDataDetailGridAgainstGP(GrnIds, OrderId);
			}
			else
			{
				LoadDataDetailGridAgainstGP(GrnIds);
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtFreight.Rows.Clear();
			dtGrdGL.Rows.Clear();
			LoadFreightData(GrnIds);
			if (dtFreight.Rows.Count == 0 || dtFreight == null)
			{
				AddRowInFreightGrid();
			}
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			if (dtGrdGL.Rows.Count == 0 || dtGrdGL == null)
			{
				AddRowInGLGrid();
			}
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Clear();
			LoadExpData(GrnIds);
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.RetrieveStructure();
			dtEmptyBags.Clear();
			LoadEmptyBagsData(GrnIds, OrderId);
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			TotalCommissionAmount();
			grdInvExpSettings();
			FreightProportion();
			ExpProportion();
			LedgerProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			GrnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double ItemQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					obj.Cells["Expense"].Value = (TotalAmount / ItemQty * Qty).ToString();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			double DebitForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Debit"], (AggregateFunction)2).ToString());
			if (DebitForFreight < CreditForFreight)
			{
				double TotalDiffernce = CreditForFreight - DebitForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
					obj.Cells["Freights"].Value = Math.Round(TotalDiffernce) / NetWeight * Weight;
				}
			}
			else
			{
				if (DebitForFreight < CreditForFreight)
				{
					double TotalDiffernce3 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Freights"].Value = TotalDiffernce3;
					}
				}
				if (DebitForFreight == CreditForFreight)
				{
					double TotalDiffernce2 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj3 in rows)
					{
						obj3.BeginEdit();
						obj3.Cells["Freights"].Value = TotalDiffernce2;
					}
				}
				if (DebitForFreight > CreditForFreight)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj4 in rows)
					{
						obj4.BeginEdit();
						obj4.Cells["Freights"].Value = 0;
					}
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LedgerProportion()
	{
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["ItemNetAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Freights"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double CommPercent = Conversion.ToDouble((object)txtcommrate.Text);
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)cmbcommtype).Text == "Percent")
					{
						double ItemAmount = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString());
						r.Cells["Commission"].Value = ItemAmount * CommPercent / 100.0;
						r.EndEdit();
					}
					else
					{
						double Weight = Conversion.ToDouble((object)r.Cells["NetBillWeight"].Value.ToString());
						r.Cells["Commission"].Value = (TotalAmount / NetWeight * Weight).ToString();
						r.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll(int NoOfRecords = 0)
	{
		try
		{
			DataTable dthistory = CommonServices.PurchaseInvoiceHistory(166, formright.DoHaveCanViewAllRecordRights, NoOfRecords);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("VoucherHeadId");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("InvoiceDate");
				dt.Columns.Add("InvoiceNo");
				dt.Columns.Add("DueDate");
				dt.Columns.Add("DueDays");
				dt.Columns.Add("ManualNo");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("RemarksHeader");
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("EntryDate");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ApprovedStatus");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), dthistory.Rows[i]["DocNo"], dthistory.Rows[i]["DueDate"], dthistory.Rows[i]["DueDays"], dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ApprovedStatus"], dthistory.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dt;
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["SupplierName"].Width = 300;
			grdHistory.RootTable.Columns.Add("Detail");
			grdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdHistory.RootTable.Columns["Detail"].Width = 50;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 60;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].Image = Resources.icons8_pencil_16;
			grdHistory.RootTable.Columns["Edit"].Width = 25;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = "#,##0";
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = "#,##0.##";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Reset()
	{
		try
		{
			Id = 0;
			POID = 0;
			VoucherHeadId = 0;
			Approved = false;
			DocDate.Focus();
			((Control)(object)cmbcommagent).Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)cmbcommuom).Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtOrderNo.Text = string.Empty;
			DueDate.Value = DateTime.Now;
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			dtGrdGL.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Rows.Clear();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtFreight.Rows.Clear();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			dtEmptyBags.Rows.Clear();
			grdEmptyBags.DataSource = dtEmptyBags;
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			cmbsuppliername.Focus();
			dtGPGRNS.Rows.Clear();
			((Control)(object)cmbsuppliername).Enabled = true;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			txtBillAmount.Text = "0";
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
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Id);
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			cmbsuppliername.Value = invPI.SupplierCustomerId;
			((Control)(object)cmbsuppliername).Enabled = false;
			txtbillno.Text = invPI.ManualBillNo;
			cmbcommagent.Value = invPI.CommissionAgentId;
			((Control)(object)cmbcommtype).Text = Conversion.ToString((object)invPI.CommissionType);
			txtcommrate.Text = invPI.CommRate.ToString();
			((Control)(object)cmbcommuom).Text = invPI.UomScheduleIdCmRate;
			txtcommamount.Text = invPI.CommAmount.ToString();
			txtcommremarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
			txtRemarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
			txtBillAmount.Text = invPI.BillAmount.ToString();
			DueDate.Value = invPI.DueDate;
			txtDueDays.Text = Conversion.ToString((object)invPI.DueDays);
			Approved = invPI.IsApproved;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int m = 0; m < invPI.invPurchaseInvoiceDetailList.Count; m++)
			{
				dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[m].Id, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderId, invPI.invPurchaseInvoiceDetailList[m].InvGrnDetailId, invPI.invPurchaseInvoiceDetailList[m].InvGrnId, invPI.invPurchaseInvoiceDetailList[m].ItemId, invPI.invPurchaseInvoiceDetailList[m].ItemName, invPI.invPurchaseInvoiceDetailList[m].CropYear, invPI.invPurchaseInvoiceDetailList[m].ItemUOMId, invPI.invPurchaseInvoiceDetailList[m].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[m].ItemQty, invPI.invPurchaseInvoiceDetailList[m].GrossWeight, invPI.invPurchaseInvoiceDetailList[m].ScaleKart, invPI.invPurchaseInvoiceDetailList[m].EBTotalWt, invPI.invPurchaseInvoiceDetailList[m].NetBillWeight, invPI.invPurchaseInvoiceDetailList[m].NetStockWeight, invPI.invPurchaseInvoiceDetailList[m].ItemRate, invPI.invPurchaseInvoiceDetailList[m].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[m].EquivalentPoRate, invPI.invPurchaseInvoiceDetailList[m].ItemAmount, invPI.invPurchaseInvoiceDetailList[m].BillAmount, invPI.invPurchaseInvoiceDetailList[m].FreightAmount, invPI.invPurchaseInvoiceDetailList[m].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[m].CommissionAmount, invPI.invPurchaseInvoiceDetailList[m].JobLotId, invPI.invPurchaseInvoiceDetailList[m].WarehouseId, invPI.invPurchaseInvoiceDetailList[m].WareHouseName, invPI.invPurchaseInvoiceDetailList[m].PackingTypeId, invPI.invPurchaseInvoiceDetailList[m].LabAnalisysNo, invPI.invPurchaseInvoiceDetailList[m].GpNo, invPI.invPurchaseInvoiceDetailList[m].VehicleNo);
			}
			dtInvExp.Rows.Clear();
			for (int l = 0; l < invPI.InvPurchaseInvoiceExpList.Count; l++)
			{
				dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[l].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[l].Qty, invPI.InvPurchaseInvoiceExpList[l].Rate, invPI.InvPurchaseInvoiceExpList[l].Amount, invPI.InvPurchaseInvoiceExpList[l].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int k = 0; k < invPI.InvPurchaseInvoicejournalList.Count; k++)
			{
				dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId, invPI.InvPurchaseInvoicejournalList[k].JvRemarks, invPI.InvPurchaseInvoicejournalList[k].JvPrcnt, invPI.InvPurchaseInvoicejournalList[k].JvQty, invPI.InvPurchaseInvoicejournalList[k].JvRate, invPI.InvPurchaseInvoicejournalList[k].JvDebit, invPI.InvPurchaseInvoicejournalList[k].JvCredit);
			}
			dtFreight.Rows.Clear();
			for (int j = 0; j < invPI.InvPurchaseInvoiceFreightList.Count; j++)
			{
				dtFreight.Rows.Add(invPI.InvPurchaseInvoiceFreightList[j].PurchaseOrderId, invPI.InvPurchaseInvoiceFreightList[j].InvGrnId, invPI.InvPurchaseInvoiceFreightList[j].TansporterId, invPI.InvPurchaseInvoiceFreightList[j].Percentage, invPI.InvPurchaseInvoiceFreightList[j].FrQty, invPI.InvPurchaseInvoiceFreightList[j].FrRate, Math.Round(invPI.InvPurchaseInvoiceFreightList[j].FreightAmount), invPI.InvPurchaseInvoiceFreightList[j].Debit, invPI.InvPurchaseInvoiceFreightList[j].Remarks);
			}
			dtEmptyBags.Rows.Clear();
			for (int i = 0; i < invPI.InvPurchaseInvoiceEmptyBagslist.Count; i++)
			{
				dtEmptyBags.Rows.Add(invPI.InvPurchaseInvoiceEmptyBagslist[i].PurchaseOrderId, invPI.InvPurchaseInvoiceEmptyBagslist[i].TypeId, invPI.InvPurchaseInvoiceEmptyBagslist[i].EmptyBagsType, invPI.InvPurchaseInvoiceEmptyBagslist[i].InvGrnId, invPI.InvPurchaseInvoiceEmptyBagslist[i].ItemId, invPI.InvPurchaseInvoiceEmptyBagslist[i].ItemName, invPI.InvPurchaseInvoiceEmptyBagslist[i].PurchaseQty, invPI.InvPurchaseInvoiceEmptyBagslist[i].Rate, invPI.InvPurchaseInvoiceEmptyBagslist[i].Amount, invPI.InvPurchaseInvoiceEmptyBagslist[i].Remarks);
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdFreight.DataSource = dtFreight;
			grdFreight.RetrieveStructure();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdFreightSettings();
			grdInvExpSettings();
			gridGLSettings();
			FreightProportion();
			ExpProportion();
			LedgerProportion();
			CommissionProportion();
			BillProportion();
			tabControl1.SelectedIndex = 0;
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
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
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			if (Id == 0)
			{
				throw new Exception("Record Not Update beacause Record Id not found");
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
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dad: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Expected O, but got Unknown
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Expected O, but got Unknown
		//IL_121f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				purchaseInvoice.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			TotalCommissionAmount();
			BillAmount();
			ExpProportion();
			FreightProportion();
			CommissionProportion();
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (!(Conversion.ToDouble(r3.Cells["Freight"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r3.Cells["Transporter"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against Freight First");
				}
				if (Conversion.ToDouble(r3.Cells["Freight"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						((Control)(object)grdFreight).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if ((Conversion.ToDouble(r7.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r7.Cells["Debit"].Value) > 0) && Conversion.ToInt(r7.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				if (Conversion.ToDouble(r8.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r8.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against Expense First");
				}
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				if (Conversion.ToInt(r9.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r9.Cells["TypeId"].Value) == 1 && Conversion.ToDouble(r9.Cells["Amount"].Value) == 0.0)
				{
					throw new Exception("Packing Material Amount Required");
				}
			}
			if (Conversion.ToDouble((object)txtcommamount.Text) > 0.0 && Conversion.ToInt(cmbcommagent.Value) == 0)
			{
				throw new Exception("Please Select Commission Agent Account First");
			}
			purchaseInvoice.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			purchaseInvoice.CommissionAgentId = Conversion.ToInt(cmbcommagent.Value);
			purchaseInvoice.CommissionType = ((Control)(object)cmbcommtype).Text.Trim();
			purchaseInvoice.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
			if (txtcommremarks.Text.Trim() == string.Empty)
			{
				purchaseInvoice.CommissionRemarks = " CommRate:   " + purchaseInvoice.CommRate + "  CommType: " + purchaseInvoice.CommissionType;
			}
			else
			{
				purchaseInvoice.CommissionRemarks = txtcommremarks.Text.Trim();
			}
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocumentTypeId = 166;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.RemarksHeader = txtRemarks.Text.Trim();
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			purchaseInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.SupplierInvoiceDate = DateTime.Now;
			purchaseInvoice.DueDate = DueDate.Value;
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text.Trim());
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r6 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r6.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r6.Cells["PurchaseOrderId"].Value);
					if (Conversion.ToInt(r6.Cells["ItemId"].Value) != 0)
					{
						Pd.ItemId = Conversion.ToInt(r6.Cells["ItemId"].Value);
						if (Conversion.ToSingle(r6.Cells["ItemQty"].Value) != 0f)
						{
							Pd.ItemQty = Conversion.ToDouble(r6.Cells["ItemQty"].Value);
							Pd.CropYear = Conversion.ToString(r6.Cells["CropYear"].Value);
							if (Pd.CropYear == string.Empty)
							{
								MessageBox.Show("CropYear Required");
								return;
							}
							Pd.InvGrnDetailId = Conversion.ToInt(r6.Cells["InvGrnDetailId"].Value);
							if (Pd.InvGrnDetailId == 0)
							{
								MessageBox.Show("InvGrnDetailId Required");
								return;
							}
							Pd.InvGrnId = Conversion.ToInt(r6.Cells["InvGrnId"].Value);
							if (Pd.InvGrnId == 0)
							{
								MessageBox.Show("InvGrnId Required");
								return;
							}
							if (Conversion.ToSingle(r6.Cells["GrossWeight"].Value) != 0f)
							{
								Pd.GrossWeight = Conversion.ToDouble(r6.Cells["GrossWeight"].Value);
								if (Conversion.ToSingle(r6.Cells["ItemRate"].Value) != 0f)
								{
									Pd.ItemRate = Conversion.ToDouble(r6.Cells["ItemRate"].Value);
									if (Conversion.ToInt(r6.Cells["ItemUomId"].Value) != 0)
									{
										Pd.ItemUOMId = Conversion.ToInt(r6.Cells["ItemUomId"].Value);
										if (Conversion.ToSingle(r6.Cells["ItemAmount"].Value) != 0f)
										{
											Pd.ItemAmount = Conversion.ToDouble(r6.Cells["ItemAmount"].Value);
											Pd.ItemCgsRate = 0.0;
											Pd.JobLotId = Conversion.ToInt(r6.Cells["JobLotId"].Value);
											if (Pd.JobLotId == 0)
											{
												MessageBox.Show("JobLot Required");
												return;
											}
											Pd.LabAnalisysNo = Conversion.ToString(r6.Cells["LabNo"].Value);
											if (Conversion.ToSingle(r6.Cells["NetBillWeight"].Value) != 0f)
											{
												Pd.NetBillWeight = Conversion.ToDouble(r6.Cells["NetBillWeight"].Value);
												Pd.NetStockWeight = Conversion.ToDouble(r6.Cells["StockWeight"].Value);
												if (Pd.NetStockWeight == 0.0)
												{
													MessageBox.Show("NetStockWeight Required");
													return;
												}
												Pd.PackingTypeId = Conversion.ToInt(r6.Cells["PackingTypeId"].Value);
												if (Pd.PackingTypeId == 0)
												{
													MessageBox.Show("PackingType Required");
													return;
												}
												Pd.UomScheduleIdRate = Conversion.ToInt(r6.Cells["ItemRateUOMId"].Value);
												if (Pd.UomScheduleIdRate == 0)
												{
													DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r6.Cells["ItemId"].Value));
													if (dt.Rows.Count > 0)
													{
														DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r6.Cells["RateUom"].Value) + "'");
														if (dr.Length == 0)
														{
															MessageBox.Show("this RateUom not define please check");
															return;
														}
														Pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
													}
												}
												Pd.WarehouseId = Conversion.ToInt(r6.Cells["WarehouseId"].Value);
												Pd.WareHouseName = Conversion.ToString(r6.Cells["WareHouseName"].Value);
												if (Pd.WarehouseId == 0)
												{
													MessageBox.Show("WareHouse Required");
													return;
												}
												Pd.EBTotalWt = Conversion.ToDouble(r6.Cells["EbTotal"].Value);
												Pd.ScaleKart = Conversion.ToDouble(r6.Cells["ScaleKart"].Value);
												Pd.FreightAmount = Conversion.ToDouble(r6.Cells["Freights"].Value);
												Pd.ExpenseAmount = Conversion.ToDouble(r6.Cells["Expense"].Value);
												Pd.CommissionAmount = Conversion.ToDouble(r6.Cells["Commission"].Value);
												Pd.GpNo = Conversion.ToInt(r6.Cells["GpNo"].Value);
												Pd.VehicleNo = Conversion.ToString(r6.Cells["VehicleNo"].Value);
												purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
												continue;
											}
											MessageBox.Show("NetBillWeight Required");
											return;
										}
										MessageBox.Show("ItemAmount Required");
										return;
									}
									MessageBox.Show("ItemUom Required");
									return;
								}
								MessageBox.Show("Item Rate Required");
								return;
							}
							MessageBox.Show("GrossWeight Required");
							return;
						}
						MessageBox.Show("ItemQty  Required");
						return;
					}
					MessageBox.Show("Item  Required");
					return;
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					if (Conversion.ToInt((object)r5.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.PurchaseOrderId = Conversion.ToInt(r5.Cells["OrderId"].Value);
						PF.InvGrnId = Conversion.ToInt(r5.Cells["InvGrnId"].Value);
						PF.TansporterId = Conversion.ToInt(r5.Cells["Transporter"].Value);
						PF.Percentage = Conversion.ToDouble(r5.Cells["Percentage"].Value);
						PF.FrQty = Conversion.ToDouble(r5.Cells["Qty"].Value);
						PF.FrRate = Conversion.ToDouble(r5.Cells["Rate"].Value);
						PF.FreightAmount = Conversion.ToDouble(r5.Cells["Freight"].Value);
						PF.Debit = Conversion.ToDouble(r5.Cells["Debit"].Value);
						PF.Remarks = Conversion.ToString(r5.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
						PE.InvRevExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToDouble(r4.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text.Trim());
						if (PE.Remarks == "0" || PE.Remarks == string.Empty || PE.Remarks == null)
						{
							PE.Remarks = "ItemName : " + Conversion.ToString((object)r4.Cells["ItemId"].Text.Trim()) + "  " + PE.Qty + "  @" + PE.Rate;
						}
						purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
					}
				}
				purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r2.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r2.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r2.Cells["Credit"].Value);
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceEmptyBags PEB = new InvPurchaseInvoiceEmptyBags();
						PEB.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						PEB.TypeId = Conversion.ToInt(r.Cells["TypeId"].Value);
						PEB.InvGrnId = Conversion.ToInt(r.Cells["InvGrnId"].Value);
						PEB.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						PEB.PurchaseQty = Conversion.ToDouble(r.Cells["PurchaseQty"].Value);
						PEB.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PEB.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						if (PEB.TypeId == 1 && PEB.Amount == 0.0)
						{
							throw new Exception("Packing Material Amount Required");
						}
						PEB.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceEmptyBagslist.Add(PEB);
					}
				}
				purchaseInvoice.AttachmentsList = new List<DMSAttachments>();
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.ScreenName = base.Name;
					purchaseInvoice.AttachmentsList.Add(attach);
				}
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				Reset();
				if (ChkBoxParty.Checked)
				{
					Id = sucess;
					CommonServices.PurchaseInvoicettSlip_220A(Id);
				}
				if (ChkBok.Checked)
				{
					Id = sucess;
					VoucherHeadIdGet();
					Id = 0;
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 166);
				}
				return;
			}
			throw new Exception("Grid Record Not found");
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
				DocDate.Focus();
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
				Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Control && e.KeyCode == Keys.Left && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdFreight).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdInvExp).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdGLedger).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdEmptyBags).Focus();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
				LoadGRN = new frmLoadGRNForPaddyPurchase(UserAccount);
				LoadGRN.DocumentTypeId = 165;
				LoadGRN.ShowDialog();
				LoadGRN.BringToFront();
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

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OtherItemsBind();
			AccountsFill();
			SupplierNameFilll();
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Id, base.Name);
			}
			if (e.Column.Key == "InvoiceNo" && item != null && (int)item.RowType == 0)
			{
				new PurchaseInvoiceReverseActivity(UserAccount, Id).Show();
			}
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
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(cmbsuppliername.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(cmbsuppliername.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
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
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DateTime.Now;
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

	private void cmbcommagent_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommtype_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void cmbcommuom_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
		CommissionProportion();
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DuedaysCalculates();
	}

	private void btnSlip220A_Click(object sender, EventArgs e)
	{
		CommonServices.PurchaseInvoicettSlip_220A(Id);
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		try
		{
			frmPaddyPurchaseRegister frmPaddyPurchaseRegister = new frmPaddyPurchaseRegister(UserAccount);
			frmPaddyPurchaseRegister.DocumentTypeId = 166;
			frmPaddyPurchaseRegister.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_Leave(object sender, EventArgs e)
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
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Expected O, but got Unknown
		//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0c: Expected O, but got Unknown
		//IL_0d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6a: Expected O, but got Unknown
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Expected O, but got Unknown
		//IL_0f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5a: Expected O, but got Unknown
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f71: Expected O, but got Unknown
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cb: Expected O, but got Unknown
		//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Expected O, but got Unknown
		//IL_3cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cfb: Expected O, but got Unknown
		//IL_3d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d12: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Paddy_Purchase.frmPurchaseInvoiceForGate));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdFreight = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ChkBoxParty = new System.Windows.Forms.CheckBox();
		this.txtcommremarks = new System.Windows.Forms.RichTextBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.cmbcommtype = new UltraCombo();
		this.cmbcommuom = new UltraCombo();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbcommagent = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbsuppliername = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlip220A = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.txtOrderNo = new System.Windows.Forms.TextBox();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1324, 742);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1324, 742);
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
		this.tabForm.Size = new System.Drawing.Size(1316, 716);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 176);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1310, 537);
		this.panel5.TabIndex = 3;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 263);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1310, 274);
		this.panel8.TabIndex = 4;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1310, 274);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.panel12.Controls.Add(this.panel16);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 126);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1310, 137);
		this.panel12.TabIndex = 3;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(641, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(668, 137);
		this.panel16.TabIndex = 3;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(668, 137);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(641, 137);
		this.panel13.TabIndex = 2;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(641, 137);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1310, 126);
		this.panel7.TabIndex = 2;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(641, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(668, 126);
		this.panel10.TabIndex = 1;
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
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(668, 126);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(641, 126);
		this.panel9.TabIndex = 0;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(641, 126);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtDeliveryTerm);
		this.panel4.Controls.Add(this.txtOrderNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.ChkBoxParty);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommuom);
		this.panel4.Controls.Add(this.txtDueDays);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.DueDate);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.ChkBok);
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.txtBillAmount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtbillno);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1310, 145);
		this.panel4.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(169, 121);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 13);
		this.label1.TabIndex = 33;
		this.label1.Text = "Order No";
		this.ChkBoxParty.AutoSize = true;
		this.ChkBoxParty.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxParty.Location = new System.Drawing.Point(923, 33);
		this.ChkBoxParty.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxParty.Name = "ChkBoxParty";
		this.ChkBoxParty.Size = new System.Drawing.Size(89, 17);
		this.ChkBoxParty.TabIndex = 14;
		this.ChkBoxParty.Text = "Print Preview";
		this.ChkBoxParty.UseVisualStyleBackColor = true;
		this.txtcommremarks.Location = new System.Drawing.Point(394, 93);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(229, 45);
		this.txtcommremarks.TabIndex = 11;
		this.txtcommremarks.Text = "";
		this.txtRemarks.Location = new System.Drawing.Point(692, 12);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(224, 76);
		this.txtRemarks.TabIndex = 12;
		this.txtRemarks.Text = "";
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(394, 35);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(85, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		this.cmbcommuom.AllowNull = (DefaultableBoolean)1;
		this.cmbcommuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Location = new System.Drawing.Point(394, 63);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Name = "cmbcommuom";
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Size = new System.Drawing.Size(85, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TextChanged += new System.EventHandler(cmbcommuom_TextChanged);
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(90, 90);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(70, 22);
		this.txtDueDays.TabIndex = 3;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(8, 95);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(54, 13);
		this.label22.TabIndex = 32;
		this.label22.Text = "Due Days";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Enabled = false;
		this.DueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(233, 91);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(76, 20);
		this.DueDate.TabIndex = 4;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(169, 95);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(53, 13);
		this.label16.TabIndex = 31;
		this.label16.Text = "Due Date";
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Calibri", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.Location = new System.Drawing.Point(923, 13);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(65, 17);
		this.ChkBok.TabIndex = 13;
		this.ChkBok.Text = "Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
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
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(394, 7);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(229, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Leave += new System.EventHandler(cmbcommagent_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(318, 13);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(67, 13);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(485, 69);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(43, 13);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(318, 41);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(63, 13);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(90, 63);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 2;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(318, 99);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 13);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(482, 41);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 13);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 69);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 13);
		this.label7.TabIndex = 0;
		this.label7.Text = "Supplier Name";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(553, 36);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(70, 22);
		this.txtcommrate.TabIndex = 8;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(629, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(49, 13);
		this.label4.TabIndex = 20;
		this.label4.Text = "Remarks";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(318, 69);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(58, 13);
		this.label19.TabIndex = 0;
		this.label19.Text = "Rate UOM";
		this.txtcommamount.BackColor = System.Drawing.SystemColors.Control;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(553, 63);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.Size = new System.Drawing.Size(70, 25);
		this.txtcommamount.TabIndex = 10;
		this.txtcommamount.TabStop = false;
		this.txtBillAmount.BackColor = System.Drawing.Color.White;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(1005, 113);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(161, 25);
		this.txtBillAmount.TabIndex = 22;
		this.txtBillAmount.TabStop = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(8, 121);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(37, 13);
		this.label6.TabIndex = 0;
		this.label6.Text = "Bill No";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(922, 120);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 13);
		this.label3.TabIndex = 17;
		this.label3.Text = "Bill Amount";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(90, 116);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(70, 22);
		this.txtbillno.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(44, 13);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(90, 9);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(219, 21);
		this.DocDate.TabIndex = 0;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(90, 36);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(219, 22);
		this.txtdocno.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(8, 13);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(53, 13);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1310, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripLabel1, this.btnFrmRefresh, this.toolStripSeparator1, this.btnNew, this.btnUpdate, this.btnSave, this.btnDelete, this.btnAttachment, this.toolStripButton3, this.btnPrint,
			this.btnSlip220A
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1310, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(154, 24);
		this.toolStripLabel1.Text = "Purchase Invoice         ";
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(64, 24);
		this.toolStripButton3.Text = "Load &GRN";
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
		this.btnSlip220A.Size = new System.Drawing.Size(87, 24);
		this.btnSlip220A.Text = "220A-Print";
		this.btnSlip220A.Click += new System.EventHandler(btnSlip220A_Click);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1316, 716);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 459);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1310, 254);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1310, 254);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 65);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1310, 394);
		this.panel15.TabIndex = 30;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1310, 394);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 44;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 31);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1310, 34);
		this.panel14.TabIndex = 29;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(14, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(281, 24);
		this.label9.TabIndex = 24;
		this.label9.Text = "PURCHASE INVOICE HISTORY";
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1310, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1310, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = (System.Drawing.Image)resources.GetObject("btnLoadAll.Image");
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(73, 24);
		this.btnRegister.Text = "Register";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.txtOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNo.Location = new System.Drawing.Point(233, 117);
		this.txtOrderNo.Name = "txtOrderNo";
		this.txtOrderNo.ReadOnly = true;
		this.txtOrderNo.Size = new System.Drawing.Size(77, 22);
		this.txtOrderNo.TabIndex = 34;
		this.txtOrderNo.TabStop = false;
		this.txtDeliveryTerm.BackColor = System.Drawing.Color.White;
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(1005, 84);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.ReadOnly = true;
		this.txtDeliveryTerm.Size = new System.Drawing.Size(161, 25);
		this.txtDeliveryTerm.TabIndex = 35;
		this.txtDeliveryTerm.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(922, 89);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 13);
		this.label2.TabIndex = 36;
		this.label2.Text = "Delivery Term";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1261, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1261, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1208, -2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1324, 742);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmPurchaseInvoiceForGate";
		this.Text = "Purchase Invoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceForGate_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
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
