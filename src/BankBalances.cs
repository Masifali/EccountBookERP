// // using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using AggregateFunction = Janus.Windows.GridEX.AggregateFunction;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Account_Reports
{

public class BankBalances : Form
{
	private DataTable dtSummery = new DataTable();

	private DataTable dtReciptandPayment = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public DateTime LastRecordDate;

	public static int GroupId;

	public int CompanyIdFromBreakUp;

	private bool AllowEditOnVoucherNoinReports;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public string BranchesIds = "";

	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrintReceiptandPayment;

	internal GridEX grdPayment;

	internal GridEX grdReceipt;

	internal GridEX grdHistory;

	private ToolStripButton btnPrintSummery;

	private Label label5;

	private CtrlGrdBar ctrlGrdSummary;

	private ToolStripButton btnBankManualBalance;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdPayment;

	private Label label4;

	private CtrlGrdBar ctrlGrdReceipt;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label label33;

	private Label label27;

	internal UltraCombo cmbperemeter;

	private Label label12;

	private Label label1;

	private ToolStripButton BtnRefresh;

	private RadioButton RadGroupbyParty;

	private RadioButton RadGroupbyBankName;

	private RadioButton RadNoGroup;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel7;

	private Panel panel8;

	private Panel panel6;

	private Panel panel9;

	private Label label2;

	internal UltraCombo CmbBranches;

	private Label Branches;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	private Label label3;

	private TextBox textBox2;

	private Label label6;

	private TextBox textBox1;

	private ToolStripButton BtnPrintII;

	private ToolStripButton BtnPrint590II;

	private CheckBox chkFilterSummaryOnlyTransactional;

	public BankBalances(UserAccount userAccount)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void BankBalances_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			ctrlGrdReceipt.mGridChooseFielder.Visible = false;
			ctrlGrdReceipt.mGridSaveLayouts.Visible = false;
			ctrlGrdPayment.mGridChooseFielder.Visible = false;
			ctrlGrdPayment.mGridSaveLayouts.Visible = false;
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
			}
			else
			{
				BranchesFill();
				if (BranchesIds != null && BranchesIds != string.Empty)
				{
					((Control)(object)CmbBranches).Text = string.Empty;
					if (BranchFeatureConsolidated)
					{
						string[] valuesToMatch = BranchesIds.Split(',');
						RowEnumerator enumerator = ((UltraGridBase)CmbBranches).Rows.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								UltraGridRow row = enumerator.Current;
								string[] array = valuesToMatch;
								for (int i = 0; i < array.Length; i++)
								{
									if (Conversion.ToInt((object)array[i]) == Conversion.ToInt(row.Cells["Id"].Value))
									{
										row.Cells["Selected"].Value = true;
										break;
									}
								}
							}
						}
						finally
						{
							if (enumerator is IDisposable disposable)
							{
								disposable.Dispose();
							}
						}
					}
					else
					{
						CmbBranches.Value = Conversion.ToInt((object)BranchesIds);
					}
				}
			}
			MultiLanguages();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				txtFromDate.Value = Conversion.ToDateTime((object)FromDate);
				txtToDate.Value = Conversion.ToDateTime((object)ToDate);
				ReciptandPaymentGrid(BranchIds);
			}
			SummeryGrd(BranchIds);
			DataTable dtLastDate = CommonServices.Vouchers_GetLastRecord(15);
			if (dtLastDate != null && dtLastDate.Rows.Count > 0)
			{
				LastRecordDate = Conversion.ToDateTime(dtLastDate.Rows[0]["voucherdate"]);
			}
			ParameterFill();
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

	private void ParameterFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			dt.Rows.Add(6, "Last Record Date");
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeter).Rows[5].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiLanguages()
	{
		try
		{
			DataTable dt = CommonServices.MultiLanguagesGetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLanguage, "Id", "LanguageDescription", "Language Name", false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBranches, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				if (BranchFeature && BranchFeatureConsolidated)
				{
					UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj).Key = "Selected";
					((HeaderBase)obj.Header).Caption = string.Empty;
					obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj.DataType = typeof(bool);
					((HeaderBase)obj.Header).VisiblePosition = 0;
					CmbBranches.LimitToList = false;
					CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
					((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				((Control)(object)CmbBranches).Text = UserAccount.BranchName;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				txtFromDate.Value = DateTime.Today;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtFromDate.Value = DateTime.Today.AddDays(-7.0);
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDate.Value = first;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDate.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 6)
			{
				txtFromDate.Value = LastRecordDate;
				txtToDate.Value = DateTime.Today;
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
			wf.Show();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			ReciptandPaymentGrid(BranchIds);
			SummeryGrd(BranchIds);
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

	private void ReciptandPaymentGrid(string BranchIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		try
		{
			ReportsParameters reportParams = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				LanguageId = Conversion.ToInt(CmbLanguage.Value),
				BranchesIds = ((BranchIds != "") ? BranchIds : "")
			};
			dtReciptandPayment = VoucherReports.BankBalances(reportParams);
			if (dtReciptandPayment != null && dtReciptandPayment.Rows.Count > 0)
			{
				DataTable dtRCol = CreateTableStructure("ReceivedFrom");
				DataTable dtPCol = CreateTableStructure("PaidTo");
				foreach (DataRow row in dtReciptandPayment.Rows)
				{
					string tranType = Conversion.ToString(row["TranType"]);
					object[] newRow = new object[14]
					{
						row["BranchesId"],
						row["BranchName"],
						Conversion.ToDateTime(row["voucherdate"]).ToString("dd-MMM-yyyy"),
						row["Id"],
						row["DocumentTypeId"],
						row["DocumentTypeSrNo"],
						row["DocumentTypeCode"],
						row["Vouchercode"],
						row["AccountTitle"],
						row["AgainstAccountId"],
						(tranType == "Receipts") ? row["OffsetAccountTitle"] : row["OffsetAccountTitle"],
						row["SubsidiaryAccountId"],
						row["DebitAmount"],
						row["ChequeNo"]
					};
					if (tranType == "Receipts")
					{
						dtRCol.Rows.Add(newRow);
					}
					else if (tranType == "Payments")
					{
						dtPCol.Rows.Add(newRow);
					}
				}
				BindAndSetupGrid(grdReceipt, dtRCol, "ReceivedFrom");
				BindAndSetupGrid(grdPayment, dtPCol, "PaidTo");
			}
			else
			{
				grdReceipt.ClearStructure();
				grdPayment.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable CreateTableStructure(string accountColumnName)
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"BranchesId",
					typeof(int)
				},
				{
					"BranchName",
					typeof(string)
				},
				{
					"V.Date",
					typeof(DateTime)
				},
				"Id",
				"DocumentTypeId",
				"DocumentTypeSrNo",
				"V.Type",
				{
					"V.No",
					typeof(int)
				},
				"BankName",
				"AgainstAccountId",
				accountColumnName,
				"SubsidiaryAccountId",
				{
					"Amount",
					typeof(double)
				},
				"ChequeNo"
			}
		};
	}

	private void BindAndSetupGrid(GridEX grid, DataTable dataSource, string accountColumnName)
	{
		grid.DataSource = dataSource;
		grid.RetrieveStructure();
		GridEXTable rootTable = grid.RootTable;
		rootTable.Columns["Id"].Visible = false;
		rootTable.Columns["BranchesId"].Visible = false;
		rootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
		rootTable.Columns["DocumentTypeId"].Visible = false;
		rootTable.Columns["DocumentTypeSrNo"].Visible = false;
		rootTable.Columns["AgainstAccountId"].Visible = false;
		rootTable.Columns["SubsidiaryAccountId"].Visible = false;
		if (RadGroupbyBankName.Checked || RadGroupbyParty.Checked)
		{
			string groupColumn = (RadGroupbyBankName.Checked ? "BankName" : accountColumnName);
			rootTable.Groups.Add(groupColumn);
			rootTable.GroupTotals = (GroupTotals)2;
		}
		rootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		rootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
		rootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
		rootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
		rootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
		rootTable.Columns["V.Date"].Width = 90;
		rootTable.Columns["V.Type"].Width = 68;
		rootTable.Columns["V.No"].Width = 40;
		rootTable.Columns["BankName"].Width = 180;
		rootTable.Columns[accountColumnName].Width = 180;
		rootTable.Columns["Amount"].Width = 100;
		rootTable.Columns["ChequeNo"].Width = 80;
		rootTable.Columns["V.No"].ColumnType = (ColumnType)5;
		rootTable.Columns["V.Type"].ColumnType = (ColumnType)5;
		rootTable.Columns[accountColumnName].ColumnType = (ColumnType)5;
		if (grid == grdPayment)
		{
			ctrlGrdPayment_Load(null, null);
		}
		else
		{
			ctrlGrdReceipt_Load(null, null);
		}
	}

	private void grdReceipt_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdReceipt.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdReceipt.CurrentColumn != null)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
				int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
				int AgainstAccId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(r.Cells["BranchesId"].Value);
				if (e.Column.Key == "V.No")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else if (e.Column.Key == "V.Type")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeSrNo, Id);
				}
				else if (e.Column.Key == "ReceivedFrom")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AgainstAccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPayment_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPayment.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPayment.CurrentColumn != null)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
				int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
				int AgainstAccId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(r.Cells["BranchesId"].Value);
				if (e.Column.Key == "V.No")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else if (e.Column.Key == "V.Type")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeSrNo, Id);
				}
				else if (e.Column.Key == "PaidTo")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AgainstAccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummeryGrd(string BranchIds)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		try
		{
			dtSummery.Clear();
			DataSet ds = VoucherReports.CashandBankBalancesSummery(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CompanyIdCoa = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				AccountTypeId = 15,
				ApprovedFilter = "All",
				BranchesIds = ((BranchIds != "") ? BranchIds : ""),
				LanguageId = Conversion.ToInt(CmbLanguage.Value)
			});
			DataTable dtSummeryFromDb = ds?.Tables[0];
			if (dtSummeryFromDb != null && dtSummeryFromDb.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtSummeryFromDb, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				dtSummery = (chkFilterSummaryOnlyTransactional.Checked ? (from r in dtSummeryFromDb.AsEnumerable()
					where Convert.ToDouble(r.Field<double>("CurrDebit")) > 0.0 || Convert.ToDouble(r.Field<double>("CurrCredit")) > 0.0
					select r).CopyToDataTable() : dtSummeryFromDb);
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("AccountId");
				dtcol.Columns.Add("BranchesId", typeof(int));
				dtcol.Columns.Add("BranchName");
				dtcol.Columns.Add("CustomGroup");
				dtcol.Columns.Add("AccountCode");
				dtcol.Columns.Add("AccountTitle");
				dtcol.Columns.Add("Opening", typeof(double));
				dtcol.Columns.Add("Debit", typeof(double));
				dtcol.Columns.Add("Credit", typeof(double));
				dtcol.Columns.Add("Closing", typeof(double));
				dtcol.Columns.Add("PendingPrematureReceipts", typeof(double));
				dtcol.Columns.Add("PendingPrematurePayments", typeof(double));
				dtcol.Columns.Add("BalanceAfterPrematureReceiptsClearance", typeof(double));
				dtcol.Columns.Add("PostDateCheqsReceipts", typeof(double));
				dtcol.Columns.Add("PostDateCheqsPayments", typeof(double));
				dtcol.Columns.Add("BalanceAfterCheqClearance", typeof(double));
				dtcol.Columns.Add("PostDatedCheqAmount", typeof(double));
				dtcol.Columns.Add("BalanceTime");
				dtcol.Columns.Add("ManualBalance", typeof(double));
				dtcol.Columns.Add("Source");
				dtcol.Columns.Add("ConfirmBy");
				for (int i = 0; i < dtSummery.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtSummery.Rows[i]["AccountId"], dtSummery.Rows[i]["BranchesId"], dtSummery.Rows[i]["BranchName"], dtSummery.Rows[i]["CustomGroup"], dtSummery.Rows[i]["AccountCode"], dtSummery.Rows[i]["AccountTitle"], dtSummery.Rows[i]["Opening"], dtSummery.Rows[i]["CurrDebit"], dtSummery.Rows[i]["CurrCredit"], dtSummery.Rows[i]["Closing"], dtSummery.Rows[i]["PendingPrematureReceipts"], dtSummery.Rows[i]["PendingPrematureDebit"], dtSummery.Rows[i]["BalanceAfterPrematureReceiptsClearance"], dtSummery.Rows[i]["PdcReceipts"], dtSummery.Rows[i]["PdcPayments"], dtSummery.Rows[i]["BalanceAfterCheqClearance"], dtSummery.Rows[i]["PostDatedCheqAmount"], dtSummery.Rows[i]["BalanceTime"], dtSummery.Rows[i]["ManualBankBalance"], dtSummery.Rows[i]["SourceBy"], dtSummery.Rows[i]["ConfirmedBy"]);
				}
				grdHistory.DataSource = dtcol;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.RootTable.Columns["BranchesId"].Visible = false;
			grdHistory.RootTable.Columns["AccountId"].Visible = false;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["CustomGroup"].Width = 160;
			grdHistory.RootTable.Columns["AccountCode"].Width = 80;
			grdHistory.RootTable.Columns["AccountTitle"].Width = 250;
			grdHistory.RootTable.Columns["Opening"].Width = 110;
			grdHistory.RootTable.Columns["Debit"].Width = 110;
			grdHistory.RootTable.Columns["Credit"].Width = 110;
			grdHistory.RootTable.Columns["Closing"].Width = 110;
			grdHistory.RootTable.Columns["PendingPrematureReceipts"].Width = 110;
			grdHistory.RootTable.Columns["PendingPrematurePayments"].Width = 110;
			grdHistory.RootTable.Columns["BalanceAfterPrematureReceiptsClearance"].Width = 130;
			grdHistory.RootTable.Columns["PostDateCheqsReceipts"].Width = 110;
			grdHistory.RootTable.Columns["PostDateCheqsPayments"].Width = 110;
			grdHistory.RootTable.Columns["BalanceAfterCheqClearance"].Width = 110;
			grdHistory.RootTable.Columns["PostDatedCheqAmount"].Width = 110;
			grdHistory.RootTable.Columns["BalanceTime"].Width = 65;
			grdHistory.RootTable.Columns["ManualBalance"].Width = 90;
			grdHistory.RootTable.Columns["Source"].Width = 140;
			grdHistory.RootTable.Columns["ConfirmBy"].Width = 140;
			grdHistory.RootTable.Columns["BalanceTime"].FormatString = "hh:mm";
			grdHistory.RootTable.Groups.Add("CustomGroup");
			grdHistory.RootTable.GroupTotals = (GroupTotals)2;
			grdHistory.RootTable.Columns["CustomGroup"].Visible = false;
			grdHistory.RootTable.Columns["Opening"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Opening"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Opening"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Opening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PendingPrematureReceipts"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["PendingPrematureReceipts"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["PendingPrematureReceipts"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PendingPrematureReceipts"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PendingPrematurePayments"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["PendingPrematurePayments"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["PendingPrematurePayments"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PendingPrematurePayments"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["BalanceAfterPrematureReceiptsClearance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BalanceAfterPrematureReceiptsClearance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BalanceAfterPrematureReceiptsClearance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["BalanceAfterPrematureReceiptsClearance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDateCheqsReceipts"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["PostDateCheqsReceipts"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["PostDateCheqsReceipts"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDateCheqsReceipts"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDateCheqsPayments"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["PostDateCheqsPayments"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["PostDateCheqsPayments"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDateCheqsPayments"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["BalanceAfterCheqClearance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BalanceAfterCheqClearance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BalanceAfterCheqClearance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["BalanceAfterCheqClearance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDatedCheqAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["PostDatedCheqAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["PostDatedCheqAmount"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["PostDatedCheqAmount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ManualBalance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ManualBalance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ManualBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["ManualBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			ctrlGrdSummary_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdHistory.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["AccountId"].Value);
				int branchesId = Conversion.ToInt(r.Cells["BranchesId"].Value);
				if (e.Column.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, txtFromDate.Value, txtToDate.Value, 0, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtToDate.Value = DateTime.Now;
			txtFromDate.Focus();
			grdPayment.DataSource = null;
			grdReceipt.DataSource = null;
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			SummeryGrd(BranchIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankBalances_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtLastDate = CommonServices.Vouchers_GetLastRecord(15);
			if (dtLastDate != null && dtLastDate.Rows.Count > 0)
			{
				LastRecordDate = Conversion.ToDateTime(dtLastDate.Rows[0]["voucherdate"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintReceiptandPayment_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReciptandPayment == null || dtReciptandPayment.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtReciptandPayment, "590-BankBalances.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSummery_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummery.Rows.Count > 0 && dtSummery != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummery, "593-CashBankBalancesSummery_Rpt.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdReceipt_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdReceipt).Name);
			if (fs != null)
			{
				grdReceipt.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdPayment_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPayment).Name);
			if (fs != null)
			{
				grdPayment.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdSummary_Load(object sender, EventArgs e)
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

	private void btnBankManualBalance_Click(object sender, EventArgs e)
	{
		try
		{
			new BankBalanceManualEntry(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankBalances_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Alt)
			{
				switch (e.KeyCode)
				{
				case Keys.D1:
					btnPrintSummery_Click(null, null);
					break;
				case Keys.NumPad1:
					btnPrintReceiptandPayment_Click(null, null);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				lblLanguage.Visible = true;
				((Control)(object)CmbLanguage).Visible = true;
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			dt.Rows.Add("Ctrl+S", "For Shoe Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 334-Register");
			dt.Rows.Add("Alt+2", "For Print 335-Register");
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdHistory.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdHistory.CurrentColumn.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["AccountId"].Value), BranchId: Conversion.ToInt(r.Cells["BranchesId"].Value), FromDate: txtFromDate.Value, ToDate: txtToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdReceipt_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdReceipt.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdReceipt.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
			int AgainstAccId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(r.Cells["BranchesId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdReceipt.CurrentColumn.Key == "V.No")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else if (grdReceipt.CurrentColumn.Key == "V.Type")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeSrNo, Id);
				}
				else if (grdReceipt.CurrentColumn.Key == "RecivedFrom")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AgainstAccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPayment_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPayment.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPayment.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
			int AgainstAccId = Conversion.ToInt(r.Cells["AgainstAccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(r.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(r.Cells["BranchesId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPayment.CurrentColumn.Key == "V.No")
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else if (grdPayment.CurrentColumn.Key == "V.Type")
				{
					CommonServices.GetSlipsByDocumentTypeId(DocumentTypeId, DocumentTypeSrNo, Id);
				}
				else if (grdPayment.CurrentColumn.Key == "PaidTo")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AgainstAccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadGroupbyBankName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GroupBySettingOfReceiptPaymentGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupBySettingOfReceiptPaymentGrid()
	{
		try
		{
			if (grdReceipt.GetRows().Count() > 0)
			{
				((JanusCollectionBase)grdReceipt.RootTable.Groups).Clear();
				if (RadGroupbyBankName.Checked)
				{
					grdReceipt.RootTable.Groups.Add("BankName");
					grdReceipt.GroupTotals = (GroupTotals)2;
				}
				else if (RadGroupbyParty.Checked)
				{
					grdReceipt.RootTable.Groups.Add("ReceivedFrom");
					grdReceipt.GroupTotals = (GroupTotals)2;
				}
			}
			if (grdPayment.GetRows().Count() > 0)
			{
				((JanusCollectionBase)grdPayment.RootTable.Groups).Clear();
				if (RadGroupbyBankName.Checked)
				{
					grdPayment.RootTable.Groups.Add("BankName");
					grdPayment.GroupTotals = (GroupTotals)2;
				}
				else if (RadGroupbyParty.Checked)
				{
					grdPayment.RootTable.Groups.Add("PaidTo");
					grdPayment.GroupTotals = (GroupTotals)2;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadGroupbyParty_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GroupBySettingOfReceiptPaymentGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadNoGroup_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GroupBySettingOfReceiptPaymentGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummery.Rows.Count > 0 && dtSummery != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummery, "593_01_CashBankBalancesSummeryReport.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint590II_Click(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReciptandPayment == null || dtReciptandPayment.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(dtSummery, "CashBankBalancesSummery.rpt");
			lstSubReports.Add(subReportObject1);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtReciptandPayment, "590_01_BankBalancesWithSummary.rpt", lstSubReports);
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
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5c: Expected O, but got Unknown
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdReceipt_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPayment_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdSummary = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdReceipt = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdReceipt = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdPayment = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdPayment = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.chkFilterSummaryOnlyTransactional = new System.Windows.Forms.CheckBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbLanguage = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.RadNoGroup = new System.Windows.Forms.RadioButton();
		this.RadGroupbyParty = new System.Windows.Forms.RadioButton();
		this.RadGroupbyBankName = new System.Windows.Forms.RadioButton();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSummery = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.btnPrintReceiptandPayment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint590II = new System.Windows.Forms.ToolStripButton();
		this.btnBankManualBalance = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayment).BeginInit();
		this.panel8.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1317, 670);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 108);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1317, 562);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1309, 536);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Summary";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1303, 505);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.ctrlGrdSummary);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1303, 25);
		this.panel4.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(74, 19);
		this.label5.TabIndex = 7;
		this.label5.Text = "Summary";
		this.ctrlGrdSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdSummary.FormName = this;
		this.ctrlGrdSummary.Location = new System.Drawing.Point(1264, 1);
		this.ctrlGrdSummary.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdSummary.MyGrid = this.grdHistory;
		this.ctrlGrdSummary.Name = "ctrlGrdSummary";
		this.ctrlGrdSummary.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdSummary.TabIndex = 542;
		this.ctrlGrdSummary.Load += new System.EventHandler(ctrlGrdSummary_Load);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1309, 539);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Payment/Receipts Detail";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceipt);
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(760, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(546, 533);
		this.panel6.TabIndex = 1;
		this.grdReceipt.AllowEdit = (InheritableBoolean)2;
		grdReceipt_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceipt.DesignTimeLayout = grdReceipt_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceipt.DynamicFiltering = true;
		this.grdReceipt.FilterMode = (FilterMode)2;
		this.grdReceipt.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceipt.GroupByBoxVisible = false;
		this.grdReceipt.GroupTotals = (GroupTotals)2;
		this.grdReceipt.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdReceipt).Name = "grdReceipt";
		((ScrollableBase)this.grdReceipt).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Size = new System.Drawing.Size(546, 508);
		((System.Windows.Forms.Control)(object)this.grdReceipt).TabIndex = 0;
		this.grdReceipt.TotalRow = (InheritableBoolean)1;
		this.grdReceipt.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceipt).VisualStyle = (VisualStyle)4;
		this.grdReceipt.LinkClicked += new ColumnActionEventHandler(grdReceipt_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdReceipt).KeyDown += new System.Windows.Forms.KeyEventHandler(grdReceipt_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label2);
		this.panel9.Controls.Add(this.ctrlGrdReceipt);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(546, 25);
		this.panel9.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(66, 19);
		this.label2.TabIndex = 542;
		this.label2.Text = "Receipts";
		this.ctrlGrdReceipt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdReceipt.FormName = this;
		this.ctrlGrdReceipt.Location = new System.Drawing.Point(506, 2);
		this.ctrlGrdReceipt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdReceipt.MyGrid = this.grdReceipt;
		this.ctrlGrdReceipt.Name = "ctrlGrdReceipt";
		this.ctrlGrdReceipt.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdReceipt.TabIndex = 541;
		this.ctrlGrdReceipt.Load += new System.EventHandler(ctrlGrdReceipt_Load);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPayment);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(757, 533);
		this.panel7.TabIndex = 0;
		this.grdPayment.AllowEdit = (InheritableBoolean)2;
		grdPayment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPayment.DesignTimeLayout = grdPayment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPayment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPayment.DynamicFiltering = true;
		this.grdPayment.FilterMode = (FilterMode)2;
		this.grdPayment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPayment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPayment.GroupByBoxVisible = false;
		this.grdPayment.GroupTotals = (GroupTotals)2;
		this.grdPayment.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdPayment).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdPayment).Name = "grdPayment";
		((ScrollableBase)this.grdPayment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPayment).Size = new System.Drawing.Size(757, 508);
		((System.Windows.Forms.Control)(object)this.grdPayment).TabIndex = 0;
		this.grdPayment.TotalRow = (InheritableBoolean)1;
		this.grdPayment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPayment).VisualStyle = (VisualStyle)4;
		this.grdPayment.LinkClicked += new ColumnActionEventHandler(grdPayment_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPayment).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPayment_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdPayment);
		this.panel8.Controls.Add(this.label4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(757, 25);
		this.panel8.TabIndex = 0;
		this.ctrlGrdPayment.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdPayment.FormName = this;
		this.ctrlGrdPayment.Location = new System.Drawing.Point(717, 2);
		this.ctrlGrdPayment.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdPayment.MyGrid = this.grdPayment;
		this.ctrlGrdPayment.Name = "ctrlGrdPayment";
		this.ctrlGrdPayment.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdPayment.TabIndex = 541;
		this.ctrlGrdPayment.Load += new System.EventHandler(ctrlGrdPayment_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 19);
		this.label4.TabIndex = 5;
		this.label4.Text = "Payment";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.chkFilterSummaryOnlyTransactional);
		this.panel3.Controls.Add(this.textBox2);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.textBox1);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel3.Controls.Add(this.lblLanguage);
		this.panel3.Controls.Add(this.RadNoGroup);
		this.panel3.Controls.Add(this.RadGroupbyParty);
		this.panel3.Controls.Add(this.RadGroupbyBankName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.txtToDate);
		this.panel3.Controls.Add(this.txtFromDate);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1317, 53);
		this.panel3.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(700, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 23);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.chkFilterSummaryOnlyTransactional.AutoSize = true;
		this.chkFilterSummaryOnlyTransactional.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
		this.chkFilterSummaryOnlyTransactional.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkFilterSummaryOnlyTransactional.Location = new System.Drawing.Point(597, 4);
		this.chkFilterSummaryOnlyTransactional.Name = "chkFilterSummaryOnlyTransactional";
		this.chkFilterSummaryOnlyTransactional.Size = new System.Drawing.Size(105, 43);
		this.chkFilterSummaryOnlyTransactional.TabIndex = 599;
		this.chkFilterSummaryOnlyTransactional.Text = "Exclude 0\r\nDebit/Credit\r\nFrom Summary";
		this.chkFilterSummaryOnlyTransactional.UseVisualStyleBackColor = true;
		this.textBox2.Location = new System.Drawing.Point(468, 24);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(125, 20);
		this.textBox2.TabIndex = 598;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(468, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(114, 15);
		this.label6.TabIndex = 597;
		this.label6.Text = "OverBrawn Balances";
		this.textBox1.Location = new System.Drawing.Point(362, 24);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(105, 20);
		this.textBox1.TabIndex = 596;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(362, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(104, 15);
		this.label3.TabIndex = 595;
		this.label3.Text = "Available Balances";
		this.CmbLanguage.AllowNull = (DefaultableBoolean)1;
		this.CmbLanguage.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLanguage.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLanguage.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLanguage.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(915, 21);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.Black;
		this.lblLanguage.Location = new System.Drawing.Point(914, 5);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(59, 15);
		this.lblLanguage.TabIndex = 594;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(217, 28);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 3;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.White;
		this.Branches.Location = new System.Drawing.Point(158, 34);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(58, 15);
		this.Branches.TabIndex = 592;
		this.Branches.Text = "Branches";
		this.RadNoGroup.AutoSize = true;
		this.RadNoGroup.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoGroup.Location = new System.Drawing.Point(759, 33);
		this.RadNoGroup.Name = "RadNoGroup";
		this.RadNoGroup.Size = new System.Drawing.Size(56, 18);
		this.RadNoGroup.TabIndex = 7;
		this.RadNoGroup.Text = "None";
		this.RadNoGroup.UseVisualStyleBackColor = true;
		this.RadNoGroup.CheckedChanged += new System.EventHandler(RadNoGroup_CheckedChanged);
		this.RadGroupbyParty.AutoSize = true;
		this.RadGroupbyParty.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGroupbyParty.Location = new System.Drawing.Point(759, 18);
		this.RadGroupbyParty.Name = "RadGroupbyParty";
		this.RadGroupbyParty.Size = new System.Drawing.Size(136, 18);
		this.RadGroupbyParty.TabIndex = 6;
		this.RadGroupbyParty.Text = "Group By Account";
		this.RadGroupbyParty.UseVisualStyleBackColor = true;
		this.RadGroupbyParty.CheckedChanged += new System.EventHandler(RadGroupbyParty_CheckedChanged);
		this.RadGroupbyBankName.AutoSize = true;
		this.RadGroupbyBankName.Checked = true;
		this.RadGroupbyBankName.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGroupbyBankName.Location = new System.Drawing.Point(759, 1);
		this.RadGroupbyBankName.Name = "RadGroupbyBankName";
		this.RadGroupbyBankName.Size = new System.Drawing.Size(152, 18);
		this.RadGroupbyBankName.TabIndex = 5;
		this.RadGroupbyBankName.TabStop = true;
		this.RadGroupbyBankName.Text = "Group By Bank Name";
		this.RadGroupbyBankName.UseVisualStyleBackColor = true;
		this.RadGroupbyBankName.CheckedChanged += new System.EventHandler(RadGroupbyBankName_CheckedChanged);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance26;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(7, 21);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(9, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 590;
		this.label12.Text = "Date Type";
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(246, 23);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(114, 23);
		this.txtToDate.TabIndex = 2;
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(129, 23);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(116, 23);
		this.txtFromDate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(249, 3);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 110;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(132, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 111;
		this.label27.Text = "From Date";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Controls.Add(this.Branches);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1317, 55);
		this.panel2.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(1, 30);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(132, 19);
		this.label1.TabIndex = 5;
		this.label1.Text = "Bank Balances ";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.BtnRefresh, this.btnPrintSummery, this.BtnPrintII, this.btnPrintReceiptandPayment, this.BtnPrint590II, this.btnBankManualBalance });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1317, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.ForeColor = System.Drawing.Color.Black;
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnPrintSummery.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintSummery.ForeColor = System.Drawing.Color.Black;
		this.btnPrintSummery.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSummery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSummery.Name = "btnPrintSummery";
		this.btnPrintSummery.Size = new System.Drawing.Size(158, 24);
		this.btnPrintSummery.Text = "&593_Print Summary";
		this.btnPrintSummery.Click += new System.EventHandler(btnPrintSummery_Click);
		this.BtnPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintII.ForeColor = System.Drawing.Color.Black;
		this.BtnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(182, 24);
		this.BtnPrintII.Text = "&593_01_Print Summary";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.btnPrintReceiptandPayment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintReceiptandPayment.ForeColor = System.Drawing.Color.Black;
		this.btnPrintReceiptandPayment.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintReceiptandPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintReceiptandPayment.Name = "btnPrintReceiptandPayment";
		this.btnPrintReceiptandPayment.Size = new System.Drawing.Size(238, 24);
		this.btnPrintReceiptandPayment.Text = "&590_Print Receipt and Payment";
		this.btnPrintReceiptandPayment.Click += new System.EventHandler(btnPrintReceiptandPayment_Click);
		this.BtnPrint590II.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint590II.ForeColor = System.Drawing.Color.Black;
		this.BtnPrint590II.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint590II.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint590II.Name = "BtnPrint590II";
		this.BtnPrint590II.Size = new System.Drawing.Size(117, 24);
		this.BtnPrint590II.Text = "&590_01_Print";
		this.BtnPrint590II.Click += new System.EventHandler(BtnPrint590II_Click);
		this.btnBankManualBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBankManualBalance.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBankManualBalance.Name = "btnBankManualBalance";
		this.btnBankManualBalance.Size = new System.Drawing.Size(149, 24);
		this.btnBankManualBalance.Text = "Bank Manual Balance";
		this.btnBankManualBalance.Click += new System.EventHandler(btnBankManualBalance_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1317, 670);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "BankBalances";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(BankBalances_FormClosing);
		base.Load += new System.EventHandler(BankBalances_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(BankBalances_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPayment).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
