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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition.AdjustmentVouchers;

public class frmInvoicesAdjustmentVoucher : Form
{
	private int TransactionTypeId = 1;

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtPendingVouchers = new DataTable();

	private DataTable dtVoucherNo = new DataTable();

	private DataTable dtPaymentType = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool Approved;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool BranchFeature;

	private bool FormInitialized;

	private string ScreenName = "frmInvoicesAdjustmentVoucher";

	private int GridSupplierCustomerId;

	private List<VoucherInvoicesAdjustment> lstRemoveDetailRecord = new List<VoucherInvoicesAdjustment>();

	private UserAccount UserAccount = new UserAccount();

	private bool MultiUtilization;

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

	internal UltraCombo CmbSupplier;

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

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX grdPendingVouchers;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label2;

	private Label label8;

	private Label label3;

	private DateTimePicker txtVoucherDate;

	internal UltraCombo CmbVoucher;

	private Label label4;

	private TextBox txtVoucherAmountRegular;

	private Label label5;

	private TextBox txtPartyCellNo;

	private TextBox txtPartyCity;

	private Label label13;

	private Label label62;

	private Label label9;

	internal UltraCombo CmbPaymentType;

	private Button BtnAutoUtilize;

	internal UltraCombo CmbAccountFilter;

	private Button btnShowRecords;

	private Button BtnAutoUtilizedAllRowsOfPendingGrid;

	private CtrlGrdBar ctrlGrdBar3;

