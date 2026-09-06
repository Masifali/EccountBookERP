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
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingFreightVoucherForApproval : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridData = new DataTable();

	private DataTable dtGridDbCall = new DataTable();

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	public int DocumentTypeId;

	public bool flagUnApprove;

	private bool AllowEditOnVoucherNoinReports;

	private bool FormInitialized;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private Panel panel4;

	private CheckBox ChkPrint;

	private Button btnshow;

	internal UltraCombo CmbSupplierName;

	private Panel panel3;

	private Label labelFormHeading;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	internal GridEX grdDetail;

	private Panel panel6;

	private GroupBox groupBox1;

	private Label label11;

	private Label label10;

	private Label label3;

	private TextBox txtVehicleNo;

	private DateTimePicker GRNfromdate;

	private Label label2;

	private Label label27;

	private TextBox txtDocNoFrom;

	private Label label4;

	private TextBox txtDocNoTo;

	internal UltraCombo CmbCityName;

	private Label label13;

	private DateTimePicker GRNToDate;

	private TextBox txtGpNoFrom;

	private TextBox txtGpNoTo;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Label label5;

	private DateTimePicker txtGpDateFrom;

	private Label label1;

	private DateTimePicker txtGpDateTo;

	internal UltraCombo CmbCreditAccount;

	private Label label6;

	private ToolStripButton btnApproved;

	public PendingFreightVoucherForApproval(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1200, 640);
		InitializeComponentMethod();
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			DataTable dataForDropDownFromFreightVoucher = InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (DocumentTypeId > 0)
			{
				MainGridDbCall(filters: false);
			}
			return new
			{
				HistoryComboData = dataForDropDownFromFreightVoucher
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					GetConfigurationsFromGlobal();
					if (flagUnApprove)
					{
						labelFormHeading.Text = "Pending Freight Voucher For UnApproval";
						btnApproved.Text = "UnApprove";
					}
					else
					{
						labelFormHeading.Text = "Pending Freight Voucher For Approval";
						btnApproved.Text = "Approve";
					}
					if (DocumentTypeId > 0)
					{
						MainGridFill();
					}
					if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
					{
						GRNfromdate.Value = Convert.ToDateTime(RequestedFromDate);
						GRNToDate.Value = Convert.ToDateTime(RequestedToDate);
					}
					else
					{
						GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
					}
					HistoryComboBind(result.HistoryComboData);
					FormInitialized = true;
					GRNfromdate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowEditOnVoucherNoinReports"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id", typeof(int));
			dtSupp.Columns.Add("Name", typeof(string));
			DataTable dtCrAc = dtSupp.Clone();
			DataTable dtCityName = dtSupp.Clone();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CreditAccount")
				{
					dtCrAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CityName")
				{
					dtCityName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupp, "Id", "Name", "Party Name", AllColumns: true, Conversion.ToInt(CmbSupplierName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAccount, dtCrAc, "Id", "Name", "Credit Account", AllColumns: true, Conversion.ToInt(CmbCreditAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCityName, dtCityName, "Id", "Name", "City Name", AllColumns: true, Conversion.ToInt(CmbCityName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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

	private void Reset()
	{
		try
		{
			GRNfromdate.Focus();
			((Control)(object)CmbSupplierName).Text = string.Empty;
			txtDocNoFrom.Text = string.Empty;
			txtDocNoTo.Text = string.Empty;
			txtGpNoFrom.Text = string.Empty;
			txtGpNoTo.Text = string.Empty;
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			((Control)(object)CmbCityName).Text = string.Empty;
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
			HistoryComboBind(InvFreightVoucher.GetDataForDropDownFromFreightVoucher(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null));
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
			wf.Show();
			MainGridDbCall();
			MainGridFill();
			wf.Close();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	public void MainGridDbCall(bool filters = true)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			dtGridDbCall.Clear();
			DateTime from;
			DateTime to;
			if (!filters)
			{
				from = ((!Conversion.CheckDateTimeNull(RequestedFromDate)) ? RequestedFromDate : Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period));
				to = ((!Conversion.CheckDateTimeNull(RequestedToDate)) ? RequestedToDate : Conversion.ToDateTime((object)GRNToDate.Value));
			}
			else
			{
				from = Conversion.ToDateTime((object)GRNfromdate.Value);
				to = Conversion.ToDateTime((object)GRNToDate.Value);
			}
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = DocumentTypeId;
			grn.FromDate = from;
			grn.ToDate = to;
			grn.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text.Trim());
			if (filters)
			{
				if (txtGpDateFrom.Checked)
				{
					grn.GpDateF = Conversion.ToDateTime((object)txtGpDateFrom.Value);
				}
				if (txtGpDateTo.Checked)
				{
					grn.GpDateT = Conversion.ToDateTime((object)txtGpDateTo.Value);
				}
			}
			grn.GpSrNoF = Conversion.ToInt((object)txtGpNoFrom.Text.Trim());
			grn.GpSrNoT = Conversion.ToInt((object)txtGpNoTo.Text.Trim());
			grn.CreditAccountId = Conversion.ToInt(CmbCreditAccount.Value);
			grn.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			grn.CityId = Conversion.ToInt(CmbCityName.Value);
			grn.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
			grn.IsApproved = flagUnApprove;
			dtGridDbCall = InvFreightVoucher.FreightVoucher_PendingForApproval(grn);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void MainGridFill()
	{
		try
		{
			if (dtGridDbCall.Rows.Count > 0)
			{
				if (dtGridData.Columns.Count == 0)
				{
					dtGridData.Columns.Add("Id", typeof(int));
					dtGridData.Columns.Add("VoucherHeadId", typeof(int));
					dtGridData.Columns.Add("DocumentTypeId", typeof(int));
					dtGridData.Columns.Add("DocDate", typeof(DateTime));
					dtGridData.Columns.Add("DocNo", typeof(int));
					dtGridData.Columns.Add("CashAccountId", typeof(int));
					dtGridData.Columns.Add("CashAcCode", typeof(string));
					dtGridData.Columns.Add("CashAcTitle", typeof(string));
					dtGridData.Columns.Add("GatePassId", typeof(int));
					dtGridData.Columns.Add("GatePassDate", typeof(DateTime));
					dtGridData.Columns.Add("GatePassNo", typeof(int));
					dtGridData.Columns.Add("ShortWeight", typeof(decimal));
					dtGridData.Columns.Add("SupplierWeight", typeof(decimal));
					dtGridData.Columns.Add("FactoryWeight", typeof(decimal));
					dtGridData.Columns.Add("ToleranceWeight", typeof(decimal));
					dtGridData.Columns.Add("VehicleNo", typeof(string));
					dtGridData.Columns.Add("BiltyNo", typeof(string));
					dtGridData.Columns.Add("SupplierName", typeof(string));
					dtGridData.Columns.Add("CityName", typeof(string));
					dtGridData.Columns.Add("RemarksHeader", typeof(string));
					dtGridData.Columns.Add("EntryDate", typeof(DateTime));
					dtGridData.Columns.Add("EntryUser", typeof(string));
					dtGridData.Columns.Add("ModifyDate", typeof(DateTime));
					dtGridData.Columns.Add("ModifyUser", typeof(string));
					dtGridData.Columns.Add("ApprovalStatus", typeof(string));
					dtGridData.Columns.Add("ApprovedDate", typeof(DateTime));
					dtGridData.Columns.Add("ApprovalUser", typeof(string));
					dtGridData.Columns.Add("ApprovalRemarks", typeof(string));
					dtGridData.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtGridData.Rows.Clear();
				dtGridData.BeginLoadData();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow row in dtGridDbCall.Rows)
				{
					int currentId = Conversion.ToInt(row["Id"]);
					if (existingIds.Add(currentId))
					{
						dtGridData.Rows.Add(row["Id"], row["VoucherHeadId"], row["DocumentTypeId"], row["DocDate"], row["DocNo"], row["CashAccountId"], row["CashAcCode"], row["CashAcTitle"], row["GatePassId"], row["GatePassDate"], row["GatePassNo"], row["ShortWeight"], row["SupplierWeight"], row["FactoryWeight"], row["ToleranceWeight"], row["VehicleNo"], row["BiltyNo"], row["SupplierName"], row["CityName"], row["RemarksHeader"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["ApprovalStatus"], row["ApprovedDate"], row["ApprovalUser"], row["NoOfAttachments"]);
					}
				}
				dtGridData.EndLoadData();
				DataGridHistory.DataSource = dtGridData;
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
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "VoucherHeadId", "DocumentTypeId", "CashAccountId", "GatePassId", "ApprovalStatus" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"ApprovalRemarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, hiddenColumns, editableColumns);
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			if (AllowEditOnVoucherNoinReports)
			{
				DataGridHistory.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			}
			DataGridHistory.RootTable.Columns["GatePassNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 80;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 60;
			DataGridHistory.RootTable.Columns["CashAcCode"].Width = 100;
			DataGridHistory.RootTable.Columns["CashAcTitle"].Width = 150;
			DataGridHistory.RootTable.Columns["GatePassDate"].Width = 80;
			DataGridHistory.RootTable.Columns["GatePassNo"].Width = 60;
			DataGridHistory.RootTable.Columns["ShortWeight"].Width = 90;
			DataGridHistory.RootTable.Columns["SupplierWeight"].Width = 90;
			DataGridHistory.RootTable.Columns["FactoryWeight"].Width = 90;
			DataGridHistory.RootTable.Columns["ToleranceWeight"].Width = 90;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = 80;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = 100;
			DataGridHistory.RootTable.Columns["SupplierName"].Width = 150;
			DataGridHistory.RootTable.Columns["CityName"].Width = 120;
			DataGridHistory.RootTable.Columns["RemarksHeader"].Width = 160;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = 135;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = 110;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = 135;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = 110;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = 135;
			DataGridHistory.RootTable.Columns["ApprovalUser"].Width = 110;
			GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 30, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Slip", "Slip", 40, 1);
			string caption = (flagUnApprove ? "UnApprove" : "Approve");
			string Name = (flagUnApprove ? "UnPost" : "Post");
			GridEX_Helper.AddButton(DataGridHistory, Name, caption, 80, 2);
			DataGridHistory.RootTable.Columns["ApprovalRemarks"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count == 0)
			{
				return;
			}
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (e.Column.Key == "DocNo" && AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0 && DocumentTypeId == 28)
			{
				FreightVoucher obj = new FreightVoucher(UserAccount);
				if (!Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "FreightVoucher").ToList()[0].Value.ToString()))
				{
					MessageBox.Show("You dont have right");
					return;
				}
				obj.Show();
				obj.ReadById(Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "GatePassNo" && Conversion.ToInt(item.Cells["GatePassId"].Value) > 0)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(item.Cells["GatePassId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count == 0)
			{
				return;
			}
			int id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				CommonServices.FreightVoucherSlip241(id);
			}
			if (flagUnApprove)
			{
				if (e.Column.Key == "UnPost")
				{
					ApprovedRecord(id, "UP", Conversion.ToString(DataGridHistory.CurrentRow.Cells["ApprovalRemarks"].Value));
				}
			}
			else if (e.Column.Key == "Post")
			{
				ApprovedRecord(id, "AP", Conversion.ToString(DataGridHistory.CurrentRow.Cells["ApprovalRemarks"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count == 0)
			{
				grdDetail.DataSource = null;
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			GetDetailGrdByHeadId(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("DetailId", typeof(int));
			dtcol.Columns.Add("DebitAccountId", typeof(int));
			dtcol.Columns.Add("DebAcCode", typeof(string));
			dtcol.Columns.Add("DebAcTitle", typeof(string));
			dtcol.Columns.Add("BiltyFreight", typeof(decimal));
			dtcol.Columns.Add("PerMtonFreight", typeof(decimal));
			dtcol.Columns.Add("ScaleCharges", typeof(decimal));
			dtcol.Columns.Add("TotalFreight", typeof(decimal));
			dtcol.Columns.Add("DeductedAmount", typeof(decimal));
			dtcol.Columns.Add("ChargeToParty", typeof(decimal));
			dtcol.Columns.Add("PaidAmount", typeof(decimal));
			dtcol.Columns.Add("ShortageAmount", typeof(decimal));
			dtcol.Columns.Add("DiscountAmount", typeof(decimal));
			dtcol.Columns.Add("RemainingAmount", typeof(decimal));
			dtcol.Columns.Add("RemarksDetail", typeof(string));
			dtcol.Rows.Clear();
			DataTable resultRows = (from row in dtGridDbCall.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row).CopyToDataTable();
			for (int i = 0; i < resultRows.Rows.Count; i++)
			{
				dtcol.Rows.Add(resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["DebitAccountId"], resultRows.Rows[i]["DebAcCode"], resultRows.Rows[i]["DebAcTitle"], resultRows.Rows[i]["BiltyFreight"], resultRows.Rows[i]["PerMtonFreight"], resultRows.Rows[i]["ScaleCharges"], resultRows.Rows[i]["TotalFreight"], resultRows.Rows[i]["DeductedAmount"], resultRows.Rows[i]["ChargeToParty"], resultRows.Rows[i]["PaidAmount"], resultRows.Rows[i]["ShortageAmount"], resultRows.Rows[i]["DiscountAmount"], resultRows.Rows[i]["RemainingAmount"], resultRows.Rows[i]["RemarksDetail"]);
			}
			grdDetail.DataSource = dtcol;
			grdDetail.RetrieveStructure();
			DetailGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "DetailId", "DebitAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, hiddenColumns);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedRecord(int recId, string reqType, string remarks)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		try
		{
			if (string.IsNullOrWhiteSpace(remarks))
			{
				MessageBox.Show("Approval Remarks Required in Grid");
			}
			else if (MessageBox.Show((reqType == "AP") ? "Are you sure to Approve Voucher?" : "Are you sure to UnApprove Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList approvalItem = new ApprovalList
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = DocumentTypeId,
					Id = recId,
					PostDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ReqType = reqType,
					Comments = remarks
				};
				InvFreightVoucher.FreightVoucher_UpdateApprovalStatus(new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList> { approvalItem }
				});
				MessageBox.Show((reqType == "AP") ? "Record Approved Successfully" : "Record UnApproved Successfully");
				if (ChkPrint.Checked)
				{
					CommonServices.FreightVoucherSlip241(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
				MainGridDbCall();
				MainGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		try
		{
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			if (checkedRows.Count() == 0)
			{
				MessageBox.Show("Please select check box first", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string action = (flagUnApprove ? "UnApprove" : "Approve");
			string reqType = (flagUnApprove ? "UP" : "AP");
			if (MessageBox.Show("Are you sure to " + action + " Voucher?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList
			{
				AllApprovalLists = new List<ApprovalList>()
			};
			GridEXRow[] array = checkedRows;
			foreach (GridEXRow r in array)
			{
				string remarks = Conversion.ToString(r.Cells["ApprovalRemarks"].Value);
				if (string.IsNullOrWhiteSpace(remarks))
				{
					MessageBox.Show($"ApprovalRemarks Required in Row No: {r.RowIndex + 1}", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				ApprovalList item = new ApprovalList
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = DocumentTypeId,
					Id = Conversion.ToInt(r.Cells["Id"].Value),
					PostDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ReqType = reqType,
					Comments = remarks
				};
				obj.AllApprovalLists.Add(item);
			}
			if (flagUnApprove)
			{
				InvFreightVoucher.FreightVoucher_UpdateApprovalStatus(obj);
			}
			else
			{
				InvFreightVoucher.FreightVoucher_UpdateApprovalStatus(obj);
			}
			MessageBox.Show("Record " + action + "d Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			MainGridDbCall();
			MainGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGridData);
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
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
				GRNfromdate.Focus();
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (!e.Control || e.KeyCode != Keys.Space || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (DataGridHistory.CurrentColumn.Key == "DocNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0 && DocumentTypeId == 28)
				{
					FreightVoucher obj = new FreightVoucher(UserAccount);
					if (!Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "FreightVoucher").ToList()[0].Value.ToString()))
					{
						MessageBox.Show("You dont have right");
						return;
					}
					obj.Show();
					obj.ReadById(Id);
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			else if (DataGridHistory.CurrentColumn.Key == "Slip")
			{
				CommonServices.FreightVoucherSlip241(Id);
			}
			else if (DataGridHistory.CurrentColumn.Key == "GatePassNo")
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(item.Cells["GatePassId"].Value));
			}
			if (flagUnApprove)
			{
				if (DataGridHistory.CurrentColumn.Key == "UnPost")
				{
					ApprovedRecord(Id, "UP", Conversion.ToString(DataGridHistory.CurrentRow.Cells["ApprovalRemarks"].Value));
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "Post")
			{
				ApprovedRecord(Id, "AP", Conversion.ToString(DataGridHistory.CurrentRow.Cells["ApprovalRemarks"].Value));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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

	private void GpsNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GpsNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrnNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrnNoTo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingFreightVoucherForApproval));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.txtGpNoFrom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbCreditAccount = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.txtGpDateFrom = new System.Windows.Forms.DateTimePicker();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.CmbCityName = new UltraCombo();
		this.txtGpDateTo = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.txtGpNoTo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelFormHeading = new System.Windows.Forms.Label();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 561);
		this.tabControl1.TabIndex = 49;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 535);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Grid View";
		this.tabPage1.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 184);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Tag = "MasterGrid";
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(3, 342);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 19);
		this.panel6.TabIndex = 50;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 361);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(950, 171);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 162);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 22);
		this.panel5.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(910, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 103);
		this.panel4.TabIndex = 4;
		this.groupBox1.Controls.Add(this.txtDocNoFrom);
		this.groupBox1.Controls.Add(this.txtGpNoFrom);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox1.Controls.Add(this.txtGpDateFrom);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtDocNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add(this.txtGpDateTo);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Controls.Add(this.txtGpNoTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(942, 99);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(78, 70);
		this.txtDocNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(60, 23);
		this.txtDocNoFrom.TabIndex = 1;
		this.txtDocNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.txtGpNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoFrom.Location = new System.Drawing.Point(289, 70);
		this.txtGpNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtGpNoFrom.Name = "txtGpNoFrom";
		this.txtGpNoFrom.ShortcutsEnabled = false;
		this.txtGpNoFrom.Size = new System.Drawing.Size(69, 23);
		this.txtGpNoFrom.TabIndex = 5;
		this.txtGpNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGpNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoFrom_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(885, 42);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 23);
		this.btnshow.TabIndex = 11;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(733, 19);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(64, 15);
		this.label11.TabIndex = 145;
		this.label11.Text = "Vehicle No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 74);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From,To Doc";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(204, 46);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(66, 15);
		this.label5.TabIndex = 143;
		this.label5.Text = "Gp Date To";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 46);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 143;
		this.label3.Text = "To Date";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(800, 15);
		this.txtVehicleNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.ShortcutsEnabled = false;
		this.txtVehicleNo.Size = new System.Drawing.Size(140, 23);
		this.txtVehicleNo.TabIndex = 10;
		this.txtVehicleNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(513, 13);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 7;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(513, 40);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 8;
		this.txtGpDateFrom.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpDateFrom.Checked = false;
		this.txtGpDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtGpDateFrom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpDateFrom.Location = new System.Drawing.Point(289, 15);
		this.txtGpDateFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtGpDateFrom.Name = "txtGpDateFrom";
		this.txtGpDateFrom.ShowCheckBox = true;
		this.txtGpDateFrom.Size = new System.Drawing.Size(140, 22);
		this.txtGpDateFrom.TabIndex = 3;
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(78, 15);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(121, 22);
		this.GRNfromdate.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(431, 19);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(86, 15);
		this.label6.TabIndex = 121;
		this.label6.Text = "Credit Account";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(204, 19);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 119;
		this.label1.Text = "Gp Date From";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(431, 46);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 121;
		this.label2.Text = "Supplier";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(3, 19);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(431, 74);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(27, 15);
		this.label4.TabIndex = 137;
		this.label4.Text = "City";
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(139, 70);
		this.txtDocNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ShortcutsEnabled = false;
		this.txtDocNoTo.Size = new System.Drawing.Size(60, 23);
		this.txtDocNoTo.TabIndex = 2;
		this.txtDocNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoTo_KeyPress);
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(513, 68);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 9;
		this.txtGpDateTo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpDateTo.Checked = false;
		this.txtGpDateTo.CustomFormat = "dd-MMM-yy";
		this.txtGpDateTo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpDateTo.Location = new System.Drawing.Point(289, 42);
		this.txtGpDateTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtGpDateTo.Name = "txtGpDateTo";
		this.txtGpDateTo.ShowCheckBox = true;
		this.txtGpDateTo.Size = new System.Drawing.Size(140, 22);
		this.txtGpDateTo.TabIndex = 4;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(204, 74);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(88, 15);
		this.label13.TabIndex = 127;
		this.label13.Text = "Gp No From,To";
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(78, 42);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(121, 22);
		this.GRNToDate.TabIndex = 1;
		this.txtGpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNoTo.Location = new System.Drawing.Point(359, 70);
		this.txtGpNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtGpNoTo.Name = "txtGpNoTo";
		this.txtGpNoTo.ShortcutsEnabled = false;
		this.txtGpNoTo.Size = new System.Drawing.Size(70, 23);
		this.txtGpNoTo.TabIndex = 6;
		this.txtGpNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGpNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpsNoTo_KeyPress);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.labelFormHeading);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 35);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 24);
		this.panel3.TabIndex = 3;
		this.labelFormHeading.AutoSize = true;
		this.labelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.labelFormHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormHeading.ForeColor = System.Drawing.Color.White;
		this.labelFormHeading.Location = new System.Drawing.Point(3, 2);
		this.labelFormHeading.Name = "labelFormHeading";
		this.labelFormHeading.Size = new System.Drawing.Size(318, 19);
		this.labelFormHeading.TabIndex = 4;
		this.labelFormHeading.Text = "Pending Freight Voucher For Approval";
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(860, 3);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(84, 20);
		this.ChkPrint.TabIndex = 13;
		this.ChkPrint.Text = "Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 32);
		this.panel2.TabIndex = 1;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnApproved, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(950, 27);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(85, 24);
		this.btnApproved.Text = "Approve";
		this.btnApproved.ToolTipText = "ShortCutKeys";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "PendingFreightVoucherForApproval";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
