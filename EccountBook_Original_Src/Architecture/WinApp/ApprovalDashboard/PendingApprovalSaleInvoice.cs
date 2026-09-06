using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalSaleInvoice : Form
{
	public int BranchesId;

	public bool flagUP;

	private bool FormInitialized;

	private bool BranchImplemented;

	public static int DocumentTypeId;

	private bool AllowEditOnVoucherNoinReports;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	public int BaseDocumentTypeId;

	private DataTable dtForLayout = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl1;

	private TabPage GridTab;

	private Panel panel6;

	internal GridEX grdDetail;

	private Panel panel5;

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

	public PendingApprovalSaleInvoice(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentMethod();
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowEditOnVoucherNoinReports"));
			if (DocumentTypeId == 99 || DocumentTypeId == 1608)
			{
				BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceDirectBranchWise"));
			}
			else if (DocumentTypeId == 184)
			{
				BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceDirectBranchWise"));
			}
			else if (DocumentTypeId == 95)
			{
				BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceBranchWise"));
			}
			if (BranchImplemented)
			{
				UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			}
			DataTable combosData = AllComboData();
			DateTime fromDate = ((!Conversion.CheckDateTimeNull(RequestedFromDate)) ? Conversion.ToDateTime((object)RequestedFromDate) : Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period));
			DateTime toDate = ((!Conversion.CheckDateTimeNull(RequestedToDate)) ? Conversion.ToDateTime((object)RequestedToDate) : datToDate.Value);
			DataTable historyData = InvSaleInvoice.PendingInvSaleInvoiceForApproval(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				BaseDocumentTypeId = BaseDocumentTypeId,
				FromDate = fromDate,
				ToDate = toDate,
				IsApproved = flagUP,
				BranchesIds = (BranchImplemented ? Conversion.ToString((object)UserAccount.BranchesId) : null)
			});
			return new
			{
				CombosData = combosData,
				HistoryData = historyData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				FormInitialized = true;
				BindCombos(result.CombosData);
				if (flagUP)
				{
					btnApproved.Text = "UnApprove";
				}
				else
				{
					btnApproved.Text = "Approve";
				}
				if (DocumentTypeId == 99 || DocumentTypeId == 1608)
				{
					label23.Text = "Sale Invoice Direct";
					BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceDirectBranchWise"));
				}
				else if (DocumentTypeId == 184)
				{
					label23.Text = "Sale Invoice Direct Auto Rated";
					BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceDirectBranchWise"));
				}
				else if (DocumentTypeId == 1611)
				{
					label23.Text = "Sale Invoice Return";
				}
				else if (DocumentTypeId == 126)
				{
					if (BaseDocumentTypeId == 1)
					{
						label23.Text = "Sale Invoice Store";
					}
					else if (BaseDocumentTypeId == 2)
					{
						label23.Text = "Sale Invoice PM";
					}
				}
				else if (DocumentTypeId == 145)
				{
					if (BaseDocumentTypeId == 1)
					{
						label23.Text = "Purchase Invoice Return Store";
					}
					else if (BaseDocumentTypeId == 2)
					{
						label23.Text = "Purchase Invoice Return PM";
					}
				}
				else if (DocumentTypeId == 95)
				{
					BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceBranchWise"));
				}
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
				dtHistory = result.HistoryData;
				BindHistoryData(result.HistoryData);
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmPurchaseInvoiceHistory_Load(object sender, EventArgs e)
	{
	}

	private DataTable AllComboData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			return InvSaleInvoice.AllComboBindAgainstSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString(),
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable FilterData(DataTable source, string activityType)
	{
		EnumerableRowCollection<DataRow> filteredRows = from row in source.AsEnumerable()
			where row.Field<string>("Activity") == activityType
			select row;
		if (!filteredRows.Any())
		{
			return source.Clone();
		}
		return filteredRows.CopyToDataTable();
	}

	private void BindComboBox(DataTable data, UltraCombo combo, string placeholder, int columnWidth = -1)
	{
		if (data.Rows.Count > 0)
		{
			DDL.BindDDL(data, combo, "Id", "ReferenceName", placeholder, ZeroIndex: false);
			((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)combo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (columnWidth > 0)
			{
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[1].Width = columnWidth;
			}
		}
	}

	private void BindCombos(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DataTable dtsup = FilterData(dt, "Supplier");
				DataTable dtItem = FilterData(dt, "ItemName");
				BindComboBox(dtsup, CmbSupplier, "Supplier Name");
				BindComboBox(dtItem, cmbItem, "Item Name");
			}
			else
			{
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
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
			BindCombos(AllComboData());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public DataTable GetHistoryData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.BaseDocumentTypeId = BaseDocumentTypeId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.IsApproved = flagUP;
			obj.BranchesIds = (BranchImplemented ? Conversion.ToString((object)UserAccount.BranchesId) : null);
			dtHistory = InvSaleInvoice.PendingInvSaleInvoiceForApproval(obj);
			return dtHistory.Copy();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void BindHistoryData(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				((Control)(object)DataGridHistory).Name = (flagUP ? "UnApproval_Grid" : "Approval_Grid");
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocumentTypeId");
				dtGrid.Columns.Add("BaseDocumentTypeId");
				dtGrid.Columns.Add("VoucherHeadId");
				dtGrid.Columns.Add("InvoiceDate", typeof(DateTime));
				dtGrid.Columns.Add("InvoiceNo", typeof(int));
				dtGrid.Columns.Add("BranchSrNo", typeof(int));
				dtGrid.Columns.Add("BranchName", typeof(string));
				dtGrid.Columns.Add("ManualBillNo");
				dtGrid.Columns.Add("SupplierCustomerId");
				dtGrid.Columns.Add("CustomerName");
				if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
				{
					dtGrid.Columns.Add("CommissionAgent");
					dtGrid.Columns.Add("CommType");
					dtGrid.Columns.Add("CommRate", typeof(double));
					dtGrid.Columns.Add("CommAmount", typeof(double));
				}
				dtGrid.Columns.Add("ItemQty", typeof(double));
				dtGrid.Columns.Add("NetWeight", typeof(double));
				dtGrid.Columns.Add("BillAmount", typeof(double));
				dtGrid.Columns.Add("Remarks");
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Columns.Add("NoOfAttachments");
				dtGrid.Rows.Clear();
				dtForLayout = dtGrid.Clone();
				HashSet<int> existingIds = new HashSet<int>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dt.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						DataRow newRow = dtGrid.NewRow();
						newRow["Id"] = dt.Rows[i]["Id"];
						newRow["DocumentTypeId"] = dt.Rows[i]["DocumentTypeId"];
						newRow["BaseDocumentTypeId"] = dt.Rows[i]["BaseDocumentTypeId"];
						newRow["VoucherHeadId"] = dt.Rows[i]["VoucherHeadId"];
						newRow["InvoiceDate"] = Conversion.ToDateTime(dt.Rows[i]["InvoiceDate"]);
						newRow["InvoiceNo"] = dt.Rows[i]["InvoiceNo"];
						newRow["BranchSrNo"] = dt.Rows[i]["BranchSrNo"];
						newRow["BranchName"] = dt.Rows[i]["BranchName"];
						newRow["ManualBillNo"] = dt.Rows[i]["ManualBillNo"];
						newRow["SupplierCustomerId"] = dt.Rows[i]["SupplierCustomerId"];
						newRow["CustomerName"] = dt.Rows[i]["CustomerName"];
						if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
						{
							newRow["CommissionAgent"] = dt.Rows[i]["CommissionAgent"];
							newRow["CommType"] = dt.Rows[i]["CommType"];
							newRow["CommRate"] = dt.Rows[i]["CommRate"];
							newRow["CommAmount"] = dt.Rows[i]["CommAmount"];
						}
						newRow["ItemQty"] = dt.Rows[i]["HeaderQty"];
						newRow["NetWeight"] = dt.Rows[i]["HeaderNetWeight"];
						newRow["BillAmount"] = dt.Rows[i]["BillAmount"];
						newRow["Remarks"] = dt.Rows[i]["RemarksHeader"];
						newRow["EntryDate"] = Conversion.ToDateTime(dt.Rows[i]["EntryDate"]);
						newRow["EntryUser"] = dt.Rows[i]["UserName"];
						newRow["NoOfAttachments"] = dt.Rows[i]["NoOfAttachments"];
						dtGrid.Rows.Add(newRow);
						existingIds.Add(id);
					}
				}
				dtForLayout = dtGrid.Copy();
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
			throw new Exception(ex.Message);
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			DataGridHistory.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["BaseDocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			if (AllowEditOnVoucherNoinReports)
			{
				DataGridHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			}
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["InvoiceDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.OrderNo;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns.Add("Select");
			DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridHistory.RootTable.Columns["Select"].Width = 35;
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
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Width = 60;
			DataGridHistory.RootTable.Columns["Detail"].Position = 4;
			DataGridHistory.FrozenColumns = 5;
			ctrlGrdBar1_Load(null, null);
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				DataGridHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
				DataGridHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
				DataGridHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
				DataGridHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
				DataGridHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
				DataGridHistory.RootTable.Columns["CommRate"].FormatString = "#,##0.###";
				DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
				DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
				DataGridHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistoryData(GetHistoryData());
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
				GridEXRow r = DataGridHistory.CurrentRow;
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (DocumentTypeId == 95)
				{
					int SupCustId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(r.Cells["InvoiceDate"].Value);
					CommonServices.SaleInvoicetSlip_301(Id, SupCustId, 0, Date, Date);
				}
				else if (DocumentTypeId == 99)
				{
					CommonServices.SaleInvoiceDirectSlip_294("294-InvRptSaleBillDirectWithoutSO.rpt", Id);
				}
				else if (DocumentTypeId == 184)
				{
					CommonServices.SaleInvoiceAutoRatedPartySlip_294(Id);
				}
				else if (DocumentTypeId == 1608)
				{
					CommonServices.SaleInvoicePrint1608(Id);
				}
				else if (DocumentTypeId == 1609)
				{
					CommonServices.SaleInvoicelSlip_1609(Id);
				}
				else if (DocumentTypeId == 1611)
				{
					CommonServices.SaleInvoicelReturnSlip_1611A(Id);
				}
				else if (DocumentTypeId == 1660)
				{
					CommonServices.SaleInvoicelEngr_Slip_1660A(Id);
				}
				else if (DocumentTypeId == 1661)
				{
					CommonServices.SaleInvoiceEngr_Slip_1661A(Id);
				}
				else if (DocumentTypeId == 126)
				{
					CommonServices.SaleInvoiceStorePMSlip(Id, DocumentTypeId);
				}
				else if (DocumentTypeId == 145)
				{
					CommonServices.PurchaseInvoiceReturn_StoreItemSip145A(Id);
				}
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()), DocumentTypeId);
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
			if (e.Column.Key == "Detail")
			{
				new SaleInvoiceReverseActivity(UserAccount, Conversion.ToInt(DataGridHistory.CurrentRow.Cells[0].Value)).Show();
			}
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
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
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "AP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.InvSaleInvoiceandVoucherApprove(val);
				MessageBox.Show("Record Approved Successfully");
				if (chkPrint.Checked)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString()), DocumentTypeId);
				}
				BindHistoryData(GetHistoryData());
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
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
				item.DocumentTypeId = DocumentTypeId;
				item.Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
				item.PostDate = DateTime.Now;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "UP";
				val.AllApprovalLists.Add(item);
				InvSaleInvoice.InvSaleInvoiceandVoucherApprove(val);
				MessageBox.Show("Record UnApproved Successfully");
				BindHistoryData(GetHistoryData());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		try
		{
			DataTable dtdetail = new DataTable();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("SaleOrderId");
			dtdetail.Columns.Add("SaleOrder");
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("ItemName");
			if (DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 126 && DocumentTypeId != 145)
			{
				dtdetail.Columns.Add("CropYear");
				dtdetail.Columns.Add("JobLot");
				dtdetail.Columns.Add("PackType");
			}
			if (DocumentTypeId != 184)
			{
				dtdetail.Columns.Add("GpNo");
				dtdetail.Columns.Add("VehicleNo");
			}
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("ItemUOM");
			if (DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				dtdetail.Columns.Add("ModalDescription");
				dtdetail.Columns.Add("CastingType");
				dtdetail.Columns.Add("Specification/Remarks");
			}
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				dtdetail.Columns.Add("GrossWeight", typeof(double));
				dtdetail.Columns.Add("AddLessWeight", typeof(double));
				dtdetail.Columns.Add("EBTotalWt", typeof(double));
				dtdetail.Columns.Add("WeightCutTotal", typeof(double));
			}
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				dtdetail.Columns.Add("NetStockWeight", typeof(double));
			}
			dtdetail.Columns.Add("ItemRate", typeof(double));
			dtdetail.Columns.Add("RateUOM");
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				dtdetail.Columns.Add("RateCut", typeof(double));
				dtdetail.Columns.Add("RateCutAmount", typeof(double));
			}
			dtdetail.Columns.Add("ItemAmount", typeof(double));
			if (DocumentTypeId == 184 || DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				dtdetail.Columns.Add("DiscountType", typeof(string));
				dtdetail.Columns.Add("DiscountRate", typeof(double));
				dtdetail.Columns.Add("DiscountAmount", typeof(double));
			}
			if (DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				dtdetail.Columns.Add("TaxType");
				dtdetail.Columns.Add("Tax%", typeof(double));
				dtdetail.Columns.Add("TaxAmount", typeof(double));
			}
			dtdetail.Columns.Add("ExpenseAmount", typeof(double));
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661)
			{
				dtdetail.Columns.Add("CommissionAmount", typeof(double));
				dtdetail.Columns.Add("FreightAmount", typeof(double));
			}
			DataGridHistory.UpdateData();
			int Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			EnumerableRowCollection<DataRow> rows = from row in dtHistory.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtHistory.Clone());
			if (dt.Rows.Count > 0)
			{
				dtdetail.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow newRow = dtdetail.NewRow();
					newRow["ItemId"] = Conversion.ToInt(dt.Rows[i]["ItemId"]);
					newRow["SaleOrderId"] = Conversion.ToInt(dt.Rows[i]["SaleOrderId"]);
					newRow["SaleOrder"] = Conversion.ToInt(dt.Rows[i]["OrderNo"]);
					newRow["WareHouseName"] = Conversion.ToString(dt.Rows[i]["WareHouseName"]);
					newRow["ItemName"] = Conversion.ToString(dt.Rows[i]["ItemName"]);
					if (DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 126 && DocumentTypeId != 145)
					{
						newRow["CropYear"] = Conversion.ToString(dt.Rows[i]["cropYear"]);
						newRow["JobLot"] = Conversion.ToString(dt.Rows[i]["JobLot"]);
						newRow["PackType"] = Conversion.ToInt(dt.Rows[i]["PackingType"]);
					}
					if (DocumentTypeId != 184)
					{
						newRow["GpNo"] = Conversion.ToInt(dt.Rows[i]["GpNo"]);
						newRow["VehicleNo"] = Conversion.ToString(dt.Rows[i]["VehicleNo"]);
					}
					newRow["ItemQty"] = Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
					newRow["ItemUOM"] = Conversion.ToString(dt.Rows[i]["PackUom"]);
					if (DocumentTypeId == 1660 || DocumentTypeId == 1661)
					{
						newRow["ModalDescription"] = Conversion.ToString(dt.Rows[i]["ModalDescription"]);
						newRow["CastingType"] = Conversion.ToString(dt.Rows[i]["CastingType"]);
						newRow["Specification/Remarks"] = Conversion.ToInt(dt.Rows[i]["RemarksDetail"]);
					}
					if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
					{
						newRow["GrossWeight"] = Conversion.ToDouble(dt.Rows[i]["GrossWeight"]);
						newRow["AddLessWeight"] = Conversion.ToDouble(dt.Rows[i]["AddLessWeight"]);
						newRow["EBTotalWt"] = Conversion.ToDouble(dt.Rows[i]["EBTotalWt"]);
						newRow["WeightCutTotal"] = Conversion.ToDouble(dt.Rows[i]["WeightCutTotal"]);
					}
					newRow["NetBillWeight"] = Conversion.ToDouble(dt.Rows[i]["NetWeight"]);
					if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
					{
						newRow["NetStockWeight"] = Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					newRow["ItemRate"] = Conversion.ToDouble(dt.Rows[i]["ItemRate"]);
					newRow["RateUOM"] = Conversion.ToString(dt.Rows[i]["RateUom"]);
					if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
					{
						newRow["RateCut"] = Conversion.ToDouble(dt.Rows[i]["RateCut"]);
						newRow["RateCutAmount"] = Conversion.ToDouble(dt.Rows[i]["RateCutAmount"]);
					}
					newRow["ItemAmount"] = Conversion.ToDouble(dt.Rows[i]["ItemAmount"]);
					if (DocumentTypeId == 184 || DocumentTypeId == 1660 || DocumentTypeId == 1661)
					{
						newRow["DiscountType"] = Conversion.ToString(dt.Rows[i]["DiscountType"]);
						newRow["DiscountRate"] = Conversion.ToDouble(dt.Rows[i]["ItemDiscount"]);
						newRow["DiscountAmount"] = Conversion.ToDouble(dt.Rows[i]["ItemDiscountAmount"]);
					}
					if (DocumentTypeId == 1660 || DocumentTypeId == 1661)
					{
						newRow["TaxType"] = Conversion.ToString(dt.Rows[i]["TaxName"]);
						newRow["Tax%"] = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
						newRow["TaxAmount"] = Conversion.ToDouble(dt.Rows[i]["TaxAmount"]);
					}
					newRow["ExpenseAmount"] = Conversion.ToDouble(dt.Rows[i]["ExpenseAmount"]);
					if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
					{
						newRow["CommissionAmount"] = Conversion.ToDouble(dt.Rows[i]["ItemCommission"]);
						newRow["FreightAmount"] = Conversion.ToDouble(dt.Rows[i]["ItemFreightAmount"]);
					}
					dtdetail.Rows.Add(newRow);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["SaleOrder"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			if (DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 126 && DocumentTypeId != 145)
			{
				grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
				grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
				grdDetail.RootTable.Columns["PackType"].Width = Constants.InventoryConstants.PackingType;
			}
			if (DocumentTypeId != 184)
			{
				grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
				grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			}
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdDetail.RootTable.Columns["AddLessWeight"].Width = Constants.InventoryConstants.AddLessConstant;
				grdDetail.RootTable.Columns["EBTotalWt"].Width = Constants.InventoryConstants.EbWt;
				grdDetail.RootTable.Columns["WeightCutTotal"].Width = Constants.InventoryConstants.WtCut;
				grdDetail.RootTable.Columns["NetStockWeight"].Width = Constants.InventoryConstants.WeightConstant;
				grdDetail.RootTable.Columns["RateCut"].Width = Constants.InventoryConstants.ExRateConstant;
				grdDetail.RootTable.Columns["RateCutAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.AmountConstant;
			}
			if (DocumentTypeId == 184 || DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				grdDetail.RootTable.Columns["DiscountType"].Width = Constants.InventoryConstants.DiscountType;
				grdDetail.RootTable.Columns["DiscountRate"].Width = Constants.InventoryConstants.ExRateConstant;
				grdDetail.RootTable.Columns["DiscountAmount"].Width = Constants.InventoryConstants.AmountConstant;
			}
			if (DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				grdDetail.RootTable.Columns["ModalDescription"].Width = Constants.InventoryConstants.Variant;
				grdDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
				grdDetail.RootTable.Columns["Specification/Remarks"].Width = 100;
				grdDetail.RootTable.Columns["TaxType"].Width = Constants.InventoryConstants.DiscountType;
				grdDetail.RootTable.Columns["Tax%"].Width = 65;
				grdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
				grdDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["Tax%"].FormatString = clsGlobalVariables.DecimalRateFormate;
				grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["TaxAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.###";
			}
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ExpenseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			if (Conversion.ToInt(grdDetail.CurrentRow.Cells["SaleOrder"].Value) == 0)
			{
				grdDetail.RootTable.Columns["SaleOrder"].Visible = false;
			}
			else
			{
				grdDetail.RootTable.Columns["SaleOrder"].ColumnType = (ColumnType)5;
			}
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				double AddLs = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["AddLessWeight"], (AggregateFunction)2));
				double EbtotalWt = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["EBTotalWt"], (AggregateFunction)2));
				double WtCutTotal = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["WeightCutTotal"], (AggregateFunction)2));
				double num = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["CommissionAmount"], (AggregateFunction)2));
				double Freight = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["FreightAmount"], (AggregateFunction)2));
				double num2 = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["RateCut"], (AggregateFunction)2));
				double RateCutAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["RateCutAmount"], (AggregateFunction)2));
				if (AddLs == 0.0)
				{
					grdDetail.RootTable.Columns["AddLessWeight"].Visible = false;
				}
				if (EbtotalWt == 0.0)
				{
					grdDetail.RootTable.Columns["EBTotalWt"].Visible = false;
				}
				if (WtCutTotal == 0.0)
				{
					grdDetail.RootTable.Columns["WeightCutTotal"].Visible = false;
				}
				if (num2 == 0.0)
				{
					grdDetail.RootTable.Columns["RateCut"].Visible = false;
				}
				if (RateCutAmount == 0.0)
				{
					grdDetail.RootTable.Columns["RateCutAmount"].Visible = false;
				}
				if (num == 0.0)
				{
					grdDetail.RootTable.Columns["CommissionAmount"].Visible = false;
				}
				if (Freight == 0.0)
				{
					grdDetail.RootTable.Columns["FreightAmount"].Visible = false;
				}
			}
			if (Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["ExpenseAmount"], (AggregateFunction)2)) == 0.0)
			{
				grdDetail.RootTable.Columns["ExpenseAmount"].Visible = false;
			}
			grdDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			if (DocumentTypeId != 184 && DocumentTypeId != 1660 && DocumentTypeId != 1661 && DocumentTypeId != 145)
			{
				grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["AddLessWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["AddLessWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["AddLessWeight"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["AddLessWeight"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["EBTotalWt"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["EBTotalWt"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["EBTotalWt"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["EBTotalWt"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["WeightCutTotal"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["WeightCutTotal"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["WeightCutTotal"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["NetStockWeight"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["NetStockWeight"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["NetStockWeight"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["NetStockWeight"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["RateCut"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["RateCutAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["RateCutAmount"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["FreightAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.###";
				grdDetail.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["CommissionAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["CommissionAmount"].TotalFormatString = "#,##0.###";
			}
			else if (DocumentTypeId == 184 || DocumentTypeId == 1660 || DocumentTypeId == 1661)
			{
				grdDetail.RootTable.Columns["DiscountRate"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["DiscountRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				grdDetail.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
				grdDetail.RootTable.Columns["DiscountAmount"].FormatString = "#,##0.###";
				grdDetail.RootTable.Columns["DiscountAmount"].TotalFormatString = "#,##0.###";
			}
			grdDetail.RootTable.Columns["ExpenseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ExpenseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ExpenseAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ExpenseAmount"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (e.Column.Key == "InvoiceNo" && AllowEditOnVoucherNoinReports)
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, 0, Id, BaseDocumentTypeId);
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

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (flagUP)
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
					InvSaleInvoice.InvSaleInvoiceandVoucherApprove(obj);
					MessageBox.Show("Record UnApproved Successfully");
					BindHistoryData(GetHistoryData());
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
					item2.DocumentTypeId = DocumentTypeId;
					item2.Id = Conversion.ToInt(r2.Cells[0].Value);
					item2.PostDate = DateTime.Now;
					item2.EntryUser = UserAccount.ID;
					item2.ReqType = "AP";
					obj2.AllApprovalLists.Add(item2);
				}
				InvSaleInvoice.InvSaleInvoiceandVoucherApprove(obj2);
				MessageBox.Show("Record Approved Successfully");
				BindHistoryData(GetHistoryData());
			}
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
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
			}
			if (e.Column.Key == "SaleOrder")
			{
				int SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
				new SaleActivityDetails(UserAccount, SaleOrderId).Show();
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
			tabControl1.TabPages.Remove(CardsTab);
			if (tabControl1.TabPages.Count == 1)
			{
				tabControl1.TabPages.Add("Cards View");
				InvoiceHistory frm = new InvoiceHistory(UserAccount);
				frm.InvoiceType = 2;
				frm.DocumentTypeId = DocumentTypeId;
				frm.ApproveOrUnApprove = flagUP;
				frm.RequestFromScreen = "Sale Invoice Approval";
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
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
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
				else if (DocumentTypeId == 1608)
				{
					CommonServices.SaleInvoicePrint1608(Id);
				}
				else if (DocumentTypeId == 1609)
				{
					CommonServices.SaleInvoicelSlip_1609(Id);
				}
				else if (DocumentTypeId == 1611)
				{
					CommonServices.SaleInvoicelReturnSlip_1611A(Id);
				}
				else if (DocumentTypeId == 1660)
				{
					CommonServices.SaleInvoicelEngr_Slip_1660A(Id);
				}
				else if (DocumentTypeId == 1661)
				{
					CommonServices.SaleInvoiceEngr_Slip_1661A(Id);
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
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Expected O, but got Unknown
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingApprovalSaleInvoice));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.GridTab = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
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
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.CardsTab = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.GridTab.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1147, 701);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.GridTab);
		this.tabControl1.Controls.Add(this.CardsTab);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1147, 701);
		this.tabControl1.TabIndex = 5;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.GridTab.Controls.Add(this.panel6);
		this.GridTab.Controls.Add(this.panel5);
		this.GridTab.Controls.Add(this.panel4);
		this.GridTab.Controls.Add(this.panel3);
		this.GridTab.Controls.Add(this.ctrlGrdBar1);
		this.GridTab.Controls.Add(this.toolStrip3);
		this.GridTab.Location = new System.Drawing.Point(4, 4);
		this.GridTab.Name = "GridTab";
		this.GridTab.Padding = new System.Windows.Forms.Padding(3);
		this.GridTab.Size = new System.Drawing.Size(1139, 675);
		this.GridTab.TabIndex = 0;
		this.GridTab.Text = "Grid View";
		this.GridTab.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 370);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1133, 302);
		this.panel6.TabIndex = 12;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1133, 302);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 47;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 114);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1133, 256);
		this.panel5.TabIndex = 11;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1133, 256);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Tag = "MasterGrid";
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.White;
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
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1133, 55);
		this.panel4.TabIndex = 10;
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(717, 23);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(74, 26);
		this.btnApproved.TabIndex = 6;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(662, 23);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
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
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(452, 24);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(452, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Item Name";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(126, 26);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(115, 23);
		this.datToDate.TabIndex = 1;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(6, 26);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(119, 23);
		this.datFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(126, 5);
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
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(243, 24);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(243, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 132;
		this.label1.Text = "Customer Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1133, 29);
		this.panel3.TabIndex = 9;
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1058, 4);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(75, 21);
		this.chkPrint.TabIndex = 4;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(171, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Sale Invoice History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1097, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNew });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1133, 27);
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
		this.CardsTab.Size = new System.Drawing.Size(1139, 675);
		this.CardsTab.TabIndex = 1;
		this.CardsTab.Text = "Cards View";
		this.CardsTab.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1147, 701);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingApprovalSaleInvoice";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmPurchaseInvoiceHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.GridTab.ResumeLayout(false);
		this.GridTab.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
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
