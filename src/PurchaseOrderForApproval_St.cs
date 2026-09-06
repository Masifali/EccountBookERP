// using Architecture.Reports;
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
namespace Architecture.WinApp.Steel.Approvals
{

public class PurchaseOrderForApproval_St : Form
{
	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable grdlst = new DataTable();

	public DataTable dtPending = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private bool AllowEditOnVoucherNoinReports;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Label label1;

	internal UltraCombo CmbItem;

	internal UltraCombo CmbSupplier;

	private Label label3;

	private ToolStripButton btnPurchsaeOrderDetailPrint;

	private TextBox txtSrTo;

	private TextBox txtSrFrom;

	private Label label5;

	private Label label4;

	private Label label6;

	private ToolStripButton btnPrintRegister;

	private Button btnshow;

	internal UltraCombo CmbApproved;

	internal UltraCombo CmbStatus;

	private CtrlGrdBar ctrlGrdBar3;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX grdPendingViewVoucher;

	internal GridEX GridDetail;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GridDetailForPendingView;

	private Panel panel10;

	private Panel panel9;

	private Label label8;

	private Panel panel12;

	private Panel panel13;

	private Panel panel14;

	private Label label7;

	private Label label9;

	private Button btnApprove;

	private Panel panel18;

	private Panel panel17;

	private Panel panel16;

	private Panel panel11;

	private Panel panel15;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label10;

	private Button btnPendingForView;

	private Label label12;

	private Button btnApprovedPendingForview;

	private Label label13;

