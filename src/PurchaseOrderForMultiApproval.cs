// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.DAW;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
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
namespace Architecture.WinApp.MultiApprovalDashboard
{

public class PurchaseOrderForMultiApproval : Form
{
	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable grdlst = new DataTable();

	public DataTable dtPending = new DataTable();

	public int DocumentTypeId;

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

	private Label label2;

	private Label label1;

	internal UltraCombo CmbItem;

	internal UltraCombo CmbSupplier;

	private Label label3;

	private ToolStripButton btnPurchsaeOrderDetail;

	private TextBox txtSrTo;

	private TextBox txtSrFrom;

	private Label label5;

	private Label label4;

	private Label label6;

	private ToolStripButton toolStripButton2;

	private Button btnshow;

	internal UltraCombo CmbApproved;

	internal UltraCombo CmbStatus;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX grdPendingViewVoucher;

	private ToolStripButton btnApprovedOrder;

	private ToolStripButton btnPendingForView;

	private Label label7;

	private RichTextBox txtComments;

	private Panel panel7;

	private Label label8;

	private Panel panel8;

	internal GridEX GridDetail;

	private Panel panel9;

	internal GridEX GridDetailForPendingView;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	private Label label10;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel14;

	private Panel panel13;

	private Label label13;

	private Panel panel12;

	private Panel panel11;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar3;

