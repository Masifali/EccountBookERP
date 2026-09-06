// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
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

public class frmGdUtilizationAgainstFcyReceipt : Form
{
	private DataTable dtGrid = new DataTable();

	private DataTable dtPendingVouchers = new DataTable();

	private int RecId;

	private bool Approved;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool FormInitialized;

	private string ScreenName = "frmGdUtilizationAgainstFcyReceipt";

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<FcyBankReceiptBreakUp> lstRemoveDetailRecord = new List<FcyBankReceiptBreakUp>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnSlip;

	private Label label7;

	private ToolStripButton btnDelete;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdHistory;

	private CheckBox ChkPrintslip;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Label label54;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel6;

	private Label label58;

	private Panel panel5;

	private Panel panel7;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private DateTimePicker txtFromdateHistory;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX grdPendingReceipts;

	private Label label2;

	private Label label8;

	private DateTimePicker txtReceiptDate;

	private TextBox txtFcyAmount;

	private Label label5;

	private TextBox txtLcyAmount;

	private TextBox txtExchangeRate;

	private Label label62;

	private Button BtnAutoUtilize;

	private Button btnShowRecords;

	private Button BtnAutoUtilizedAllRowsOfPendingGrid;

	private TextBox txtReceiptNo;

	private Label label1;

	private GroupBox groupBox1;

	internal UltraCombo CmbAccountFilter;

	private Label label13;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label3;

	internal GridEX GridGdUtilizedHistory;

