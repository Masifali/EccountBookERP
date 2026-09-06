using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalVouchersHistory : Form
{
	public static int Id;

	public int DocumentTypeId;

	public bool flagAproved;

	private bool AllowEditOnVoucherNoinReports;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnApproved;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPendingVouchersForView;

	internal GridEX grdPendingViewVoucher;

	private Button btnPendingForView;

	private Panel panel7;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label27;

	private Label label33;

	private Label label3;

	private Label label4;

	private TextBox txtdocnoto;

	private TextBox txtdocnofrom;

	private Button btnShow;

	private CheckBox ChkPrint;

	private TabControl tabControl2;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private Panel panel8;

	private Panel panel2;

	private Panel panel9;

	private TabPage tabPage4;

	private Panel panel5;

	private Panel panel4;

	private Label label1;

	private Panel panel10;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar4;

	public PendingApprovalVouchersHistory(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private DataTable CreateApprovalTable()
	{
		return new DataTable
		{
			Columns = 
			{
				"Id",
				"DocumentTypeId",
				"DocumentTypeCode",
				"DocumentTypeSrNo",
				{
					"VoucherCode",
					typeof(int)
				},
				{
					"VoucherDate",
					typeof(DateTime)
				},
				"RefAccountId",
				"AccountTitle",
				"Remarks",
				{
					"VoucherAmount",
					typeof(double)
				},
				"ChequeNo",
				{
					"ChequeDate",
					typeof(DateTime)
				},
				"PayTitle",
				"CurrencyCode",
				{
					"ExchangeCurrencyRate",
					typeof(double)
				},
				{
					"FcAmount",
					typeof(double)
				},
				"EntryUser",
				{
					"EntryDate",
					typeof(DateTime)
				},
				"IsApproved",
				{
					"NoOfAttachments",
					typeof(int)
				}
			}
		};
	}

	private void PendingApprovalVouchersHistory_Load(object sender, EventArgs e)
	{
		try
		{
			fromdate.Focus();
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowEditOnVoucherNoinReports"));
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			Dictionary<int, int> documentTypeMap = new Dictionary<int, int>
			{
				{ 0, 1 },
				{ 1, 2 },
				{ 2, 3 },
				{ 3, 4 },
				{ 4, 6 },
				{ 5, 7 },
				{ 6, 10 },
				{ 7, 26 },
				{ 8, 5 },
				{ 9, 27 },
				{ 18, 203 },
				{ 34, 34 },
				{ 35, 35 }
			};
			DocumentTypeId = (documentTypeMap.TryGetValue(Id, out var docTypeId) ? docTypeId : 0);
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				fromdate.Value = Convert.ToDateTime(RequestedFromDate);
				todate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			if (flagAproved)
			{
				btnPendingForView.Visible = false;
				btnApproved.Text = "UnApprove";
				label23.Text = "Vouchers For UnApproval";
				tabControl1.TabPages.Remove(tabPendingVouchersForView);
				PendingVoucherForUnApprovalDashboard();
			}
			else
			{
				btnPendingForView.Visible = true;
				btnApproved.Text = "Approve";
				label23.Text = "Vouchers For Approval";
				PendingForViewVouchers();
				HistoryFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.VoucherForApprovalDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				IsApproved = flagAproved,
				ApprovedFilter = "All",
				PendingForViewFilter = "All",
				FromDate = Conversion.ToDateTime((object)fromdate.Value),
				ToDate = Conversion.ToDateTime((object)todate.Value),
				FromDocNo = Conversion.ToInt((object)txtdocnofrom.Text),
				ToDocNo = Conversion.ToInt((object)txtdocnoto.Text)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtApproval = CreateApprovalTable();
				dtApproval.BeginLoadData();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtApproval.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocumentTypeCode"], dt.Rows[i]["DocumentTypeSrNo"], dt.Rows[i]["Vouchercode"], dt.Rows[i]["VoucherDate"], dt.Rows[i]["RefAccountId"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["Remarks"], dt.Rows[i]["VoucherAmount"], dt.Rows[i]["ChequeNo"], dt.Rows[i]["ChequeDate"], dt.Rows[i]["PayTitle"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["ExchangeCurrencyRate"], dt.Rows[i]["FcAmount"], dt.Rows[i]["UserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["IsApproved"], dt.Rows[i]["NoOfAttachments"]);
				}
				dtApproval.EndLoadData();
				grd.DataSource = dtApproval;
				grd.RetrieveStructure();
				Gridsetting();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PendingVoucherForUnApprovalDashboard()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.PendingVoucherForUnApprovalDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = flagAproved,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				FromDate = Conversion.ToDateTime((object)fromdate.Value),
				ToDate = Conversion.ToDateTime((object)todate.Value),
				FromDocNo = Conversion.ToInt((object)txtdocnofrom.Text),
				ToDocNo = Conversion.ToInt((object)txtdocnoto.Text)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtUnApproval = CreateApprovalTable();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUnApproval.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocumentTypeCode"], dt.Rows[i]["DocumentTypeSrNo"], dt.Rows[i]["Vouchercode"], dt.Rows[i]["VoucherDate"], dt.Rows[i]["RefAccountId"], dt.Rows[i]["AccountTitle"], dt.Rows[i]["Remarks"], dt.Rows[i]["VoucherAmount"], dt.Rows[i]["ChequeNo"], dt.Rows[i]["ChequeDate"], dt.Rows[i]["PayTitle"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["ExchangeCurrencyRate"], dt.Rows[i]["FcAmount"], dt.Rows[i]["UserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["IsApproved"], dt.Rows[i]["NoOfAttachments"]);
				}
				grd.DataSource = dtUnApproval;
				grd.RetrieveStructure();
				Gridsetting();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingForViewVouchers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable lst = VoucherHead.PendingVoucherReconcilationForApprovalDashboard(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = flagAproved,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dtPenView = new DataTable();
				dtPenView.Columns.Add("Id");
				dtPenView.Columns.Add("DocumentTypeId");
				dtPenView.Columns.Add("DocumentTypeCode");
				dtPenView.Columns.Add("DocumentTypeSrNo");
				dtPenView.Columns.Add("VoucherCode", typeof(int));
				dtPenView.Columns.Add("VoucherDate", typeof(DateTime));
				dtPenView.Columns.Add("RefAccountId");
				dtPenView.Columns.Add("AccountTitle");
				dtPenView.Columns.Add("Remarks");
				dtPenView.Columns.Add("VoucherAmount", typeof(double));
				dtPenView.Columns.Add("ChequeNo");
				dtPenView.Columns.Add("ChequeDate", typeof(DateTime));
				dtPenView.Columns.Add("PayTitle");
				dtPenView.Columns.Add("CurrencyCode");
				dtPenView.Columns.Add("ExchangeCurrencyRate", typeof(double));
				dtPenView.Columns.Add("FcAmount", typeof(double));
				dtPenView.Columns.Add("EntryUser");
				dtPenView.Columns.Add("EntryDate", typeof(DateTime));
				dtPenView.Columns.Add("IsApproved");
				dtPenView.Columns.Add("NoOfAttachments", typeof(int));
				dtPenView.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtPenView.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], lst.Rows[i]["DocumentTypeSrNo"], lst.Rows[i]["Vouchercode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yy"), lst.Rows[i]["RefAccountId"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["ChequeDate"], lst.Rows[i]["PayTitle"], lst.Rows[i]["CurrencyCode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["EntryDate"], lst.Rows[i]["IsApproved"], lst.Rows[i]["NoOfAttachments"]);
				}
				grdPendingViewVoucher.DataSource = dtPenView;
				grdPendingViewVoucher.RetrieveStructure();
				GridPendingViewsetting();
			}
			else
			{
				grdPendingViewVoucher.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridsetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["IsApproved"].Visible = false;
			grd.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grd.RootTable.Columns["RefAccountId"].Visible = false;
			grd.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocumentTypeCode"].Caption = "V Type";
			grd.RootTable.Columns["VoucherCode"].Caption = "V Code";
			grd.RootTable.Columns["VoucherDate"].Caption = "V Date";
			grd.RootTable.Columns["ExchangeCurrencyRate"].Caption = "Exchange Rate";
			grd.RootTable.Columns["DocumentTypeCode"].Width = 60;
			grd.RootTable.Columns["VoucherCode"].Width = 50;
			grd.RootTable.Columns["VoucherDate"].Width = 90;
			grd.RootTable.Columns["AccountTitle"].Width = 250;
			grd.RootTable.Columns["Remarks"].Width = 300;
			grd.RootTable.Columns["VoucherAmount"].Width = 130;
			grd.RootTable.Columns["ChequeNo"].Width = 75;
			grd.RootTable.Columns["ChequeDate"].Width = 80;
			grd.RootTable.Columns["PayTitle"].Width = 120;
			grd.RootTable.Columns["CurrencyCode"].Width = 75;
			grd.RootTable.Columns["ExchangeCurrencyRate"].Width = 70;
			grd.RootTable.Columns["FcAmount"].Width = 100;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["NoOfAttachments"].Width = 85;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VoucherAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["VoucherAmount"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ExchangeCurrencyRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeCurrencyRate"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcAmount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcAmount"].TotalFormatString = "#,##0.###";
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["ChequeNo"].Visible = DocumentTypeId != 203;
			grd.RootTable.Columns["ChequeDate"].Visible = DocumentTypeId != 203;
			grd.RootTable.Columns["PayTitle"].Visible = DocumentTypeId != 203;
			grd.RootTable.Columns["CurrencyCode"].Visible = DocumentTypeId == 203;
			grd.RootTable.Columns["ExchangeCurrencyRate"].Visible = DocumentTypeId == 203;
			grd.RootTable.Columns["FcAmount"].Visible = DocumentTypeId == 203;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 25;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 40;
			grd.RootTable.Columns["Print"].Position = 1;
			if (flagAproved)
			{
				grd.RootTable.Columns.Add("UnPost");
				grd.RootTable.Columns["UnPost"].Caption = "UnApprove";
				grd.RootTable.Columns["UnPost"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["UnPost"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["UnPost"].ButtonText = "UnApprove";
				grd.RootTable.Columns["UnPost"].Width = 60;
				grd.RootTable.Columns["UnPost"].Position = 2;
			}
			else
			{
				grd.RootTable.Columns.Add("Post");
				grd.RootTable.Columns["Post"].Caption = "Approve";
				grd.RootTable.Columns["Post"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Post"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Post"].ButtonText = "Approve";
				grd.RootTable.Columns["Post"].Width = 60;
				grd.RootTable.Columns["Post"].Position = 2;
			}
			grd.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingViewsetting()
	{
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPendingViewVoucher.RootTable.Columns["Id"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["IsApproved"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["RefAccountId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["ChequeNo"].Visible = DocumentTypeId != 203;
			grdPendingViewVoucher.RootTable.Columns["ChequeDate"].Visible = DocumentTypeId != 203;
			grdPendingViewVoucher.RootTable.Columns["PayTitle"].Visible = DocumentTypeId != 203;
			grdPendingViewVoucher.RootTable.Columns["CurrencyCode"].Visible = DocumentTypeId == 203;
			grdPendingViewVoucher.RootTable.Columns["ExchangeCurrencyRate"].Visible = DocumentTypeId == 203;
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].Visible = DocumentTypeId == 203;
			grdPendingViewVoucher.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPendingViewVoucher.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeCode"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns["VoucherCode"].Width = 50;
			grdPendingViewVoucher.RootTable.Columns["VoucherDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdPendingViewVoucher.RootTable.Columns["AccountTitle"].Width = 250;
			grdPendingViewVoucher.RootTable.Columns["Remarks"].Width = 300;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].Width = 130;
			grdPendingViewVoucher.RootTable.Columns["ChequeNo"].Width = 75;
			grdPendingViewVoucher.RootTable.Columns["ChequeDate"].Width = 80;
			grdPendingViewVoucher.RootTable.Columns["PayTitle"].Width = 120;
			grdPendingViewVoucher.RootTable.Columns["CurrencyCode"].Width = 75;
			grdPendingViewVoucher.RootTable.Columns["ExchangeCurrencyRate"].Width = 70;
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].Width = 100;
			grdPendingViewVoucher.RootTable.Columns["EntryUser"].Width = 110;
			grdPendingViewVoucher.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].Width = 85;
			grdPendingViewVoucher.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdPendingViewVoucher.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].FormatString = "#,##0.###";
			grdPendingViewVoucher.RootTable.Columns["VoucherAmount"].TotalFormatString = "#,##0.###";
			grdPendingViewVoucher.RootTable.Columns["ExchangeCurrencyRate"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["ExchangeCurrencyRate"].FormatString = "#,##0.###";
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].FormatString = "#,##0.###";
			grdPendingViewVoucher.RootTable.Columns["FcAmount"].TotalFormatString = "#,##0.###";
			if (AllowEditOnVoucherNoinReports)
			{
				grdPendingViewVoucher.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			}
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["VoucherCode"].Caption = "V Code";
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeCode"].Caption = "V Type";
			grdPendingViewVoucher.RootTable.Columns["VoucherDate"].Caption = "V Date";
			grdPendingViewVoucher.RootTable.Columns.Add("Print");
			grdPendingViewVoucher.RootTable.Columns["Print"].Caption = "Print";
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Print"].ButtonText = "Print";
			grdPendingViewVoucher.RootTable.Columns["Print"].Width = 40;
			grdPendingViewVoucher.RootTable.Columns["Print"].Position = 0;
			grdPendingViewVoucher.RootTable.Columns.Add("Post");
			grdPendingViewVoucher.RootTable.Columns["Post"].Caption = "Approve";
			grdPendingViewVoucher.RootTable.Columns["Post"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Post"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Post"].ButtonText = "Approve";
			grdPendingViewVoucher.RootTable.Columns["Post"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns["Post"].Position = 1;
			grdPendingViewVoucher.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailsetting()
	{
		try
		{
			grdDetail.RootTable.Columns["AccountId"].Visible = false;
			grdDetail.RootTable.Columns["AgainstAccountId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
			grdDetail.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 50;
			grdDetail.RootTable.Columns["OffsetAccount"].Width = Constants.InventoryConstants.AccountTitle + 50;
			grdDetail.RootTable.Columns["Comments"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			grdDetail.RootTable.Columns["CreditAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["DebitAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["OffsetAccount"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CreditAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CreditAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DebitAmount"].FormatString = "#,#";
			grdDetail.RootTable.Columns["CreditAmount"].FormatString = "#,#";
			grdDetail.RootTable.Columns["DebitAmount"].TotalFormatString = "#,#";
			grdDetail.RootTable.Columns["CreditAmount"].TotalFormatString = "#,#";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grd.CurrentRow.Cells[0].Value);
			int documentTypeId = Conversion.ToInt(grd.CurrentRow.Cells[1].Value);
			if (e.Column.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
			if (flagAproved)
			{
				if (e.Column.Key == "UnPost")
				{
					UnApproveVoucher(Id);
				}
			}
			else if (e.Column.Key == "Post")
			{
				ApproveVoucher(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				int documentTypeId = Conversion.ToInt((object)item.Cells["DocumentTypeId"].Value.ToString());
				int DocumentSrNo = Conversion.ToInt((object)item.Cells["DocumentTypeSrNo"].Value.ToString());
				if (e.Column.Key == "VoucherCode")
				{
					CommonServices.EditMethodFromLinked(documentTypeId, Id, DocumentSrNo);
				}
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID((documentTypeId == 203) ? DocumentSrNo : Id, documentTypeId);
				}
				if (e.Column.Key == "AccountTitle")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt((object)item.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveVoucher(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = Id;
				detail.PostDate = DateTime.Now;
				detail.EntryUser = UserAccount.ID;
				detail.ActionTypeId = false;
				detail.ReqType = "AP";
				val.AllApprovalLists.Add(detail);
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(val);
				MessageBox.Show("Record Approved Successfully");
				if (ChkPrint.Checked)
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Conversion.ToInt((object)detail.Id));
				}
				HistoryFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnApproveVoucher(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to UnApprove Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = Id;
				detail.PostDate = DateTime.Now;
				detail.EntryUser = UserAccount.ID;
				detail.ActionTypeId = false;
				detail.ReqType = "UP";
				val.AllApprovalLists.Add(detail);
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(val);
				PendingVoucherForUnApprovalDashboard();
				MessageBox.Show("Record UnApproved Successfully");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailByVoucherHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.CurrentRow != null && (int)grd.CurrentRow.RowType == 0 && grd.CurrentRow.Cells.Count > 0)
			{
				DataTable dt = VoucherHead.GetDetailByVoucherHeadId(Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString()));
				if (dt.Rows.Count > 0)
				{
					grdDetail.DataSource = dt;
					grdDetail.RetrieveStructure();
					GridDetailsetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingApprovalVouchersHistory_FormClosed(object sender, FormClosedEventArgs e)
	{
		Id = 0;
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.CurrentRow != null && grd.CurrentRow.Cells.Count > 0)
			{
				GridDetailByVoucherHeadId();
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (flagAproved)
			{
				if (MessageBox.Show("Are you sure to UnApprove Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow item in checkedRows)
					{
						ApprovalList detail = new ApprovalList();
						detail.OrganizationId = UserAccount.OrganizationId;
						detail.CompanyId = UserAccount.CompanyId;
						detail.Id = Conversion.ToInt(item.Cells[0].Value);
						detail.PostDate = DateTime.Now;
						detail.EntryUser = UserAccount.ID;
						detail.ActionTypeId = false;
						detail.ReqType = "UP";
						obj.AllApprovalLists.Add(detail);
					}
					VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj);
					MessageBox.Show("Record UnApproved Successfully");
					PendingVoucherForUnApprovalDashboard();
				}
			}
			else if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj2 = new ApprovalList();
				obj2.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow item2 in checkedRows)
				{
					ApprovalList detail2 = new ApprovalList();
					detail2.OrganizationId = UserAccount.OrganizationId;
					detail2.CompanyId = UserAccount.CompanyId;
					detail2.Id = Conversion.ToInt(item2.Cells[0].Value);
					detail2.PostDate = DateTime.Now;
					detail2.EntryUser = UserAccount.ID;
					detail2.ActionTypeId = false;
					detail2.ReqType = "AP";
					obj2.AllApprovalLists.Add(detail2);
				}
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj2);
				MessageBox.Show("Record Approved Successfully");
				HistoryFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			if (flagAproved)
			{
				PendingVoucherForUnApprovalDashboard();
				return;
			}
			PendingForViewVouchers();
			HistoryFill();
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
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "OffsetAccount")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["AgainstAccountId"].Value.ToString()), fromdate.Value, todate.Value);
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
			int Id = Conversion.ToInt(grdPendingViewVoucher.CurrentRow.Cells["Id"].Value);
			int documentTypeId = Conversion.ToInt(grdPendingViewVoucher.CurrentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
			else if (e.Column.Key == "Post")
			{
				ApproveVoucherForPendingGrid(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print(int id, int documentTypeId)
	{
		try
		{
			CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveVoucherForPendingGrid(int id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Voucher?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = id;
				detail.PostDate = DateTime.Now;
				detail.EntryUser = UserAccount.ID;
				detail.ActionTypeId = false;
				detail.ReqType = "AP";
				val.AllApprovalLists.Add(detail);
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(val);
				PendingForViewVouchers();
				MessageBox.Show("Record Approved Successfully");
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
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Move Voucher in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					ApprovalList detail = new ApprovalList();
					detail.OrganizationId = UserAccount.OrganizationId;
					detail.CompanyId = UserAccount.CompanyId;
					detail.Id = Conversion.ToInt(item.Cells[0].Value);
					detail.PostDate = DateTime.Now;
					detail.EntryUser = UserAccount.ID;
					detail.ActionTypeId = true;
					detail.ReqType = "AP";
					obj.AllApprovalLists.Add(detail);
				}
				VoucherHead.UpdateStatusandIsapprovedByVoucherId(obj);
				MessageBox.Show("Record Move on PendingGrid Successfully");
				HistoryFill();
				PendingForViewVouchers();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (e.Column.Key == "VoucherCode")
			{
				int Id = int.Parse(item.Cells["Id"].Value.ToString());
				CommonServices.EditMethodFromLinked(int.Parse(item.Cells["DocumentTypeId"].Value.ToString()), Id);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), Conversion.ToInt(item.Cells["DocumentTypeId"].Value));
			}
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(item.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl2.SelectedIndex == 1)
		{
			VoucherModernHistory frm = new VoucherModernHistory(UserAccount);
			frm.ApproveOrUnApprove = flagAproved;
			frm.DocumentTypeId = DocumentTypeId;
			frm.RequestFromScreen = "ApproveOrUnApprove Voucher";
			frm.TopLevel = false;
			frm.Visible = true;
			frm.FormBorderStyle = FormBorderStyle.None;
			frm.Dock = DockStyle.Fill;
			tabControl2.TabPages["tabPage3"].Controls.Add(frm);
		}
		else if (tabControl2.SelectedIndex == 2)
		{
			VoucherModernHistory form = new VoucherModernHistory(UserAccount);
			form.ApproveOrUnApprove = flagAproved;
			form.DocumentTypeId = DocumentTypeId;
			form.RequestFromScreen = "Pending For Review";
			form.TopLevel = false;
			form.Visible = true;
			form.FormBorderStyle = FormBorderStyle.None;
			form.Dock = DockStyle.Fill;
			tabControl2.TabPages["tabPage4"].Controls.Add(form);
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
			int Id = int.Parse(r.Cells["Id"].Value.ToString());
			int documentTypeId = int.Parse(r.Cells["DocumentTypeId"].Value.ToString());
			if (grd.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(documentTypeId, Id);
				}
			}
			else if (grd.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, documentTypeId);
			}
			else if (grd.CurrentColumn.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "Print")
			{
				Print(Id, documentTypeId);
			}
			else if (grd.CurrentColumn.Key == "UnPost")
			{
				UnApproveVoucher(Id);
			}
			else if (grd.CurrentColumn.Key == "Post")
			{
				ApproveVoucher(Id);
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
				if (grdDetail.CurrentColumn.Key == "AccountTitle")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["AccountId"].Value.ToString()), fromdate.Value, todate.Value);
				}
				else if (grdDetail.CurrentColumn.Key == "OffsetAccount")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["AgainstAccountId"].Value.ToString()), fromdate.Value, todate.Value);
				}
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
			int Id = int.Parse(r.Cells["Id"].Value.ToString());
			int DocTypeId = int.Parse(r.Cells["DocumentTypeId"].Value.ToString());
			if (grdPendingViewVoucher.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocTypeId, Id);
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(int.Parse(r.Cells["RefAccountId"].Value.ToString()), fromdate.Value, todate.Value);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Print")
			{
				Print(Id, DocTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Post")
			{
				ApproveVoucherForPendingGrid(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void PendingApprovalVouchersHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
				{
					((Control)(object)grdDetail).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				tabControl1.SelectedIndex = 0;
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
				}
				else if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 0;
				}
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPendingForView_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db0: Expected O, but got Unknown
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Expected O, but got Unknown
		//IL_1017: Unknown result type (might be due to invalid IL or missing references)
		//IL_1021: Expected O, but got Unknown
		//IL_1541: Unknown result type (might be due to invalid IL or missing references)
		//IL_154b: Expected O, but got Unknown
		//IL_1558: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.btnApproved = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.txtdocnoto = new System.Windows.Forms.TextBox();
		this.txtdocnofrom = new System.Windows.Forms.TextBox();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPendingVouchersForView = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPendingViewVoucher = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabPendingVouchersForView.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel6.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add(this.btnApproved);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.btnPendingForView);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtdocnoto);
		this.panel3.Controls.Add(this.txtdocnofrom);
		this.panel3.Controls.Add(this.todate);
		this.panel3.Controls.Add(this.fromdate);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1061, 96);
		this.panel3.TabIndex = 4;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.Location = new System.Drawing.Point(361, 42);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(95, 19);
		this.ChkPrint.TabIndex = 4;
		this.ChkPrint.Text = "Print Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShow.Location = new System.Drawing.Point(359, 66);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(48, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(411, 66);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(63, 26);
		this.btnApproved.TabIndex = 6;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(192, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "Doc No To";
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(478, 66);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(113, 26);
		this.btnPendingForView.TabIndex = 7;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(192, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 128;
		this.label4.Text = "Doc No From";
		this.txtdocnoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnoto.Location = new System.Drawing.Point(275, 68);
		this.txtdocnoto.Name = "txtdocnoto";
		this.txtdocnoto.Size = new System.Drawing.Size(80, 23);
		this.txtdocnoto.TabIndex = 3;
		this.txtdocnofrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnofrom.Location = new System.Drawing.Point(275, 41);
		this.txtdocnofrom.Name = "txtdocnofrom";
		this.txtdocnofrom.Size = new System.Drawing.Size(80, 23);
		this.txtdocnofrom.TabIndex = 2;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(74, 69);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(108, 21);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(74, 42);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(108, 21);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(8, 45);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 115;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 72);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 114;
		this.label33.Text = "To Date";
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1061, 35);
		this.panel7.TabIndex = 111;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1010, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grd;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1061, 338);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(92, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "Vouchers";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(998, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1047, 274);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPendingVouchersForView);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1061, 338);
		this.tabControl1.TabIndex = 47;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1053, 312);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 35);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1047, 274);
		this.panel5.TabIndex = 2;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.ctrlGrdBar3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1047, 32);
		this.panel4.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 25);
		this.label1.TabIndex = 2;
		this.label1.Text = "Detail";
		this.tabPendingVouchersForView.Controls.Add(this.panel10);
		this.tabPendingVouchersForView.Controls.Add(this.panel6);
		this.tabPendingVouchersForView.ForeColor = System.Drawing.SystemColors.ControlText;
		this.tabPendingVouchersForView.Location = new System.Drawing.Point(4, 22);
		this.tabPendingVouchersForView.Name = "tabPendingVouchersForView";
		this.tabPendingVouchersForView.Padding = new System.Windows.Forms.Padding(3);
		this.tabPendingVouchersForView.Size = new System.Drawing.Size(1053, 312);
		this.tabPendingVouchersForView.TabIndex = 1;
		this.tabPendingVouchersForView.Text = "PendingVoucherForView";
		this.tabPendingVouchersForView.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 32);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1047, 277);
		this.panel10.TabIndex = 49;
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingViewVoucher.DynamicFiltering = true;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		this.grdPendingViewVoucher.NewRowFormatStyle.BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.NewRowFormatStyle.ForeColor = System.Drawing.Color.White;
		((ScrollableBase)this.grdPendingViewVoucher).Office2007CustomColor = System.Drawing.Color.White;
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(1047, 277);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 47;
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1047, 29);
		this.panel6.TabIndex = 48;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(999, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdPendingViewVoucher;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar4.TabIndex = 23;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1075, 804);
		this.panel1.TabIndex = 0;
		this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1075, 804);
		this.tabControl2.TabIndex = 48;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1067, 778);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "Grid View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel2);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.panel3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1061, 772);
		this.panel8.TabIndex = 0;
		this.panel2.Controls.Add(this.tabControl1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 434);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1061, 338);
		this.panel2.TabIndex = 6;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 96);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1061, 338);
		this.panel9.TabIndex = 5;
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1067, 778);
		this.tabPage3.TabIndex = 1;
		this.tabPage3.Text = "Cards View";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage4.Location = new System.Drawing.Point(4, 4);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1067, 778);
		this.tabPage4.TabIndex = 2;
		this.tabPage4.Text = "Pending For Review";
		this.tabPage4.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1075, 804);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PendingApprovalVouchersHistory";
		this.Text = "Approvals";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(PendingApprovalVouchersHistory_FormClosed);
		base.Load += new System.EventHandler(PendingApprovalVouchersHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PendingApprovalVouchersHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tabPendingVouchersForView.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
