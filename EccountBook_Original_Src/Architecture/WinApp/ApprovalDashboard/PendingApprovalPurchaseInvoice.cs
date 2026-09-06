using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.PackingMaterial_Store;
using Architecture.WinApp.Purchase;
using Architecture.WinApp.Sale;
using Architecture.WinApp.StoreManagement;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalPurchaseInvoice : Form
{
	public int BranchesId;

	public static int DocumentTypeId;

	private bool BranchImplemented;

	public bool flagUnApprove;

	private bool AllowEditOnVoucherNoinReports;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel6;

	internal GridEX grdDetail;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private Panel panel4;

	private CheckBox ChkPrint;

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

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private TabPage tabPage2;

	private Panel panel7;

	private Label label2;

	private Panel panel8;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar2;

	public PendingApprovalPurchaseInvoice(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			if (flagUnApprove)
			{
				btnApproved.Text = "UnApprove";
			}
			else
			{
				btnApproved.Text = "Approve";
			}
			if (DocumentTypeId == 98)
			{
				label23.Text = "Sale Invoice Return";
				BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceReturnBranchWise"));
			}
			else if (DocumentTypeId == 57)
			{
				label23.Text = "Purchase Invoice Direct";
				BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceDirectBranchWise"));
			}
			else if (DocumentTypeId == 138)
			{
				label23.Text = "Purchase Invoice Against Grn Direct";
			}
			else if (DocumentTypeId == 702)
			{
				label23.Text = "Packing Material Invoice";
			}
			else if (DocumentTypeId == 245)
			{
				label23.Text = "Packing Material Invoice Direct";
			}
			else if (DocumentTypeId == 64)
			{
				label23.Text = "Purchase Invoice Store";
			}
			else if (DocumentTypeId == 59)
			{
				label23.Text = "Packing Invoice Return";
				BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceReturnBranchWise"));
			}
			else if (DocumentTypeId == 56)
			{
				BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceBranchWise"));
			}
			if (BranchImplemented)
			{
				UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			}
			AllComboBindAgainstPurchaseInvoice();
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				datFromDate.Value = Convert.ToDateTime(RequestedFromDate);
				datToDate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			datFromDate.Focus();
			if (DocumentTypeId > 0)
			{
				gridHisoryDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllComboBindAgainstPurchaseInvoice()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = Conversion.ToString((object)DocumentTypeId),
				BranchesIds = (BranchImplemented ? UserAccount.BranchesId.ToString() : "")
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Name");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemName")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, CmbSupplier, "Id", "Name", "Customer", ZeroIndex: false);
			}
			else
			{
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "Name", "Item Name", ZeroIndex: false);
				return;
			}
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
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
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstPurchaseInvoice(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.ItemsAgainstPurchaseInvoice(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
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
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			datToDate.Value = DateTime.Today;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridHisoryDetail()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.IsApproved = flagUnApprove;
			obj.BranchesIds = (BranchImplemented ? Conversion.ToString((object)UserAccount.BranchesId) : null);
			dtHistory = InvPurchaseInvoice.PendingInvPurchaseInvoiceForApproval(obj);
			HashSet<int> existingIds = new HashSet<int>();
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocumentTypeId");
				dtGrid.Columns.Add("VoucherHeadId");
				dtGrid.Columns.Add("InvoiceDate", typeof(DateTime));
				dtGrid.Columns.Add("InvoiceNo", typeof(int));
				dtGrid.Columns.Add("BranchSrNo", typeof(int));
				dtGrid.Columns.Add("BranchName", typeof(string));
				dtGrid.Columns.Add("ManualBillNo");
				dtGrid.Columns.Add("SupplierCustomerId");
				dtGrid.Columns.Add("SupplierName");
				dtGrid.Columns.Add("CommissionAgent");
				dtGrid.Columns.Add("CommType");
				dtGrid.Columns.Add("CommRate", typeof(double));
				dtGrid.Columns.Add("CommAmount", typeof(double));
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("NetWeight", typeof(double));
				dtGrid.Columns.Add("AvgRate", typeof(double));
				dtGrid.Columns.Add("BillAmount", typeof(double));
				dtGrid.Columns.Add("Remarks");
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Columns.Add("IsApproved");
				dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				dtGrid.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dtHistory.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						dtGrid.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocumentTypeId"], dtHistory.Rows[i]["VoucherHeadId"], dtHistory.Rows[i]["InvoiceDate"], dtHistory.Rows[i]["InvoiceNo"], dtHistory.Rows[i]["BranchSrNo"], dtHistory.Rows[i]["BranchName"], dtHistory.Rows[i]["ManualBillNo"], dtHistory.Rows[i]["SupplierCustomerId"], dtHistory.Rows[i]["SupplierName"], dtHistory.Rows[i]["CommissionAgent"], dtHistory.Rows[i]["CommType"], dtHistory.Rows[i]["CommRate"], dtHistory.Rows[i]["CommAmount"], dtHistory.Rows[i]["HeaderQty"], dtHistory.Rows[i]["HeaderNetWeight"], dtHistory.Rows[i]["AvgRate"], dtHistory.Rows[i]["BillAmount"], dtHistory.Rows[i]["RemarksHeader"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["UserName"], dtHistory.Rows[i]["IsApproved"], dtHistory.Rows[i]["NoOfAttachments"]);
					}
					existingIds.Add(id);
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["InvoiceDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.OrderNo;
			DataGridHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			DataGridHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			DataGridHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["IsApproved"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["CommType"].Visible = false;
			DataGridHistory.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			DataGridHistory.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			if (DocumentTypeId == 702 || DocumentTypeId == 245)
			{
				DataGridHistory.RootTable.Columns["CommissionAgent"].Visible = false;
				DataGridHistory.RootTable.Columns["CommType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommAmount"].Visible = false;
			}
			if (AllowEditOnVoucherNoinReports)
			{
				DataGridHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			}
			DataGridHistory.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistory.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["AvgRate"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
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
			DataGridHistory.RootTable.Columns["Slip"].Width = 40;
			DataGridHistory.RootTable.Columns["Slip"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 2;
			if (flagUnApprove)
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
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Width = 50;
			DataGridHistory.RootTable.Columns["Detail"].Position = 4;
			DataGridHistory.FrozenColumns = 5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
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
			if (e.Control && e.KeyCode == Keys.A)
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
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridHisoryDetail();
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
				int id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				if (DocumentTypeId == 56)
				{
					CommonServices.PurchaseInvoicettSlip_220A(id);
				}
				else if (DocumentTypeId == 57 || DocumentTypeId == 98)
				{
					CommonServices.PurchaseInvoicetDirectSlip_225A(id);
				}
				else if (DocumentTypeId == 702)
				{
					CommonServices.PurchaseInvoicePMSlip_231(id);
				}
				else if (DocumentTypeId == 64)
				{
					CommonServices.StorePurchaseInvoiceSlip230(id);
				}
				else if (DocumentTypeId == 245)
				{
					CommonServices.PurchaseInvoiceDirectPM_PartySlip_245(id);
				}
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
			}
			if (flagUnApprove)
			{
				if (e.Column.Key == "UnPost")
				{
					ApprovedRecord(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()), "UP");
				}
			}
			else if (e.Column.Key == "Post")
			{
				ApprovedRecord(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()), "AP");
			}
			if (e.Column.Key == "Detail")
			{
				new PurchaseInvoiceReverseActivity(UserAccount, Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString())).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedRecord(int RecId, string ReqType)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			if (ReqType == "AP")
			{
				if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList val = new ApprovalList
			{
				AllApprovalLists = new List<ApprovalList>()
			};
			ApprovalList item = new ApprovalList();
			item.OrganizationId = UserAccount.OrganizationId;
			item.CompanyId = UserAccount.CompanyId;
			item.DocumentTypeId = DocumentTypeId;
			item.Id = RecId;
			item.PostDate = DateTime.Now;
			item.EntryUser = UserAccount.ID;
			item.ReqType = ReqType;
			val.AllApprovalLists.Add(item);
			InvPurchaseInvoice.InvPurchaseInvoiceandVoucherApprove(val);
			if (ReqType == "AP")
			{
				MessageBox.Show("Record Approved Successfully");
			}
			else
			{
				MessageBox.Show("Record UnApproved Successfully");
			}
			if (ChkPrint.Checked)
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString()), DocumentTypeId);
			}
			gridHisoryDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (DocumentTypeId == 56)
			{
				((Control)(object)grdDetail).Name = "PurchaseInvoiceAppovalDetail";
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("ItemId");
				dtdetail.Columns.Add("OrderId");
				dtdetail.Columns.Add("PurchaseOrder");
				dtdetail.Columns.Add("WareHouseName");
				dtdetail.Columns.Add("ItemName");
				dtdetail.Columns.Add("PackType");
				dtdetail.Columns.Add("CropYear");
				dtdetail.Columns.Add("JobLot");
				dtdetail.Columns.Add("GpNo");
				dtdetail.Columns.Add("VehicleNo");
				dtdetail.Columns.Add("ItemQty", typeof(double));
				dtdetail.Columns.Add("ItemUOM");
				dtdetail.Columns.Add("GrossWeight", typeof(double));
				dtdetail.Columns.Add("EBTotalWt", typeof(double));
				dtdetail.Columns.Add("WeightCutTotal", typeof(double));
				dtdetail.Columns.Add("NetBillWeight", typeof(double));
				dtdetail.Columns.Add("NetStockWeight", typeof(double));
				dtdetail.Columns.Add("ItemRate", typeof(double));
				dtdetail.Columns.Add("RateUOM");
				dtdetail.Columns.Add("RateCut", typeof(double));
				dtdetail.Columns.Add("RateCutAmount", typeof(double));
				dtdetail.Columns.Add("ItemAmount", typeof(double));
				dtdetail.Columns.Add("CommissionAmount", typeof(double));
				dtdetail.Columns.Add("ExpenseAmount", typeof(double));
				dtdetail.Columns.Add("FreightAmount", typeof(double));
				dtdetail.Columns.Add("PurchaseAnalysisId", typeof(int));
				dtdetail.Columns.Add("PurchaseAnalysisNo", typeof(int));
				dtdetail.Columns.Add("SampleAnalysisId", typeof(int));
				dtdetail.Columns.Add("SampleAnalysisNo", typeof(int));
				dtdetail.Rows.Clear();
				int Id2 = Conversion.ToInt(item.Cells["Id"].Value);
				EnumerableRowCollection<DataRow> rows = from row in dtHistory.AsEnumerable()
					where row.Field<int>("Id") == Id2
					select row;
				DataTable dtdetailgrid = (rows.Any() ? rows.CopyToDataTable() : dtHistory.Clone());
				if (dtdetailgrid.Rows.Count > 0)
				{
					for (int i = 0; i < dtdetailgrid.Rows.Count; i++)
					{
						dtdetail.Rows.Add(dtdetailgrid.Rows[i]["ItemId"], dtdetailgrid.Rows[i]["PurchaseOrderId"], dtdetailgrid.Rows[i]["OrderNo"], dtdetailgrid.Rows[i]["WareHouseName"], dtdetailgrid.Rows[i]["ItemName"], dtdetailgrid.Rows[i]["PackTypeDesc"], dtdetailgrid.Rows[i]["CropYear"], dtdetailgrid.Rows[i]["JobLotDescription"], dtdetailgrid.Rows[i]["GpNo"], dtdetailgrid.Rows[i]["VehicleNo"], dtdetailgrid.Rows[i]["ItemQty"], dtdetailgrid.Rows[i]["PackUom"], dtdetailgrid.Rows[i]["GrossWeight"], dtdetailgrid.Rows[i]["EBTotalWt"], dtdetailgrid.Rows[i]["WeightCutTotal"], dtdetailgrid.Rows[i]["NetWeight"], dtdetailgrid.Rows[i]["NetStockWeight"], dtdetailgrid.Rows[i]["ItemRate"], dtdetailgrid.Rows[i]["RateUom"], dtdetailgrid.Rows[i]["RateCut"], dtdetailgrid.Rows[i]["RateCutAmount"], dtdetailgrid.Rows[i]["ItemAmount"], dtdetailgrid.Rows[i]["CommissionAmount"], dtdetailgrid.Rows[i]["ExpenseAmount"], dtdetailgrid.Rows[i]["FreightAmount"], dtdetailgrid.Rows[i]["LabPurchaseId"], dtdetailgrid.Rows[i]["LabAnalisysNo"], dtdetailgrid.Rows[i]["InvLabSampleLogRegisterId"], dtdetailgrid.Rows[i]["SampleNo"]);
					}
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					DetailGridSetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
				return;
			}
			if (DocumentTypeId == 702 || DocumentTypeId == 245)
			{
				((Control)(object)grdDetail).Name = "PM_PurchaseInvoiceAppovalDetail";
			}
			else if (DocumentTypeId == 57)
			{
				((Control)(object)grdDetail).Name = "PurchaseInvoiceDirectDetail";
			}
			else if (DocumentTypeId == 98)
			{
				((Control)(object)grdDetail).Name = "SaleReturnInvoice";
			}
			else if (DocumentTypeId == 138)
			{
				((Control)(object)grdDetail).Name = "PurchaseInvoiceAgainstGrnDirect";
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			EnumerableRowCollection<DataRow> rows2 = from row in dtHistory.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable dtdetail2 = (rows2.Any() ? rows2.CopyToDataTable() : dtHistory.Clone());
			DataTable dt = new DataTable();
			dt.Columns.Add("OrderId");
			dt.Columns.Add("PurchaseOrder");
			dt.Columns.Add("ItemId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackUom");
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("NetBillWeight", typeof(double));
			dt.Columns.Add("Rate");
			dt.Columns.Add("RateUom");
			dt.Columns.Add("ItemAmount", typeof(double));
			dt.Columns.Add("Freight", typeof(double));
			dt.Columns.Add("ItemNetAmount", typeof(double));
			dt.Columns.Add("Warehouse");
			if (dtdetail2.Rows.Count > 0)
			{
				for (int j = 0; j < dtdetail2.Rows.Count; j++)
				{
					dt.Rows.Add(dtdetail2.Rows[j]["PurchaseOrderId"], dtdetail2.Rows[j]["OrderNo"], dtdetail2.Rows[j]["ItemId"], dtdetail2.Rows[j]["ItemName"], dtdetail2.Rows[j]["PackUom"], dtdetail2.Rows[j]["ItemQty"], dtdetail2.Rows[j]["NetWeight"], dtdetail2.Rows[j]["ItemRate"], dtdetail2.Rows[j]["RateUom"], dtdetail2.Rows[j]["ItemAmount"], dtdetail2.Rows[j]["FreightAmount"], dtdetail2.Rows[j]["BillAmount"], dtdetail2.Rows[j]["WareHouseName"]);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				SettingForPackingMatrialInvoice();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SettingForPackingMatrialInvoice()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["PurchaseOrder"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdDetail.RootTable.Columns["ItemNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["PurchaseOrder"].Caption = "Order No";
			grdDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailGridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["PurchaseOrder"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["EBTotalWt"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["NetStockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ExpenseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.FreightConstant;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PurchaseAnalysisId"].Visible = false;
			grdDetail.RootTable.Columns["SampleAnalysisId"].Visible = false;
			grdDetail.RootTable.Columns["PurchaseAnalysisNo"].Width = 80;
			grdDetail.RootTable.Columns["SampleAnalysisNo"].Width = 80;
			grdDetail.RootTable.Columns["PurchaseOrder"].Caption = "Order No";
			grdDetail.RootTable.Columns["PurchaseAnalysisNo"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["SampleAnalysisNo"].ColumnType = (ColumnType)5;
			if (Conversion.ToInt(grdDetail.CurrentRow.Cells["PurchaseOrder"].Value) == 0)
			{
				grdDetail.RootTable.Columns["PurchaseOrder"].Visible = false;
			}
			else
			{
				grdDetail.RootTable.Columns["PurchaseOrder"].ColumnType = (ColumnType)5;
			}
			if (Conversion.ToDouble(grdDetail.CurrentRow.Cells["EBTotalWt"].Value) == 0.0)
			{
				grdDetail.RootTable.Columns["EBTotalWt"].Visible = false;
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
			grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCut"].FormatString = "0,0";
			grdDetail.RootTable.Columns["RateCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["RateCutAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "0,0";
			grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.##";
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
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "InvoiceNo")
			{
				int Id = int.Parse(item.Cells["Id"].Value.ToString());
				if (!AllowEditOnVoucherNoinReports || clsGlobalVariables.ScreenViewReights.Count <= 0)
				{
					return;
				}
				if (DocumentTypeId == 56)
				{
					InvfrmPurchaseInvoice obj = new InvfrmPurchaseInvoice(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmPurchaseInvoice").ToList()[0].Value.ToString()))
					{
						obj.Show();
						obj.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
				else if (DocumentTypeId == 57)
				{
					InvfrmPurchasedirectInvoice obj2 = new InvfrmPurchasedirectInvoice(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmPurchasedirectInvoice").ToList()[0].Value.ToString()))
					{
						obj2.Show();
						obj2.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
				else if (DocumentTypeId == 98)
				{
					InvfrmSaleInvoiceReturn obj3 = new InvfrmSaleInvoiceReturn(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmSaleInvoiceReturn").ToList()[0].Value.ToString()))
					{
						obj3.Show();
						obj3.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
				else if (DocumentTypeId == 702)
				{
					PurchaseInvoicePackingMaterial obj5 = new PurchaseInvoicePackingMaterial(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchaseInvoicePackingMaterial").ToList()[0].Value.ToString()))
					{
						obj5.Show();
						obj5.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
				else if (DocumentTypeId == 245)
				{
					frmPurchaseInvoiceDirectPM obj6 = new frmPurchaseInvoiceDirectPM(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmPurchaseInvoiceDirectPM").ToList()[0].Value.ToString()))
					{
						obj6.Show();
						obj6.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
				else if (DocumentTypeId == 64)
				{
					PurchaseInvoiceStoreManagement obj4 = new PurchaseInvoiceStoreManagement(UserAccount);
					if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchaseInvoiceStoreManagement").ToList()[0].Value.ToString()))
					{
						obj4.Show();
						obj4.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You dont have right");
					}
				}
			}
			else if (e.Column.Key == "NoOfAttachments")
			{
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
				}
			}
			else if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), datFromDate.Value, datToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (DataGridHistory.GetRows().Length != 0)
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (flagUnApprove)
			{
				if (MessageBox.Show("Are you sure to UnApprove Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.DocumentTypeId = DocumentTypeId;
						item.Id = Conversion.ToInt(r.Cells[0].Value);
						item.PostDate = DateTime.Now;
						item.EntryUser = UserAccount.ID;
						item.ReqType = "UP";
						obj.AllApprovalLists.Add(item);
					}
					InvPurchaseInvoice.InvPurchaseInvoiceandVoucherApprove(obj);
					MessageBox.Show("Record UnApproved Successfully");
					gridHisoryDetail();
				}
			}
			else if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					ApprovalList item2 = new ApprovalList();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Conversion.ToInt(r2.Cells[0].Value);
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				InvPurchaseInvoice.InvPurchaseInvoiceandVoucherApprove(obj2);
				MessageBox.Show("Record Approved Successfully");
				gridHisoryDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["AccountId"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
			}
			else if (e.Column.Key == "PurchaseOrder")
			{
				int OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				new ActivityDetails(UserAccount, OrderId).Show();
			}
			else if (e.Column.Key == "PurchaseAnalysisNo")
			{
				CommonServices.LabAnalysisReport653(Conversion.ToInt(r.Cells["PurchaseAnalysisId"].Value));
			}
			else
			{
				if (!(e.Column.Key == "SampleAnalysisNo"))
				{
					return;
				}
				int SampleId = Conversion.ToInt(r.Cells["SampleAnalysisId"].Value);
				if (SampleId > 0)
				{
					ReportsParameters obj = new ReportsParameters();
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.Id = SampleId;
					DataTable dt = new DataTable();
					dt = InvLabSampleAnalysisHeader.GetPrintSlipAndReport(obj);
					if (dt.Rows.Count == 0 || dt == null)
					{
						MessageBox.Show("No Record Found For Display");
						return;
					}
					List<SubReportObject> lstSubReports = new List<SubReportObject>();
					SubReportObject subReportObject = new SubReportObject(InvLabSampleAnalysisHeader.GetDataForLabSampleAnalysisSubReport(obj.Id), "LabSampleAnalysisSubParameterReport.rpt");
					lstSubReports.Add(subReportObject);
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTableSubReprt(dt, "657-RptInvLabSampleAnalysisSlipA.rpt", lstSubReports);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			tabControl1.TabPages.Remove(tabPage2);
			if (tabControl1.TabPages.Count == 1)
			{
				tabControl1.TabPages.Add("Cards View");
				InvoiceHistory frm = new InvoiceHistory(UserAccount);
				frm.InvoiceType = 1;
				frm.DocumentTypeId = DocumentTypeId;
				frm.ApproveOrUnApprove = flagUnApprove;
				frm.RequestFromScreen = "Purchase Invoice Approval";
				frm.TopLevel = false;
				frm.Visible = true;
				frm.FormBorderStyle = FormBorderStyle.None;
				frm.Dock = DockStyle.Fill;
				tabControl1.TabPages[1].Controls.Add(frm);
				tabControl1.SelectedIndex = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (DataGridHistory.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), datFromDate.Value, datToDate.Value);
			}
			else if (DataGridHistory.CurrentColumn.Key == "InvoiceNo")
			{
				if (AllowEditOnVoucherNoinReports)
				{
					if (clsGlobalVariables.ScreenViewReights.Count > 0)
					{
						if (DocumentTypeId == 56)
						{
							InvfrmPurchaseInvoice obj = new InvfrmPurchaseInvoice(UserAccount);
							if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmPurchaseInvoice").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("You dont have right");
								return;
							}
							obj.Show();
							obj.ReadById(Id);
						}
						else if (DocumentTypeId == 57)
						{
							InvfrmPurchasedirectInvoice obj2 = new InvfrmPurchasedirectInvoice(UserAccount);
							if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmPurchasedirectInvoice").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("You dont have right");
								return;
							}
							obj2.Show();
							obj2.ReadById(Id);
						}
						else if (DocumentTypeId == 98)
						{
							InvfrmSaleInvoiceReturn obj3 = new InvfrmSaleInvoiceReturn(UserAccount);
							if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "InvfrmSaleInvoiceReturn").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("You dont have right");
								return;
							}
							obj3.Show();
							obj3.ReadById(Id);
						}
						else if (DocumentTypeId == 702)
						{
							PurchaseInvoicePackingMaterial obj4 = new PurchaseInvoicePackingMaterial(UserAccount);
							if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchaseInvoicePackingMaterial").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("You dont have right");
								return;
							}
							obj4.Show();
							obj4.ReadById(Id);
						}
						else if (DocumentTypeId == 245)
						{
							frmPurchaseInvoiceDirectPM obj5 = new frmPurchaseInvoiceDirectPM(UserAccount);
							if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmPurchaseInvoiceDirectPM").ToList()[0].Value.ToString()))
							{
								MessageBox.Show("You dont have right");
								return;
							}
							obj5.Show();
							obj5.ReadById(Id);
						}
					}
				}
				else
				{
					new PurchaseInvoiceReverseActivity(UserAccount, Id).Show();
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Detail")
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				new PurchaseInvoiceReverseActivity(UserAccount, id).Show();
			}
			else if (DataGridHistory.CurrentColumn.Key == "Slip")
			{
				if (DocumentTypeId == 56)
				{
					CommonServices.PurchaseInvoicettSlip_220A(Id);
				}
				else if (DocumentTypeId == 57 || DocumentTypeId == 98)
				{
					CommonServices.PurchaseInvoicetDirectSlip_225A(Id);
				}
				else if (DocumentTypeId == 702)
				{
					CommonServices.PurchaseInvoicePMSlip_231(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (DocumentTypeId == 245)
				{
					CommonServices.PurchaseInvoiceDirectPM_PartySlip_245(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (DocumentTypeId == 64)
				{
					CommonServices.StorePurchaseInvoiceSlip230(Conversion.ToInt(item.Cells["Id"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
			}
			if (flagUnApprove)
			{
				if (DataGridHistory.CurrentColumn.Key == "UnPost")
				{
					ApprovedRecord(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()), "UP");
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Post")
			{
				ApprovedRecord(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()), "AP");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				if (grdDetail.CurrentColumn.Key == "PurchaseOrder")
				{
					int OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					new ActivityDetails(UserAccount, OrderId).Show();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
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
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		//IL_0a79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Expected O, but got Unknown
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingApprovalPurchaseInvoice));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.btnApproved = new System.Windows.Forms.Button();
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
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(948, 709);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(948, 709);
		this.tabControl1.TabIndex = 49;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(940, 683);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Grid View";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 448);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(934, 232);
		this.panel6.TabIndex = 49;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(894, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 9;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(934, 206);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 47;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 26);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(934, 206);
		this.panel8.TabIndex = 49;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(934, 26);
		this.label3.TabIndex = 48;
		this.label3.Text = "Detail Information of Above Selected Row";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 132);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(934, 316);
		this.panel5.TabIndex = 48;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(894, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		this.DataGridHistory.FocusStyle = (FocusStyle)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(934, 290);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Tag = "MasterGrid";
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.CellValueChanged += new ColumnActionEventHandler(DataGridHistory_CellValueChanged);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 26);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(934, 290);
		this.panel7.TabIndex = 2;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(0, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(934, 26);
		this.label2.TabIndex = 1;
		this.label2.Text = "Main Information";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.ChkPrint);
		this.panel4.Controls.Add(this.btnApproved);
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
		this.panel4.Location = new System.Drawing.Point(3, 65);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(934, 67);
		this.panel4.TabIndex = 4;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.Location = new System.Drawing.Point(498, 7);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(67, 19);
		this.ChkPrint.TabIndex = 149;
		this.ChkPrint.Text = "Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(546, 34);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(80, 26);
		this.btnApproved.TabIndex = 5;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(497, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(47, 26);
		this.btnshow.TabIndex = 4;
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
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(285, 34);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(197, 40);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(43, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Variety";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(78, 37);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(112, 21);
		this.datToDate.TabIndex = 1;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(78, 8);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(112, 21);
		this.datFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 11);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(7, 40);
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
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(285, 5);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(197, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 132;
		this.label1.Text = "Supplier Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 35);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(934, 30);
		this.panel3.TabIndex = 3;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(210, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Purchase Invoice History";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(934, 32);
		this.panel2.TabIndex = 1;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(934, 27);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(940, 683);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Cards View";
		this.tabPage2.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(948, 709);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingApprovalPurchaseInvoice";
		this.Text = "Purchase Invoices Approval";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