	public PurchaseOrderForMultiApproval(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void AllDropDownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtAll = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString()
			});
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Supplier Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void GenerateReport(int Id)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport201(int Id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = PurchaseOrderReports.PurchaseOrderSlipReport201(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Id,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "201-InvRptPurchaseOrderGeneralSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.IsApproved = false;
			obj.ReqType = "False";
			obj.ActionTypeId = false;
			obj.UserId = UserAccount.ID;
			grdlst = PurchaseOrder.PurchaseOrderHistory(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("OrderSupCustId");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("CommissionAgent");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("FinalApprovalUser");
				dt.Columns.Add("IsFinalStatus");
				dt.Columns.Add("IsMandatoryUsers");
				dt.Columns.Add("IsMandatoryStatus");
				dt.Columns.Add("NoOfAttachments");
				dt.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					int id = Conversion.ToInt(grdlst.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						dt.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["DocNo"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["CompanyNameSpCAgent"], grdlst.Rows[i]["DeliveryTerm"], Conversion.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToShortDateString(), Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), grdlst.Rows[i]["UserNameEusr"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(grdlst.Rows[i]["ModifyDate"]).ToShortDateString(), grdlst.Rows[i]["UserNameMusr"], grdlst.Rows[i]["FinalApprovalUser"], grdlst.Rows[i]["IsFinalStatus"], grdlst.Rows[i]["IsMandatoryUsers"], grdlst.Rows[i]["IsMandatoryStatus"], grdlst.Rows[i]["NoOfAttachments"]);
						existingIds.Add(id);
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
			grd.RootTable.Columns["OrderSupCustId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			if (DocumentTypeId == 41)
			{
				grd.RootTable.Columns.Add("Slip");
				grd.RootTable.Columns["Slip"].Caption = "Slip203";
				grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Slip"].ButtonText = "Slip203";
				grd.RootTable.Columns["Slip"].Width = 60;
			}
			else
			{
				grd.RootTable.Columns.Add("Slip");
				grd.RootTable.Columns["Slip"].Caption = "Slip201";
				grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Slip"].ButtonText = "Slip201";
				grd.RootTable.Columns["Slip"].Width = 60;
			}
			grd.RootTable.Columns.Add("Detail");
			grd.RootTable.Columns["Detail"].Caption = "Detail";
			grd.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Detail"].ButtonText = "Detail";
			grd.RootTable.Columns["Detail"].Width = 60;
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
				grd.RootTable.Columns["Detail"].Position = 3;
				grd.FrozenColumns = 4;
			}
			else
			{
				grd.RootTable.Columns["Slip"].Position = 0;
				grd.RootTable.Columns["Detail"].Position = 1;
				grd.FrozenColumns = 2;
			}
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.IsApproved = false;
			obj.ReqType = "True";
			obj.ActionTypeId = true;
			dtPending = PurchaseOrder.PurchaseOrderHistory(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("CommissionAgent");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("NoOfAttachments");
				dt.Clear();
				int i;
				for (i = 0; i < dtPending.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])))
					{
						dt.Rows.Add(dtPending.Rows[i]["Id"], dtPending.Rows[i]["DocumentTypeId"], dtPending.Rows[i]["DocNo"], Conversion.ToDateTime(dtPending.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtPending.Rows[i]["OrderSupCustId"], dtPending.Rows[i]["SupplierName"], dtPending.Rows[i]["CompanyNameSpCAgent"], dtPending.Rows[i]["DeliveryTerm"], Conversion.ToDateTime(dtPending.Rows[i]["OrderExpiryDate"]).ToShortDateString(), Conversion.ToDateTime(dtPending.Rows[i]["EntryDate"]).ToShortDateString(), dtPending.Rows[i]["UserNameEusr"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"])) ? "" : Conversion.ToDateTime(dtPending.Rows[i]["ModifyDate"]).ToShortDateString(), dtPending.Rows[i]["UserNameMusr"], dtPending.Rows[i]["NoOfAttachments"]);
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
			grdPendingViewVoucher.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustment(AryColWidth, grdPendingViewVoucher);
			grdPendingViewVoucher.RootTable.Columns.Add("Slip");
			grdPendingViewVoucher.RootTable.Columns["Slip"].Caption = "Slip203";
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonText = "Slip203";
			grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 0;
			grdPendingViewVoucher.RootTable.Columns["Slip"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns.Add("Approve");
			grdPendingViewVoucher.RootTable.Columns["Approve"].Caption = "Approve";
			grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonText = "Approve";
			grdPendingViewVoucher.RootTable.Columns["Approve"].Position = 1;
			grdPendingViewVoucher.RootTable.Columns["Approve"].Width = 60;
			grdPendingViewVoucher.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveOrder(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				ApprovalList item = new ApprovalList();
				if (txtComments.Text == string.Empty)
				{
					MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtComments.Focus();
					return;
				}
				item.Comments = txtComments.Text.Trim();
				item.DocumentTypeId = Conversion.ToInt(grd.CurrentRow.Cells["DocumentTypeId"].Value);
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(item);
				DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
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
			print_Click_1(null, null);
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
			GetDataForApproval();
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

	private void PurchaseOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			dtViewAttachments.Columns.Add("AttchmentPath");
			DocumentTypeId = Conversion.ToInt(base.Tag);
			if (DocumentTypeId == 700)
			{
				btnPurchsaeOrderDetail.Visible = false;
				toolStripButton2.Visible = false;
			}
			else
			{
				btnPurchsaeOrderDetail.Visible = true;
				toolStripButton2.Visible = true;
			}
			AllDropDownBind();
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

	private void btnclose_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
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
			dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "206-RptPurchaseOrderRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
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
			DataTable dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "205-InvRptPurchaseOrderDetailRegisterRice.rpt");
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				if (txtComments.Text == string.Empty)
				{
					MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtComments.Focus();
					return;
				}
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.Comments = txtComments.Text.Trim();
					item.DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Approve";
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
				MessageBox.Show("Record Approve Successfully");
				GetDataForApproval();
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Move PurchaseOrder in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				if (txtComments.Text == string.Empty)
				{
					MessageBox.Show("Comments Required For Approval", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					txtComments.Focus();
					return;
				}
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt(r.Cells[0].Value);
					item.DocumentTypeId = DocumentTypeId;
					item.ReqType = "PendingForView";
					item.EntryUser = UserAccount.ID;
					item.Comments = txtComments.Text.Trim();
					obj.AllApprovalLists.Add(item);
				}
				DocumentApprovalPolicy.MultiApproveAndRejectDocumentByUserId(obj);
				MessageBox.Show("Record Updated Successfully");
				GetDataForApproval();
				GetPendingPurchaseOrder();
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
			}
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["OrderSupCustId"].Value), fromdate.Value, todate.Value);
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
			int Id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(grd.CurrentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "Slip")
			{
				if (DocumentTypeId == 41)
				{
					GenerateReport(Id);
				}
				else
				{
					GenerateReport201(Id);
				}
			}
			if (e.Column.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			if (e.Column.Key == "Detail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grd.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = DocumentTypeId;
					obj.RefDocNoId = Id;
					obj.Show();
				}
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
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
				}
			}
			else if (grd.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["OrderSupCustId"].Value), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grd.CurrentColumn.Key == "Slip")
			{
				if (DocumentTypeId == 41)
				{
					GenerateReport(Id);
				}
				else
				{
					GenerateReport201(Id);
				}
			}
			else if (grd.CurrentColumn.Key == "Detail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grd.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = DocumentTypeId;
					obj.RefDocNoId = Id;
					obj.Show();
				}
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
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("PPBagPrice", typeof(double));
				dt.Columns.Add("JuteBagPrice", typeof(double));
				dt.Columns.Add("PPBagWeight", typeof(double));
				dt.Columns.Add("JuteBagWeight", typeof(double));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CityArea");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["Crop"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["PPBagRate"], resultRows.Rows[i]["JuteBagRate"], resultRows.Rows[i]["PPBagWeight"], resultRows.Rows[i]["JuteBagWeight"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityArea"]);
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
			GridDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			if (DocumentTypeId == 700)
			{
				GridDetail.RootTable.Columns["CropYear"].Visible = false;
				GridDetail.RootTable.Columns["PPBagPrice"].Visible = false;
				GridDetail.RootTable.Columns["JuteBagPrice"].Visible = false;
				GridDetail.RootTable.Columns["PPBagWeight"].Visible = false;
				GridDetail.RootTable.Columns["JuteBagWeight"].Visible = false;
				GridDetail.RootTable.Columns["JobLot"].Visible = false;
				GridDetail.RootTable.Columns["CityArea"].Visible = false;
			}
			else
			{
				GridDetail.RootTable.Columns["CropYear"].Visible = true;
				GridDetail.RootTable.Columns["PPBagPrice"].Visible = true;
				GridDetail.RootTable.Columns["JuteBagPrice"].Visible = true;
				GridDetail.RootTable.Columns["PPBagWeight"].Visible = true;
				GridDetail.RootTable.Columns["JuteBagWeight"].Visible = true;
				GridDetail.RootTable.Columns["JobLot"].Visible = true;
				GridDetail.RootTable.Columns["CityArea"].Visible = true;
			}
			GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetail.RootTable.Columns["PPBagPrice"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["PPBagPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetail.RootTable.Columns["JuteBagPrice"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["JuteBagPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetail.RootTable.Columns["PPBagWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["PPBagWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["PPBagWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["PPBagWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["JuteBagWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["JuteBagWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["JuteBagWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["JuteBagWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetail);
			ctrlGrdBar1_Load(null, null);
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null && GridDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Slip")
			{
				if (DocumentTypeId == 41)
				{
					GenerateReport(Id);
				}
				else
				{
					GenerateReport201(Id);
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Approve")
			{
				ApproveOrder(Id);
			}
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
				if (DocumentTypeId == 41)
				{
					GenerateReport(Id);
				}
				else
				{
					GenerateReport201(Id);
				}
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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
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
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("PPBagPrice", typeof(double));
				dt.Columns.Add("JuteBagPrice", typeof(double));
				dt.Columns.Add("PPBagWeight", typeof(double));
				dt.Columns.Add("JuteBagWeight", typeof(double));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CityArea");
				DataTable resultRows = (from row in dtPending.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["Crop"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["PPBagRate"], resultRows.Rows[i]["JuteBagRate"], resultRows.Rows[i]["PPBagWeight"], resultRows.Rows[i]["JuteBagWeight"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityArea"]);
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
			GridDetailForPendingView.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			if (DocumentTypeId == 700)
			{
				GridDetailForPendingView.RootTable.Columns["CropYear"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["PPBagPrice"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["JuteBagPrice"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["PPBagWeight"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["JobLot"].Visible = false;
				GridDetailForPendingView.RootTable.Columns["CityArea"].Visible = false;
			}
			else
			{
				GridDetailForPendingView.RootTable.Columns["CropYear"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["PPBagPrice"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["JuteBagPrice"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["PPBagWeight"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["JobLot"].Visible = true;
				GridDetailForPendingView.RootTable.Columns["CityArea"].Visible = true;
			}
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
			GridDetailForPendingView.RootTable.Columns["PPBagPrice"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["PPBagPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetailForPendingView.RootTable.Columns["JuteBagPrice"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["JuteBagPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GridDetailForPendingView.RootTable.Columns["PPBagWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["PPBagWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["PPBagWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["PPBagWeight"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridDetailForPendingView);
			ctrlGrdBar4_Load(null, null);
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetailForPendingView.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null && GridDetailForPendingView.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSrFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSrTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingViewVoucher).Name);
			if (fs != null)
			{
				grdPendingViewVoucher.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailForPendingView).Name);
			if (fs != null)
			{
				GridDetailForPendingView.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Expected O, but got Unknown
		//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Expected O, but got Unknown
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		//IL_1228: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Expected O, but got Unknown
		//IL_161c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1626: Expected O, but got Unknown
		//IL_164a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1654: Expected O, but got Unknown
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
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
		Appearance appearance79 = new Appearance();
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
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.GridDetail = new GridEX();
		this.label11 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label9 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.GridDetailForPendingView = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdPendingViewVoucher = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtComments = new System.Windows.Forms.RichTextBox();
		this.label7 = new System.Windows.Forms.Label();
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
		this.btnPurchsaeOrderDetail = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnApprovedOrder = new System.Windows.Forms.ToolStripButton();
		this.btnPendingForView = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		this.panel13.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel11.SuspendLayout();
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
		this.panel3.Size = new System.Drawing.Size(974, 25);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(212, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Purchase Order Approval";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(974, 633);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 144);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(974, 489);
		this.panel6.TabIndex = 110;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(974, 489);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add(this.ctrlGrdBar1);
		this.tabPage1.Controls.Add(this.label11);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.label10);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(966, 463);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Po_History";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(922, 349);
		this.ctrlGrdBar1.MyGrid = this.GridDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 25;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		this.GridDetail.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(960, 87);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 3;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.Teal;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 349);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(355, 19);
		this.label11.TabIndex = 24;
		this.label11.Text = "Detail Information Of Above Selected Row";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 373);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(960, 87);
		this.panel10.TabIndex = 6;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Dock = System.Windows.Forms.DockStyle.Top;
		this.label10.Location = new System.Drawing.Point(3, 346);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(960, 27);
		this.label10.TabIndex = 5;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(960, 343);
		this.panel7.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(919, 3);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(960, 316);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "MasterGrid";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick_1);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 4);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(201, 19);
		this.label9.TabIndex = 23;
		this.label9.Text = "Order Main Information";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(960, 316);
		this.panel8.TabIndex = 22;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(960, 27);
		this.label8.TabIndex = 2;
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(966, 463);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending Order For View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 398);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(960, 62);
		this.panel14.TabIndex = 7;
		this.GridDetailForPendingView.AllowEdit = (InheritableBoolean)2;
		this.GridDetailForPendingView.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(960, 62);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 3;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel13.Controls.Add(this.ctrlGrdBar4);
		this.panel13.Controls.Add(this.label13);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 373);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(960, 25);
		this.panel13.TabIndex = 6;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(919, 2);
		this.ctrlGrdBar4.MyGrid = this.GridDetailForPendingView;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 25;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(6, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(353, 19);
		this.label13.TabIndex = 1;
		this.label13.Text = "Detail Information of Above Selected Row";
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(960, 370);
		this.panel9.TabIndex = 2;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 25);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(960, 345);
		this.panel12.TabIndex = 6;
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		this.grdPendingViewVoucher.AlternatingColors = true;
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
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(960, 345);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Tag = "MasterGridPendingView";
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(960, 25);
		this.panel11.TabIndex = 5;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(920, 2);
		this.ctrlGrdBar3.MyGrid = this.grdPendingViewVoucher;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 25;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(201, 19);
		this.label12.TabIndex = 1;
		this.label12.Text = "Order Main Information";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.txtComments);
		this.panel5.Controls.Add(this.label7);
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
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(974, 144);
		this.panel5.TabIndex = 109;
		this.txtComments.Location = new System.Drawing.Point(81, 90);
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(468, 49);
		this.txtComments.TabIndex = 111;
		this.txtComments.Text = "";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(13, 95);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 15);
		this.label7.TabIndex = 110;
		this.label7.Text = "Comments";
		this.CmbApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Location = new System.Drawing.Point(735, 64);
		((System.Windows.Forms.Control)(object)this.CmbApproved).Name = "CmbApproved";
		((System.Windows.Forms.Control)(object)this.CmbApproved).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.CmbApproved).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Visible = false;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(735, 36);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Visible = false;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(482, 58);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(67, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(617, 70);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(68, 15);
		this.label6.TabIndex = 108;
		this.label6.Text = "IsApproved";
		this.label6.Visible = false;
		this.txtSrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrTo.Location = new System.Drawing.Point(321, 60);
		this.txtSrTo.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrTo.Name = "txtSrTo";
		this.txtSrTo.Size = new System.Drawing.Size(107, 23);
		this.txtSrTo.TabIndex = 3;
		this.txtSrTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrTo_KeyPress);
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(81, 60);
		this.txtSrFrom.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(101, 23);
		this.txtSrFrom.TabIndex = 2;
		this.txtSrFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrFrom_KeyPress);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(321, 30);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 5;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(321, 3);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(205, 64);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 106;
		this.label5.Text = "PoSrTo";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(203, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(13, 64);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 106;
		this.label4.Text = "PoSrFrom";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(203, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Supplier Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(617, 43);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 15);
		this.label3.TabIndex = 106;
		this.label3.Text = "Status";
		this.label3.Visible = false;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(81, 32);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(101, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(81, 5);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(101, 23);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(13, 9);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(13, 36);
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
		this.panel1.Size = new System.Drawing.Size(974, 687);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(974, 29);
		this.panel2.TabIndex = 3;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnPurchsaeOrderDetail, this.toolStripButton2, this.btnApprovedOrder, this.btnPendingForView });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(526, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPurchsaeOrderDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchsaeOrderDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchsaeOrderDetail.Name = "btnPurchsaeOrderDetail";
		this.btnPurchsaeOrderDetail.Size = new System.Drawing.Size(98, 24);
		this.btnPurchsaeOrderDetail.Text = "206-PoDetail";
		this.btnPurchsaeOrderDetail.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(140, 24);
		this.toolStripButton2.Text = "205-PoDetailRegister";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnApprovedOrder.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprovedOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprovedOrder.Name = "btnApprovedOrder";
		this.btnApprovedOrder.Size = new System.Drawing.Size(83, 24);
		this.btnApprovedOrder.Text = "Approved";
		this.btnApprovedOrder.Click += new System.EventHandler(btnApprovedOrder_Click);
		this.btnPendingForView.Image = Architecture.WinApp.Properties.Resources.eye_fill;
		this.btnPendingForView.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(123, 24);
		this.btnPendingForView.Text = "Pending For View";
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(974, 687);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchaseOrderForMultiApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchaseOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseOrderHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
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