	public frmGdUtilizationAgainstFcyReceipt(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1030, 700);
			ScreenName = Conversion.ToString((object)base.Name);
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("FcyBankReceiptId", typeof(int));
			dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("InvoiceId", typeof(int));
			dtGrid.Columns.Add("InvoiceNo", typeof(string));
			dtGrid.Columns.Add("GdRefDocTypeId", typeof(int));
			dtGrid.Columns.Add("GDId", typeof(int));
			dtGrid.Columns.Add("GDNo", typeof(string));
			dtGrid.Columns.Add("GDValue", typeof(decimal));
			dtGrid.Columns.Add("AdvanceUtilize", typeof(decimal));
			dtGrid.Columns.Add("UtilizedAmount", typeof(decimal));
			dtGrid.Columns.Add("GDBalance", typeof(decimal));
			dtGrid.Columns.Add("UtilizingAmount", typeof(decimal));
			dtGrid.Columns.Add("Commission", typeof(decimal));
			dtGrid.Columns.Add("FDBCNo", typeof(string));
			dtGrid.Columns.Add("PrevSavedStepStatus", typeof(string));
			dtGrid.Columns.Add("StepStatus", typeof(string));
			dtGrid.Columns.Add("BalanceAmount", typeof(decimal));
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)grdPendingReceipts).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			DataTable pendingFcyReceiptsAgainstBank = ExImFCBankReceipts.GetPendingFcyReceiptsAgainstBank(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			return new
			{
				formright = formright,
				PendingData = pendingFcyReceiptsAgainstBank
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
				btnSave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnSlip.Enabled = formright.DoHavePrintRights;
				ChkPrintslip.Checked = formright.DoHavePrintRights;
				_ = RecId;
				if (dtGrid != null && dtGrid.Columns.Count > 0)
				{
					grd.DataSource = dtGrid;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtPendingVouchers != null)
				{
					FillGrdPendingOrders(result.PendingData);
				}
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				txtToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void FillGrdPendingOrders(DataTable dtGridOrders)
	{
		try
		{
			dtPendingVouchers.Rows.Clear();
			grdPendingReceipts.DataSource = null;
			if (dtPendingVouchers.Columns.Count == 0)
			{
				dtPendingVouchers.Columns.Add("Id", typeof(int));
				dtPendingVouchers.Columns.Add("ReceiptDate", typeof(DateTime));
				dtPendingVouchers.Columns.Add("ReceiptCode", typeof(int));
				dtPendingVouchers.Columns.Add("SupplierCustomerId", typeof(int));
				dtPendingVouchers.Columns.Add("Customer", typeof(string));
				dtPendingVouchers.Columns.Add("PaymentTerm", typeof(string));
				dtPendingVouchers.Columns.Add("InvoiceNo", typeof(string));
				dtPendingVouchers.Columns.Add("BankName", typeof(string));
				dtPendingVouchers.Columns.Add("FcyCode", typeof(string));
				dtPendingVouchers.Columns.Add("ExchangeRate", typeof(decimal));
				dtPendingVouchers.Columns.Add("FcyAmount", typeof(decimal));
				dtPendingVouchers.Columns.Add("LcyAmount", typeof(decimal));
			}
			if (dtGridOrders.Rows.Count > 0)
			{
				foreach (DataRow row in dtGridOrders.Rows)
				{
					dtPendingVouchers.Rows.Add(row["Id"], row["DocumentDate"], row["DocumentNo"], row["SupplierCustomerId"], row["CustomerName"], row["ExmLcPaymentTerm"], row["InvoiceNo"], row["BankName"], row["FcyCode"], row["ExchangeRate"], row["FcyAmount"], row["LcyAmount"]);
				}
			}
			if (dtPendingVouchers != null && dtPendingVouchers.Columns.Count > 0)
			{
				grdPendingReceipts.DataSource = dtPendingVouchers;
				grdPendingReceipts.RetrieveStructure();
				grdPendingOrdersSetting();
			}
			GetDistinctParties(dtGridOrders);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDistinctParties(DataTable sourceData)
	{
		try
		{
			DataTable dt = sourceData.DefaultView.ToTable(true, "SupplierCustomerId", "CustomerName");
			DDL.BindDDL(dt, CmbAccountFilter, "SupplierCustomerId", "CustomerName", "Customer Name", ZeroIndex: false);
			((UltraGridBase)CmbAccountFilter).DisplayLayout.Bands[0].Columns["SupplierCustomerId"].Hidden = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingOrdersSetting(bool addcheckBox = false)
	{
		try
		{
			grdPendingReceipts.RootTable.Columns["Id"].Visible = false;
			grdPendingReceipts.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdPendingReceipts, 2);
			grdPendingReceipts.RootTable.Columns["ReceiptDate"].Width = Constants.InventoryConstants.DateConstant;
			grdPendingReceipts.RootTable.Columns["ReceiptCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grdPendingReceipts.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdPendingReceipts.RootTable.Columns["PaymentTerm"].Width = 110;
			grdPendingReceipts.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant + 30;
			grdPendingReceipts.RootTable.Columns["BankName"].Width = 150;
			grdPendingReceipts.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grdPendingReceipts.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPendingReceipts.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingReceipts.RootTable.Columns["LcyAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdPendingReceipts.RootTable.Columns.Add("Load");
			grdPendingReceipts.RootTable.Columns["Load"].Key = "Load";
			grdPendingReceipts.RootTable.Columns["Load"].Caption = "Load";
			grdPendingReceipts.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingReceipts.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdPendingReceipts.RootTable.Columns["Load"].ButtonText = "Load";
			grdPendingReceipts.RootTable.Columns["Load"].Width = 50;
			grdPendingReceipts.RootTable.Columns["Load"].Position = 0;
			grdPendingReceipts.FrozenColumns = 1;
			if (addcheckBox)
			{
				GridEX_Helper.AddSelectButton(grdPendingReceipts, "Select", 30, 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingOrders_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingReceipts.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Load")
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					throw new Exception("Please Reset the Form First to Load New Data");
				}
				txtReceiptDate.Value = Conversion.ToDateTime(r.Cells["ReceiptDate"].Value);
				txtReceiptNo.Text = Conversion.ToString(r.Cells["ReceiptCode"].Value);
				txtFcyAmount.Text = Conversion.ToDouble(r.Cells["FcyAmount"].Value).ToString("#,##0.000");
				txtExchangeRate.Text = Conversion.ToDouble(r.Cells["ExchangeRate"].Value).ToString("#,##0.000");
				txtLcyAmount.Text = Conversion.ToDouble(r.Cells["LcyAmount"].Value).ToString("#,##0.000");
				GetGdData(r);
				((Control)(object)grd).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGdData(GridEXRow r = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int FcyReceiptId = 0;
			if (r != null && (int)r.RowType == 0)
			{
				FcyReceiptId = Conversion.ToInt(r.Cells["Id"].Value);
			}
			dtGrid.Rows.Clear();
			DataTable dt = ExImInvoice.GetGDsFromCustomInvoicePaymentScheduleAndGdMappingForFcyReciptsMapping(UserAccount.OrganizationId, UserAccount.CompanyId, 0, 0, 0);
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dataRow in dt.Rows)
			{
				dtGrid.Rows.Add(0, FcyReceiptId, 4, dataRow["ExImInvoiceId"], dataRow["BankInvoiceNo"], dataRow["DocumentTypeId"], dataRow["Id"], dataRow["GDNO"], dataRow["GDValue"], dataRow["AdvanceUtilize"], dataRow["FcyRealized"], dataRow["GDBalance"], 0, dataRow["FTTAmount"], "", dataRow["GdStepStatus"], "", dataRow["GDBalance"]);
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPendingOrders_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingReceipts.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CuurID = Conversion.ToInt(r.Cells["Id"].Value);
				int CuurDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, CuurDocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDetailDataFromInvoice(DataTable dtData)
	{
		try
		{
			if (dtData.Rows.Count == 0)
			{
				return;
			}
			Dictionary<int, List<DataRow>> existingRows = (from row in dtGrid.AsEnumerable()
				group row by Conversion.ToInt(row["InvoiceId"])).ToDictionary((IGrouping<int, DataRow> g) => g.Key, (IGrouping<int, DataRow> g) => g.ToList());
			foreach (DataRow dataRow in dtData.Rows)
			{
				int invoiceId = Conversion.ToInt(dataRow["Id"]);
				decimal billAmount = Conversion.ToDecimal(dataRow["BillAmount"]);
				decimal availableInvoiceAmount = Conversion.ToDecimal(dataRow["AvailableBillAmount"]);
				if (existingRows.ContainsKey(invoiceId))
				{
					foreach (DataRow item in existingRows[invoiceId])
					{
						item["InvoiceAmount"] = billAmount;
						decimal adjustmentAmount = Conversion.ToDecimal(item["AdjustmentAmount"]);
						availableInvoiceAmount += adjustmentAmount;
						item["AvailableInvoiceAmount"] = availableInvoiceAmount;
						item["BalanceAmount"] = availableInvoiceAmount - adjustmentAmount;
					}
				}
				else
				{
					dtGrid.Rows.Add(0, dataRow["DocumentTypeId"], dataRow["DocumentType"], invoiceId, dataRow["InvoiceNo"], dataRow["InvoiceDate"], dataRow["InvoiceDueDate"], billAmount, availableInvoiceAmount, 0, availableInvoiceAmount, "");
				}
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["FcyBankReceiptId"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["GdRefDocTypeId"].Visible = false;
			grd.RootTable.Columns["GDId"].Visible = false;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 3);
				column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
				column.EditType = (EditType)((column.Key == "UtilizingAmount" || column.Key == "Remarks" || column.Key == "Commission" || column.Key == "FDBCNo") ? 1 : 0);
			}
			grd.RootTable.Columns["Commission"].Caption = "FTT Commission";
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grd.RootTable.Columns["GDNo"].Width = 150;
			grd.RootTable.Columns["GDValue"].Width = 110;
			grd.RootTable.Columns["UtilizedAmount"].Width = 100;
			grd.RootTable.Columns["GDBalance"].Width = 100;
			grd.RootTable.Columns["UtilizingAmount"].Width = 95;
			grd.RootTable.Columns["Commission"].Width = 95;
			grd.RootTable.Columns["FDBCNo"].Width = 150;
			grd.RootTable.Columns["StepStatus"].Width = 90;
			grd.RootTable.Columns["BalanceAmount"].Width = 90;
			grd.RootTable.Columns["BalanceAmount"].Width = 90;
			grd.RootTable.Columns["UtilizingAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["UtilizingAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["UtilizingAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["UtilizingAmount"].TotalFormatString = "#,##0.####";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				double Id = Conversion.ToInt(item.Cells["Id"].Value);
				double UtilizingAmount = Conversion.ToDouble(item.Cells["UtilizingAmount"].Value);
				double GDBalance = Conversion.ToDouble(item.Cells["GDBalance"].Value);
				double Commission = Conversion.ToDouble(item.Cells["Commission"].Value);
				string LastSavedStatus = Conversion.ToString(item.Cells["PrevSavedStepStatus"].Value);
				if (e.Column.Key == "UtilizingAmount" && UtilizingAmount + Commission > GDBalance)
				{
					double UtilizingAmnt = GDBalance - Commission;
					item.Cells["UtilizingAmount"].Value = UtilizingAmnt;
					MessageBox.Show($"UtilizingAmount:{UtilizingAmount} can not be greater than GD Balance Amount:{GDBalance}");
				}
				if (e.Column.Key == "Commission" && UtilizingAmount + Commission > GDBalance)
				{
					double Comm = GDBalance - UtilizingAmount;
					item.Cells["Commission"].Value = Comm;
					MessageBox.Show($"Commission:{Commission} + UtilizingAmount:{UtilizingAmount} = {Commission + UtilizingAmount} can not be greater than GD Balance Amount:{GDBalance}");
				}
				double BalanceAmount = GDBalance - UtilizingAmount - Commission;
				item.Cells["BalanceAmount"].Value = BalanceAmount;
				if (LastSavedStatus == "Final Part")
				{
					item.Cells["StepStatus"].Value = LastSavedStatus;
				}
				else if (string.IsNullOrEmpty(LastSavedStatus))
				{
					item.Cells["StepStatus"].Value = ((BalanceAmount == 0.0) ? "Last Part" : "1 Part");
				}
				else
				{
					int lastSavedNumber = int.Parse(LastSavedStatus.Split()[0]);
					string newStatus = lastSavedNumber + " Part";
					if (Id == 0.0)
					{
						newStatus = lastSavedNumber + 1 + " Part";
					}
					item.Cells["StepStatus"].Value = ((BalanceAmount == 0.0) ? "Last Part" : newStatus);
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				FcyBankReceiptBreakUp detail = new FcyBankReceiptBreakUp();
				detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				detail.FcyBankReceiptId = Conversion.ToInt(r.Cells["FcyBankReceiptId"].Value);
				detail.RefDocumentTypeId = 4;
				detail.RefDocRecordId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				detail.GdRefDocTypeId = Conversion.ToInt(r.Cells["GdRefDocTypeId"].Value);
				detail.GDId = Conversion.ToInt(r.Cells["GDId"].Value);
				detail.RealizedAmount = Conversion.ToDecimal(r.Cells["UtilizingAmount"].Value);
				detail.AgencyComm = Conversion.ToDecimal(r.Cells["Commission"].Value);
				detail.ReceiverRefNo = Conversion.ToString(r.Cells["FDBCNo"].Value);
				detail.StepStatus = Conversion.ToString(r.Cells["StepStatus"].Value);
				detail.ActionTypeId = 3;
				lstRemoveDetailRecord.Add(detail);
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtFcyAmount.Text.Trim() == "" || txtFcyAmount.Text.Trim() == "0" || txtFcyAmount.Text.Trim() == null)
		{
			MessageBox.Show("Fcy Amount Field is Required");
			txtFcyAmount.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			Approved = false;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtReceiptNo.Text = string.Empty;
			txtReceiptDate.Value = DateTime.Now;
			txtFcyAmount.Text = string.Empty;
			txtLcyAmount.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			lstRemoveDetailRecord.Clear();
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			FillGrdPendingOrders(ExImFCBankReceipts.GetPendingFcyReceiptsAgainstBank(UserAccount.OrganizationId, UserAccount.CompanyId, 0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void InvfrmInvSaleInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Insert(bool Auto = false)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (!Auto)
			{
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			FcyBankReceiptBreakUp obj = new FcyBankReceiptBreakUp();
			obj.FcyBankReceiptBreakUpList = new List<FcyBankReceiptBreakUp>();
			decimal TotalAmount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0 || Conversion.ToDecimal(r.Cells["UtilizingAmount"].Value) > 0m)
				{
					FcyBankReceiptBreakUp detail = new FcyBankReceiptBreakUp();
					detail.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					detail.ActionTypeId = ((detail.Id <= 0) ? 1 : 2);
					detail.FcyBankReceiptId = Conversion.ToInt(r.Cells["FcyBankReceiptId"].Value);
					obj.Id = detail.FcyBankReceiptId;
					detail.RefDocumentTypeId = 4;
					detail.RefDocRecordId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					detail.GdRefDocTypeId = Conversion.ToInt(r.Cells["GdRefDocTypeId"].Value);
					detail.GDId = Conversion.ToInt(r.Cells["GDId"].Value);
					detail.RealizedAmount = Conversion.ToDecimal(r.Cells["UtilizingAmount"].Value);
					detail.AgencyComm = Conversion.ToDecimal(r.Cells["Commission"].Value);
					detail.ReceiverRefNo = Conversion.ToString(r.Cells["FDBCNo"].Value);
					detail.StepStatus = Conversion.ToString(r.Cells["StepStatus"].Value);
					TotalAmount += detail.RealizedAmount;
					if (detail.RefDocumentTypeId == 0)
					{
						throw new Exception("RefDocumentType Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.RefDocRecordId == 0)
					{
						throw new Exception("Invoice Id Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.RealizedAmount == 0m)
					{
						throw new Exception("Realized Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.GDId == 0)
					{
						throw new Exception("GDId Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.ReceiverRefNo == "")
					{
						throw new Exception("FDBC No Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					obj.FcyBankReceiptBreakUpList.Add(detail);
				}
			}
			if (obj.FcyBankReceiptBreakUpList.Count == 0)
			{
				throw new Exception("At least enter one row in detail ");
			}
			if (lstRemoveDetailRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveDetailRecord.Count; i++)
				{
					obj.FcyBankReceiptBreakUpList.Add(lstRemoveDetailRecord[i]);
				}
			}
			TotalAmount = Math.Round(TotalAmount, 4, MidpointRounding.AwayFromZero);
			if (TotalAmount > Conversion.ToDecimal((object)txtFcyAmount.Text) || TotalAmount < Conversion.ToDecimal((object)txtFcyAmount.Text))
			{
				throw new Exception($"Total Utilizing Amount {TotalAmount} In Grid should be equal to receipt amount {Conversion.ToDecimal((object)txtFcyAmount.Text)}");
			}
			FcyBankReceiptBreakUp.Save(obj);
			if (!Auto)
			{
				if (RecId == 0)
				{
					MessageBox.Show("Record Saved Successfully of GD No [" + obj.GDNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully of GD No [" + obj.GDNo + "] ");
				}
				Reset();
				_ = ChkPrintslip.Checked;
				btnShowRecords_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	public void ReadById(int Id)
	{
		try
		{
			Reset();
			RecId = Id;
			DataTable invPI = FcyBankReceiptBreakUp.GetGDUtilizationHistoryByFcyReceipt(Id);
			if (invPI == null || invPI.Rows.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			((Control)(object)grd).Focus();
			foreach (DataRow dataRow in invPI.Rows)
			{
				dtGrid.Rows.Add(dataRow["Id"], dataRow["RefDocumentTypeId"], dataRow["RefDocumentType"], Conversion.ToInt(dataRow["RefDocNo"]), dataRow["RefDocDate"], dataRow["DueDate"]);
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found for deletion...");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				MessageBox.Show("Deleted Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			HashSet<int> gridIds = (from row in grd.GetDataRows()
				where Conversion.ToInt(row.Cells["Id"].Value) > 0
				select Conversion.ToInt(row.Cells["Id"].Value)).ToHashSet();
			lstRemoveDetailRecord.RemoveAll((FcyBankReceiptBreakUp item) => gridIds.Contains(item.Id));
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not found");
			}
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			if (txtFromdateHistory.Checked)
			{
				PI.FromDate = txtFromdateHistory.Value;
			}
			if (txtToDateHistory.Checked)
			{
				PI.ToDate = txtToDateHistory.Value;
			}
			DataTable invPI = FcyBankReceiptBreakUp.FormHistory(PI);
			if (invPI != null && invPI.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ReceiptDate", typeof(DateTime));
				dt.Columns.Add("ReceiptCode", typeof(string));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("Customer", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("BankName", typeof(string));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(decimal));
				dt.Columns.Add("FcyAmount", typeof(decimal));
				dt.Columns.Add("LcyAmount", typeof(decimal));
				foreach (DataRow dataRow in invPI.Rows)
				{
					dt.Rows.Add(dataRow["Id"], dataRow["DocumentDate"], dataRow["DocumentNo"], dataRow["SupplierCustomerId"], dataRow["CustomerName"], dataRow["ExmLcPaymentTerm"], dataRow["InvoiceNo"], dataRow["BankName"], dataRow["FcyCode"], dataRow["ExchangeRate"], dataRow["FcyAmount"], dataRow["LcyAmount"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				grdHistory.DataSource = null;
				GridGdUtilizedHistory.DataSource = null;
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
			if (tabControl1.SelectedIndex == 1 && (((UltraGridBase)cmbDateTypeHistory).DataSource == null || ((DisposableObjectCollectionBase)((UltraGridBase)cmbDateTypeHistory).Rows).Count == 0))
			{
				ParameterFill();
				cmbDateTypeHistory.Focus();
				if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0 && ((DisposableObjectCollectionBase)((UltraGridBase)cmbDateTypeHistory).Rows).Count > 0)
				{
					((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null)
			{
				_ = item.RowType;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Delete")
				{
					DeleteRecordAgainstFcyReceipt(Id);
				}
				if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				_ = e.Column.Key == "Print";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeleteRecordAgainstFcyReceipt(int Id)
	{
		try
		{
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				FcyBankReceiptBreakUp.DeleteByFcyReceipt(Id);
				FillHistory();
				FillGrdPendingOrders(ExImFCBankReceipts.GetPendingFcyReceiptsAgainstBank(UserAccount.OrganizationId, UserAccount.CompanyId, 0));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdHistory, 2);
			grdHistory.RootTable.Columns["ReceiptDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ReceiptCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["PaymentTerm"].Width = 110;
			grdHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant + 30;
			grdHistory.RootTable.Columns["BankName"].Width = 150;
			grdHistory.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grdHistory.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["LcyAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdHistory.RootTable.Columns.Add("Delete");
			grdHistory.RootTable.Columns["Delete"].Caption = "X";
			grdHistory.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Delete"].ButtonText = "X";
			grdHistory.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int VoucherHeadId, int PaymentTypeId, int SupplierCustomerId, int PartyGlId)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherInvoicesAdjustment.GetByVoucherHeadId(1, VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "901_VoucherInvoicesAdjustment_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEmptybagsUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtStockWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtgatepassno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					((Control)(object)grd).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbDateTypeHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.E)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 116;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdPendingReceipts).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 38;
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
			}
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print Slip 901");
			dt.Rows.Add("Alt+1", "For Print Slip 901");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				((Control)(object)grd).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				_ = grdHistory.CurrentColumn.Key == "Print";
				_ = grdHistory.CurrentColumn.Key == "NoOfAttachments";
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById(Id);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				_ = formright.DoHavePrintRights;
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAutoUtilize_Click(object sender, EventArgs e)
	{
		try
		{
			TotalAmountAutoUtilizeInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while auto-utilizing: " + ex.Message);
		}
	}

	private void TotalAmountAutoUtilizeInGrid()
	{
		try
		{
			decimal totalAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			decimal totalGridAmount = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["UtilizingAmount"], (AggregateFunction)2).ToString()) : 0m);
			List<GridEXRow> rows = (from r in grd.GetRows()
				where Conversion.ToDecimal(r.Cells["UtilizingAmount"].Value) == 0m
				select r).ToList();
			if (totalAmount >= totalGridAmount)
			{
				totalAmount -= totalGridAmount;
				foreach (GridEXRow row in rows)
				{
					row.BeginEdit();
					int Id = Conversion.ToInt(row.Cells["Id"].Value);
					decimal GDBalance = Conversion.ToDecimal(row.Cells["GDBalance"].Value);
					decimal commission = Conversion.ToDecimal(row.Cells["Commission"].Value);
					decimal balAmount = Conversion.ToDecimal(row.Cells["BalanceAmount"].Value);
					decimal applyAmount = Math.Min(totalAmount, Math.Min(balAmount, GDBalance));
					row.Cells["UtilizingAmount"].Value = (double)applyAmount;
					double BalanceAmount = (double)(GDBalance - commission - applyAmount);
					row.Cells["BalanceAmount"].Value = BalanceAmount;
					string LastSavedStatus = Conversion.ToString(row.Cells["PrevSavedStepStatus"].Value);
					if (LastSavedStatus == "Final Part")
					{
						row.Cells["StepStatus"].Value = LastSavedStatus;
					}
					else if (string.IsNullOrEmpty(LastSavedStatus))
					{
						row.Cells["StepStatus"].Value = ((BalanceAmount == 0.0) ? "Final Part" : "1 Part");
					}
					else
					{
						int lastSavedNumber = int.Parse(LastSavedStatus.Split()[0]);
						string newStatus = lastSavedNumber + " Part";
						if (Id == 0)
						{
							newStatus = lastSavedNumber + 1 + " Part";
						}
						row.Cells["StepStatus"].Value = ((BalanceAmount == 0.0) ? "Final Part" : newStatus);
					}
					totalAmount -= applyAmount;
					row.EndEdit();
					if (totalAmount <= 0m)
					{
						break;
					}
				}
			}
			else
			{
				decimal utilizedAmount = default(decimal);
				GridEXRow[] rows2 = grd.GetRows();
				foreach (GridEXRow row2 in rows2)
				{
					row2.BeginEdit();
					if (utilizedAmount >= totalAmount)
					{
						decimal GdBalance = Conversion.ToDecimal(row2.Cells["GDBalance"].Value);
						row2.Cells["UtilizingAmount"].Value = 0;
						row2.Cells["BalanceAmount"].Value = GdBalance;
					}
					utilizedAmount += Conversion.ToDecimal(row2.Cells["UtilizingAmount"].Value);
					row2.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShowRecords_Click(object sender, EventArgs e)
	{
		try
		{
			dtGrid.Rows.Clear();
			if (((UltraGridBase)CmbAccountFilter).ActiveRow != null && Conversion.ToInt(CmbAccountFilter.Value) > 0)
			{
				int accountId = Conversion.ToInt(CmbAccountFilter.Value);
				List<DataRow> filteredRows = (from row in dtPendingVouchers.AsEnumerable()
					where row.Field<int>("SupplierCustomerId") == accountId
					select row).ToList();
				if (filteredRows.Any())
				{
					DataTable filteredDt = filteredRows.CopyToDataTable();
					grdPendingReceipts.DataSource = filteredDt;
					grdPendingReceipts.RetrieveStructure();
					grdPendingOrdersSetting(addcheckBox: true);
				}
				else
				{
					MessageBox.Show("No pending vouchers found for the selected account.");
				}
			}
			else
			{
				grdPendingReceipts.DataSource = dtPendingVouchers;
				grdPendingReceipts.RetrieveStructure();
				grdPendingOrdersSetting();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmInvoicesAdjustmentVoucher_Load(object sender, EventArgs e)
	{
	}

	private void BtnAutoUtilizedAllRowsOfPendingGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled)
			{
				MessageBox.Show("Please Reset the Form First to Load New Data");
				return;
			}
			if (grdPendingReceipts.GetDataRows().Count() == 0)
			{
				throw new Exception("Pending Vouchers Detail have no Records...");
			}
			if (grdPendingReceipts.GetCheckedRows().Count() != 0)
			{
				return;
			}
			throw new Exception("Please Select Any row First.Press show Button If 'Select' Button not Appear");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				GetGdInfoAgainstReceipt(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGdInfoAgainstReceipt(int Id)
	{
		try
		{
			DataTable dt = FcyBankReceiptBreakUp.GetGDUtilizationHistoryByFcyReceipt(Id);
			if (dt == null || dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtDetail = new DataTable();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("GDId", typeof(int));
			dtDetail.Columns.Add("GdRefDocTypeId", typeof(int));
			dtDetail.Columns.Add("RefDocRecordId", typeof(int));
			dtDetail.Columns.Add("GDNo", typeof(string));
			dtDetail.Columns.Add("GDValue", typeof(double));
			dtDetail.Columns.Add("InvoiceNo", typeof(string));
			dtDetail.Columns.Add("FDBCNo", typeof(string));
			dtDetail.Columns.Add("RealizedAmount", typeof(double));
			dtDetail.Columns.Add("AgencyComm", typeof(double));
			dtDetail.Columns.Add("StepStatus", typeof(string));
			foreach (DataRow dr in dt.Rows)
			{
				dtDetail.Rows.Add(dr["Id"], dr["GDId"], dr["GdRefDocTypeId"], dr["RefDocRecordId"], dr["GDNo"], dr["GDValue"], dr["InvoiceNo"], dr["ReceiverRefNo"], dr["RealizedAmount"], dr["AgencyComm"], dr["StepStatus"]);
			}
			GridGdUtilizedHistory.DataSource = dtDetail;
			GridGdUtilizedHistory.RetrieveStructure();
			GridGdHistorySetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridGdHistorySetting()
	{
		try
		{
			GridGdUtilizedHistory.RootTable.Columns["Id"].Visible = false;
			GridGdUtilizedHistory.RootTable.Columns["GDId"].Visible = false;
			GridGdUtilizedHistory.RootTable.Columns["GdRefDocTypeId"].Visible = false;
			GridGdUtilizedHistory.RootTable.Columns["RefDocRecordId"].Visible = false;
			GridGdUtilizedHistory.RootTable.Columns["AgencyComm"].Caption = "Commission";
			GridEX_Helper.GridWrappingAndColumnSettings(GridGdUtilizedHistory, 2);
			GridGdUtilizedHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			GridGdUtilizedHistory.RootTable.Columns["FDBCNo"].Width = 120;
			GridGdUtilizedHistory.RootTable.Columns["GDNo"].Width = 120;
			GridGdUtilizedHistory.RootTable.Columns["GDValue"].Width = Constants.InventoryConstants.AmountConstant;
			GridGdUtilizedHistory.RootTable.Columns["RealizedAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GridGdUtilizedHistory.RootTable.Columns["AgencyComm"].Width = Constants.InventoryConstants.AmountConstant;
			GridGdUtilizedHistory.RootTable.Columns["StepStatus"].Width = 80;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Expected O, but got Unknown
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Expected O, but got Unknown
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Expected O, but got Unknown
		//IL_282d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2837: Expected O, but got Unknown
		//IL_285b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2865: Expected O, but got Unknown
		GridEXLayout grdPendingReceipts_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout GridGdUtilizedHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPendingReceipts = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.BtnAutoUtilize = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbAccountFilter = new UltraCombo();
		this.btnShowRecords = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtReceiptNo = new System.Windows.Forms.TextBox();
		this.txtLcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtReceiptDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.BtnAutoUtilizedAllRowsOfPendingGrid = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GridGdUtilizedHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingReceipts).BeginInit();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFilter).BeginInit();
		this.groupBox2.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridGdUtilizedHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(986, 692);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(978, 663);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingReceipts);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(972, 657);
		this.panel1.TabIndex = 7;
		this.grdPendingReceipts.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingReceipts).BackColor = System.Drawing.Color.White;
		this.grdPendingReceipts.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingReceipts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingReceipts.DesignTimeLayout = grdPendingReceipts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingReceipts.FilterMode = (FilterMode)2;
		this.grdPendingReceipts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingReceipts.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).Location = new System.Drawing.Point(0, 484);
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).Name = "grdPendingReceipts";
		((ScrollableBase)this.grdPendingReceipts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).Size = new System.Drawing.Size(972, 173);
		((System.Windows.Forms.Control)(object)this.grdPendingReceipts).TabIndex = 0;
		this.grdPendingReceipts.TotalRow = (InheritableBoolean)1;
		this.grdPendingReceipts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingReceipts).VisualStyle = (VisualStyle)4;
		this.grdPendingReceipts.ColumnButtonClick += new ColumnActionEventHandler(grdPendingOrders_ColumnButtonClick);
		this.grdPendingReceipts.LinkClicked += new ColumnActionEventHandler(grdPendingOrders_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label58);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 464);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(972, 20);
		this.panel6.TabIndex = 16;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 2);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(122, 16);
		this.label58.TabIndex = 126;
		this.label58.Text = "Pending Receipts ";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(0, 124);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(972, 340);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(964, 311);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(958, 284);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.BtnAutoUtilizedAllRowsOfPendingGrid);
		this.panel16.Controls.Add(this.BtnAutoUtilize);
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(958, 21);
		this.panel16.TabIndex = 33;
		this.BtnAutoUtilize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAutoUtilize.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.BtnAutoUtilize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAutoUtilize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAutoUtilize.ForeColor = System.Drawing.Color.FromArgb(64, 0, 0);
		this.BtnAutoUtilize.Location = new System.Drawing.Point(802, -1);
		this.BtnAutoUtilize.Name = "BtnAutoUtilize";
		this.BtnAutoUtilize.Size = new System.Drawing.Size(113, 22);
		this.BtnAutoUtilize.TabIndex = 36;
		this.BtnAutoUtilize.Text = "Auto Utilize";
		this.BtnAutoUtilize.UseVisualStyleBackColor = false;
		this.BtnAutoUtilize.Click += new System.EventHandler(BtnAutoUtilize_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(918, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(972, 99);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountFilter);
		this.groupBox1.Controls.Add(this.btnShowRecords);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(553, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(309, 71);
		this.groupBox1.TabIndex = 114;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 21);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(93, 15);
		this.label13.TabIndex = 112;
		this.label13.Text = "Customer Name";
		this.CmbAccountFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Location = new System.Drawing.Point(3, 41);
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Name = "CmbAccountFilter";
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Size = new System.Drawing.Size(242, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).TabIndex = 113;
		this.btnShowRecords.BackColor = System.Drawing.Color.Teal;
		this.btnShowRecords.FlatAppearance.BorderSize = 0;
		this.btnShowRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowRecords.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnShowRecords.ForeColor = System.Drawing.Color.White;
		this.btnShowRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowRecords.Location = new System.Drawing.Point(247, 42);
		this.btnShowRecords.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowRecords.Name = "btnShowRecords";
		this.btnShowRecords.Size = new System.Drawing.Size(57, 24);
		this.btnShowRecords.TabIndex = 111;
		this.btnShowRecords.Text = "Show";
		this.btnShowRecords.UseVisualStyleBackColor = false;
		this.btnShowRecords.Click += new System.EventHandler(btnShowRecords_Click);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtReceiptNo);
		this.groupBox2.Controls.Add(this.txtLcyAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtReceiptDate);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 28);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(553, 71);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(335, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 15);
		this.label1.TabIndex = 113;
		this.label1.Text = "Lcy Amount";
		this.txtReceiptNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceiptNo.Enabled = false;
		this.txtReceiptNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceiptNo.Location = new System.Drawing.Point(104, 18);
		this.txtReceiptNo.Name = "txtReceiptNo";
		this.txtReceiptNo.Size = new System.Drawing.Size(111, 23);
		this.txtReceiptNo.TabIndex = 112;
		this.txtLcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLcyAmount.Enabled = false;
		this.txtLcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLcyAmount.Location = new System.Drawing.Point(425, 42);
		this.txtLcyAmount.Name = "txtLcyAmount";
		this.txtLcyAmount.Size = new System.Drawing.Size(124, 23);
		this.txtLcyAmount.TabIndex = 6;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Enabled = false;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(425, 18);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(124, 23);
		this.txtExchangeRate.TabIndex = 5;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(335, 22);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(84, 15);
		this.label62.TabIndex = 109;
		this.label62.Text = "Exchange Rate";
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Location = new System.Drawing.Point(104, 42);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(225, 23);
		this.txtFcyAmount.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(72, 15);
		this.label5.TabIndex = 66;
		this.label5.Text = "Fcy Amount";
		this.txtReceiptDate.CustomFormat = "dd-MMM-yy";
		this.txtReceiptDate.Enabled = false;
		this.txtReceiptDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtReceiptDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtReceiptDate.Location = new System.Drawing.Point(217, 18);
		this.txtReceiptDate.Name = "txtReceiptDate";
		this.txtReceiptDate.Size = new System.Drawing.Size(112, 22);
		this.txtReceiptDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 14);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 30);
		this.label7.TabIndex = 65;
		this.label7.Text = "Receipt No &&\r\nDate";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(972, 28);
		this.panel11.TabIndex = 110;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.ForeColor = System.Drawing.Color.FromArgb(64, 0, 0);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Location = new System.Drawing.Point(554, -3);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Name = "BtnAutoUtilizedAllRowsOfPendingGrid";
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Size = new System.Drawing.Size(242, 26);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.TabIndex = 38;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Text = "Auto Utilize All Pending Vouchers";
		this.BtnAutoUtilizedAllRowsOfPendingGrid.UseVisualStyleBackColor = false;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Visible = false;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Click += new System.EventHandler(BtnAutoUtilizedAllRowsOfPendingGrid_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(665, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Fcy Receipts Utilization Against Bank Invoice / GD (Fcy Receipts Mapping to GD)";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(882, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(88, 20);
		this.ChkPrintslip.TabIndex = 1;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.btnshortcutkeys, this.toolStripSeparator5
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(972, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Visible = false;
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(88, 22);
		this.btnSlip.Text = "901-Print";
		this.btnSlip.Visible = false;
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(978, 663);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GridGdUtilizedHistory.AllowEdit = (InheritableBoolean)2;
		GridGdUtilizedHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridGdUtilizedHistory.DesignTimeLayout = GridGdUtilizedHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridGdUtilizedHistory.DynamicFiltering = true;
		this.GridGdUtilizedHistory.FilterMode = (FilterMode)2;
		this.GridGdUtilizedHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridGdUtilizedHistory.GroupByBoxVisible = false;
		this.GridGdUtilizedHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).Location = new System.Drawing.Point(3, 444);
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).Name = "GridGdUtilizedHistory";
		((ScrollableBase)this.GridGdUtilizedHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).Size = new System.Drawing.Size(972, 216);
		((System.Windows.Forms.Control)(object)this.GridGdUtilizedHistory).TabIndex = 50;
		this.GridGdUtilizedHistory.TotalRow = (InheritableBoolean)1;
		this.GridGdUtilizedHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridGdUtilizedHistory).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar3);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 417);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(972, 27);
		this.panel4.TabIndex = 49;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(931, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.GridGdUtilizedHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(1, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(144, 19);
		this.label3.TabIndex = 35;
		this.label3.Text = "GD Utilizing Info";
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 160);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(972, 257);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 133);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(972, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(931, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(2, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(142, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "Filtered Records";
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 57);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(972, 76);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label8);
		this.groupBox4.Controls.Add(this.label2);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(530, 68);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(461, 37);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(6, 17);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(6, 36);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yyyy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(303, 38);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(155, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(303, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 171;
		this.label8.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(146, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 171;
		this.label2.Text = "Date From";
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yyyy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(146, 38);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(155, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(972, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(3, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(728, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Fcy Receipts Utilization Against Bank Invoice / GD (Fcy Receipts Mapping to GD) History";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(972, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(972, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(986, 692);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmGdUtilizationAgainstFcyReceipt";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(frmInvoicesAdjustmentVoucher_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingReceipts).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFilter).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridGdUtilizedHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