	public frmInvoicesAdjustmentVoucher(UserAccount user)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
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
			dtVoucherNo.Columns.Add("Id", typeof(int));
			dtVoucherNo.Columns.Add("VoucherNo", typeof(int));
			dtVoucherNo.Columns.Add("VoucherDate", typeof(DateTime));
			dtVoucherNo.Columns.Add("VoucherAmount", typeof(double));
			dtPaymentType.Columns.Add("Id", typeof(int));
			dtPaymentType.Columns.Add("PaymentType", typeof(string));
			dtSupplier.Columns.Add("SupplierCustomerId", typeof(int));
			dtSupplier.Columns.Add("PartyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("AccountId", typeof(int));
			dtSupplier.Columns.Add("AccountTitle", typeof(string));
			dtSupplier.Columns.Add("AccountCode", typeof(string));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("RefDocumentType", typeof(string));
			dtGrid.Columns.Add("InvoiceId", typeof(int));
			dtGrid.Columns.Add("InvoiceNo", typeof(string));
			dtGrid.Columns.Add("InvoiceDate", typeof(DateTime));
			dtGrid.Columns.Add("InvoiceDueDate", typeof(DateTime));
			dtGrid.Columns.Add("InvoiceAmount", typeof(decimal));
			dtGrid.Columns.Add("AvailableInvoiceAmount", typeof(decimal));
			dtGrid.Columns.Add("AdjustmentAmount", typeof(decimal));
			dtGrid.Columns.Add("BalanceAmount", typeof(decimal));
			dtGrid.Columns.Add("Remarks", typeof(string));
			CmbAccountFilter.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
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
			BranchFeature = CommonServices.GetERPFeatureById(11);
			DataTable pendingVoucherData = VoucherInvoicesAdjustment.Vouchers_OutstandingForAdjustment(UserAccount.OrganizationId, UserAccount.CompanyId, TransactionTypeId, 0);
			return new
			{
				formright = formright,
				PendingVoucherData = pendingVoucherData
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
					FillGrdPendingOrders(result.PendingVoucherData);
				}
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				txtToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void BindCustomerCity()
	{
		try
		{
			txtPartyCity.Text = string.Empty;
			txtPartyCellNo.Text = string.Empty;
			int Id = Conversion.ToInt(CmbSupplier.Value);
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Id > 0)
			{
				string CustomerCellNo = Conversion.ToString(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["MobileNo"].Value);
				txtPartyCellNo.Text = CustomerCellNo;
				string CityName = Conversion.ToString(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["CityName"].Value);
				txtPartyCity.Text = CityName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierdtFromPendingGrid(int voucherHeadId, int PaymentTypeId)
	{
		try
		{
			dtSupplier.Clear();
			HashSet<int> uniqueSuppliers = new HashSet<int>();
			foreach (GridEXRow row in from rrr in grdPendingVouchers.GetDataRows()
				where Conversion.ToInt(rrr.Cells["Id"].Value) == voucherHeadId && Conversion.ToInt(rrr.Cells["PaymentTypeId"].Value) == PaymentTypeId
				select rrr)
			{
				int supplierId = Conversion.ToInt(row.Cells["SupplierCustomerId"].Value);
				int glAccountId = Conversion.ToInt(row.Cells["AccountId"].Value);
				if (uniqueSuppliers.Add(supplierId))
				{
					dtSupplier.Rows.Add(supplierId, row.Cells["PartyName"].Value, row.Cells["PartyCode"].Value, glAccountId, row.Cells["AccountTitle"].Value, row.Cells["AccountCode"].Value, row.Cells["PartyCity"].Value, row.Cells["PartyCellNo"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindPartyName()
	{
		try
		{
			int Id = Conversion.ToInt(CmbSupplier.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					DDL.BindDDL(dtSupplier, CmbSupplier, "SupplierCustomerId", "PartyName", "Party Name", ZeroIndex: true);
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("AccountId"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["AccountId"].Hidden = true;
					}
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("AccountTitle"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["AccountTitle"].Hidden = true;
					}
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("AccountCode"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["AccountCode"].Hidden = true;
					}
					if (Id > 0)
					{
						CommonServices.SetComboValue(CmbSupplier, "SupplierCustomerId", Id, dtSupplier);
					}
				}
				else
				{
					DDL.BindDDL(dtSupplier, CmbSupplier, "AccountId", "AccountTitle", "Party Name", ZeroIndex: false);
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("SupplierCustomerId"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["SupplierCustomerId"].Hidden = true;
					}
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("PartyName"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["PartyName"].Hidden = true;
					}
					if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns).Exists("PartyCode"))
					{
						((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns["PartyCode"].Hidden = true;
					}
					CommonServices.SetComboValue(CmbSupplier, "AccountId", Id, dtSupplier, ActivateRow: true);
				}
			}
			else
			{
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindPaymentTypeFromPending(GridEXRow r)
	{
		try
		{
			int Id = Conversion.ToInt(CmbPaymentType.Value);
			dtPaymentType.Clear();
			dtPaymentType.Rows.Add(r.Cells["PaymentTypeId"].Value, r.Cells["PaymentType"].Value);
			if (dtPaymentType.Rows.Count > 0)
			{
				DDL.BindDDL(dtPaymentType, CmbPaymentType, "Id", "PaymentType", "PaymentType", ZeroIndex: false);
				CommonServices.SetComboValue(CmbPaymentType, "Id", Id, dtPaymentType, ActivateRow: true);
			}
			else
			{
				((Control)(object)CmbPaymentType).Text = string.Empty;
				((UltraGridBase)CmbPaymentType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindVoucherFromPending(GridEXRow r)
	{
		try
		{
			int Id = Conversion.ToInt(CmbVoucher.Value);
			dtVoucherNo.Clear();
			dtVoucherNo.Rows.Add(r.Cells["Id"].Value, r.Cells["VoucherCode"].Value, r.Cells["VoucherDate"].Value, r.Cells["Amount"].Value);
			if (dtVoucherNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtVoucherNo, CmbVoucher, "Id", "VoucherNo", "VoucherNo", ZeroIndex: false);
				CommonServices.SetComboValue(CmbVoucher, "Id", Id, dtVoucherNo, ActivateRow: true);
			}
			else
			{
				((Control)(object)CmbVoucher).Text = string.Empty;
				((UltraGridBase)CmbVoucher).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BindCustomerCity();
			BindGridData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbVoucher_Leave(object sender, EventArgs e)
	{
		try
		{
			int voucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			if (((UltraGridBase)CmbVoucher).ActiveRow != null && voucherHeadId > 0)
			{
				DateTime VoucherDate = Conversion.ToDateTime(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["VoucherDate"].Value);
				double VoucherAmount = Conversion.ToDouble(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["VoucherAmount"].Value);
				txtVoucherDate.Value = VoucherDate;
				txtVoucherAmountRegular.Text = VoucherAmount.ToString("#,##.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGrdPendingOrders(DataTable dtGridOrders)
	{
		try
		{
			dtPendingVouchers.Rows.Clear();
			grdPendingVouchers.DataSource = null;
			if (dtPendingVouchers.Columns.Count == 0)
			{
				dtPendingVouchers.Columns.Add("Id", typeof(int));
				dtPendingVouchers.Columns.Add("PaymentTypeId", typeof(int));
				dtPendingVouchers.Columns.Add("PaymentType", typeof(string));
				dtPendingVouchers.Columns.Add("DocumentTypeId", typeof(int));
				dtPendingVouchers.Columns.Add("DocumentType", typeof(string));
				dtPendingVouchers.Columns.Add("VoucherDate", typeof(DateTime));
				dtPendingVouchers.Columns.Add("VoucherCode", typeof(string));
				dtPendingVouchers.Columns.Add("VoucherAmount", typeof(decimal));
				dtPendingVouchers.Columns.Add("AccountId", typeof(int));
				dtPendingVouchers.Columns.Add("AccountCode", typeof(string));
				dtPendingVouchers.Columns.Add("AccountTitle", typeof(string));
				dtPendingVouchers.Columns.Add("Amount", typeof(decimal));
				dtPendingVouchers.Columns.Add("SupplierCustomerId", typeof(int));
				dtPendingVouchers.Columns.Add("PartyName", typeof(string));
				dtPendingVouchers.Columns.Add("PartyCode", typeof(string));
				dtPendingVouchers.Columns.Add("PartyCity", typeof(string));
				dtPendingVouchers.Columns.Add("PartyCellNo", typeof(string));
				dtPendingVouchers.Columns.Add("VoucherRemarks", typeof(string));
				dtPendingVouchers.Columns.Add("NoOfAttachments", typeof(int));
			}
			if (dtGridOrders.Rows.Count > 0)
			{
				foreach (DataRow row in dtGridOrders.Rows)
				{
					dtPendingVouchers.Rows.Add(row["Id"], row["PaymentTypeId"], row["PaymentType"], row["DocumentTypeId"], row["DocumentType"], row["VoucherDate"], row["VoucherCode"], row["VoucherAmount"], row["AccountId"], row["AccountCode"], row["AccountTitle"], row["Amount"], row["SupplierCustomerId"], row["PartyName"], row["PartyCode"], row["MobilePersonal"], row["CityName"], row["Remarks"], row["NoOfAttachments"]);
				}
			}
			if (dtPendingVouchers != null && dtPendingVouchers.Columns.Count > 0)
			{
				grdPendingVouchers.DataSource = dtPendingVouchers;
				grdPendingVouchers.RetrieveStructure();
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
			DataTable dt = sourceData.DefaultView.ToTable(true, "SupplierCustomerId", "PartyName", "AccountId");
			DDL.BindDDL(dt, CmbAccountFilter, "SupplierCustomerId", "PartyName", "Party Name", ZeroIndex: false);
			((UltraGridBase)CmbAccountFilter).DisplayLayout.Bands[0].Columns["SupplierCustomerId"].Hidden = true;
			((UltraGridBase)CmbAccountFilter).DisplayLayout.Bands[0].Columns["AccountId"].Hidden = true;
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
			grdPendingVouchers.RootTable.Columns["Id"].Visible = false;
			grdPendingVouchers.RootTable.Columns["VoucherAmount"].Visible = false;
			grdPendingVouchers.RootTable.Columns["PaymentTypeId"].Visible = false;
			grdPendingVouchers.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingVouchers.RootTable.Columns["AccountId"].Visible = false;
			grdPendingVouchers.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdPendingVouchers.RootTable.Columns["PartyName"].Visible = false;
			grdPendingVouchers.RootTable.Columns["PartyCode"].Visible = false;
			grdPendingVouchers.RootTable.Columns["PartyCity"].Visible = false;
			grdPendingVouchers.RootTable.Columns["PartyCellNo"].Visible = false;
			grdPendingVouchers.RootTable.Columns["DocumentType"].Caption = "V.Type";
			grdPendingVouchers.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdPendingVouchers, 2);
			grdPendingVouchers.RootTable.Columns["PaymentType"].Width = 130;
			grdPendingVouchers.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grdPendingVouchers.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grdPendingVouchers.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.DocNoConstant;
			grdPendingVouchers.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingVouchers.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdPendingVouchers.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grdPendingVouchers.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingVouchers.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
			grdPendingVouchers.RootTable.Columns["PartyCode"].Width = Constants.InventoryConstants.PartyCode;
			grdPendingVouchers.RootTable.Columns["VoucherRemarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			grdPendingVouchers.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdPendingVouchers.RootTable.Columns.Add("Load");
			grdPendingVouchers.RootTable.Columns["Load"].Key = "Load";
			grdPendingVouchers.RootTable.Columns["Load"].Caption = "Load";
			grdPendingVouchers.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingVouchers.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdPendingVouchers.RootTable.Columns["Load"].ButtonText = "Load";
			grdPendingVouchers.RootTable.Columns["Load"].Width = 50;
			grdPendingVouchers.RootTable.Columns["Load"].Position = 0;
			grdPendingVouchers.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
			if (addcheckBox)
			{
				GridEX_Helper.AddSelectButton(grdPendingVouchers, "Select", 30, 0);
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
			GridEXRow r = grdPendingVouchers.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Load")
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					throw new Exception("Please Reset the Form First to Load New Data");
				}
				BindVoucherFromPending(r);
				BindPaymentTypeFromPending(r);
				SupplierdtFromPendingGrid(Conversion.ToInt(CmbVoucher.Value), Conversion.ToInt(CmbPaymentType.Value));
				BindPartyName();
				comsupplier_ValueChanged(null, null);
				CmbSupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingOrders_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPendingVouchers.CurrentRow;
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

	private void BindGridData()
	{
		try
		{
			if (dtSupplier == null || dtSupplier.Rows.Count == 0 || ((UltraGridBase)CmbSupplier).DataSource == null || ((UltraGridBase)CmbSupplier).ActiveRow == null)
			{
				return;
			}
			int comboSupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
			if (comboSupplierCustomerId != Conversion.ToInt(CmbAccountFilter.Value) && grd.RowCount == 0)
			{
				int voucherHeadId = Conversion.ToInt(CmbVoucher.Value);
				if (((UltraGridBase)CmbVoucher).ActiveRow == null || voucherHeadId == 0)
				{
					CmbVoucher.Focus();
					throw new Exception("VoucherNo is Required...");
				}
				if (((UltraGridBase)CmbSupplier).ActiveRow == null || comboSupplierCustomerId == 0)
				{
					CmbSupplier.Focus();
					throw new Exception("Party is Required...");
				}
				if (grd.RowCount > 0)
				{
					LoadInvoiceData(comboSupplierCustomerId);
					return;
				}
				dtGrid.Clear();
				grd.DataSource = null;
				LoadInvoiceData(comboSupplierCustomerId);
			}
			else if (MultiUtilization && grd.RowCount > 0)
			{
				LoadInvoiceData(comboSupplierCustomerId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInvoiceData(int supplierId)
	{
		dtGrid.Rows.Clear();
		DataTable dt = VoucherInvoicesAdjustment.PurchaseInvoice_GetDataForAdjustment(UserAccount.OrganizationId, UserAccount.CompanyId, supplierId);
		if (dt.Rows.Count > 0)
		{
			LoadDetailDataFromInvoice(dt);
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
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["RefDocumentType"].Caption = "Invoice Type";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 3);
				column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
				column.EditType = (EditType)((column.Key == "AdjustmentAmount" || column.Key == "Remarks") ? 1 : 0);
			}
			grd.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			grd.RootTable.Columns["InvoiceDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["InvoiceDueDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["InvoiceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AvailableInvoiceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AdjustmentAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["BalanceAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "AdjustmentAmount")
			{
				double AdjustmentAmount = Conversion.ToDouble(item.Cells["AdjustmentAmount"].Value);
				double InvoiceAmount = Conversion.ToDouble(item.Cells["AvailableInvoiceAmount"].Value);
				if (AdjustmentAmount > InvoiceAmount)
				{
					AdjustmentAmount = InvoiceAmount;
					item.Cells["AdjustmentAmount"].Value = AdjustmentAmount;
					MessageBox.Show("AdjustmentAmount can not be greater than AvailableInvoiceAmount");
				}
				double BalanceAmount = InvoiceAmount - AdjustmentAmount;
				item.Cells["BalanceAmount"].Value = BalanceAmount;
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
				VoucherInvoicesAdjustment detail = new VoucherInvoicesAdjustment();
				detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				detail.EntryDate = DateTime.Now;
				detail.ModifyDate = DateTime.Now;
				detail.EntryUserId = UserAccount.ID;
				detail.ModifyUserId = UserAccount.ID;
				detail.TransactionTypeId = TransactionTypeId;
				detail.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
				detail.VoucherCode = Conversion.ToInt((object)((Control)(object)CmbVoucher).Text);
				detail.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
				detail.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
				detail.PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["GlAccountId"].Value);
				detail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				detail.RefDocNoId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				detail.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
				detail.AdjustmentAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
				detail.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
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
		if (((UltraGridBase)CmbVoucher).ActiveRow == null || Conversion.ToInt(CmbVoucher.Value) == 0)
		{
			MessageBox.Show("Voucher No Field is Required");
			CmbVoucher.Focus();
			return false;
		}
		if (txtVoucherAmountRegular.Text.Trim() == "" || txtVoucherAmountRegular.Text.Trim() == "0" || txtVoucherAmountRegular.Text.Trim() == null)
		{
			MessageBox.Show("VoucherAmount Field is Required");
			txtVoucherAmountRegular.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentType).ActiveRow == null || Conversion.ToInt(CmbPaymentType.Value) == 0)
		{
			MessageBox.Show("Payment Type Field is Required");
			CmbPaymentType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplier).ActiveRow == null || Conversion.ToInt(CmbSupplier.Value) == 0)
		{
			MessageBox.Show("Party Field is Required");
			CmbSupplier.Focus();
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
			MultiUtilization = false;
			GridSupplierCustomerId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			MultiUtilization = false;
			((Control)(object)CmbVoucher).Text = string.Empty;
			((UltraGridBase)CmbVoucher).DataSource = null;
			dtVoucherNo.Clear();
			((Control)(object)CmbVoucher).Enabled = true;
			txtVoucherAmountRegular.Text = string.Empty;
			((Control)(object)CmbPaymentType).Text = string.Empty;
			((UltraGridBase)CmbPaymentType).DataSource = null;
			dtPaymentType.Clear();
			((Control)(object)CmbPaymentType).Enabled = true;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)CmbSupplier).DataSource = null;
			dtSupplier.Clear();
			((Control)(object)CmbSupplier).Enabled = true;
			txtPartyCellNo.Text = string.Empty;
			txtPartyCity.Text = string.Empty;
			lstRemoveDetailRecord.Clear();
			dtGrid.Rows.Clear();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			FillGrdPendingOrders(VoucherInvoicesAdjustment.Vouchers_OutstandingForAdjustment(UserAccount.OrganizationId, UserAccount.CompanyId, TransactionTypeId, 0));
			CmbAccountFilter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierdtFromPendingGrid(Conversion.ToInt(CmbVoucher.Value), Conversion.ToInt(CmbPaymentType.Value));
			BindPartyName();
			comsupplier_ValueChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		((Control)(object)CmbAccountFilter).Text = string.Empty;
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
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
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
			VoucherInvoicesAdjustment obj = new VoucherInvoicesAdjustment();
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.TransactionTypeId = TransactionTypeId;
			obj.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			obj.VoucherCode = Conversion.ToInt((object)((Control)(object)CmbVoucher).Text);
			obj.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
			obj.PaymentTypeId = Conversion.ToInt(CmbPaymentType.Value);
			obj.SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
			obj.PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountId"].Value);
			obj.VoucherInvoicesAdjustmentList = new List<VoucherInvoicesAdjustment>();
			decimal TotalAdjustmentAmount = default(decimal);
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0 || Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value) > 0m)
				{
					VoucherInvoicesAdjustment detail = new VoucherInvoicesAdjustment();
					detail.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					detail.ActionTypeId = ((detail.Id <= 0) ? 1 : 2);
					detail.EntryDate = DateTime.Now;
					detail.ModifyDate = DateTime.Now;
					detail.EntryUserId = UserAccount.ID;
					detail.ModifyUserId = UserAccount.ID;
					detail.ScreenName = ScreenName;
					detail.TransactionTypeId = TransactionTypeId;
					detail.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
					detail.VoucherCode = Conversion.ToInt((object)((Control)(object)CmbVoucher).Text);
					detail.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
					detail.VoucherAmount = Conversion.ToDecimal((object)txtVoucherAmountRegular.Text);
					detail.PaymentTypeId = Conversion.ToInt(CmbPaymentType.Value);
					detail.SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
					detail.PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountId"].Value);
					detail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					detail.RefDocNoId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					detail.DueDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
					detail.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
					if (detail.PaymentTypeId == 1)
					{
						detail.AdvanceAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
						detail.AdjustmentAmount = 0m;
					}
					else
					{
						detail.AdjustmentAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
						detail.AdvanceAmount = 0m;
					}
					TotalAdjustmentAmount += Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
					detail.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					if (detail.RefDocumentTypeId == 0)
					{
						throw new Exception("RefDocumentType Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.RefDocNoId == 0)
					{
						throw new Exception("RefDocNo Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.InvoiceAmount == 0m)
					{
						throw new Exception("InvoiceAmount Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.PaymentTypeId == 1 && detail.AdvanceAmount == 0m)
					{
						throw new Exception("AdjustmentAmount Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					if (detail.PaymentTypeId != 1 && detail.AdjustmentAmount == 0m)
					{
						throw new Exception("AdjustmentAmount Required in Detail Grid And row No: " + (r.RowIndex + 1));
					}
					obj.VoucherInvoicesAdjustmentList.Add(detail);
				}
			}
			if (obj.VoucherInvoicesAdjustmentList.Count == 0)
			{
				throw new Exception("At least enter one row in detail ");
			}
			if (lstRemoveDetailRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveDetailRecord.Count; i++)
				{
					obj.VoucherInvoicesAdjustmentList.Add(lstRemoveDetailRecord[i]);
				}
			}
			if (Conversion.ToInt(CmbPaymentType.Value) == 1 && Math.Round(TotalAdjustmentAmount, 0) > Math.Round(Conversion.ToDecimal((object)txtVoucherAmountRegular.Text), 0))
			{
				throw new Exception($"Total adjustment Amount {Math.Round(TotalAdjustmentAmount, 0)} In Grid cannot be greater then to voucher amount {Math.Round(Conversion.ToDecimal((object)txtVoucherAmountRegular.Text), 0)}");
			}
			if (Conversion.ToInt(CmbPaymentType.Value) != 1 && Math.Round(TotalAdjustmentAmount, 0) != Math.Round(Conversion.ToDecimal((object)txtVoucherAmountRegular.Text), 0))
			{
				throw new Exception($"Total adjustment Amount {Math.Round(TotalAdjustmentAmount, 0)} In Grid Should be equal to voucher amount {Math.Round(Conversion.ToDecimal((object)txtVoucherAmountRegular.Text), 0)}");
			}
			VoucherInvoicesAdjustment.Save(obj);
			if (!Auto)
			{
				if (RecId == 0)
				{
					MessageBox.Show("Record Saved Successfully [" + obj.VoucherCode + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + obj.VoucherCode + "] ");
				}
				Reset();
				if (ChkPrintslip.Checked)
				{
					GenerateSlip(obj.VoucherHeadId, obj.PaymentTypeId, obj.SupplierCustomerId, obj.PartyGlId);
				}
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

	public void ReadById(int VoucherHeadId, int PaymentTypeId, int SupplierCustomerId, int PartyGlId)
	{
		try
		{
			Reset();
			RecId = VoucherHeadId;
			DataTable invPI = VoucherInvoicesAdjustment.GetByVoucherHeadId(TransactionTypeId, VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
			if (invPI == null || invPI.Rows.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			((Control)(object)grd).Focus();
			dtVoucherNo.Clear();
			dtVoucherNo.Rows.Add(invPI.Rows[0]["VoucherHeadId"], invPI.Rows[0]["VoucherCode"], invPI.Rows[0]["VoucherDate"], invPI.Rows[0]["VoucherAmount"]);
			DDL.BindDDL(dtVoucherNo, CmbVoucher, "Id", "VoucherNo", "VoucherNo", ZeroIndex: false);
			((UltraGridBase)CmbVoucher).Rows[0].Activate();
			if (((UltraGridBase)CmbVoucher).ActiveRow != null && VoucherHeadId > 0)
			{
				DateTime VoucherDate = Conversion.ToDateTime(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["VoucherDate"].Value);
				double VoucherAmount = Conversion.ToDouble(((UltraDropDownBase)CmbVoucher).SelectedRow.Cells["VoucherAmount"].Value);
				txtVoucherDate.Value = VoucherDate;
				txtVoucherAmountRegular.Text = VoucherAmount.ToString("#,##.##");
			}
			dtPaymentType.Clear();
			dtPaymentType.Rows.Add(invPI.Rows[0]["PaymentTypeId"], invPI.Rows[0]["PaymentType"]);
			DDL.BindDDL(dtPaymentType, CmbPaymentType, "Id", "PaymentType", "PaymentType", ZeroIndex: false);
			((UltraGridBase)CmbPaymentType).Rows[0].Activate();
			dtSupplier.Clear();
			dtSupplier.Rows.Add(invPI.Rows[0]["SupplierCustomerId"], invPI.Rows[0]["PartyName"], invPI.Rows[0]["PartyCode"], invPI.Rows[0]["PartyGlId"], invPI.Rows[0]["PartyGlAccountTitle"], invPI.Rows[0]["PartyGlAccountCode"], invPI.Rows[0]["PartyCityName"], invPI.Rows[0]["MobilePersonal"]);
			BindPartyName();
			((UltraGridBase)CmbSupplier).Rows[0].Activate();
			foreach (DataRow dataRow in invPI.Rows)
			{
				int invoiceId = Conversion.ToInt(dataRow["RefDocNoId"]);
				decimal invoiceAmount = Conversion.ToDecimal(dataRow["InvoiceAmount"]);
				decimal adjustmentAmount = ((PaymentTypeId == 1) ? Conversion.ToDecimal(dataRow["AdvanceAmount"]) : Conversion.ToDecimal(dataRow["AdjustmentAmount"]));
				string Remarks = Conversion.ToString(dataRow["Remarks"]);
				dtGrid.Rows.Add(dataRow["Id"], dataRow["RefDocumentTypeId"], dataRow["RefDocumentType"], invoiceId, Conversion.ToInt(dataRow["RefDocNo"]), dataRow["RefDocDate"], dataRow["DueDate"], invoiceAmount, invoiceAmount, adjustmentAmount, invoiceAmount - adjustmentAmount, Remarks);
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			comsupplier_ValueChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found for deletion...");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			VoucherInvoicesAdjustment obj = new VoucherInvoicesAdjustment();
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.TransactionTypeId = TransactionTypeId;
			obj.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			obj.VoucherCode = Conversion.ToInt((object)((Control)(object)CmbVoucher).Text);
			obj.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
			obj.SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
			obj.PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountId"].Value);
			obj.VoucherInvoicesAdjustmentList = new List<VoucherInvoicesAdjustment>();
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				bool exists = lstRemoveDetailRecord.Any((VoucherInvoicesAdjustment rrr) => rrr.Id == Id);
				if (Id > 0 && !exists)
				{
					VoucherInvoicesAdjustment detail = new VoucherInvoicesAdjustment();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.EntryDate = DateTime.Now;
					detail.ModifyDate = DateTime.Now;
					detail.EntryUserId = UserAccount.ID;
					detail.ModifyUserId = UserAccount.ID;
					detail.ScreenName = ScreenName;
					detail.TransactionTypeId = TransactionTypeId;
					detail.VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
					detail.VoucherCode = Conversion.ToInt((object)((Control)(object)CmbVoucher).Text);
					detail.VoucherDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
					detail.VoucherAmount = Conversion.ToDecimal((object)txtVoucherAmountRegular.Text);
					detail.PaymentTypeId = Conversion.ToInt(CmbPaymentType.Value);
					detail.SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
					detail.PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountId"].Value);
					detail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					detail.RefDocNoId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					detail.DueDate = Conversion.ToDateTime((object)txtVoucherDate.Value);
					detail.InvoiceAmount = Conversion.ToDecimal(r.Cells["InvoiceAmount"].Value);
					if (detail.PaymentTypeId == 1)
					{
						detail.AdvanceAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
						detail.AdjustmentAmount = 0m;
					}
					else
					{
						detail.AdjustmentAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
						detail.AdvanceAmount = 0m;
					}
					detail.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
				}
			}
			if (lstRemoveDetailRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveDetailRecord.Count; i++)
				{
					obj.VoucherInvoicesAdjustmentList.Add(lstRemoveDetailRecord[i]);
				}
			}
			VoucherInvoicesAdjustment.Save(obj);
			MessageBox.Show("Deleted Successfully");
			Reset();
		}
		catch (Exception ex)
		{
			HashSet<int> gridIds = (from row in grd.GetDataRows()
				where Conversion.ToInt(row.Cells["Id"].Value) > 0
				select Conversion.ToInt(row.Cells["Id"].Value)).ToHashSet();
			lstRemoveDetailRecord.RemoveAll((VoucherInvoicesAdjustment item) => gridIds.Contains(item.Id));
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
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			((UltraGridBase)CmbCustomerHistory).DataSource = null;
			DataTable dtSupplier = VoucherInvoicesAdjustment.PartyDropDown(TransactionTypeId);
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DDL.BindDDL(dtSupplier, CmbCustomerHistory, "SupplierCustomerId", "PartyName", "Party Name", ZeroIndex: true);
				if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns).Exists("AccountId"))
				{
					((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns["AccountId"].Hidden = true;
				}
				if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns).Exists("AccountTitle"))
				{
					((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns["AccountTitle"].Hidden = true;
				}
				if (Id > 0)
				{
					CommonServices.SetComboValue(CmbCustomerHistory, "SupplierCustomerId", Id, dtSupplier);
				}
			}
			else
			{
				DDL.BindDDL(dtSupplier, CmbCustomerHistory, "AccountId", "AccountTitle", "Party Name", ZeroIndex: true);
				if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns).Exists("SupplierCustomerId"))
				{
					((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns["SupplierCustomerId"].Hidden = true;
				}
				if (((KeyedSubObjectsCollectionBase)((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns).Exists("PartyName"))
				{
					((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns["PartyName"].Hidden = true;
				}
				if (Id > 0)
				{
					CommonServices.SetComboValue(CmbCustomerHistory, "AccountId", Id, dtSupplier);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			PI.TransactionTypeId = TransactionTypeId;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.FromDate = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			if (((UltraGridBase)CmbCustomerHistory).ActiveRow != null && Conversion.ToInt(CmbCustomerHistory.Value) > 0)
			{
				PI.SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbCustomerHistory).SelectedRow.Cells["SupplierCustomerId"].Value);
				PI.GlAccountId = Conversion.ToInt(((UltraDropDownBase)CmbCustomerHistory).SelectedRow.Cells["AccountId"].Value);
			}
			DataTable invPI = VoucherInvoicesAdjustment.FormHistory(PI);
			if (invPI != null && invPI.Rows.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("TransactionTypeId", typeof(int));
				dtdetail.Columns.Add("VoucherHeadId", typeof(int));
				dtdetail.Columns.Add("VoucherCode", typeof(int));
				dtdetail.Columns.Add("VoucherDate", typeof(DateTime));
				dtdetail.Columns.Add("VoucherAmount", typeof(decimal));
				dtdetail.Columns.Add("PartyGlId", typeof(int));
				dtdetail.Columns.Add("AccountCode", typeof(string));
				dtdetail.Columns.Add("AccountTitle", typeof(string));
				dtdetail.Columns.Add("PaymentTypeId", typeof(int));
				dtdetail.Columns.Add("PaymentType", typeof(string));
				dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
				dtdetail.Columns.Add("RefDocumentType", typeof(string));
				dtdetail.Columns.Add("InvoiceNos", typeof(string));
				dtdetail.Columns.Add("InvoiceDates", typeof(string));
				dtdetail.Columns.Add("AdjustmentAmount", typeof(decimal));
				dtdetail.Columns.Add("SupplierCustomerId", typeof(int));
				dtdetail.Columns.Add("PartyName", typeof(string));
				dtdetail.Columns.Add("EntryDate", typeof(DateTime));
				dtdetail.Columns.Add("EntryUserName", typeof(string));
				dtdetail.Columns.Add("ModifyDate", typeof(DateTime));
				dtdetail.Columns.Add("ModifyUserName", typeof(string));
				dtdetail.Columns.Add("VoucherRemarks", typeof(string));
				foreach (DataRow dataRow in invPI.Rows)
				{
					decimal adjustmentAmount = ((Conversion.ToInt(dataRow["PaymentTypeId"]) == 1) ? Conversion.ToDecimal(dataRow["AdvanceAmount"]) : Conversion.ToDecimal(dataRow["AdjustmentAmount"]));
					string Remarks = Conversion.ToString(dataRow["Remarks"]);
					dtdetail.Rows.Add(dataRow["TransactionTypeId"], dataRow["VoucherHeadId"], dataRow["VoucherCode"], dataRow["VoucherDate"], dataRow["VoucherAmount"], dataRow["PartyGlId"], dataRow["PartyGlAccountCode"], dataRow["PartyGlAccountTitle"], dataRow["PaymentTypeId"], dataRow["PaymentType"], dataRow["RefDocumentTypeId"], dataRow["RefDocumentType"], dataRow["RefDocNos"], dataRow["RefDocDates"], adjustmentAmount, dataRow["SupplierCustomerId"], dataRow["PartyName"], dataRow["EntryDate"], dataRow["EntryUserName"], dataRow["ModifyDate"], dataRow["ModifyUserName"], Remarks);
				}
				grdHistory.DataSource = dtdetail;
				grdHistory.RetrieveStructure();
				grdHistorySetting();
			}
			else
			{
				grdHistory.DataSource = null;
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
			if (tabControl1.SelectedIndex != 1)
			{
				return;
			}
			if (((UltraGridBase)CmbCustomerHistory).DataSource == null || ((DisposableObjectCollectionBase)((UltraGridBase)CmbCustomerHistory).Rows).Count == 0)
			{
				HistoryComboBind();
			}
			if (((UltraGridBase)cmbDateTypeHistory).DataSource == null || ((DisposableObjectCollectionBase)((UltraGridBase)cmbDateTypeHistory).Rows).Count == 0)
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
				int PaymentTypeId = Conversion.ToInt((object)r.Cells["PaymentTypeId"].Value.ToString());
				int SupplierCustomerId = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString());
				int PartyGlId = Conversion.ToInt((object)r.Cells["PartyGlId"].Value.ToString());
				ReadById(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
				int PaymentTypeId = Conversion.ToInt((object)r.Cells["PaymentTypeId"].Value.ToString());
				int SupplierCustomerId = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString());
				int PartyGlId = Conversion.ToInt((object)r.Cells["PartyGlId"].Value.ToString());
				if (e.Column.Key == "Edit")
				{
					ReadById(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
				}
				if (e.Column.Key == "Print")
				{
					GenerateSlip(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistorySetting()
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		try
		{
			grdHistory.RootTable.Columns["TransactionTypeId"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["PaymentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["PartyGlId"].Visible = false;
			grdHistory.RootTable.Columns["PartyName"].Visible = false;
			grdHistory.RootTable.Columns["RefDocumentType"].Caption = "Invoice Type";
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 3);
				column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
				column.EditType = (EditType)((column.Key == "AdjustmentAmount" || column.Key == "VoucherRemarks") ? 1 : 0);
			}
			grdHistory.RootTable.Columns["VoucherCode"].Width = Constants.InventoryConstants.VoucherNo;
			grdHistory.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["VoucherAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistory.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grdHistory.RootTable.Columns["PaymentType"].Width = 130;
			grdHistory.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grdHistory.RootTable.Columns["InvoiceNos"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			grdHistory.RootTable.Columns["InvoiceDates"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["AdjustmentAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["VoucherRemarks"].Width = Constants.InventoryConstants.RemarksConstants + 10;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 50;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Print"].Position = 1;
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
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
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
			int VoucherHeadId = Conversion.ToInt(CmbVoucher.Value);
			int PaymentTypeId = Conversion.ToInt(CmbPaymentType.Value);
			int SupplierCustomerId = 0;
			int PartyGlId = 0;
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0)
			{
				SupplierCustomerId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["SupplierCustomerId"].Value);
				PartyGlId = Conversion.ToInt(((UltraDropDownBase)CmbSupplier).SelectedRow.Cells["AccountId"].Value);
			}
			GenerateSlip(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int VoucherHeadId, int PaymentTypeId, int SupplierCustomerId, int PartyGlId)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherInvoicesAdjustment.GetByVoucherHeadId(TransactionTypeId, VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPendingVouchers, dtPendingVouchers);
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
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
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbSupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdPendingVouchers).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbSupplier.Focus();
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
			int VoucherHeadId = Conversion.ToInt((object)r.Cells["VoucherHeadId"].Value.ToString());
			int PaymentTypeId = Conversion.ToInt((object)r.Cells["PaymentTypeId"].Value.ToString());
			int SupplierCustomerId = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString());
			int PartyGlId = Conversion.ToInt((object)r.Cells["PartyGlId"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					GenerateSlip(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
				}
				_ = grdHistory.CurrentColumn.Key == "NoOfAttachments";
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				GenerateSlip(VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId);
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
			decimal totalAmount = Conversion.ToDecimal((object)txtVoucherAmountRegular.Text);
			decimal totalGridAmount = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["AdjustmentAmount"], (AggregateFunction)2).ToString()) : 0m);
			List<GridEXRow> rows = (from r in grd.GetRows()
				where Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value) == 0m
				select r).ToList();
			if (totalAmount >= totalGridAmount)
			{
				totalAmount -= totalGridAmount;
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					decimal invoiceAmount = Conversion.ToDecimal(item.Cells["AvailableInvoiceAmount"].Value);
					decimal applyAmount = Math.Min(val2: Math.Min(Conversion.ToDecimal(item.Cells["BalanceAmount"].Value), invoiceAmount), val1: totalAmount);
					item.Cells["AdjustmentAmount"].Value = (double)applyAmount;
					item.Cells["BalanceAmount"].Value = (double)(invoiceAmount - applyAmount);
					totalAmount -= applyAmount;
					item.EndEdit();
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
				foreach (GridEXRow row in rows2)
				{
					row.BeginEdit();
					if (utilizedAmount >= totalAmount)
					{
						decimal invoiceAmount2 = Conversion.ToDecimal(row.Cells["InvoiceAmount"].Value);
						row.Cells["AdjustmentAmount"].Value = 0;
						row.Cells["BalanceAmount"].Value = invoiceAmount2;
					}
					utilizedAmount += Conversion.ToDecimal(row.Cells["AdjustmentAmount"].Value);
					row.EndEdit();
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
				LoadInvoiceData(accountId);
				List<DataRow> filteredRows = (from row in dtPendingVouchers.AsEnumerable()
					where row.Field<int>("SupplierCustomerId") == accountId
					select row).ToList();
				if (!filteredRows.Any())
				{
					throw new Exception("No pending vouchers found for the selected account.");
				}
				DataTable filteredDt = filteredRows.CopyToDataTable();
				grdPendingVouchers.DataSource = filteredDt;
				grdPendingVouchers.RetrieveStructure();
				grdPendingOrdersSetting(addcheckBox: true);
			}
			else
			{
				grdPendingVouchers.DataSource = dtPendingVouchers;
				grdPendingVouchers.RetrieveStructure();
				grdPendingOrdersSetting();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
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
			int accountId = Conversion.ToInt(CmbAccountFilter.Value);
			if (((UltraGridBase)CmbAccountFilter).ActiveRow == null || accountId == 0)
			{
				CmbAccountFilter.Focus();
				MessageBox.Show("Please select an account first.");
				return;
			}
			if (grdPendingVouchers.GetDataRows().Count() == 0)
			{
				throw new Exception("Pending Vouchers Detail have no Records...");
			}
			if (grdPendingVouchers.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Select Any row First.Press show Button If 'Select' Button not Appear");
			}
			MultiUtilization = true;
			GridEXRow[] checkedRows = grdPendingVouchers.GetCheckedRows();
			foreach (GridEXRow row in checkedRows)
			{
				BindVoucherFromPending(row);
				BindPaymentTypeFromPending(row);
				SupplierdtFromPendingGrid(Conversion.ToInt(CmbVoucher.Value), Conversion.ToInt(CmbPaymentType.Value));
				BindPartyName();
				comsupplier_ValueChanged(null, null);
				BtnAutoUtilize_Click(null, null);
				Insert(Auto: true);
			}
			Reset();
			((Control)(object)CmbAccountFilter).Text = "";
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Expected O, but got Unknown
		//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Expected O, but got Unknown
		//IL_0d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d26: Expected O, but got Unknown
		//IL_3348: Unknown result type (might be due to invalid IL or missing references)
		//IL_3352: Expected O, but got Unknown
		//IL_335f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3369: Expected O, but got Unknown
		GridEXLayout grdPendingVouchers_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdPendingVouchers = new GridEX();
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
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnShowRecords = new System.Windows.Forms.Button();
		this.txtPartyCellNo = new System.Windows.Forms.TextBox();
		this.txtPartyCity = new System.Windows.Forms.TextBox();
		this.CmbAccountFilter = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtVoucherAmountRegular = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtVoucherDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbVoucher = new UltraCombo();
		this.CmbPaymentType = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
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
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingVouchers).BeginInit();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucher).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
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
		this.tabControl1.Size = new System.Drawing.Size(1096, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1088, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingVouchers);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1082, 576);
		this.panel1.TabIndex = 7;
		this.grdPendingVouchers.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingVouchers).BackColor = System.Drawing.Color.White;
		this.grdPendingVouchers.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingVouchers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingVouchers.DesignTimeLayout = grdPendingVouchers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingVouchers.FilterMode = (FilterMode)2;
		this.grdPendingVouchers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingVouchers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Location = new System.Drawing.Point(0, 488);
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Name = "grdPendingVouchers";
		((ScrollableBase)this.grdPendingVouchers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).Size = new System.Drawing.Size(1082, 88);
		((System.Windows.Forms.Control)(object)this.grdPendingVouchers).TabIndex = 0;
		this.grdPendingVouchers.TotalRow = (InheritableBoolean)1;
		this.grdPendingVouchers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingVouchers).VisualStyle = (VisualStyle)4;
		this.grdPendingVouchers.ColumnButtonClick += new ColumnActionEventHandler(grdPendingOrders_ColumnButtonClick);
		this.grdPendingVouchers.LinkClicked += new ColumnActionEventHandler(grdPendingOrders_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Controls.Add(this.label58);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 464);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1082, 24);
		this.panel6.TabIndex = 16;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(3, 4);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(227, 16);
		this.label58.TabIndex = 126;
		this.label58.Text = "Pending Vouchers For Adjustment";
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(0, 124);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1082, 340);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1074, 311);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1068, 284);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.BtnAutoUtilize);
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1068, 21);
		this.panel16.TabIndex = 33;
		this.BtnAutoUtilize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAutoUtilize.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.BtnAutoUtilize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAutoUtilize.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAutoUtilize.ForeColor = System.Drawing.Color.FromArgb(64, 0, 0);
		this.BtnAutoUtilize.Location = new System.Drawing.Point(814, -1);
		this.BtnAutoUtilize.Name = "BtnAutoUtilize";
		this.BtnAutoUtilize.Size = new System.Drawing.Size(211, 22);
		this.BtnAutoUtilize.TabIndex = 36;
		this.BtnAutoUtilize.Text = "Auto Utilize Voucher Amount";
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1028, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1082, 99);
		this.panel3.TabIndex = 0;
		this.groupBox2.Controls.Add(this.btnShowRecords);
		this.groupBox2.Controls.Add(this.txtPartyCellNo);
		this.groupBox2.Controls.Add(this.txtPartyCity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountFilter);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtVoucherAmountRegular);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtVoucherDate);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVoucher);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 28);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1082, 69);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.btnShowRecords.BackColor = System.Drawing.Color.Teal;
		this.btnShowRecords.FlatAppearance.BorderSize = 0;
		this.btnShowRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowRecords.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnShowRecords.ForeColor = System.Drawing.Color.White;
		this.btnShowRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowRecords.Location = new System.Drawing.Point(1015, 40);
		this.btnShowRecords.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowRecords.Name = "btnShowRecords";
		this.btnShowRecords.Size = new System.Drawing.Size(57, 25);
		this.btnShowRecords.TabIndex = 111;
		this.btnShowRecords.Text = "Show";
		this.btnShowRecords.UseVisualStyleBackColor = false;
		this.btnShowRecords.Click += new System.EventHandler(btnShowRecords_Click);
		this.txtPartyCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyCellNo.Enabled = false;
		this.txtPartyCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyCellNo.Location = new System.Drawing.Point(880, 15);
		this.txtPartyCellNo.Name = "txtPartyCellNo";
		this.txtPartyCellNo.Size = new System.Drawing.Size(133, 23);
		this.txtPartyCellNo.TabIndex = 6;
		this.txtPartyCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyCity.Enabled = false;
		this.txtPartyCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyCity.Location = new System.Drawing.Point(754, 15);
		this.txtPartyCity.Name = "txtPartyCity";
		this.txtPartyCity.Size = new System.Drawing.Size(124, 23);
		this.txtPartyCity.TabIndex = 5;
		this.CmbAccountFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Location = new System.Drawing.Point(755, 40);
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Name = "CmbAccountFilter";
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountFilter).TabIndex = 110;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(681, 46);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 108;
		this.label13.Text = "Party Name";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(681, 15);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(68, 30);
		this.label62.TabIndex = 109;
		this.label62.Text = "Party City / \r\nCell";
		this.txtVoucherAmountRegular.Enabled = false;
		this.txtVoucherAmountRegular.Location = new System.Drawing.Point(104, 42);
		this.txtVoucherAmountRegular.Name = "txtVoucherAmountRegular";
		this.txtVoucherAmountRegular.Size = new System.Drawing.Size(225, 23);
		this.txtVoucherAmountRegular.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(98, 15);
		this.label5.TabIndex = 66;
		this.label5.Text = "Voucher Amount";
		this.txtVoucherDate.CustomFormat = "dd-MMM-yy";
		this.txtVoucherDate.Enabled = false;
		this.txtVoucherDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtVoucherDate.Location = new System.Drawing.Point(221, 15);
		this.txtVoucherDate.Name = "txtVoucherDate";
		this.txtVoucherDate.Size = new System.Drawing.Size(108, 22);
		this.txtVoucherDate.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(333, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(82, 15);
		this.label9.TabIndex = 65;
		this.label9.Text = "Payment Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(333, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 65;
		this.label4.Text = "Party Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(98, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Voucher No,Date";
		this.CmbVoucher.AllowNull = (DefaultableBoolean)1;
		this.CmbVoucher.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVoucher.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVoucher.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVoucher).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVoucher.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Location = new System.Drawing.Point(104, 13);
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Name = "CmbVoucher";
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbVoucher).TabIndex = 0;
		this.CmbVoucher.ValueChanged += new System.EventHandler(CmbVoucher_Leave);
		((System.Windows.Forms.Control)(object)this.CmbVoucher).Leave += new System.EventHandler(CmbVoucher_Leave);
		this.CmbPaymentType.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Location = new System.Drawing.Point(416, 13);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Name = "CmbPaymentType";
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPaymentType).Leave += new System.EventHandler(comsupplier_ValueChanged);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance46;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(416, 40);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Leave += new System.EventHandler(comsupplier_ValueChanged);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.BtnAutoUtilizedAllRowsOfPendingGrid);
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1082, 28);
		this.panel11.TabIndex = 110;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.ForeColor = System.Drawing.Color.FromArgb(64, 0, 0);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Location = new System.Drawing.Point(746, 1);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Name = "BtnAutoUtilizedAllRowsOfPendingGrid";
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Size = new System.Drawing.Size(242, 26);
		this.BtnAutoUtilizedAllRowsOfPendingGrid.TabIndex = 36;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Text = "Auto Utilize All Pending Vouchers";
		this.BtnAutoUtilizedAllRowsOfPendingGrid.UseVisualStyleBackColor = false;
		this.BtnAutoUtilizedAllRowsOfPendingGrid.Click += new System.EventHandler(BtnAutoUtilizedAllRowsOfPendingGrid_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(252, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Payment Adjustment Voucher";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(992, 4);
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
		this.toolStrip1.Size = new System.Drawing.Size(1082, 25);
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
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1088, 582);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 157);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1082, 422);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 130);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1082, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1029, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(49, 25);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(142, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "Filtered Records";
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 57);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1082, 73);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label8);
		this.groupBox4.Controls.Add(this.label2);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(966, 69);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.2f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(705, 40);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(873, 5);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 43);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Voucher Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(503, 18);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(34, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Party";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(767, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(97, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Voucher Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(767, 43);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(127, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Voucher Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(541, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(6, 18);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(6, 39);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(214, 41);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtToDateHistory.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(150, 45);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 171;
		this.label8.Text = "Date To";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(150, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 171;
		this.label2.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(428, 41);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(74, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(214, 14);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(344, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Voucher To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(344, 18);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(82, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Voucher From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(428, 14);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(74, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1082, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(251, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Adjustment Vouchers History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1082, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1082, 25);
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
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1042, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ctrlGrdBar3.MyGrid = this.grdPendingVouchers;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 37;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1096, 611);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmInvoicesAdjustmentVoucher";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingVouchers).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVoucher).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
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
