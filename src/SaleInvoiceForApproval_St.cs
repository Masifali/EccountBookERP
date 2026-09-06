using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Steel.Inventory;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Steel.Approvals
{

public class SaleInvoiceForApproval_St : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	public static int DocumentTypeId;

	public bool flagUP;

	private bool AllowEditOnVoucherNoinReports;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl1;

	private TabPage GridTab;

	private Panel panel6;

	internal GridEX grdDetail;

	internal GridEX DataGridHistory;

	private Panel panel4;

	private CheckBox chkPrint;

	private Button btnApproved;

	private Button btnshow;

	internal UltraCombo cmbItem;

	private Label label6;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Label label27;

	private Label label33;

	internal UltraCombo CmbSupplier;

	private Label label1;

	private Panel panel3;

	private Label label23;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private TabPage CardsTab;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private Panel panel10;

	private Panel panel9;

	private Panel panel7;

	private Panel panel2;

	private Panel panel8;

	private TabPage tabPage2;

	private Panel panel5;

	private Panel panel11;

	internal GridEX grdDetailPendingForView;

	private Panel panel12;

	private Panel panel13;

	internal GridEX grdPendingForView;

	private Panel panel14;

	private Panel panel15;

	private Button btnApprovePendingForView;

	private Label label9;

	private Label label2;

	private Button btnPendingForView;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label5;

	private Label label4;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar3;

	public SaleInvoiceForApproval_St(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			dtViewAttachments.Columns.Add("AttchmentPath");
			if (flagUP)
			{
				btnApproved.Text = "UnApprove";
			}
			else
			{
				btnApproved.Text = "Approve";
			}
			if (DocumentTypeId == 1510)
			{
				label23.Text = "Sale Invoice Direct Approval";
			}
			combobind();
			datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			datFromDate.Focus();
			gridHisoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAll = InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Customer"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItem, cmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			datToDate.Value = DateTime.Today;
			gridHisoryDetail();
			GridPendingForViewFill();
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisoryDetail();
			GridPendingForViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridHisoryDetail()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.SaleInvoiceApprovalHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value,
				SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				IsApproved = flagUP,
				PendingForView = 1
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtGrid.Columns.Add("VoucherHeadId", typeof(int));
				dtGrid.Columns.Add("InvoiceDate", typeof(DateTime));
				dtGrid.Columns.Add("InvoiceNo", typeof(int));
				dtGrid.Columns.Add("ManualBillNo", typeof(string));
				dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrid.Columns.Add("CustomerName", typeof(string));
				dtGrid.Columns.Add("CommissionAgent", typeof(string));
				dtGrid.Columns.Add("CommType", typeof(string));
				dtGrid.Columns.Add("CommRate", typeof(double));
				dtGrid.Columns.Add("CommAmount", typeof(double));
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("NetWeight", typeof(double));
				dtGrid.Columns.Add("BillAmount", typeof(double));
				dtGrid.Columns.Add("Remarks", typeof(string));
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser", typeof(string));
				dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				dtGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dt.Rows[i]["InvoiceDate"]).ToShortDateString(), dt.Rows[i]["InvoiceNo"], dt.Rows[i]["ManualBillNo"], dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["CommissionAgent"], dt.Rows[i]["CommType"], dt.Rows[i]["CommRate"], dt.Rows[i]["CommAmount"], dt.Rows[i]["TotalQty"], dt.Rows[i]["TotalWeight"], dt.Rows[i]["BillAmount"], dt.Rows[i]["RemarksHeader"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToShortDateString(), dt.Rows[i]["UserName"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtGrid;
				DataGridHistory.RetrieveStructure();
				GridSettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			if (AllowEditOnVoucherNoinReports)
			{
				DataGridHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			}
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Select");
			DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridHistory.RootTable.Columns["Select"].Width = 40;
			DataGridHistory.RootTable.Columns["Select"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 60;
			DataGridHistory.RootTable.Columns["Slip"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 2;
			if (flagUP)
			{
				DataGridHistory.RootTable.Columns.Add("UnPost");
				DataGridHistory.RootTable.Columns["UnPost"].Caption = "UnApprove";
				DataGridHistory.RootTable.Columns["UnPost"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["UnPost"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["UnPost"].ButtonText = "UnApprove";
				DataGridHistory.RootTable.Columns["UnPost"].Width = 80;
				DataGridHistory.RootTable.Columns["UnPost"].Position = 3;
			}
			else
			{
				DataGridHistory.RootTable.Columns.Add("Post");
				DataGridHistory.RootTable.Columns["Post"].Caption = "Approve";
				DataGridHistory.RootTable.Columns["Post"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Post"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Post"].ButtonText = "Approve";
				DataGridHistory.RootTable.Columns["Post"].Width = 60;
				DataGridHistory.RootTable.Columns["Post"].Position = 3;
			}
			DataGridHistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Slip")
			{
				int Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				if (DocumentTypeId == 1509)
				{
					CommonServices.SaleInvoiceSteelSlip_1514(Id);
				}
				else if (DocumentTypeId == 1510)
				{
					CommonServices.SaleInvoiceDirectSteelSlip_1516(Id);
				}
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()), DocumentTypeId);
			}
			if (flagUP)
			{
				if (e.Column.Key == "UnPost")
				{
					GridColumnButtonUnpostClick();
				}
			}
			else if (e.Column.Key == "Post")
			{
				GridColumnButtonPostClick();
			}
			_ = e.Column.Key == "Detail";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (e.Column.Key == "InvoiceNo" && AllowEditOnVoucherNoinReports)
			{
				CommonServices.EditMethodFromLinkedForSteel(DocumentTypeId, 0, Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), datFromDate.Value, datToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("AccountId");
			dtdetail.Columns.Add("SaleOrderId");
			dtdetail.Columns.Add("SaleOrder");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("ExchangeRate", typeof(double));
			dtdetail.Columns.Add("FcyAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			DataGridHistory.UpdateData();
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].ItemId, obj.InvSaleInvoiceDetaillist[i].SaleGLAC, obj.InvSaleInvoiceDetaillist[i].SaleOrderId, obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].PackingType, obj.InvSaleInvoiceDetaillist[i].UOMCode, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].RateUom, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].ExchangeRate, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount);
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
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["AccountId"].Visible = false;
			grdDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			if (Conversion.ToInt(grdDetail.CurrentRow.Cells["SaleOrder"].Value) == 0)
			{
				grdDetail.RootTable.Columns["SaleOrder"].Visible = false;
			}
			else
			{
				grdDetail.RootTable.Columns["SaleOrder"].ColumnType = (ColumnType)5;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["WeightCutTotal"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["WeightCutTotal"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["RateCut"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["RateCut"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["RateCutAmount"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["RateCutAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["CommissionAmount"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["CommissionAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["ExpenseAmount"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["ExpenseAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["FreightAmount"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["FreightAmount"].Visible = false;
			}
			grdDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["AccountId"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
			}
			_ = e.Column.Key == "SaleOrder";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridColumnButtonPostClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "AP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(val);
				MessageBox.Show("Record Approved Successfully");
				if (chkPrint.Checked)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
				}
				gridHisoryDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridColumnButtonUnpostClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "UP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(val);
				MessageBox.Show("Record UnApproved Successfully");
				gridHisoryDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridPendingForViewFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.SaleInvoiceApprovalHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value,
				SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				IsApproved = flagUP,
				PendingForView = 2
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGridPendingForView = new DataTable();
				dtGridPendingForView.Columns.Add("Id", typeof(int));
				dtGridPendingForView.Columns.Add("DocumentTypeId", typeof(int));
				dtGridPendingForView.Columns.Add("VoucherHeadId", typeof(int));
				dtGridPendingForView.Columns.Add("InvoiceDate", typeof(DateTime));
				dtGridPendingForView.Columns.Add("InvoiceNo", typeof(int));
				dtGridPendingForView.Columns.Add("ManualBillNo", typeof(string));
				dtGridPendingForView.Columns.Add("SupplierCustomerId", typeof(int));
				dtGridPendingForView.Columns.Add("CustomerName", typeof(string));
				dtGridPendingForView.Columns.Add("CommissionAgent", typeof(string));
				dtGridPendingForView.Columns.Add("CommType", typeof(string));
				dtGridPendingForView.Columns.Add("CommRate", typeof(double));
				dtGridPendingForView.Columns.Add("CommAmount", typeof(double));
				dtGridPendingForView.Columns.Add("ItemQty", typeof(double));
				dtGridPendingForView.Columns.Add("NetWeight", typeof(double));
				dtGridPendingForView.Columns.Add("BillAmount", typeof(double));
				dtGridPendingForView.Columns.Add("Remarks", typeof(string));
				dtGridPendingForView.Columns.Add("EntryDate", typeof(DateTime));
				dtGridPendingForView.Columns.Add("EntryUser", typeof(string));
				dtGridPendingForView.Columns.Add("NoOfAttachments", typeof(int));
				dtGridPendingForView.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGridPendingForView.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["VoucherHeadId"], Conversion.ToDateTime(dt.Rows[i]["InvoiceDate"]).ToShortDateString(), dt.Rows[i]["InvoiceNo"], dt.Rows[i]["ManualBillNo"], dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["CustomerName"], dt.Rows[i]["CommissionAgent"], dt.Rows[i]["CommType"], dt.Rows[i]["CommRate"], dt.Rows[i]["CommAmount"], dt.Rows[i]["TotalQty"], dt.Rows[i]["TotalWeight"], dt.Rows[i]["BillAmount"], dt.Rows[i]["RemarksHeader"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToShortDateString(), dt.Rows[i]["UserName"], dt.Rows[i]["NoOfAttachments"]);
				}
				grdPendingForView.DataSource = dtGridPendingForView;
				grdPendingForView.RetrieveStructure();
				GridPendingForViewSettings();
			}
			else
			{
				grdPendingForView.DataSource = null;
				grdDetailPendingForView.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewSettings()
	{
		try
		{
			grdPendingForView.RootTable.Columns["Id"].Visible = false;
			grdPendingForView.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingForView.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdPendingForView.RootTable.Columns["SupplierCustomerId"].Visible = false;
			if (AllowEditOnVoucherNoinReports)
			{
				grdPendingForView.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			}
			grdPendingForView.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingForView.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			grdPendingForView.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForView.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdPendingForView.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingForView.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingForView.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingForView.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdPendingForView.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingForView.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingForView);
			grdPendingForView.RootTable.Columns.Add("Select");
			grdPendingForView.RootTable.Columns["Select"].ActAsSelector = true;
			grdPendingForView.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdPendingForView.RootTable.Columns["Select"].Width = 40;
			grdPendingForView.RootTable.Columns["Select"].Position = 0;
			grdPendingForView.RootTable.Columns.Add("Slip");
			grdPendingForView.RootTable.Columns["Slip"].Caption = "Slip";
			grdPendingForView.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingForView.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdPendingForView.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdPendingForView.RootTable.Columns["Slip"].Width = 60;
			grdPendingForView.RootTable.Columns["Slip"].Position = 1;
			grdPendingForView.RootTable.Columns.Add("Voucher");
			grdPendingForView.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdPendingForView.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingForView.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdPendingForView.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdPendingForView.RootTable.Columns["Voucher"].Width = 60;
			grdPendingForView.RootTable.Columns["Voucher"].Position = 2;
			if (flagUP)
			{
				grdPendingForView.RootTable.Columns.Add("UnPost");
				grdPendingForView.RootTable.Columns["UnPost"].Caption = "UnApprove";
				grdPendingForView.RootTable.Columns["UnPost"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingForView.RootTable.Columns["UnPost"].ButtonStyle = (ButtonStyle)5;
				grdPendingForView.RootTable.Columns["UnPost"].ButtonText = "UnApprove";
				grdPendingForView.RootTable.Columns["UnPost"].Width = 80;
				grdPendingForView.RootTable.Columns["UnPost"].Position = 3;
			}
			else
			{
				grdPendingForView.RootTable.Columns.Add("Post");
				grdPendingForView.RootTable.Columns["Post"].Caption = "Approve";
				grdPendingForView.RootTable.Columns["Post"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingForView.RootTable.Columns["Post"].ButtonStyle = (ButtonStyle)5;
				grdPendingForView.RootTable.Columns["Post"].ButtonText = "Approve";
				grdPendingForView.RootTable.Columns["Post"].Width = 60;
				grdPendingForView.RootTable.Columns["Post"].Position = 3;
			}
			grdPendingForView.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForView_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Slip")
			{
				int Id = Conversion.ToInt(grdPendingForView.CurrentRow.Cells["Id"].Value);
				if (DocumentTypeId == 1509)
				{
					CommonServices.SaleInvoiceSteelSlip_1514(Id);
				}
				else if (DocumentTypeId == 1510)
				{
					CommonServices.SaleInvoiceDirectSteelSlip_1516(Id);
				}
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)grdPendingForView.CurrentRow.Cells["Id"].Value.ToString()), DocumentTypeId);
			}
			if (flagUP)
			{
				if (e.Column.Key == "UnPost")
				{
					GridPendingForViewColumnButtonUnpostClick();
				}
			}
			else if (e.Column.Key == "Post")
			{
				GridPendingForViewColumnButtonPostClick();
			}
			if (e.Column.Key == "Detail")
			{
				GridEXRow item = DataGridHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
				{
					GetDetailGrdByHeadId();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingForView_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingForView.CurrentRow;
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (e.Column.Key == "InvoiceNo" && AllowEditOnVoucherNoinReports)
			{
				CommonServices.EditMethodFromLinkedForSteel(DocumentTypeId, 0, Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), datFromDate.Value, datToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingForView_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				GetDetailGrdPendingForViewByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailGrdPendingForViewByHeadId()
	{
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("AccountId");
			dtdetail.Columns.Add("SaleOrderId");
			dtdetail.Columns.Add("SaleOrder");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("NetStockWeight", typeof(double));
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateCut", typeof(double));
			dtdetail.Columns.Add("RateCutAmount", typeof(double));
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("GpNo");
			dtdetail.Columns.Add("VehicleNo");
			dtdetail.Columns.Add("ExchangeRate", typeof(double));
			dtdetail.Columns.Add("FcyAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			dtdetail.Columns.Add("FreightAmount", typeof(double));
			grdPendingForView.UpdateData();
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdPendingForView.CurrentRow.Cells["Id"].Value));
			if (obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].ItemId, obj.InvSaleInvoiceDetaillist[i].SaleGLAC, obj.InvSaleInvoiceDetaillist[i].SaleOrderId, obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].JobLotDescription, obj.InvSaleInvoiceDetaillist[i].PackingType, obj.InvSaleInvoiceDetaillist[i].UOMCode, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].GrossWeight, obj.InvSaleInvoiceDetaillist[i].WeightCutTotal, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].NetStockWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].RateUom, obj.InvSaleInvoiceDetaillist[i].RateCut, obj.InvSaleInvoiceDetaillist[i].RateCutAmount, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].ExchangeRate, obj.InvSaleInvoiceDetaillist[i].FcyAmount, obj.InvSaleInvoiceDetaillist[i].CommissionAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].FreightAmount);
				}
				grdDetailPendingForView.DataSource = dtdetail;
				grdDetailPendingForView.RetrieveStructure();
				DetailGridPendingForViewSetting();
			}
			else
			{
				grdDetailPendingForView.ClearStructure();
			}
		}
		catch (Exception)
		{
		}
	}

	private void DetailGridPendingForViewSetting()
	{
		try
		{
			grdDetailPendingForView.RootTable.Columns["ItemId"].Visible = false;
			grdDetailPendingForView.RootTable.Columns["AccountId"].Visible = false;
			grdDetailPendingForView.RootTable.Columns["SaleOrderId"].Visible = false;
			if (Conversion.ToInt(grdDetailPendingForView.CurrentRow.Cells["SaleOrder"].Value) == 0)
			{
				grdDetailPendingForView.RootTable.Columns["SaleOrder"].Visible = false;
			}
			else
			{
				grdDetailPendingForView.RootTable.Columns["SaleOrder"].ColumnType = (ColumnType)5;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["WeightCutTotal"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["WeightCutTotal"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["RateCut"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["RateCut"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["RateCutAmount"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["RateCutAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["CommissionAmount"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["CommissionAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["ExpenseAmount"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["ExpenseAmount"].Visible = false;
			}
			if (Conversion.ToDouble(grdDetailPendingForView.CurrentRow.Cells["FreightAmount"].Value) == 0.0)
			{
				grdDetailPendingForView.RootTable.Columns["FreightAmount"].Visible = false;
			}
			grdDetailPendingForView.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdDetailPendingForView.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailPendingForView.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailPendingForView.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailPendingForView.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
			grdDetailPendingForView.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["ExpenseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["ExpenseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailPendingForView.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetailPendingForView.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailPendingForView.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetailPendingForView);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewColumnButtonPostClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)grdPendingForView.CurrentRow.Cells["Id"].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "AP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(val);
				MessageBox.Show("Record Approved Successfully");
				if (chkPrint.Checked)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)grdPendingForView.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
				}
				GridPendingForViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewColumnButtonUnpostClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)grdPendingForView.CurrentRow.Cells["Id"].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "UP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(val);
				MessageBox.Show("Record UnApproved Successfully");
				GridPendingForViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				tabControl1.SelectedIndex = 0;
				datFromDate.Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
				{
					((Control)(object)grdDetail).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (DataGridHistory.CurrentColumn.Key == "InvoiceNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, 0, Id);
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (DataGridHistory.CurrentColumn.Key == "Slip")
			{
				if (DocumentTypeId == 95)
				{
					int SupCustId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(item.Cells["InvoiceDate"].Value);
					CommonServices.SaleInvoicetSlip_301(Id, SupCustId, 0, Date, Date);
				}
				else if (DocumentTypeId == 99)
				{
					CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Id);
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Post")
			{
				GridColumnButtonPostClick();
			}
			else if (DataGridHistory.CurrentColumn.Key == "UnPost")
			{
				GridColumnButtonUnpostClick();
			}
			else if (DataGridHistory.CurrentColumn.Key == "Detail")
			{
				new SaleInvoiceReverseActivity(UserAccount, Id).Show();
			}
			else if (DataGridHistory.CurrentColumn.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Id, DocumentTypeId);
			}
			else if (DataGridHistory.CurrentColumn.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), datFromDate.Value, datToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDetail.CurrentColumn != null)
			{
				if (grdDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["AccountId"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
				}
				else if (grdDetail.CurrentColumn.Key == "SaleOrder")
				{
					int SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
					new SaleActivityDetails(UserAccount, SaleOrderId).Show();
				}
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
		if (fs != null)
		{
			grdDetail.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingForView).Name);
		if (fs != null)
		{
			grdPendingForView.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailPendingForView).Name);
		if (fs != null)
		{
			grdDetailPendingForView.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select Rows first To " + btnApproved.Text);
			}
			if (flagUP)
			{
				if (MessageBox.Show("Are you sure to UApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.DocumentTypeId = DocumentTypeId;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.PostDate = DateTime.Now;
						item.EntryUser = UserAccount.ID;
						item.ReqType = "UP";
						obj.AllApprovalLists.Add(item);
					}
					InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(obj);
					MessageBox.Show("Record UnApproved Successfully");
					gridHisoryDetail();
				}
			}
			else if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.BranchesId = UserAccount.BranchesId;
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(obj2);
				MessageBox.Show("Record Approved Successfully");
				gridHisoryDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovePendingForView_Click(object sender, EventArgs e)
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (grdPendingForView.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (flagUP)
			{
				if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdPendingForView.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.DocumentTypeId = DocumentTypeId;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.PostDate = DateTime.Now;
						item.EntryUser = UserAccount.ID;
						item.ReqType = "UP";
						obj.AllApprovalLists.Add(item);
					}
					InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(obj);
					MessageBox.Show("Record UnApproved Successfully");
					GridPendingForViewFill();
				}
			}
			else if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdPendingForView.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.BranchesId = UserAccount.BranchesId;
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(obj2);
				MessageBox.Show("Record Approved Successfully");
				GridPendingForViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select Check Box first");
			}
			if (MessageBox.Show("Are you sure to transfer Invoice's In PendingForView Grid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.BranchesId = UserAccount.BranchesId;
					item.DocumentTypeId = DocumentTypeId;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.PostDate = DateTime.Now;
					item.EntryUser = UserAccount.ID;
					item.ReqType = "PendingForView";
					obj.AllApprovalLists.Add(item);
				}
				InvSaleInvoice.SaleInvoiceandVoucherApproveForSteel(obj);
				MessageBox.Show("Record UnApproved Successfully");
				gridHisoryDetail();
				GridPendingForViewFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
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
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Expected O, but got Unknown
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb0: Expected O, but got Unknown
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Expected O, but got Unknown
		//IL_1845: Unknown result type (might be due to invalid IL or missing references)
		//IL_184f: Expected O, but got Unknown
		//IL_1873: Unknown result type (might be due to invalid IL or missing references)
		//IL_187d: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailPendingForView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingForView_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Approvals.SaleInvoiceForApproval_St));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.GridTab = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.btnApproved = new System.Windows.Forms.Button();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetailPendingForView = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdPendingForView = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnApprovePendingForView = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.CardsTab = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.GridTab.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailPendingForView).BeginInit();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingForView).BeginInit();
		this.panel14.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.GridTab);
		this.tabControl1.Controls.Add(this.CardsTab);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 5;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.GridTab.Controls.Add(this.tabControl2);
		this.GridTab.Controls.Add(this.panel4);
		this.GridTab.Controls.Add(this.panel3);
		this.GridTab.Controls.Add(this.toolStrip3);
		this.GridTab.Location = new System.Drawing.Point(4, 4);
		this.GridTab.Name = "GridTab";
		this.GridTab.Padding = new System.Windows.Forms.Padding(3);
		this.GridTab.Size = new System.Drawing.Size(956, 635);
		this.GridTab.TabIndex = 0;
		this.GridTab.Text = "Grid View";
		this.GridTab.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 129);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(950, 503);
		this.tabControl2.TabIndex = 13;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(942, 477);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(936, 471);
		this.panel6.TabIndex = 12;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 332);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(936, 139);
		this.panel10.TabIndex = 3;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(936, 139);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 47;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 307);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(936, 25);
		this.panel9.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(892, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 9;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "Detail";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 60);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(936, 247);
		this.panel7.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(936, 247);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Tag = "MasterGrid";
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.btnApproved);
		this.panel2.Controls.Add(this.btnPendingForView);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.panel8);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(936, 60);
		this.panel2.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.DarkBlue;
		this.label2.Location = new System.Drawing.Point(440, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(315, 17);
		this.label2.TabIndex = 149;
		this.label2.Text = "Click To Transfer Selected rows In Pending for View";
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(229, 29);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(80, 26);
		this.btnApproved.TabIndex = 0;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(760, 29);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(134, 26);
		this.btnPendingForView.TabIndex = 1;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.DarkBlue;
		this.label3.Location = new System.Drawing.Point(4, 34);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(222, 17);
		this.label3.TabIndex = 146;
		this.label3.Text = "Click To Approve The Selected rows";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(936, 28);
		this.panel8.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(5, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(40, 17);
		this.label4.TabIndex = 5;
		this.label4.Text = "Main";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(891, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(942, 477);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending For View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.panel13);
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(936, 471);
		this.panel5.TabIndex = 13;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailPendingForView);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 334);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(936, 137);
		this.panel11.TabIndex = 3;
		this.grdDetailPendingForView.AllowEdit = (InheritableBoolean)2;
		grdDetailPendingForView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailPendingForView.DesignTimeLayout = grdDetailPendingForView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailPendingForView.DynamicFiltering = true;
		this.grdDetailPendingForView.FilterMode = (FilterMode)2;
		this.grdDetailPendingForView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailPendingForView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).Name = "grdDetailPendingForView";
		((ScrollableBase)this.grdDetailPendingForView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).Size = new System.Drawing.Size(936, 137);
		((System.Windows.Forms.Control)(object)this.grdDetailPendingForView).TabIndex = 47;
		this.grdDetailPendingForView.TotalRow = (InheritableBoolean)1;
		this.grdDetailPendingForView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailPendingForView).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label8);
		this.panel12.Controls.Add(this.ctrlGrdBar4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 309);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(936, 25);
		this.panel12.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(46, 17);
		this.label8.TabIndex = 10;
		this.label8.Text = "Detail";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(892, -1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdDetailPendingForView;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar4.TabIndex = 10;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingForView);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 60);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(936, 249);
		this.panel13.TabIndex = 1;
		this.grdPendingForView.AllowEdit = (InheritableBoolean)2;
		grdPendingForView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingForView.DesignTimeLayout = grdPendingForView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingForView.DynamicFiltering = true;
		this.grdPendingForView.FilterMode = (FilterMode)2;
		this.grdPendingForView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingForView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Name = "grdPendingForView";
		((ScrollableBase)this.grdPendingForView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Size = new System.Drawing.Size(936, 249);
		((System.Windows.Forms.Control)(object)this.grdPendingForView).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdPendingForView).Tag = "MasterGrid";
		this.grdPendingForView.TotalRow = (InheritableBoolean)1;
		this.grdPendingForView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingForView).VisualStyle = (VisualStyle)4;
		this.grdPendingForView.ColumnButtonClick += new ColumnActionEventHandler(grdPendingForView_ColumnButtonClick);
		this.grdPendingForView.SelectionChanged += new System.EventHandler(grdPendingForView_SelectionChanged);
		this.grdPendingForView.LinkClicked += new ColumnActionEventHandler(grdPendingForView_LinkClicked);
		this.panel14.Controls.Add(this.btnApprovePendingForView);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(936, 60);
		this.panel14.TabIndex = 0;
		this.btnApprovePendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovePendingForView.FlatAppearance.BorderSize = 0;
		this.btnApprovePendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovePendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovePendingForView.ForeColor = System.Drawing.Color.White;
		this.btnApprovePendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovePendingForView.Location = new System.Drawing.Point(229, 30);
		this.btnApprovePendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovePendingForView.Name = "btnApprovePendingForView";
		this.btnApprovePendingForView.Size = new System.Drawing.Size(78, 24);
		this.btnApprovePendingForView.TabIndex = 143;
		this.btnApprovePendingForView.Text = "Approve";
		this.btnApprovePendingForView.UseVisualStyleBackColor = false;
		this.btnApprovePendingForView.Click += new System.EventHandler(btnApprovePendingForView_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.DarkBlue;
		this.label9.Location = new System.Drawing.Point(4, 34);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(222, 17);
		this.label9.TabIndex = 142;
		this.label9.Text = "Click To Approve The Selected rows";
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label7);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(936, 28);
		this.panel15.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(40, 17);
		this.label7.TabIndex = 9;
		this.label7.Text = "Main";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(891, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdPendingForView;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar3.TabIndex = 9;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.chkPrint);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 61);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 68);
		this.panel4.TabIndex = 0;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Location = new System.Drawing.Point(586, 10);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(67, 19);
		this.chkPrint.TabIndex = 4;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(583, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(293, 36);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(287, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(197, 42);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Item Name";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(82, 39);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(109, 23);
		this.datToDate.TabIndex = 1;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(82, 9);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(109, 23);
		this.datFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(11, 42);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "To Date";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(293, 6);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(287, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(197, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 132;
		this.label1.Text = "Customer Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 31);
		this.panel3.TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(186, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Sale Invoice Approval";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(950, 27);
		this.toolStrip3.TabIndex = 8;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.CardsTab.Location = new System.Drawing.Point(4, 4);
		this.CardsTab.Name = "CardsTab";
		this.CardsTab.Padding = new System.Windows.Forms.Padding(3);
		this.CardsTab.Size = new System.Drawing.Size(956, 635);
		this.CardsTab.TabIndex = 1;
		this.CardsTab.Text = "Cards View";
		this.CardsTab.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "SaleInvoiceForApproval_St";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.GridTab.ResumeLayout(false);
		this.GridTab.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailPendingForView).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingForView).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
