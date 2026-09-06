// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
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

public class CashAndBankBalances : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtReciptandPayment = new DataTable();

	private DataTable dtReciptandPaymentClassWise = new DataTable();

	private DataTable dtSummery = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int CompanyIdFromBreakUp;

	private int SetCompanyId;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public string BranchesIds = "";

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Button btnshow;

	private DateTimePicker txtToDate;

	private DateTimePicker txtFromDate;

	private Label label33;

	private Label label27;

	internal GridEX grdPayment;

	internal GridEX grdHistory;

	private Panel panel8;

	private Label label5;

	private CtrlGrdBar ctrlGrdSummary;

	private CtrlGrdBar ctrlGrdPayment;

	private GroupBox groupBox1;

	private Panel panel10;

	private Label label1;

	private Panel panel4;

	private Panel panel5;

	private Panel panel11;

	private Label label3;

	private Label lblTotalBankBalance;

	private Label lblTotalCashBalance;

	private Label label6;

	private Label label4;

	private GroupBox SummaryInfo;

	private Panel panel6;

	private Panel panel14;

	private Panel panel12;

	private Panel panel13;

	internal GridEX grdReceipt;

	private Panel panel7;

	private Label label2;

	private CtrlGrdBar ctrlGrdReceipt;

	private Label label7;

	internal UltraCombo CmbTranType;

	private ToolStripButton btnPrintSummery;

	private ToolStripButton btnPrintReceiptandPayment;

	private ToolStripButton btnShortCutKey;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel9;

	private Panel panel15;

	private Panel panel16;

	internal GridEX grdPaymentClassWise;

	private Panel panel17;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel18;

	private Panel panel19;

	internal GridEX grdReceiptClassWise;

	private Panel panel20;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btn598ReceiptandPaymentAccountClassWise;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public CashAndBankBalances(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void FrmBankBalancesRpt_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 750);
			SetCompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
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
			TranTypeFill();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				txtFromDate.Value = Conversion.ToDateTime((object)FromDate);
				txtToDate.Value = Conversion.ToDateTime((object)ToDate);
				ReciptandPaymentGrid(BranchIds);
				ReciptandPaymentClassWiseGrid(BranchIds);
			}
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

	private void BranchesFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, CompanyIdFromBreakUp, "", 0);
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
			else
			{
				((Control)(object)CmbBranches).Text = string.Empty;
				((UltraGridBase)CmbBranches).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TranTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Value");
			dt.Rows.Add("Payments", "Payments");
			dt.Rows.Add("Receipts", "Receipts");
			DDL.BindDDL(dt, CmbTranType, "Id", "Value", "Tran Type", ZeroIndex: false);
			((UltraGridBase)CmbTranType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDate.Value = DateTime.Now;
			txtToDate.Value = DateTime.Now;
			((Control)(object)CmbTranType).Text = string.Empty;
			txtFromDate.Focus();
			grdHistory.DataSource = null;
			grdPayment.DataSource = null;
			grdReceipt.DataSource = null;
			grdPaymentClassWise.DataSource = null;
			grdReceiptClassWise.DataSource = null;
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			SummeryGrd(BranchIds);
			ReciptandPaymentGrid(BranchIds);
			ReciptandPaymentClassWiseGrid(BranchIds);
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

	private void SummeryGrd(string BranchIds)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		try
		{
			dtSummery.Rows.Clear();
			DataSet ds = VoucherReports.CashandBankBalancesSummery(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = SetCompanyId,
				CompanyIdCoa = SetCompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				ApprovedFilter = "All",
				BranchesIds = ((BranchIds != "") ? BranchIds : "")
			});
			dtSummery = ds?.Tables[0];
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("Type");
			dtcol.Columns.Add("BranchesId", typeof(int));
			dtcol.Columns.Add("BranchName", typeof(string));
			dtcol.Columns.Add("AccountId");
			dtcol.Columns.Add("AccountCode");
			dtcol.Columns.Add("AccountTitle");
			dtcol.Columns.Add("Balance", typeof(double));
			if (dtSummery.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtSummery, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				SummaryInfo.Visible = true;
				for (int i = 0; i < dtSummery.Rows.Count; i++)
				{
					dtcol.Rows.Add((Conversion.ToInt(dtSummery.Rows[i]["AccountTypeId"]) == 2) ? "Cash" : "Bank", dtSummery.Rows[i]["BranchesId"], dtSummery.Rows[i]["BranchName"], dtSummery.Rows[i]["AccountId"], dtSummery.Rows[i]["AccountCode"], dtSummery.Rows[i]["AccountTitle"], dtSummery.Rows[i]["Closing"]);
				}
				grdHistory.DataSource = dtcol;
				grdHistory.RetrieveStructure();
				GridSetting();
				double TotalCashBalance = Conversion.ToDouble(dtSummery.Compute("Sum(Closing)", "AccountTypeId = 2"));
				double TotalBankBalance = Conversion.ToDouble(dtSummery.Compute("Sum(Closing)", "AccountTypeId = 15"));
				lblTotalCashBalance.Text = TotalCashBalance.ToString(clsGlobalVariables.stringFormatboth);
				lblTotalBankBalance.Text = TotalBankBalance.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				SummaryInfo.Visible = false;
				grdHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["BranchesId"].Visible = false;
			grdHistory.RootTable.Columns["AccountId"].Visible = false;
			grdHistory.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdHistory.RootTable.Columns["Type"].Width = 40;
			grdHistory.RootTable.Columns["AccountCode"].Width = 75;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["AccountTitle"].Width = 205;
			grdHistory.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdHistory.RootTable.Columns["Balance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
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
		try
		{
			GridEXRow currentRow = grdHistory.CurrentRow;
			int Id = int.Parse(currentRow.Cells["AccountId"].Value.ToString());
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, txtFromDate.Value, txtToDate.Value, 0, branchesId);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void ReciptandPaymentGrid(string BranchIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		try
		{
			DataSet ds = VoucherReports.PartiesPaymentAndReceipts(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = SetCompanyId,
				FromDate = Conversion.ToDateTime((object)txtFromDate.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				Activity = Conversion.ToString(CmbTranType.Value),
				BranchesIds = ((BranchIds != "") ? BranchIds : "")
			});
			dtReciptandPayment = ds?.Tables[0];
			DataTable dtRCol = new DataTable();
			dtRCol.Columns.Add("SrNo");
			dtRCol.Columns.Add("BranchesId", typeof(int));
			dtRCol.Columns.Add("BranchName");
			dtRCol.Columns.Add("AccountId");
			dtRCol.Columns.Add("SubsidiaryAccountId");
			dtRCol.Columns.Add("AccountCode");
			dtRCol.Columns.Add("AccountTitle");
			dtRCol.Columns.Add("Amount", typeof(double));
			dtRCol.Columns.Add("CurrentBalance", typeof(double));
			DataTable dtPCol = new DataTable();
			dtPCol.Columns.Add("SrNo");
			dtPCol.Columns.Add("BranchesId", typeof(int));
			dtPCol.Columns.Add("BranchName");
			dtPCol.Columns.Add("AccountId");
			dtPCol.Columns.Add("SubsidiaryAccountId");
			dtPCol.Columns.Add("AccountCode");
			dtPCol.Columns.Add("AccountTitle");
			dtPCol.Columns.Add("Amount", typeof(double));
			dtPCol.Columns.Add("CurrentBalance", typeof(double));
			if (dtReciptandPayment.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtReciptandPayment, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				for (int i = 0; i < dtReciptandPayment.Rows.Count; i++)
				{
					if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString((object)"Receipts"))
					{
						dtRCol.Rows.Add(dtReciptandPayment.Rows[i]["RNo"], dtReciptandPayment.Rows[i]["BranchesId"], dtReciptandPayment.Rows[i]["BranchName"], dtReciptandPayment.Rows[i]["AccountId"], dtReciptandPayment.Rows[i]["SubsidiaryAccountId"], dtReciptandPayment.Rows[i]["AccountCode"], dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["Amount"], dtReciptandPayment.Rows[i]["CurrentGLBalance"]);
					}
					else if (Conversion.ToString(dtReciptandPayment.Rows[i]["TranType"]) == Conversion.ToString((object)"Payments"))
					{
						dtPCol.Rows.Add(dtReciptandPayment.Rows[i]["RNo"], dtReciptandPayment.Rows[i]["BranchesId"], dtReciptandPayment.Rows[i]["BranchName"], dtReciptandPayment.Rows[i]["AccountId"], dtReciptandPayment.Rows[i]["SubsidiaryAccountId"], dtReciptandPayment.Rows[i]["AccountCode"], dtReciptandPayment.Rows[i]["AccountTitle"], dtReciptandPayment.Rows[i]["Amount"], dtReciptandPayment.Rows[i]["CurrentGLBalance"]);
					}
				}
				if (dtRCol.Rows.Count > 0)
				{
					grdReceipt.DataSource = dtRCol;
					grdReceipt.RetrieveStructure();
					GridSettingReceipts();
				}
				else
				{
					grdReceipt.DataSource = null;
				}
				if (dtPCol.Rows.Count > 0)
				{
					grdPayment.DataSource = dtPCol;
					grdPayment.RetrieveStructure();
					GridSettingPayment();
				}
				else
				{
					grdPayment.DataSource = null;
				}
			}
			else
			{
				grdPayment.DataSource = null;
				grdReceipt.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingPayment()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPayment.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPayment.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdPayment.RootTable.Columns["AccountId"].Visible = false;
			grdPayment.RootTable.Columns["BranchesId"].Visible = false;
			grdPayment.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdPayment.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdPayment.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPayment.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPayment.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayment.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayment.RootTable.Columns["CurrentBalance"].AggregateFunction = (AggregateFunction)2;
			grdPayment.RootTable.Columns["CurrentBalance"].TextAlignment = (TextAlignment)3;
			grdPayment.RootTable.Columns["CurrentBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPayment.RootTable.Columns["CurrentBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPayment.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdPayment.RootTable.Columns["SrNo"].Width = 40;
			grdPayment.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdPayment.RootTable.Columns["AccountCode"].Width = 75;
			grdPayment.RootTable.Columns["AccountTitle"].Width = 310;
			grdPayment.RootTable.Columns["Amount"].Width = 115;
			grdPayment.RootTable.Columns["CurrentBalance"].Width = 120;
			ctrlGrdPayment_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingReceipts()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdReceipt.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdReceipt.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdReceipt.RootTable.Columns["AccountId"].Visible = false;
			grdReceipt.RootTable.Columns["BranchesId"].Visible = false;
			grdReceipt.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdReceipt.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdReceipt.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdReceipt.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdReceipt.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
			grdReceipt.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdReceipt.RootTable.Columns["CurrentBalance"].AggregateFunction = (AggregateFunction)2;
			grdReceipt.RootTable.Columns["CurrentBalance"].TextAlignment = (TextAlignment)3;
			grdReceipt.RootTable.Columns["CurrentBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdReceipt.RootTable.Columns["CurrentBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdReceipt.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdReceipt.RootTable.Columns["SrNo"].Width = 40;
			grdReceipt.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdReceipt.RootTable.Columns["AccountCode"].Width = 75;
			grdReceipt.RootTable.Columns["AccountTitle"].Width = 310;
			grdReceipt.RootTable.Columns["Amount"].Width = 115;
			grdReceipt.RootTable.Columns["CurrentBalance"].Width = 120;
			ctrlGrdReceipt_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdReceipt_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdReceipt.CurrentRow;
			int AccId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(currentRow.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPayment_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdPayment.CurrentRow;
			int AccId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(currentRow.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReciptandPaymentClassWiseGrid(string BranchIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = SetCompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDate.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.Activity = Conversion.ToString(CmbTranType.Value);
			obj.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			dtReciptandPaymentClassWise = VoucherReports.PaymentAndReceipts_CustomersAndSupplirWise(obj);
			DataTable dtRCol = new DataTable();
			dtRCol.Columns.Add("SrNo");
			dtRCol.Columns.Add("BranchesId", typeof(int));
			dtRCol.Columns.Add("BranchName");
			dtRCol.Columns.Add("AccountId");
			dtRCol.Columns.Add("SubsidiaryAccountId");
			dtRCol.Columns.Add("AccountCode");
			dtRCol.Columns.Add("AccountTitle");
			dtRCol.Columns.Add("Debit/Payments", typeof(double));
			dtRCol.Columns.Add("Credit/Receipts", typeof(double));
			dtRCol.Columns.Add("CurrentBalance", typeof(double));
			DataTable dtPCol = new DataTable();
			dtPCol.Columns.Add("SrNo");
			dtPCol.Columns.Add("BranchesId", typeof(int));
			dtPCol.Columns.Add("BranchName");
			dtPCol.Columns.Add("AccountId");
			dtPCol.Columns.Add("SubsidiaryAccountId");
			dtPCol.Columns.Add("AccountCode");
			dtPCol.Columns.Add("AccountTitle");
			dtPCol.Columns.Add("Debit/Payments", typeof(double));
			dtPCol.Columns.Add("Credit/Receipts", typeof(double));
			dtPCol.Columns.Add("CurrentBalance", typeof(double));
			if (dtReciptandPaymentClassWise.Rows.Count > 0)
			{
				for (int i = 0; i < dtReciptandPaymentClassWise.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtReciptandPaymentClassWise.Rows[i]["ClassId"]) == 3)
					{
						dtRCol.Rows.Add(dtReciptandPaymentClassWise.Rows[i]["RNo"], dtReciptandPaymentClassWise.Rows[i]["BranchesId"], dtReciptandPaymentClassWise.Rows[i]["BranchName"], dtReciptandPaymentClassWise.Rows[i]["AccountId"], dtReciptandPaymentClassWise.Rows[i]["SubsidiaryAccountId"], dtReciptandPaymentClassWise.Rows[i]["AccountCode"], dtReciptandPaymentClassWise.Rows[i]["AccountTitle"], dtReciptandPaymentClassWise.Rows[i]["DebitAmount"], dtReciptandPaymentClassWise.Rows[i]["CreditAmount"], dtReciptandPaymentClassWise.Rows[i]["CurrentGLBalance"]);
					}
					else if (Conversion.ToInt(dtReciptandPaymentClassWise.Rows[i]["ClassId"]) == 2)
					{
						dtPCol.Rows.Add(dtReciptandPaymentClassWise.Rows[i]["RNo"], dtReciptandPaymentClassWise.Rows[i]["BranchesId"], dtReciptandPaymentClassWise.Rows[i]["BranchName"], dtReciptandPaymentClassWise.Rows[i]["AccountId"], dtReciptandPaymentClassWise.Rows[i]["SubsidiaryAccountId"], dtReciptandPaymentClassWise.Rows[i]["AccountCode"], dtReciptandPaymentClassWise.Rows[i]["AccountTitle"], dtReciptandPaymentClassWise.Rows[i]["DebitAmount"], dtReciptandPaymentClassWise.Rows[i]["CreditAmount"], dtReciptandPaymentClassWise.Rows[i]["CurrentGLBalance"]);
					}
				}
				if (dtRCol.Rows.Count > 0)
				{
					grdReceiptClassWise.DataSource = dtRCol;
					grdReceiptClassWise.RetrieveStructure();
					GridSettingReceiptsClassWise();
				}
				else
				{
					grdReceiptClassWise.DataSource = null;
				}
				if (dtPCol.Rows.Count > 0)
				{
					grdPaymentClassWise.DataSource = dtPCol;
					grdPaymentClassWise.RetrieveStructure();
					GridSettingPaymentClassWise();
				}
				else
				{
					grdPaymentClassWise.DataSource = null;
				}
			}
			else
			{
				grdPaymentClassWise.DataSource = null;
				grdReceiptClassWise.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingPaymentClassWise()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPaymentClassWise.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPaymentClassWise.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdPaymentClassWise.RootTable.Columns["AccountId"].Visible = false;
			grdPaymentClassWise.RootTable.Columns["BranchesId"].Visible = false;
			grdPaymentClassWise.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdPaymentClassWise.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdPaymentClassWise.RootTable.Columns["Debit/Payments"].AggregateFunction = (AggregateFunction)2;
			grdPaymentClassWise.RootTable.Columns["Debit/Payments"].TextAlignment = (TextAlignment)3;
			grdPaymentClassWise.RootTable.Columns["Debit/Payments"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["Debit/Payments"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["Credit/Receipts"].AggregateFunction = (AggregateFunction)2;
			grdPaymentClassWise.RootTable.Columns["Credit/Receipts"].TextAlignment = (TextAlignment)3;
			grdPaymentClassWise.RootTable.Columns["Credit/Receipts"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["Credit/Receipts"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["CurrentBalance"].AggregateFunction = (AggregateFunction)2;
			grdPaymentClassWise.RootTable.Columns["CurrentBalance"].TextAlignment = (TextAlignment)3;
			grdPaymentClassWise.RootTable.Columns["CurrentBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["CurrentBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdPaymentClassWise.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdPaymentClassWise.RootTable.Columns["SrNo"].Width = 45;
			grdPaymentClassWise.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdPaymentClassWise.RootTable.Columns["AccountCode"].Width = 75;
			grdPaymentClassWise.RootTable.Columns["AccountTitle"].Width = 310;
			grdPaymentClassWise.RootTable.Columns["Credit/Receipts"].Width = 115;
			grdPaymentClassWise.RootTable.Columns["Debit/Payments"].Width = 115;
			grdPaymentClassWise.RootTable.Columns["CurrentBalance"].Width = 120;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPaymentClassWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdPaymentClassWise.CurrentRow;
			int AccId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(currentRow.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingReceiptsClassWise()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdReceiptClassWise.RootTable.Columns["AccountId"].Visible = false;
			grdReceiptClassWise.RootTable.Columns["BranchesId"].Visible = false;
			grdReceiptClassWise.RootTable.Columns["SubsidiaryAccountId"].Visible = false;
			grdReceiptClassWise.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdReceiptClassWise.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdReceiptClassWise.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdReceiptClassWise.RootTable.Columns["Credit/Receipts"].AggregateFunction = (AggregateFunction)2;
			grdReceiptClassWise.RootTable.Columns["Credit/Receipts"].TextAlignment = (TextAlignment)3;
			grdReceiptClassWise.RootTable.Columns["Credit/Receipts"].FormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["Credit/Receipts"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["Debit/Payments"].AggregateFunction = (AggregateFunction)2;
			grdReceiptClassWise.RootTable.Columns["Debit/Payments"].TextAlignment = (TextAlignment)3;
			grdReceiptClassWise.RootTable.Columns["Debit/Payments"].FormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["Debit/Payments"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["CurrentBalance"].AggregateFunction = (AggregateFunction)2;
			grdReceiptClassWise.RootTable.Columns["CurrentBalance"].TextAlignment = (TextAlignment)3;
			grdReceiptClassWise.RootTable.Columns["CurrentBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["CurrentBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grdReceiptClassWise.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)5;
			grdReceiptClassWise.RootTable.Columns["SrNo"].Width = 45;
			grdReceiptClassWise.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdReceiptClassWise.RootTable.Columns["AccountCode"].Width = 75;
			grdReceiptClassWise.RootTable.Columns["AccountTitle"].Width = 310;
			grdReceiptClassWise.RootTable.Columns["Credit/Receipts"].Width = 115;
			grdReceiptClassWise.RootTable.Columns["Debit/Payments"].Width = 115;
			grdReceiptClassWise.RootTable.Columns["CurrentBalance"].Width = 120;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdReceiptClassWise_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdReceiptClassWise.CurrentRow;
			int AccId = Conversion.ToInt(currentRow.Cells["AccountId"].Value);
			int SubsidiaryAccId = Conversion.ToInt(currentRow.Cells["SubsidiaryAccountId"].Value);
			int branchesId = Conversion.ToInt(currentRow.Cells["BranchesId"].Value);
			if (e.Column.Key == "AccountCode")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
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
		try
		{
			if (dtReciptandPayment == null || dtReciptandPayment.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dtReciptandPayment, "596-PartiesPaymentReceipt.rpt");
			}
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

	private void btn598ReceiptandPaymentAccountClassWise_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtReciptandPaymentClassWise == null || dtReciptandPaymentClassWise.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
			}
			else
			{
				new Reporting().ShowReportWithDataTable(dtReciptandPaymentClassWise, "598-PaymentAndReceipts_CustomersAndSupplirWise.rpt");
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
			wf.Close();
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
			wf.Close();
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
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdReceiptClassWise).Name);
			if (fs != null)
			{
				grdReceiptClassWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPaymentClassWise).Name);
			if (fs != null)
			{
				grdPaymentClassWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void CashAndBankBalances_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtFromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)grdHistory).Focused)
				{
					((Control)(object)grdReceipt).Focus();
				}
				else if (((Control)(object)grdReceipt).Focused)
				{
					((Control)(object)grdPayment).Focus();
				}
				else if (((Control)(object)grdPayment).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
			{
				btnPrintSummery_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				btnPrintReceiptandPayment_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3))
			{
				btn598ReceiptandPaymentAccountClassWise_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (e.Control && e.KeyCode == Keys.Space && item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["AccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grdHistory.CurrentColumn != null && grdHistory.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Id, txtFromDate.Value, txtToDate.Value, 0, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdReceipt_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdReceipt.CurrentRow;
			if (e.Control && e.KeyCode == Keys.Space && item != null && (int)item.RowType == 0)
			{
				int AccId = Conversion.ToInt(item.Cells["AccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grdReceipt.CurrentColumn != null && grdReceipt.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPayment.CurrentRow;
			if (e.Control && e.KeyCode == Keys.Space && item != null && (int)item.RowType == 0)
			{
				int AccId = Conversion.ToInt(item.Cells["AccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grdPayment.CurrentColumn != null && grdPayment.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdReceiptClassWise_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdReceiptClassWise.CurrentRow;
			if (e.Control && e.KeyCode == Keys.Space && item != null && (int)item.RowType == 0)
			{
				int AccId = Conversion.ToInt(item.Cells["AccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grdReceiptClassWise.CurrentColumn != null && grdReceiptClassWise.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentClassWise_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentClassWise.CurrentRow;
			if (e.Control && e.KeyCode == Keys.Space && item != null && (int)item.RowType == 0)
			{
				int AccId = Conversion.ToInt(item.Cells["AccountId"].Value);
				int SubsidiaryAccId = Conversion.ToInt(item.Cells["SubsidiaryAccountId"].Value);
				int branchesId = Conversion.ToInt(item.Cells["BranchesId"].Value);
				if (grdPaymentClassWise.CurrentColumn != null && grdPaymentClassWise.CurrentColumn.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(AccId, txtFromDate.Value, txtToDate.Value, SubsidiaryAccId, branchesId);
				}
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 593");
			dt.Rows.Add("Alt+2", "For Print 596");
			dt.Rows.Add("Ctrl+F5", "For Focus on From Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Toggle Between Grids");
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

	private void btnShortCutKey_Click(object sender, EventArgs e)
	{
		MakeShortCutKeys();
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
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Expected O, but got Unknown
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f1: Expected O, but got Unknown
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bf: Expected O, but got Unknown
		//IL_1928: Unknown result type (might be due to invalid IL or missing references)
		//IL_1932: Expected O, but got Unknown
		GridEXLayout grdPayment_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdReceipt_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentClassWise_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdReceiptClassWise_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdPayment = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.ctrlGrdPayment = new grid.User_Conrtols.CtrlGrdBar();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdReceipt = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdReceipt = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdPaymentClassWise = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdReceiptClassWise = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdSummary = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.SummaryInfo = new System.Windows.Forms.GroupBox();
		this.label6 = new System.Windows.Forms.Label();
		this.lblTotalBankBalance = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.lblTotalCashBalance = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbTranType = new UltraCombo();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel10 = new System.Windows.Forms.Panel();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSummery = new System.Windows.Forms.ToolStripButton();
		this.btnPrintReceiptandPayment = new System.Windows.Forms.ToolStripButton();
		this.btn598ReceiptandPaymentAccountClassWise = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayment).BeginInit();
		this.panel11.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).BeginInit();
		this.panel7.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentClassWise).BeginInit();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReceiptClassWise).BeginInit();
		this.panel20.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel3.SuspendLayout();
		this.SummaryInfo.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTranType).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1158, 700);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(438, 127);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(720, 573);
		this.tabControl1.TabIndex = 6;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(712, 544);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Payment / Receipt";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(706, 538);
		this.panel5.TabIndex = 5;
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 307);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(706, 231);
		this.panel6.TabIndex = 3;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdPayment);
		this.panel14.Controls.Add(this.panel11);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(704, 231);
		this.panel14.TabIndex = 0;
		this.grdPayment.AllowEdit = (InheritableBoolean)2;
		this.grdPayment.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPayment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPayment.DesignTimeLayout = grdPayment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPayment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPayment.DynamicFiltering = true;
		this.grdPayment.FilterMode = (FilterMode)2;
		this.grdPayment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPayment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPayment.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPayment).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPayment).Name = "grdPayment";
		((ScrollableBase)this.grdPayment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPayment).Size = new System.Drawing.Size(704, 205);
		((System.Windows.Forms.Control)(object)this.grdPayment).TabIndex = 3;
		this.grdPayment.TotalRow = (InheritableBoolean)1;
		this.grdPayment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPayment).VisualStyle = (VisualStyle)4;
		this.grdPayment.LinkClicked += new ColumnActionEventHandler(grdPayment_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPayment).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPayment_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label3);
		this.panel11.Controls.Add(this.ctrlGrdPayment);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(704, 26);
		this.panel11.TabIndex = 49;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(138, 19);
		this.label3.TabIndex = 7;
		this.label3.Text = "Party Payments";
		this.ctrlGrdPayment.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdPayment.FormName = this;
		this.ctrlGrdPayment.Location = new System.Drawing.Point(663, 2);
		this.ctrlGrdPayment.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdPayment.MyGrid = this.grdPayment;
		this.ctrlGrdPayment.Name = "ctrlGrdPayment";
		this.ctrlGrdPayment.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdPayment.TabIndex = 541;
		this.ctrlGrdPayment.Load += new System.EventHandler(ctrlGrdPayment_Load);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(706, 307);
		this.panel12.TabIndex = 2;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceipt);
		this.panel13.Controls.Add(this.panel7);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(704, 307);
		this.panel13.TabIndex = 0;
		this.grdReceipt.AllowEdit = (InheritableBoolean)2;
		this.grdReceipt.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdReceipt_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceipt.DesignTimeLayout = grdReceipt_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceipt.DynamicFiltering = true;
		this.grdReceipt.FilterMode = (FilterMode)2;
		this.grdReceipt.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceipt.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdReceipt).Name = "grdReceipt";
		((ScrollableBase)this.grdReceipt).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Size = new System.Drawing.Size(704, 281);
		((System.Windows.Forms.Control)(object)this.grdReceipt).TabIndex = 2;
		this.grdReceipt.TotalRow = (InheritableBoolean)1;
		this.grdReceipt.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceipt).VisualStyle = (VisualStyle)4;
		this.grdReceipt.LinkClicked += new ColumnActionEventHandler(grdReceipt_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdReceipt).KeyDown += new System.Windows.Forms.KeyEventHandler(grdReceipt_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.ctrlGrdReceipt);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(704, 26);
		this.panel7.TabIndex = 48;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(128, 19);
		this.label2.TabIndex = 7;
		this.label2.Text = "Party Receipts";
		this.ctrlGrdReceipt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdReceipt.FormName = this;
		this.ctrlGrdReceipt.Location = new System.Drawing.Point(663, 2);
		this.ctrlGrdReceipt.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdReceipt.MyGrid = this.grdReceipt;
		this.ctrlGrdReceipt.Name = "ctrlGrdReceipt";
		this.ctrlGrdReceipt.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdReceipt.TabIndex = 541;
		this.ctrlGrdReceipt.Load += new System.EventHandler(ctrlGrdReceipt_Load);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(712, 544);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Suppliers / Customers (Receipts / Payments only)";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel15);
		this.panel9.Controls.Add(this.panel18);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(706, 538);
		this.panel9.TabIndex = 6;
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 307);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(706, 231);
		this.panel15.TabIndex = 3;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentClassWise);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(706, 231);
		this.panel16.TabIndex = 0;
		this.grdPaymentClassWise.AllowEdit = (InheritableBoolean)2;
		this.grdPaymentClassWise.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentClassWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentClassWise.DesignTimeLayout = grdPaymentClassWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentClassWise.DynamicFiltering = true;
		this.grdPaymentClassWise.FilterMode = (FilterMode)2;
		this.grdPaymentClassWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentClassWise.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).Name = "grdPaymentClassWise";
		((ScrollableBase)this.grdPaymentClassWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).Size = new System.Drawing.Size(706, 205);
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).TabIndex = 3;
		this.grdPaymentClassWise.TotalRow = (InheritableBoolean)1;
		this.grdPaymentClassWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentClassWise).VisualStyle = (VisualStyle)4;
		this.grdPaymentClassWise.LinkClicked += new ColumnActionEventHandler(grdPaymentClassWise_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPaymentClassWise).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentClassWise_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label8);
		this.panel17.Controls.Add(this.ctrlGrdBar1);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(706, 26);
		this.panel17.TabIndex = 49;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(169, 19);
		this.label8.TabIndex = 7;
		this.label8.Text = "Customers / Buyers";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(665, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPaymentClassWise;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 541;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(706, 307);
		this.panel18.TabIndex = 2;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceiptClassWise);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(706, 307);
		this.panel19.TabIndex = 0;
		this.grdReceiptClassWise.AllowEdit = (InheritableBoolean)2;
		this.grdReceiptClassWise.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdReceiptClassWise_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceiptClassWise.DesignTimeLayout = grdReceiptClassWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceiptClassWise.DynamicFiltering = true;
		this.grdReceiptClassWise.FilterMode = (FilterMode)2;
		this.grdReceiptClassWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceiptClassWise.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).Name = "grdReceiptClassWise";
		((ScrollableBase)this.grdReceiptClassWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).Size = new System.Drawing.Size(706, 281);
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).TabIndex = 2;
		this.grdReceiptClassWise.TotalRow = (InheritableBoolean)1;
		this.grdReceiptClassWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceiptClassWise).VisualStyle = (VisualStyle)4;
		this.grdReceiptClassWise.LinkClicked += new ColumnActionEventHandler(grdReceiptClassWise_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdReceiptClassWise).KeyDown += new System.Windows.Forms.KeyEventHandler(grdReceiptClassWise_KeyDown);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label9);
		this.panel20.Controls.Add(this.ctrlGrdBar2);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(706, 26);
		this.panel20.TabIndex = 48;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(169, 19);
		this.label9.TabIndex = 7;
		this.label9.Text = "Suppliers / Vendors";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(652, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdReceiptClassWise;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 541;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 127);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(438, 573);
		this.panel4.TabIndex = 4;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		this.grdHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(438, 547);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.ctrlGrdSummary);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(438, 26);
		this.panel8.TabIndex = 47;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 19);
		this.label5.TabIndex = 7;
		this.label5.Text = "Summary";
		this.ctrlGrdSummary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdSummary.FormName = this;
		this.ctrlGrdSummary.Location = new System.Drawing.Point(397, 2);
		this.ctrlGrdSummary.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdSummary.MyGrid = this.grdHistory;
		this.ctrlGrdSummary.Name = "ctrlGrdSummary";
		this.ctrlGrdSummary.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdSummary.TabIndex = 542;
		this.ctrlGrdSummary.Load += new System.EventHandler(ctrlGrdSummary_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.SummaryInfo);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1158, 102);
		this.panel3.TabIndex = 1;
		this.SummaryInfo.Controls.Add(this.label6);
		this.SummaryInfo.Controls.Add(this.lblTotalBankBalance);
		this.SummaryInfo.Controls.Add(this.label4);
		this.SummaryInfo.Controls.Add(this.lblTotalCashBalance);
		this.SummaryInfo.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SummaryInfo.Location = new System.Drawing.Point(440, 28);
		this.SummaryInfo.Name = "SummaryInfo";
		this.SummaryInfo.Size = new System.Drawing.Size(322, 69);
		this.SummaryInfo.TabIndex = 549;
		this.SummaryInfo.TabStop = false;
		this.SummaryInfo.Text = "Summary Info";
		this.SummaryInfo.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(16, 43);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(92, 16);
		this.label6.TabIndex = 546;
		this.label6.Text = "Bank Balance";
		this.lblTotalBankBalance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalBankBalance.ForeColor = System.Drawing.Color.Black;
		this.lblTotalBankBalance.Location = new System.Drawing.Point(115, 43);
		this.lblTotalBankBalance.Name = "lblTotalBankBalance";
		this.lblTotalBankBalance.Size = new System.Drawing.Size(193, 16);
		this.lblTotalBankBalance.TabIndex = 548;
		this.lblTotalBankBalance.Text = "0";
		this.lblTotalBankBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(16, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(92, 16);
		this.label4.TabIndex = 545;
		this.label4.Text = "Cash Balance";
		this.lblTotalCashBalance.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalCashBalance.ForeColor = System.Drawing.Color.Black;
		this.lblTotalCashBalance.Location = new System.Drawing.Point(115, 20);
		this.lblTotalCashBalance.Name = "lblTotalCashBalance";
		this.lblTotalCashBalance.Size = new System.Drawing.Size(193, 16);
		this.lblTotalCashBalance.TabIndex = 547;
		this.lblTotalCashBalance.Text = "0";
		this.lblTotalCashBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTranType);
		this.groupBox1.Controls.Add(this.txtFromDate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(432, 69);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(239, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(96, 15);
		this.label7.TabIndex = 113;
		this.label7.Text = "Transaction Type";
		this.CmbTranType.AllowNull = (DefaultableBoolean)1;
		this.CmbTranType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTranType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTranType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTranType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTranType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTranType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTranType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTranType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTranType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTranType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbTranType).Location = new System.Drawing.Point(239, 37);
		((System.Windows.Forms.Control)(object)this.CmbTranType).Name = "CmbTranType";
		((System.Windows.Forms.Control)(object)this.CmbTranType).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.CmbTranType).TabIndex = 2;
		this.txtFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.CustomFormat = "dd-MMM-yy";
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(6, 39);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(117, 23);
		this.txtFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 111;
		this.label27.Text = "From Date";
		this.txtToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(125, 39);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(112, 23);
		this.txtToDate.TabIndex = 1;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(125, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 110;
		this.label33.Text = "To Date";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(355, 38);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 23);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel10.Controls.Add(this.Branches);
		this.panel10.Controls.Add(this.label1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1158, 27);
		this.panel10.TabIndex = 543;
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(303, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 120;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.White;
		this.Branches.Location = new System.Drawing.Point(238, 5);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(63, 17);
		this.Branches.TabIndex = 121;
		this.Branches.Text = "Branches";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(208, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Cash And Bank Balances";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1158, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnPrintSummery, this.btnPrintReceiptandPayment, this.btn598ReceiptandPaymentAccountClassWise, this.btnShortCutKey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1158, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrintSummery.ForeColor = System.Drawing.Color.Black;
		this.btnPrintSummery.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSummery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSummery.Name = "btnPrintSummery";
		this.btnPrintSummery.Size = new System.Drawing.Size(158, 24);
		this.btnPrintSummery.Text = "&593_Print Summary";
		this.btnPrintSummery.Click += new System.EventHandler(btnPrintSummery_Click);
		this.btnPrintReceiptandPayment.ForeColor = System.Drawing.Color.Black;
		this.btnPrintReceiptandPayment.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintReceiptandPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintReceiptandPayment.Name = "btnPrintReceiptandPayment";
		this.btnPrintReceiptandPayment.Size = new System.Drawing.Size(238, 24);
		this.btnPrintReceiptandPayment.Text = "&596_Print Receipt and Payment";
		this.btnPrintReceiptandPayment.Click += new System.EventHandler(btnPrintReceiptandPayment_Click);
		this.btn598ReceiptandPaymentAccountClassWise.ForeColor = System.Drawing.Color.Black;
		this.btn598ReceiptandPaymentAccountClassWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn598ReceiptandPaymentAccountClassWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn598ReceiptandPaymentAccountClassWise.Name = "btn598ReceiptandPaymentAccountClassWise";
		this.btn598ReceiptandPaymentAccountClassWise.Size = new System.Drawing.Size(376, 24);
		this.btn598ReceiptandPaymentAccountClassWise.Text = "&598_Print Receipt and Payment_Account Class Wise";
		this.btn598ReceiptandPaymentAccountClassWise.Click += new System.EventHandler(btn598ReceiptandPaymentAccountClassWise_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1158, 700);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "CashAndBankBalances";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(BankBalances_FormClosing);
		base.Load += new System.EventHandler(FrmBankBalancesRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CashAndBankBalances_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPayment).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentClassWise).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReceiptClassWise).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.SummaryInfo.ResumeLayout(false);
		this.SummaryInfo.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTranType).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