	public PurchaseOrderForApproval_St(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchaseOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			dtViewAttachments.Columns.Add("AttchmentPath");
			comboBind();
			ComboStatusFill();
			ComboApprovedfill();
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			((UltraGridBase)CmbApproved).Rows[1].Activate();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			todate.Value = DateTime.Today;
			fromdate.Focus();
			GetDataForApproval();
			GetPendingPurchaseOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comboBind()
	{
		try
		{
			DataTable dtAll = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ComboStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDL(dt, CmbStatus, "Id", "Name", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboApprovedfill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Approved");
			dt.Rows.Add(2, "Not Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbApproved, "Id", "Name", "Status", ZeroIndex: false);
			((UltraGridBase)CmbApproved).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GetDataForApproval();
			GetPendingPurchaseOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataForApproval()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1500;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			obj.PendingForView = 1;
			if (((Control)(object)CmbApproved).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			grdlst = PurchaseOrder.PurchaseOrderRegisterST(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("AvgRate40Kg", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				int i;
				for (i = 0; i < grdlst.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])))
					{
						decimal totalAmount = (from row in grdlst.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("Amount"));
						decimal totalQty = (from row in grdlst.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("OrderItemQty"));
						decimal totalWeight = (from row in grdlst.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(grdlst.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("NetWeight"));
						decimal AvgRate40kg = totalAmount / totalWeight * 40m;
						dt.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["DocNo"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["OrderSupplierId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["CommAgentName"], grdlst.Rows[i]["DeliveryTerm"], Conversion.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToShortDateString(), totalQty, totalWeight, AvgRate40kg, totalAmount, Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), grdlst.Rows[i]["UserNameEusr"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"]).ToShortDateString(), grdlst.Rows[i]["UserNameMusr"], grdlst.Rows[i]["NoOfAttachments"]);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.ClearStructure();
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvgRate40kg"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgRate40kg"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AvgRate40kg"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["AvgRate40kg"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns.Add("Slip");
			grd.RootTable.Columns["Slip"].Caption = "Slip";
			grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip"].ButtonText = "Slip";
			grd.RootTable.Columns["Slip"].Width = 60;
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				grd.RootTable.Columns.Add("Select");
				grd.RootTable.Columns["Select"].ActAsSelector = true;
				grd.RootTable.Columns["Select"].UseHeaderSelector = true;
				grd.RootTable.Columns["Select"].Width = 40;
				grd.RootTable.Columns["Select"].Position = 0;
				grd.RootTable.Columns.Add("IsApproved");
				grd.RootTable.Columns["IsApproved"].Caption = "Approve";
				grd.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["IsApproved"].ButtonText = "Approve";
				grd.RootTable.Columns["IsApproved"].Width = 60;
				grd.RootTable.Columns["IsApproved"].Position = 1;
				grd.RootTable.Columns["Slip"].Position = 2;
				grd.FrozenColumns = 3;
			}
			else
			{
				grd.RootTable.Columns["Slip"].Position = 0;
				grd.FrozenColumns = 1;
			}
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grd.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (e.Column.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			else if (e.Column.Key == "Detail")
			{
				grd_SelectionChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (grd.CurrentColumn.Key == "DocNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.PurchaseOrderSlipReport1500(Id);
				}
			}
			else if (grd.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["OrderSupCustId"].Value), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
				Conversion.ToInt(r.Cells["ItemId"].Value);
			}
			else if (grd.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grd.CurrentColumn.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (grd.CurrentColumn.Key == "Detail")
			{
				grd_SelectionChanged(null, null);
			}
			else if (grd.CurrentColumn.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseGLAC");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CityName");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityName"]);
				}
				GridDetail.DataSource = dt;
				GridDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GridDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemQty"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetWeight"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["NetWeight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null && GridDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetPendingPurchaseOrder()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1500;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			obj.IsApproved = false;
			obj.PendingForView = 2;
			dtPending = PurchaseOrder.PurchaseOrderRegisterST(obj);
			if (dtPending.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("AvgRate40Kg", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				int i;
				for (i = 0; i < dtPending.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])))
					{
						decimal totalAmount = (from row in dtPending.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("Amount"));
						decimal totalQty = (from row in dtPending.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("OrderItemQty"));
						decimal totalWeight = (from row in dtPending.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("NetWeight"));
						decimal AvgRate40kg = totalAmount / totalWeight * 40m;
						dt.Rows.Add(dtPending.Rows[i]["Id"], dtPending.Rows[i]["DocumentTypeId"], dtPending.Rows[i]["DocNo"], Convert.ToDateTime(dtPending.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtPending.Rows[i]["OrderSupplierId"], dtPending.Rows[i]["SupplierName"], dtPending.Rows[i]["CommAgentName"], dtPending.Rows[i]["DeliveryTerm"], Conversion.ToDateTime(dtPending.Rows[i]["OrderExpiryDate"]).ToShortDateString(), totalQty, totalWeight, AvgRate40kg, totalAmount, Conversion.ToDateTime(dtPending.Rows[i]["EntryDate"]).ToShortDateString(), dtPending.Rows[i]["UserNameEusr"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"]).ToShortDateString(), dtPending.Rows[i]["UserNameMusr"], dtPending.Rows[i]["NoOfAttachments"]);
					}
				}
				grdPendingViewVoucher.DataSource = dt;
				grdPendingViewVoucher.RetrieveStructure();
				GridSettingsForPending();
			}
			else
			{
				grdPendingViewVoucher.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForPending()
	{
		try
		{
			grdPendingViewVoucher.RootTable.Columns["Id"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdPendingViewVoucher.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["AvgRate40kg"].AggregateFunction = (AggregateFunction)3;
			grdPendingViewVoucher.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["AvgRate40Kg"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPendingViewVoucher.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns.Add("Slip");
			grdPendingViewVoucher.RootTable.Columns["Slip"].Caption = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].Width = 60;
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				grdPendingViewVoucher.RootTable.Columns.Add("Select");
				grdPendingViewVoucher.RootTable.Columns["Select"].ActAsSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].Width = 40;
				grdPendingViewVoucher.RootTable.Columns["Select"].Position = 0;
				grdPendingViewVoucher.RootTable.Columns.Add("Approve");
				grdPendingViewVoucher.RootTable.Columns["Approve"].Caption = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonText = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].Width = 60;
				grdPendingViewVoucher.RootTable.Columns["Approve"].Position = 1;
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 2;
				grdPendingViewVoucher.FrozenColumns = 3;
			}
			else
			{
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 0;
				grdPendingViewVoucher.FrozenColumns = 1;
			}
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingViewVoucher);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grdPendingViewVoucher.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				GenerateReport(Id);
			}
			if (e.Column.Key == "Approve")
			{
				ApproveOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "NoOfAttachments")
			{
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
			else if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingViewVoucher.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingViewVoucher.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (grdPendingViewVoucher.CurrentColumn.Key == "DocNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count <= 0)
				{
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Approve")
			{
				ApproveOrder(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingViewVoucher_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseGLAC");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CityName");
				DataTable resultRows = (from row in dtPending.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityName"]);
				}
				GridDetailForPendingView.DataSource = dt;
				GridDetailForPendingView.RetrieveStructure();
				GridDetailSettingForPendingRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettingForPendingRecord()
	{
		try
		{
			GridDetailForPendingView.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetailForPendingView);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetailForPendingView.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetailForPendingView.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null && GridDetailForPendingView.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void reset()
	{
		((Control)(object)CmbSupplier).Text = string.Empty;
		((Control)(object)CmbItem).Text = string.Empty;
		txtSrFrom.Clear();
		txtSrTo.Clear();
		fromdate.Focus();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnclose_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	public void ShowReport()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			if (fromdate.Value.ToString() != string.Empty && todate.Value.ToString() != string.Empty)
			{
				Reporting rpt = new Reporting();
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@OrderId", grd.CurrentRow.Cells[0].Value.ToString());
				rpt.RptPerameter("@StartOrderDate", (string)null);
				rpt.RptPerameter("@EndOrderDate", (string)null);
				if (txtSrFrom.Text.Trim().ToString() == string.Empty)
				{
					rpt.RptPerameter("@PoSrFrom", (string)null);
				}
				else
				{
					rpt.RptPerameter("@PoSrFrom", txtSrFrom.Text.Trim().ToString());
				}
				if (txtSrTo.Text.Trim().ToString() == string.Empty)
				{
					rpt.RptPerameter("@PoSrTo", (string)null);
				}
				else
				{
					rpt.RptPerameter("@PoSrTo", txtSrTo.Text.Trim().ToString());
				}
				if (((Control)(object)CmbSupplier).Text.ToString() == string.Empty)
				{
					rpt.RptPerameter("@SupplierCustomerId", (string)null);
				}
				else
				{
					rpt.RptPerameter("@SupplierCustomerId", CmbSupplier.Value.ToString());
				}
				if (((Control)(object)CmbItem).Text.ToString() == string.Empty)
				{
					rpt.RptPerameter("@ItemId", (string)null);
				}
				else
				{
					rpt.RptPerameter("@ItemId", CmbItem.Value.ToString());
				}
				rpt.RptPerameter("@IsApproved", "false");
				rpt.RptPerameter("@Status", (string)null);
				rpt.RptPerameter("@PurchaseOrderId", grd.CurrentRow.Cells[0].Value.ToString());
				rpt.ShowReport("RptPurchaseOrderSlipWithReceivingDetail.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int Id)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport1500(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst == null || grdlst.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "1501-PurchaseOrder_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst == null || grdlst.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(grdlst, "1502-PurchaseOrder_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			tabControl1.SelectedIndex = 0;
			fromdate.Focus();
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			toolStripButton1_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			btnshow_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			btnApprovedOrder_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.B)
		{
			btnPendingForView_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				tabControl1.SelectedIndex = 1;
			}
			else
			{
				tabControl1.SelectedIndex = 0;
			}
		}
		if (!e.Control || e.KeyCode != Keys.Down)
		{
			return;
		}
		if (tabControl1.SelectedIndex == 0)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
			{
				((Control)(object)GridDetail).Focus();
			}
			else
			{
				((Control)(object)grd).Focus();
			}
		}
		else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGridPendingView")
		{
			((Control)(object)GridDetailForPendingView).Focus();
		}
		else
		{
			((Control)(object)grdPendingViewVoucher).Focus();
		}
	}

	private void ApproveOrder(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsapprovedByOrderIdForSteel(val);
				MessageBox.Show("Record Approve Successfully");
				GetDataForApproval();
				GetPendingPurchaseOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovedOrder_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells[0].Value);
						item.ReqType = "Approve";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					PurchaseOrder.UpdateStatusandIsapprovedByOrderIdForSteel(obj);
					MessageBox.Show("Record Approve Successfully");
					GetDataForApproval();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Move PurchaseOrder in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "PendingForView";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					PurchaseOrder.UpdateStatusandIsapprovedByOrderIdForSteel(obj);
					MessageBox.Show("Record Updated Successfully");
					GetDataForApproval();
					GetPendingPurchaseOrder();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovedPendingForview_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdPendingViewVoucher.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdPendingViewVoucher.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "Approve";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					PurchaseOrder.UpdateStatusandIsapprovedByOrderIdForSteel(obj);
					MessageBox.Show("Record Approve Successfully");
					GetPendingPurchaseOrder();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
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
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Expected O, but got Unknown
		//IL_1616: Unknown result type (might be due to invalid IL or missing references)
		//IL_1620: Expected O, but got Unknown
		//IL_18c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cf: Expected O, but got Unknown
		//IL_18f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Approvals.PurchaseOrderForApproval_St));
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.btnApprove = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.GridDetailForPendingView = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdPendingViewVoucher = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnApprovedPendingForview = new System.Windows.Forms.Button();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbApproved = new UltraCombo();
		this.CmbStatus = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSrTo = new System.Windows.Forms.TextBox();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPurchsaeOrderDetailPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		this.panel17.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel11.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 30);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(134, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Purchase Order";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(950, 203);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "MasterGrid";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick_1);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 59);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 602);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 126);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 476);
		this.panel6.TabIndex = 110;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 476);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 450);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Po_Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 270);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 177);
		this.panel8.TabIndex = 23;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 21);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 156);
		this.panel10.TabIndex = 26;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(950, 156);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 2;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 21);
		this.panel9.TabIndex = 25;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(46, 17);
		this.label8.TabIndex = 1;
		this.label8.Text = "Detail";
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 267);
		this.panel7.TabIndex = 22;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 64);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 203);
		this.panel12.TabIndex = 6;
		this.panel13.Controls.Add(this.label10);
		this.panel13.Controls.Add(this.btnPendingForView);
		this.panel13.Controls.Add(this.btnApprove);
		this.panel13.Controls.Add(this.label9);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(950, 64);
		this.panel13.TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.DarkBlue;
		this.label10.Location = new System.Drawing.Point(457, 36);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(315, 17);
		this.label10.TabIndex = 141;
		this.label10.Text = "Click To Transfer Selected rows In Pending for View";
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(777, 31);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(134, 26);
		this.btnPendingForView.TabIndex = 140;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.btnApprove.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprove.FlatAppearance.BorderSize = 0;
		this.btnApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprove.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprove.ForeColor = System.Drawing.Color.White;
		this.btnApprove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprove.Location = new System.Drawing.Point(231, 32);
		this.btnApprove.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(78, 24);
		this.btnApprove.TabIndex = 134;
		this.btnApprove.Text = "Approve";
		this.btnApprove.UseVisualStyleBackColor = false;
		this.btnApprove.Click += new System.EventHandler(btnApprovedOrder_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.DarkBlue;
		this.label9.Location = new System.Drawing.Point(6, 36);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(222, 17);
		this.label9.TabIndex = 133;
		this.label9.Text = "Click To Approve The Selected rows";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label7);
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(950, 27);
		this.panel14.TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(4, 4);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(40, 17);
		this.label7.TabIndex = 2;
		this.label7.Text = "Main";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(908, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 450);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending For View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(3, 328);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(950, 119);
		this.panel18.TabIndex = 9;
		this.GridDetailForPendingView.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailForPendingView).BackColor = System.Drawing.Color.White;
		this.GridDetailForPendingView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetailForPendingView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailForPendingView.DesignTimeLayout = GridDetailForPendingView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailForPendingView.FilterMode = (FilterMode)2;
		this.GridDetailForPendingView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailForPendingView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Name = "GridDetailForPendingView";
		((ScrollableBase)this.GridDetailForPendingView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(950, 119);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 2;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label13);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 305);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(950, 23);
		this.panel17.TabIndex = 8;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 17);
		this.label13.TabIndex = 22;
		this.label13.Text = "Detail";
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 64);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(950, 241);
		this.panel16.TabIndex = 7;
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingViewVoucher).BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(950, 241);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Tag = "MasterGridPendingView";
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel11.Controls.Add(this.label12);
		this.panel11.Controls.Add(this.btnApprovedPendingForview);
		this.panel11.Controls.Add(this.panel15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 61);
		this.panel11.TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.DarkBlue;
		this.label12.Location = new System.Drawing.Point(3, 36);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(222, 17);
		this.label12.TabIndex = 138;
		this.label12.Text = "Click To Approve The Selected rows";
		this.btnApprovedPendingForview.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovedPendingForview.FlatAppearance.BorderSize = 0;
		this.btnApprovedPendingForview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovedPendingForview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovedPendingForview.ForeColor = System.Drawing.Color.White;
		this.btnApprovedPendingForview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovedPendingForview.Location = new System.Drawing.Point(233, 31);
		this.btnApprovedPendingForview.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovedPendingForview.Name = "btnApprovedPendingForview";
		this.btnApprovedPendingForview.Size = new System.Drawing.Size(83, 26);
		this.btnApprovedPendingForview.TabIndex = 136;
		this.btnApprovedPendingForview.Text = "Approve";
		this.btnApprovedPendingForview.UseVisualStyleBackColor = false;
		this.btnApprovedPendingForview.Click += new System.EventHandler(btnApprovedPendingForview_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label11);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(950, 28);
		this.panel15.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(40, 17);
		this.label11.TabIndex = 2;
		this.label11.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(908, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApproved);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtSrTo);
		this.panel5.Controls.Add(this.txtSrFrom);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 126);
		this.panel5.TabIndex = 109;
		this.CmbApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Location = new System.Drawing.Point(285, 93);
		((System.Windows.Forms.Control)(object)this.CmbApproved).Name = "CmbApproved";
		((System.Windows.Forms.Control)(object)this.CmbApproved).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbApproved).TabIndex = 7;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(285, 63);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 6;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(435, 93);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(195, 99);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(68, 15);
		this.label6.TabIndex = 108;
		this.label6.Text = "IsApproved";
		this.txtSrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSrTo.Location = new System.Drawing.Point(77, 95);
		this.txtSrTo.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrTo.Name = "txtSrTo";
		this.txtSrTo.Size = new System.Drawing.Size(107, 23);
		this.txtSrTo.TabIndex = 3;
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSrFrom.Location = new System.Drawing.Point(77, 65);
		this.txtSrFrom.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(107, 23);
		this.txtSrFrom.TabIndex = 2;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(285, 34);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 5;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(285, 6);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 99);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 106;
		this.label5.Text = "PoSrFrom";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(195, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 69);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 106;
		this.label4.Text = "PoSrFrom";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(195, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Supplier Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(195, 69);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 15);
		this.label3.TabIndex = 106;
		this.label3.Text = "Status";
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(77, 36);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(107, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(77, 8);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(107, 23);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(10, 12);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(10, 40);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 29);
		this.panel2.TabIndex = 3;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnPurchsaeOrderDetailPrint, this.btnPrintRegister });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(964, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPurchsaeOrderDetailPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchsaeOrderDetailPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchsaeOrderDetailPrint.Name = "btnPurchsaeOrderDetailPrint";
		this.btnPurchsaeOrderDetailPrint.Size = new System.Drawing.Size(85, 24);
		this.btnPurchsaeOrderDetailPrint.Text = "Print-&1501";
		this.btnPurchsaeOrderDetailPrint.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintRegister.Name = "btnPrintRegister";
		this.btnPrintRegister.Size = new System.Drawing.Size(85, 24);
		this.btnPrintRegister.Text = "Print-&1502";
		this.btnPrintRegister.Click += new System.EventHandler(toolStripButton2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchaseOrderForApproval_St";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchaseOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseOrderHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
