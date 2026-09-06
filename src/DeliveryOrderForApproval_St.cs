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
using Architecture.WinApp.Steel.Sale;
using Architecture.WinApp.StoreManagement;
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

public class DeliveryOrderForApproval_St : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	public int DocumentTypeId;

	public int DeliveryOrderTypeId;

	private bool AllowEditOnVoucherNoinReports;

	private DataTable dtGrid = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtHistory = new DataTable();

	public DataTable dtPending = new DataTable();

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStrip toolStrip1;

	private Panel panel2;

	private Panel panel3;

	internal GridEX grdHistory;

	private Panel panel4;

	private Panel panel1;

	private Label label5;

	private DateTimePicker DateTo;

	private Label label4;

	private DateTimePicker DateFrom;

	private Label label2;

	private Button btnshow;

	internal UltraCombo cmbSupplier;

	internal UltraCombo cmbItem;

	private Label label7;

	internal UltraCombo cmbStatus;

	private Label label1;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnRefresh;

	internal GridEX GridDetail;

	private Panel panel5;

	private Label label23;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel13;

	private Label label10;

	private Button btnPendingForView;

	private Button btnApprove;

	private Label label9;

	private Panel panel14;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	private Panel panel9;

	private Label label8;

	private Panel panel7;

	private Panel panel11;

	private Label label12;

	private Button btnApprovePendingRecord;

	private Panel panel15;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX GridPendingForViewMain;

	private Panel panel17;

	private Label label13;

	internal GridEX GridDetailForPendingView;

	public DeliveryOrderForApproval_St(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockWithSupplierHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 800);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("DocumentTypeId");
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("DocNo", typeof(int));
			dtGrid.Columns.Add("DeliveryOrderType");
			dtGrid.Columns.Add("TotalDoQty", typeof(double));
			dtGrid.Columns.Add("TotalDoWeight", typeof(double));
			dtGrid.Columns.Add("EntryUser");
			dtGrid.Columns.Add("EntryDate", typeof(DateTime));
			dtGrid.Columns.Add("ModifyUser");
			dtGrid.Columns.Add("ModifyDate");
			dtGrid.Columns.Add("NoOfAttachments");
			dtGrid.Columns.Add("IsApproved");
			DateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			DateFrom.Focus();
			GetDataFromDeliveryOrderForComboBind();
			StatusFill();
			((UltraGridBase)cmbStatus).Rows[0].Activate();
			DeliveryOrderGridFill();
			GetPendingForViewDeliveryOrder();
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
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			((UltraGridBase)cmbStatus).Rows[0].Activate();
			DateFrom.Focus();
			DeliveryOrderGridFill();
			GetPendingForViewDeliveryOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
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
			GetDataFromDeliveryOrderForComboBind();
			StatusFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataFromDeliveryOrderForComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("CustomerName");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("ItemName");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbSupplier, "Id", "CustomerName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)cmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)cmbSupplier).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dtStatus = new DataTable();
			dtStatus.Columns.Add("Id");
			dtStatus.Columns.Add("Label");
			dtStatus.Rows.Add(1, "Not Approved");
			dtStatus.Rows.Add(2, "Approved");
			dtStatus.Rows.Add(3, "All");
			DDL.BindDDLNew(dtStatus, cmbStatus, "Id", "Label", "Status", false);
			((UltraGridBase)cmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DeliveryOrderGridFill();
			GetPendingForViewDeliveryOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderGridFill()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			obj.DocumentTypeId = DocumentTypeId;
			obj.ReportTypeId = DeliveryOrderTypeId;
			if (((Control)(object)cmbStatus).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			obj.PendingForView = 1;
			dtHistory = InvDeliveryOrder.DeliveryOrderRegister(obj);
			if (dtHistory.Rows.Count > 0)
			{
				dtGrid.Rows.Clear();
				int i;
				for (i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (!dtGrid.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Conversion.ToString(dtHistory.Rows[i]["Id"])))
					{
						decimal totalDoQty = (from row in dtHistory.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtHistory.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("DoQty"));
						decimal totalDoWeight = (from row in dtHistory.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtHistory.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("DoWeight"));
						dtGrid.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["DocNo"], dtHistory.Rows[i]["DeliveryOrderType"], totalDoQty, totalDoWeight, dtHistory.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["ModifyUserName"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"]).ToShortDateString(), dtHistory.Rows[i]["NoOfAttachments"], dtHistory.Rows[i]["IsApproved"]);
					}
				}
				grdHistory.DataSource = dtGrid;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistory.ClearStructure();
				GridDetail.ClearStructure();
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["IsApproved"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["TotalDoQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalDoQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalDoQty"].FormatString = "#,##0,##";
			grdHistory.RootTable.Columns["TotalDoQty"].TotalFormatString = "#,##0,##";
			grdHistory.RootTable.Columns["TotalDoWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalDoWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalDoWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistory);
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				grdHistory.RootTable.Columns.Add("Select");
				grdHistory.RootTable.Columns["Select"].ActAsSelector = true;
				grdHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdHistory.RootTable.Columns["Select"].Width = 40;
				grdHistory.RootTable.Columns["Select"].Position = 0;
				grdHistory.RootTable.Columns.Add("Approve");
				grdHistory.RootTable.Columns["Approve"].Caption = "Approve";
				grdHistory.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Approve"].ButtonText = "Approve";
				grdHistory.RootTable.Columns["Approve"].Width = 60;
				grdHistory.RootTable.Columns["Approve"].Position = 1;
			}
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 60;
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				grdHistory.RootTable.Columns["Print"].Position = 2;
				grdHistory.FrozenColumns = 3;
			}
			else
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("SaleOrderId");
					dt.Columns.Add("SaleOrderNo");
					dt.Columns.Add("CustomerId");
					dt.Columns.Add("CustomerName");
					dt.Columns.Add("SaleGLAC");
					dt.Columns.Add("ItemId");
					dt.Columns.Add("ItemName");
					dt.Columns.Add("PackUom");
					dt.Columns.Add("DoQty", typeof(double));
					dt.Columns.Add("DoWeight", typeof(double));
					dt.Columns.Add("LoadingQty", typeof(double));
					dt.Columns.Add("LoadingWeight", typeof(double));
					dt.Columns.Add("LoadingRemarks");
					DataTable resultRows = (from row in dtHistory.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dt.Rows.Add(resultRows.Rows[i]["SaleOrderId"], resultRows.Rows[i]["SaleOrderNo"], resultRows.Rows[i]["SupplierCustomerId"], resultRows.Rows[i]["CustomerName"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackUOM"], resultRows.Rows[i]["DoQty"], resultRows.Rows[i]["DoWeight"], resultRows.Rows[i]["LoadingQty"], resultRows.Rows[i]["LoadingWeight"], resultRows.Rows[i]["LoadingRemarks"]);
					}
					GridDetail.DataSource = dt;
					GridDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				GridDetail.ClearStructure();
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
			GridDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			GridDetail.RootTable.Columns["CustomerId"].Visible = false;
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridDetail.RootTable.Columns["SaleGLAC"].Visible = false;
			GridDetail.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			GridDetail.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["DoQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["DoWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["DoWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["LoadingQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LoadingQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LoadingQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["LoadingQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["LoadingWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["LoadingWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["LoadingWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["LoadingWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				CommonServices.DeliveryOrderSlipForSteel(Id, DocumentTypeId);
			}
			else if (e.Column.Key == "Approve")
			{
				ApproveDeliveryOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveDeliveryOrder(int Id)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvDeliveryOrder.UpdateDeliveryOrderStatusById(val);
				MessageBox.Show("Record Approve Successfully");
				DeliveryOrderGridFill();
				GetPendingForViewDeliveryOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (!(e.Column.Key == "DocNo"))
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (clsGlobalVariables.ScreenViewReights.Count <= 0)
			{
				return;
			}
			if (DocumentTypeId == 84)
			{
				DeliveryOrder_St obj2 = new DeliveryOrder_St(UserAccount);
				if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "DeliveryOrder").ToList()[0].Value.ToString()))
				{
					MessageBox.Show("Please Check Screen Rights");
					return;
				}
				obj2.Tag = "DeliveryOrder";
				obj2.Show();
				obj2.ReadById(Id);
			}
			if (DocumentTypeId == 85)
			{
				DeliveryOrderPackingMaterial obj = new DeliveryOrderPackingMaterial(UserAccount);
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "DeliveryOrderPackingMaterial").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(Id);
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (grdHistory.CurrentColumn != null)
			{
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.DeliveryOrderSlipForSteel(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "Approve")
				{
					ApproveDeliveryOrder(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingForViewDeliveryOrder()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dtPending.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			obj.DocumentTypeId = DocumentTypeId;
			obj.ReportTypeId = DeliveryOrderTypeId;
			if (((Control)(object)cmbStatus).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			obj.PendingForView = 2;
			dtPending = InvDeliveryOrder.DeliveryOrderRegister(obj);
			if (dtPending.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DeliveryOrderType");
				dt.Columns.Add("TotalDoQty", typeof(double));
				dt.Columns.Add("TotalDoWeight", typeof(double));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("NoOfAttachments");
				dt.Columns.Add("IsApproved");
				int i;
				for (i = 0; i < dtPending.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Conversion.ToString(dtPending.Rows[i]["Id"])))
					{
						decimal totalDoQty = (from row in dtPending.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("DoQty"));
						decimal totalDoWeight = (from row in dtPending.AsEnumerable()
							where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
							select row).Sum((DataRow row) => row.Field<decimal>("DoWeight"));
						dt.Rows.Add(dtPending.Rows[i]["Id"], dtPending.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dtPending.Rows[i]["DocDate"]).ToShortDateString(), dtPending.Rows[i]["DocNo"], dtPending.Rows[i]["DeliveryOrderType"], totalDoQty, totalDoWeight, dtPending.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtPending.Rows[i]["EntryDate"]).ToShortDateString(), dtPending.Rows[i]["ModifyUserName"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"]).ToShortDateString(), dtPending.Rows[i]["NoOfAttachments"], dtPending.Rows[i]["IsApproved"]);
					}
				}
				GridPendingForViewMain.DataSource = dt;
				GridPendingForViewMain.RetrieveStructure();
				GridSettingPendingForView();
			}
			else
			{
				GridPendingForViewMain.DataSource = null;
				GridPendingForViewMain.ClearStructure();
				GridDetailForPendingView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPendingForView()
	{
		try
		{
			GridPendingForViewMain.RootTable.Columns["Id"].Visible = false;
			GridPendingForViewMain.RootTable.Columns["IsApproved"].Visible = false;
			GridPendingForViewMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridPendingForViewMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridPendingForViewMain.RootTable.Columns["TotalDoQty"].AggregateFunction = (AggregateFunction)2;
			GridPendingForViewMain.RootTable.Columns["TotalDoQty"].TextAlignment = (TextAlignment)3;
			GridPendingForViewMain.RootTable.Columns["TotalDoQty"].FormatString = "#,##0,##";
			GridPendingForViewMain.RootTable.Columns["TotalDoQty"].TotalFormatString = "#,##0,##";
			GridPendingForViewMain.RootTable.Columns["TotalDoWeight"].AggregateFunction = (AggregateFunction)2;
			GridPendingForViewMain.RootTable.Columns["TotalDoWeight"].TextAlignment = (TextAlignment)3;
			GridPendingForViewMain.RootTable.Columns["TotalDoWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridPendingForViewMain);
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				GridPendingForViewMain.RootTable.Columns.Add("Select");
				GridPendingForViewMain.RootTable.Columns["Select"].ActAsSelector = true;
				GridPendingForViewMain.RootTable.Columns["Select"].UseHeaderSelector = true;
				GridPendingForViewMain.RootTable.Columns["Select"].Width = 40;
				GridPendingForViewMain.RootTable.Columns["Select"].Position = 0;
				GridPendingForViewMain.RootTable.Columns.Add("Approve");
				GridPendingForViewMain.RootTable.Columns["Approve"].Caption = "Approve";
				GridPendingForViewMain.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GridPendingForViewMain.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				GridPendingForViewMain.RootTable.Columns["Approve"].ButtonText = "Approve";
				GridPendingForViewMain.RootTable.Columns["Approve"].Width = 60;
				GridPendingForViewMain.RootTable.Columns["Approve"].Position = 1;
			}
			GridPendingForViewMain.RootTable.Columns.Add("Print");
			GridPendingForViewMain.RootTable.Columns["Print"].Caption = "Print";
			GridPendingForViewMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridPendingForViewMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GridPendingForViewMain.RootTable.Columns["Print"].ButtonText = "Print";
			GridPendingForViewMain.RootTable.Columns["Print"].Width = 60;
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				GridPendingForViewMain.RootTable.Columns["Print"].Position = 2;
				GridPendingForViewMain.FrozenColumns = 3;
			}
			else
			{
				GridPendingForViewMain.RootTable.Columns["Print"].Position = 0;
				GridPendingForViewMain.FrozenColumns = 1;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridPendingForViewMain.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("SaleOrderId");
					dt.Columns.Add("SaleOrderNo");
					dt.Columns.Add("CustomerId");
					dt.Columns.Add("CustomerName");
					dt.Columns.Add("SaleGLAC");
					dt.Columns.Add("ItemId");
					dt.Columns.Add("ItemName");
					dt.Columns.Add("PackUom");
					dt.Columns.Add("DoQty", typeof(double));
					dt.Columns.Add("DoWeight", typeof(double));
					dt.Columns.Add("LoadingQty", typeof(double));
					dt.Columns.Add("LoadingWeight", typeof(double));
					dt.Columns.Add("LoadingRemarks");
					DataTable resultRows = (from row in dtPending.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dt.Rows.Add(resultRows.Rows[i]["SaleOrderId"], resultRows.Rows[i]["SaleOrderNo"], resultRows.Rows[i]["SupplierCustomerId"], resultRows.Rows[i]["CustomerName"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackUOM"], resultRows.Rows[i]["DoQty"], resultRows.Rows[i]["DoWeight"], resultRows.Rows[i]["LoadingQty"], resultRows.Rows[i]["LoadingWeight"], resultRows.Rows[i]["LoadingRemarks"]);
					}
					GridDetailForPendingView.DataSource = dt;
					GridDetailForPendingView.RetrieveStructure();
					GridDetailForPendingViewSetting();
				}
			}
			else
			{
				GridDetailForPendingView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingViewSetting()
	{
		try
		{
			GridDetailForPendingView.RootTable.Columns["SaleOrderId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["CustomerId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["SaleGLAC"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			GridDetailForPendingView.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["DoQty"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["DoWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["DoWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoWeight"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["LoadingQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["LoadingQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["LoadingQty"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["LoadingQty"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["LoadingWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["LoadingWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["LoadingWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["LoadingWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetailForPendingView);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridPendingForViewMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(GridPendingForViewMain.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				CommonServices.DeliveryOrderSlipForSteel(Id, DocumentTypeId);
			}
			else if (e.Column.Key == "Approve")
			{
				ApproveDeliveryOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridPendingForViewMain.CurrentRow;
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingForViewMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = GridPendingForViewMain.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (GridPendingForViewMain.CurrentColumn != null)
			{
				if (GridPendingForViewMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				else if (GridPendingForViewMain.CurrentColumn.Key == "Print")
				{
					CommonServices.DeliveryOrderSlipForSteel(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
				}
				else if (GridPendingForViewMain.CurrentColumn.Key == "Approve")
				{
					ApproveDeliveryOrder(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtHistory, "1517-DeliveryOrderRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderForApproval_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GridDetail).Focus();
					}
					else
					{
						((Control)(object)grdHistory).Focus();
					}
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGridPendingView")
				{
					((Control)(object)GridDetailForPendingView).Focus();
				}
				else
				{
					((Control)(object)GridPendingForViewMain).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DateFrom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved" && MessageBox.Show("Are you sure to Approve Delivery Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdHistory.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					item.ReqType = "Approve";
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvDeliveryOrder.UpdateDeliveryOrderStatusById(obj);
				MessageBox.Show("Record Approve Successfully");
				DeliveryOrderGridFill();
			}
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
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Move Delivery Order in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdHistory.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "PendingForView";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					InvDeliveryOrder.UpdateDeliveryOrderStatusById(obj);
					MessageBox.Show("Record Moved To Pending For View Successfully");
					DeliveryOrderGridFill();
					GetPendingForViewDeliveryOrder();
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

	private void btnApprovePendingRecord_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (GridPendingForViewMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = GridPendingForViewMain.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "Approve";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					SaleOrder.UpdateSaleOrderStatusById(obj);
					MessageBox.Show("Record Approve Successfully");
					DeliveryOrderGridFill();
					GetPendingForViewDeliveryOrder();
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridPendingForViewMain).Name);
			if (fs != null)
			{
				GridPendingForViewMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetail.CurrentRow;
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
			}
			else if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), DateFrom.Value, DateTo.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GridDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null)
			{
				if (GridDetail.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
				}
				else if (GridDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
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
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
			}
			else if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), DateFrom.Value, DateTo.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailForPendingView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GridDetailForPendingView.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null)
			{
				if (GridDetailForPendingView.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
				}
				else if (GridDetailForPendingView.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_1ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eab: Expected O, but got Unknown
		//IL_285c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2866: Expected O, but got Unknown
		//IL_2aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aaf: Expected O, but got Unknown
		//IL_2ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2add: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Approvals.DeliveryOrderForApproval_St));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridPendingForViewMain_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbSupplier = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.btnApprove = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GridDetailForPendingView = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.GridPendingForViewMain = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnApprovePendingRecord = new System.Windows.Forms.Button();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel4.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridPendingForViewMain).BeginInit();
		this.panel11.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.BtnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(947, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(70, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(102, 24);
		this.btnPrint.Text = "1516-Register";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(947, 29);
		this.panel2.TabIndex = 0;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(933, 286);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Tag = "MasterGrid";
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.DateTo);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.DateFrom);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(947, 122);
		this.panel3.TabIndex = 1;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(947, 30);
		this.panel5.TabIndex = 206;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(127, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Delivery Order";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(75, 92);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 98);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(40, 15);
		this.label1.TabIndex = 205;
		this.label1.Text = "Status";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 69);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 203;
		this.label5.Text = "To Date";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(75, 65);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(130, 23);
		this.DateTo.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 39);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 202;
		this.label4.Text = "From Date";
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(75, 35);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(130, 23);
		this.DateFrom.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(211, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 201;
		this.label2.Text = "Party Name";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnshow.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(214, 92);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(286, 33);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(288, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 3;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(286, 63);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(288, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(211, 69);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 200;
		this.label7.Text = "Item Name";
		this.panel4.Controls.Add(this.tabControl1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 151);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(947, 550);
		this.panel4.TabIndex = 2;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(947, 550);
		this.tabControl1.TabIndex = 2;
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel13);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(939, 524);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Delivery Order Information";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 377);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(933, 144);
		this.panel7.TabIndex = 27;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.DynamicFiltering = true;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(933, 144);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.GridDetail).TabStop = false;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 353);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(933, 24);
		this.panel9.TabIndex = 26;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(46, 17);
		this.label8.TabIndex = 1;
		this.label8.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(889, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 67);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(933, 286);
		this.panel6.TabIndex = 7;
		this.panel13.Controls.Add(this.label10);
		this.panel13.Controls.Add(this.btnPendingForView);
		this.panel13.Controls.Add(this.btnApprove);
		this.panel13.Controls.Add(this.label9);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(933, 64);
		this.panel13.TabIndex = 6;
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
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.DarkBlue;
		this.label9.Location = new System.Drawing.Point(6, 36);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(222, 17);
		this.label9.TabIndex = 133;
		this.label9.Text = "Click To Approve The Selected rows";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar1);
		this.panel14.Controls.Add(this.label3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(933, 27);
		this.panel14.TabIndex = 5;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(890, 2);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 17);
		this.label3.TabIndex = 2;
		this.label3.Text = "Main";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridPendingForViewMain);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(939, 524);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending For View";
		this.tabPage2.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Location = new System.Drawing.Point(3, 376);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Name = "GridDetailForPendingView";
		((ScrollableBase)this.GridDetailForPendingView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(933, 145);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 11;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label13);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 353);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(933, 23);
		this.panel17.TabIndex = 10;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 17);
		this.label13.TabIndex = 22;
		this.label13.Text = "Detail";
		this.GridPendingForViewMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridPendingForViewMain).BackColor = System.Drawing.Color.White;
		this.GridPendingForViewMain.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridPendingForViewMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridPendingForViewMain.DesignTimeLayout = GridPendingForViewMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.GridPendingForViewMain.FilterMode = (FilterMode)2;
		this.GridPendingForViewMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridPendingForViewMain.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Location = new System.Drawing.Point(3, 64);
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Name = "GridPendingForViewMain";
		((ScrollableBase)this.GridPendingForViewMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Size = new System.Drawing.Size(933, 289);
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).Tag = "MasterGridPendingView";
		this.GridPendingForViewMain.TotalRow = (InheritableBoolean)1;
		this.GridPendingForViewMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridPendingForViewMain).VisualStyle = (VisualStyle)4;
		this.GridPendingForViewMain.ColumnButtonClick += new ColumnActionEventHandler(GridPendingForViewMain_ColumnButtonClick);
		this.GridPendingForViewMain.SelectionChanged += new System.EventHandler(GridPendingForViewMain_SelectionChanged);
		this.GridPendingForViewMain.LinkClicked += new ColumnActionEventHandler(GridPendingForViewMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridPendingForViewMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GridPendingForViewMain_KeyDown);
		this.panel11.Controls.Add(this.label12);
		this.panel11.Controls.Add(this.btnApprovePendingRecord);
		this.panel11.Controls.Add(this.panel15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(933, 61);
		this.panel11.TabIndex = 8;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.DarkBlue;
		this.label12.Location = new System.Drawing.Point(3, 36);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(222, 17);
		this.label12.TabIndex = 138;
		this.label12.Text = "Click To Approve The Selected rows";
		this.btnApprovePendingRecord.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovePendingRecord.FlatAppearance.BorderSize = 0;
		this.btnApprovePendingRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovePendingRecord.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovePendingRecord.ForeColor = System.Drawing.Color.White;
		this.btnApprovePendingRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovePendingRecord.Location = new System.Drawing.Point(233, 31);
		this.btnApprovePendingRecord.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovePendingRecord.Name = "btnApprovePendingRecord";
		this.btnApprovePendingRecord.Size = new System.Drawing.Size(83, 26);
		this.btnApprovePendingRecord.TabIndex = 136;
		this.btnApprovePendingRecord.Text = "Approve";
		this.btnApprovePendingRecord.UseVisualStyleBackColor = false;
		this.btnApprovePendingRecord.Click += new System.EventHandler(btnApprovePendingRecord_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label11);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(933, 28);
		this.panel15.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(40, 17);
		this.label11.TabIndex = 2;
		this.label11.Text = "Main";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(891, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridPendingForViewMain;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(947, 701);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(947, 701);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DeliveryOrderForApproval_St";
		this.Text = "Delivery Order Approval";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmStockWithSupplierHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DeliveryOrderForApproval_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridPendingForViewMain).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
