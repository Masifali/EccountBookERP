using System;
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
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class frmCashBalance : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public DateTime FromDate;

	public DateTime ToDate;

	private DataTable dtsummery = new DataTable();

	private DataTable dtReciptandPayment = new DataTable();

	private bool AllowEditOnVoucherNoinReports;

	public int CompanyIdFromBreakUp;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	public string BranchesIds = "";

	private IContainer components;

	private Panel panel1;

	private DateTimePicker txtDateTo;

	private DateTimePicker txtDateFrom;

	private Label label2;

	private Label label1;

	private Button btnSearch;

	private Panel panel4;

	private Label label3;

	private Label label5;

	private Panel panel5;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrintSummery;

	private ToolStripButton btnPrintReceiptandPayment;

	private ToolStripButton toolStripButton1;

	private ToolStripButton btn594CashInFlowOutFlow;

	private CtrlGrdBar ctrlGrdPayment;

	private Label label4;

	private CtrlGrdBar ctrlGrdReceipt;

	private Label label6;

	internal GridEX grdReceipt;

	private Panel panel10;

	internal GridEX grdPayment;

	private Panel panel9;

	private Panel panel6;

	internal UltraCombo cmbAccountTitle;

	private Label label7;

	private FlowLayoutPanel SummeryFlowLayoutPanel;

	internal UltraCombo cmbperemeter;

	private Label label12;

	private ToolStripButton btnShortCutKeys;

	private GroupBox groupBox1;

	private ToolStripButton BtnPrint599;

	internal UltraCombo CmbBranches;

	private Label Branches;

	internal UltraCombo CmbLanguage;

	private Label lblLanguage;

	private RadioButton RadNoGroup;

	private RadioButton RadGroupbyParty;

	private RadioButton RadGroupbyBankName;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel7;

	private Panel panel3;

	public frmCashBalance(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmCashBalance_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllowEditOnVoucherNoinReports"));
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				Branches.Visible = false;
				((Control)(object)CmbBranches).Visible = false;
				btnSearch.Location = new Point(591, 33);
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
			ParameterFill();
			cmbperemeter.Focus();
			if (Conversion.ToInt(cmbperemeter.Value) == 0)
			{
				((UltraGridBase)cmbperemeter).Rows[0].Activate();
			}
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)FromDate);
				txtDateTo.Value = Conversion.ToDateTime((object)ToDate);
				reciptandpayment(BranchIds);
			}
			cashSummery(BranchIds);
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
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
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

	private void AccountTitleFill(DataTable dt)
	{
		try
		{
			DataTable dtAccountTitle = new DataTable();
			dtAccountTitle.Columns.Add("Id", typeof(int));
			dtAccountTitle.Columns.Add("Name");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				if (i == 0)
				{
					dtAccountTitle.Rows.Add(Conversion.ToInt(dt.Rows[i]["AccountId"]), dt.Rows[i]["AccountTitle"]);
				}
				if ((from c in dtAccountTitle.AsEnumerable()
					where c.Field<int>("Id").Equals(Conversion.ToInt(dt.Rows[i]["AccountId"]))
					select c).Count() <= 0)
				{
					dtAccountTitle.Rows.Add(Conversion.ToInt(dt.Rows[i]["AccountId"]), dt.Rows[i]["AccountTitle"]);
				}
			}
			DDL.BindDDLNew(dtAccountTitle, cmbAccountTitle, "Id", "Name", "Account Name", false);
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

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				txtDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFrom.Value = first;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFrom.Value = first2;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtDateFrom.Focus();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			cashSummery(BranchIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmCashBalance_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnSearch_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdReceipt")
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			reciptandpayment(BranchIds);
			cashSummery(BranchIds);
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

	private void reciptandpayment(string BranchIds)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtReciptandPayment.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			obj.BranchesIds = ((BranchesIds != "") ? BranchIds : "");
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			dtReciptandPayment = VoucherReports.CashBalances(obj);
			if (dtReciptandPayment != null && dtReciptandPayment.Rows.Count > 0)
			{
				DataTable dtRCol = CreateTableStructure("ReceivedFrom");
				DataTable dtPCol = CreateTableStructure("PaidTo");
				foreach (DataRow row in dtReciptandPayment.Rows)
				{
					string tranType = Conversion.ToString(row["TranType"]);
					object[] newRow = new object[13]
					{
						row["BranchesId"],
						row["BranchName"],
						row["TranType"],
						row["DocumentTypeCode"],
						row["VoucherCode"],
						row["Id"],
						row["DocumentTypeId"],
						row["DocumentTypeSrNo"],
						Conversion.ToDateTime(row["voucherdate"]).ToString("dd-MMM-yy"),
						row["AccountTitle"],
						row["OffsetAccountTitle"],
						row["DebitAmount"],
						row["NoOfAttachments"]
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

	private void GridSettingPayment()
	{
		try
		{
			grdPayment.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdPayment.RootTable.Columns["BranchesId"].Visible = false;
			grdPayment.RootTable.Columns["TranType"].Visible = false;
			grdPayment.RootTable.Columns["Id"].Visible = false;
			grdPayment.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPayment.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdPayment.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdPayment.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdPayment.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPayment.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPayment.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			grdPayment.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			grdPayment.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			grdPayment.RootTable.Columns["VoucherCode"].Caption = "V.No";
			grdPayment.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdPayment.RootTable.Columns["TranType"].Width = 80;
			grdPayment.RootTable.Columns["DocumentTypeCode"].Width = 70;
			grdPayment.RootTable.Columns["VoucherCode"].Width = 60;
			grdPayment.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grdPayment.RootTable.Columns["VoucherDate"].Width = 90;
			grdPayment.RootTable.Columns["AccountTitle"].Width = 200;
			grdPayment.RootTable.Columns["OffsetAccountTitle"].Width = 200;
			grdPayment.RootTable.Columns["DebitAmount"].Width = 110;
			grdPayment.RootTable.Columns["NoOfAttachments"].Width = 110;
			if (AllowEditOnVoucherNoinReports)
			{
				grdPayment.RootTable.Columns.Add("Print");
				grdPayment.RootTable.Columns["Print"].Caption = "Print";
				grdPayment.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPayment.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdPayment.RootTable.Columns["Print"].ButtonText = "Print";
				grdPayment.RootTable.Columns["Print"].Width = 50;
				grdPayment.RootTable.Columns["Print"].Position = 0;
				grdPayment.FrozenColumns = 1;
			}
			grdPayment.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdPayment_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingReceipts()
	{
		try
		{
			grdReceipt.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grdReceipt.RootTable.Columns["BranchesId"].Visible = false;
			grdReceipt.RootTable.Columns["TranType"].Visible = false;
			grdReceipt.RootTable.Columns["Id"].Visible = false;
			grdReceipt.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdReceipt.RootTable.Columns["DocumentTypeSrNo"].Visible = false;
			grdReceipt.RootTable.Columns["DebitAmount"].AggregateFunction = (AggregateFunction)2;
			grdReceipt.RootTable.Columns["DebitAmount"].TextAlignment = (TextAlignment)3;
			grdReceipt.RootTable.Columns["DebitAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdReceipt.RootTable.Columns["DebitAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdReceipt.RootTable.Columns["VoucherCode"].ColumnType = (ColumnType)5;
			grdReceipt.RootTable.Columns["DocumentTypeCode"].Caption = "V.Type";
			grdReceipt.RootTable.Columns["VoucherDate"].Caption = "V.Date";
			grdReceipt.RootTable.Columns["VoucherCode"].Caption = "V.No";
			grdReceipt.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdReceipt.RootTable.Columns["TranType"].Width = 80;
			grdReceipt.RootTable.Columns["DocumentTypeCode"].Width = 70;
			grdReceipt.RootTable.Columns["VoucherCode"].Width = 60;
			grdReceipt.RootTable.Columns["DocumentTypeSrNo"].Width = 100;
			grdReceipt.RootTable.Columns["VoucherDate"].Width = 90;
			grdReceipt.RootTable.Columns["AccountTitle"].Width = 200;
			grdReceipt.RootTable.Columns["OffsetAccountTitle"].Width = 200;
			grdReceipt.RootTable.Columns["DebitAmount"].Width = 110;
			grdReceipt.RootTable.Columns["NoOfAttachments"].Width = 110;
			if (AllowEditOnVoucherNoinReports)
			{
				grdReceipt.RootTable.Columns.Add("Print");
				grdReceipt.RootTable.Columns["Print"].Caption = "Print";
				grdReceipt.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdReceipt.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdReceipt.RootTable.Columns["Print"].ButtonText = "Print";
				grdReceipt.RootTable.Columns["Print"].Width = 50;
				grdReceipt.RootTable.Columns["Print"].Position = 0;
				grdReceipt.FrozenColumns = 1;
			}
			grdReceipt.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdReceipt_Load(null, null);
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
				"BranchName",
				"TranType",
				"V.Type",
				"V.No",
				"Id",
				"DocumentTypeId",
				"DocumentTypeSrNo",
				"V.Date",
				"CashAccount",
				accountColumnName,
				{
					"Amount",
					typeof(double)
				},
				"NoOfAttachments"
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
		rootTable.Columns["TranType"].Visible = false;
		rootTable.Columns["DocumentTypeSrNo"].Visible = false;
		rootTable.Columns["NoOfAttachments"].Caption = "Attachments";
		if (RadGroupbyBankName.Checked || RadGroupbyParty.Checked)
		{
			string groupColumn = (RadGroupbyBankName.Checked ? "CashAccount" : accountColumnName);
			rootTable.Groups.Add(groupColumn);
			rootTable.Columns[groupColumn].Visible = false;
			rootTable.GroupTotals = (GroupTotals)2;
		}
		rootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		rootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
		rootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatboth;
		rootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatboth;
		rootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
		rootTable.Columns["TranType"].Width = 68;
		rootTable.Columns["V.Type"].Width = 70;
		rootTable.Columns["V.Date"].Width = 80;
		rootTable.Columns["V.No"].Width = 40;
		rootTable.Columns["CashAccount"].Width = 135;
		rootTable.Columns[accountColumnName].Width = 150;
		rootTable.Columns["Amount"].Width = 100;
		rootTable.Columns["NoOfAttachments"].Width = 85;
		rootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
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
		try
		{
			GridEXRow currentRow = grdReceipt.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(currentRow.Cells["DocumentTypeId"].Value);
			int DocumentTypeSrNo = Conversion.ToInt(currentRow.Cells["DocumentTypeSrNo"].Value);
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID((DocumentTypeSrNo > 0) ? DocumentTypeSrNo : Id, DocumentTypeId);
			}
			else if (AllowEditOnVoucherNoinReports)
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
			}
			else
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void grdPayment_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grdPayment.CurrentRow;
			int Id = Conversion.ToInt((object)currentRow.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)currentRow.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeSrNo = Conversion.ToInt((object)currentRow.Cells["DocumentTypeSrNo"].Value.ToString());
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID((DocumentTypeSrNo > 0) ? DocumentTypeSrNo : Id, DocumentTypeId);
			}
			else if (AllowEditOnVoucherNoinReports)
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
			}
			else
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void grdPayment_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt((object)grdPayment.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	private void grdReceipt_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt((object)grdReceipt.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Column.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
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
				grdReceipt.RootTable.Columns["CashAccount"].Visible = true;
				grdReceipt.RootTable.Columns["ReceivedFrom"].Visible = true;
				((JanusCollectionBase)grdReceipt.RootTable.Groups).Clear();
				if (RadGroupbyBankName.Checked)
				{
					grdReceipt.RootTable.Groups.Add("CashAccount");
					grdReceipt.RootTable.Columns["CashAccount"].Visible = false;
					grdReceipt.GroupTotals = (GroupTotals)2;
				}
				else if (RadGroupbyParty.Checked)
				{
					grdReceipt.RootTable.Groups.Add("ReceivedFrom");
					grdReceipt.RootTable.Columns["ReceivedFrom"].Visible = false;
					grdReceipt.GroupTotals = (GroupTotals)2;
				}
			}
			if (grdPayment.GetRows().Count() > 0)
			{
				grdPayment.RootTable.Columns["CashAccount"].Visible = true;
				grdPayment.RootTable.Columns["PaidTo"].Visible = true;
				((JanusCollectionBase)grdPayment.RootTable.Groups).Clear();
				if (RadGroupbyBankName.Checked)
				{
					grdPayment.RootTable.Groups.Add("CashAccount");
					grdPayment.RootTable.Columns["CashAccount"].Visible = false;
					grdPayment.GroupTotals = (GroupTotals)2;
				}
				else if (RadGroupbyParty.Checked)
				{
					grdPayment.RootTable.Groups.Add("PaidTo");
					grdPayment.RootTable.Columns["PaidTo"].Visible = false;
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
			val.ShowReportWithDataTable(dtReciptandPayment, "592-CashBalances.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSummery_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtsummery == null || dtsummery.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtsummery, "593-CashBankBalancesSummery_Rpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = (BranchFeature ? BranchIds : "");
			}
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			DataTable dt = VoucherReports.DialyCashAndBankBalances_Rpt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "595-Cash&BankPaymentReciept.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn594CashInFlowOutFlow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.AccountTypeId = 2;
			obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = (BranchFeature ? BranchIds : "");
			}
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			DataTable dt = VoucherReports.CashAndBank_InflowOutFlow_Rpt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "594-CashBalancesInOutFlow.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint599_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			obj.SkipZero = 1;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = (BranchFeature ? BranchIds : "");
			}
			obj.LanguageId = Conversion.ToInt(CmbLanguage.Value);
			DataTable dt = VoucherReports.DialyCashAndBankBalances_Rpt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "599_Cash&BankPaymentRecieptWithAllJv.rpt");
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void cashSummery(string BranchIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		try
		{
			DataSet ds = VoucherReports.CashandBankBalancesSummery(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtDateTo.Value),
				ApprovedFilter = "All",
				AccountTypeId = 2,
				BranchesIds = ((BranchIds != "") ? BranchIds : ""),
				LanguageId = Conversion.ToInt(CmbLanguage.Value)
			});
			dtsummery = ds?.Tables[0];
			if (dtsummery.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtsummery, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				AccountTitleFill(dtsummery);
				SummeryFlowLayoutPanel.Controls.Clear();
				AccountsDashboardCard[] AccountCardsList = new AccountsDashboardCard[dtsummery.Rows.Count];
				for (int i = 0; i < AccountCardsList.Length; i++)
				{
					double Diff = Conversion.ToDouble(dtsummery.Rows[i]["CurrDebit"]) - Conversion.ToDouble(dtsummery.Rows[i]["CurrCredit"]);
					AccountCardsList[i] = new AccountsDashboardCard();
					AccountCardsList[i].Id = Conversion.ToInt(dtsummery.Rows[i]["AccountId"]);
					AccountCardsList[i].Title = Conversion.ToString(dtsummery.Rows[i]["AccountTitle"]);
					AccountCardsList[i].OpeningBalance = Conversion.ToDouble(dtsummery.Rows[i]["Opening"]).ToString(clsGlobalVariables.stringFormatboth);
					AccountCardsList[i].CurrentDrBalance = Conversion.ToDouble(dtsummery.Rows[i]["CurrDebit"]).ToString(clsGlobalVariables.stringFormatboth);
					AccountCardsList[i].CurrentCrBalance = Conversion.ToDouble(dtsummery.Rows[i]["CurrCredit"]).ToString(clsGlobalVariables.stringFormatboth);
					AccountCardsList[i].DiffValue = Diff.ToString("#,#;(#,#);0");
					AccountCardsList[i].Status = ((Diff == 0.0) ? "Nothing" : ((Diff > 0.0) ? "Increase" : "Decrease"));
					AccountCardsList[i].ClosingBalance = Conversion.ToDouble(dtsummery.Rows[i]["Closing"]).ToString(clsGlobalVariables.stringFormatboth);
					SummeryFlowLayoutPanel.Controls.Add(AccountCardsList[i]);
					AccountCardsList[i].UserControlClicked += UserControl_Click;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GoToGeneralLedgerFromLinkedEvent(((AccountsDashboardCard)sender).Id, txtDateFrom.Value, txtDateTo.Value);
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 592_Print Receipt and Payment");
			dt.Rows.Add("Alt+2", "For Print 593_Print Summary");
			dt.Rows.Add("Alt+3", "For Print 594-CashInOutFlow");
			dt.Rows.Add("Alt+4", "For Print 595-CashDailyActivity");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo");
			dt.Rows.Add("Ctrl+ArrowRight", "change Focus from one Grid to another");
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

	private void frmCashBalance_KeyDown(object sender, KeyEventArgs e)
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
				btnSearch_Click(null, null);
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
					btnPrintReceiptandPayment_Click(null, null);
					break;
				case Keys.NumPad1:
					btnPrintReceiptandPayment_Click(null, null);
					break;
				case Keys.D2:
					btnPrintSummery_Click(null, null);
					break;
				case Keys.NumPad2:
					btnPrintSummery_Click(null, null);
					break;
				case Keys.D3:
					btn594CashInFlowOutFlow_Click(null, null);
					break;
				case Keys.NumPad3:
					btn594CashInFlowOutFlow_Click(null, null);
					break;
				case Keys.D4:
					toolStripButton1_Click(null, null);
					break;
				case Keys.NumPad4:
					toolStripButton1_Click(null, null);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdPayment).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdPayment).Focused)
				{
					((Control)(object)grdReceipt).Focus();
				}
				else
				{
					((Control)(object)grdPayment).Focus();
				}
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

	private void grdPayment_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPayment.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPayment.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
			if (grdPayment.CurrentColumn.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
			if (grdPayment.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (grdPayment.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
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
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdReceipt.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdReceipt.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
			int DocumentTypeSrNo = Conversion.ToInt((object)r.Cells["DocumentTypeSrNo"].Value.ToString());
			if (grdReceipt.CurrentColumn.Key == "Print")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
			}
			if (grdReceipt.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (grdReceipt.CurrentColumn.Key == "VoucherCode")
			{
				if (AllowEditOnVoucherNoinReports)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeSrNo);
				}
				else
				{
					CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
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
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_0bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Expected O, but got Unknown
		//IL_0be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Expected O, but got Unknown
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebd: Expected O, but got Unknown
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed4: Expected O, but got Unknown
		GridEXLayout grdPayment_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdReceipt_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
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
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrintReceiptandPayment = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSummery = new System.Windows.Forms.ToolStripButton();
		this.btn594CashInFlowOutFlow = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint599 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdPayment = new grid.User_Conrtols.CtrlGrdBar();
		this.grdPayment = new GridEX();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.ctrlGrdReceipt = new grid.User_Conrtols.CtrlGrdBar();
		this.grdReceipt = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnSearch = new System.Windows.Forms.Button();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.RadNoGroup = new System.Windows.Forms.RadioButton();
		this.CmbLanguage = new UltraCombo();
		this.RadGroupbyParty = new System.Windows.Forms.RadioButton();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.RadGroupbyBankName = new System.Windows.Forms.RadioButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbAccountTitle = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.SummeryFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1175, 52);
		this.panel1.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 28);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(130, 19);
		this.label6.TabIndex = 542;
		this.label6.Text = "Cash Balances ";
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1175, 25);
		this.panel5.TabIndex = 7;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnPrintReceiptandPayment, this.btnPrintSummery, this.btn594CashInFlowOutFlow, this.toolStripButton1, this.BtnPrint599, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1175, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrintReceiptandPayment.ForeColor = System.Drawing.Color.Black;
		this.btnPrintReceiptandPayment.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintReceiptandPayment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintReceiptandPayment.Name = "btnPrintReceiptandPayment";
		this.btnPrintReceiptandPayment.Size = new System.Drawing.Size(238, 24);
		this.btnPrintReceiptandPayment.Text = "&592_Print Receipt and Payment";
		this.btnPrintReceiptandPayment.Click += new System.EventHandler(btnPrintReceiptandPayment_Click);
		this.btnPrintSummery.ForeColor = System.Drawing.Color.Black;
		this.btnPrintSummery.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSummery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSummery.Name = "btnPrintSummery";
		this.btnPrintSummery.Size = new System.Drawing.Size(158, 24);
		this.btnPrintSummery.Text = "&593_Print Summary";
		this.btnPrintSummery.Click += new System.EventHandler(btnPrintSummery_Click);
		this.btn594CashInFlowOutFlow.ForeColor = System.Drawing.Color.Black;
		this.btn594CashInFlowOutFlow.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn594CashInFlowOutFlow.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn594CashInFlowOutFlow.Name = "btn594CashInFlowOutFlow";
		this.btn594CashInFlowOutFlow.Size = new System.Drawing.Size(161, 24);
		this.btn594CashInFlowOutFlow.Text = "594-CashInOutFlow";
		this.btn594CashInFlowOutFlow.Click += new System.EventHandler(btn594CashInFlowOutFlow_Click);
		this.toolStripButton1.ForeColor = System.Drawing.Color.Black;
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(176, 24);
		this.toolStripButton1.Text = "595-CashDailyActivity";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnPrint599.ForeColor = System.Drawing.Color.Black;
		this.BtnPrint599.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint599.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint599.Name = "BtnPrint599";
		this.BtnPrint599.Size = new System.Drawing.Size(200, 24);
		this.BtnPrint599.Text = "599-CashBank With All JV";
		this.BtnPrint599.Click += new System.EventHandler(BtnPrint599_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.ctrlGrdPayment.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdPayment.FormName = this;
		this.ctrlGrdPayment.Location = new System.Drawing.Point(653, 1);
		this.ctrlGrdPayment.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdPayment.MyGrid = this.grdPayment;
		this.ctrlGrdPayment.Name = "ctrlGrdPayment";
		this.ctrlGrdPayment.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdPayment.TabIndex = 541;
		this.ctrlGrdPayment.Load += new System.EventHandler(ctrlGrdPayment_Load);
		this.grdPayment.AllowEdit = (InheritableBoolean)2;
		grdPayment_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPayment.DesignTimeLayout = grdPayment_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPayment).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPayment.DynamicFiltering = true;
		this.grdPayment.FilterMode = (FilterMode)2;
		this.grdPayment.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPayment).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPayment.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPayment).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdPayment).Name = "grdPayment";
		((ScrollableBase)this.grdPayment).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPayment).Size = new System.Drawing.Size(694, 503);
		((System.Windows.Forms.Control)(object)this.grdPayment).TabIndex = 0;
		this.grdPayment.TotalRow = (InheritableBoolean)1;
		this.grdPayment.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPayment).VisualStyle = (VisualStyle)4;
		this.grdPayment.ColumnButtonClick += new ColumnActionEventHandler(grdPayment_ColumnButtonClick);
		this.grdPayment.LinkClicked += new ColumnActionEventHandler(grdPayment_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPayment).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPayment_KeyDown);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(5, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 18);
		this.label4.TabIndex = 5;
		this.label4.Text = "Payment";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 18);
		this.label3.TabIndex = 5;
		this.label3.Text = "Receipt";
		this.ctrlGrdReceipt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdReceipt.FormName = this;
		this.ctrlGrdReceipt.Location = new System.Drawing.Point(426, 1);
		this.ctrlGrdReceipt.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdReceipt.MyGrid = this.grdReceipt;
		this.ctrlGrdReceipt.Name = "ctrlGrdReceipt";
		this.ctrlGrdReceipt.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdReceipt.TabIndex = 541;
		this.ctrlGrdReceipt.Load += new System.EventHandler(ctrlGrdReceipt_Load);
		this.grdReceipt.AllowEdit = (InheritableBoolean)2;
		grdReceipt_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdReceipt.DesignTimeLayout = grdReceipt_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReceipt.DynamicFiltering = true;
		this.grdReceipt.FilterMode = (FilterMode)2;
		this.grdReceipt.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReceipt.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdReceipt).Name = "grdReceipt";
		((ScrollableBase)this.grdReceipt).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReceipt).Size = new System.Drawing.Size(467, 501);
		((System.Windows.Forms.Control)(object)this.grdReceipt).TabIndex = 0;
		this.grdReceipt.TotalRow = (InheritableBoolean)1;
		this.grdReceipt.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReceipt).VisualStyle = (VisualStyle)4;
		this.grdReceipt.ColumnButtonClick += new ColumnActionEventHandler(grdReceipt_ColumnButtonClick);
		this.grdReceipt.LinkClicked += new ColumnActionEventHandler(grdReceipt_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdReceipt).KeyDown += new System.Windows.Forms.KeyEventHandler(grdReceipt_KeyDown);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(247, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(123, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 3;
		this.label1.Text = "From Date";
		this.btnSearch.BackColor = System.Drawing.Color.Teal;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSearch.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.Location = new System.Drawing.Point(806, 34);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(55, 23);
		this.btnSearch.TabIndex = 5;
		this.btnSearch.Text = "Show";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.btnSearch.Leave += new System.EventHandler(btnSearch_Leave);
		this.txtDateTo.CustomFormat = "dd-MMM-yyyy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(246, 35);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(122, 22);
		this.txtDateTo.TabIndex = 2;
		this.txtDateFrom.CustomFormat = "dd-MMM-yyyy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(122, 35);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(122, 22);
		this.txtDateFrom.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 18);
		this.label5.TabIndex = 7;
		this.label5.Text = "Summary";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.RadNoGroup);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLanguage);
		this.panel4.Controls.Add(this.RadGroupbyParty);
		this.panel4.Controls.Add(this.lblLanguage);
		this.panel4.Controls.Add(this.RadGroupbyBankName);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 52);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1175, 67);
		this.panel4.TabIndex = 0;
		this.RadNoGroup.AutoSize = true;
		this.RadNoGroup.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoGroup.Location = new System.Drawing.Point(873, 33);
		this.RadNoGroup.Name = "RadNoGroup";
		this.RadNoGroup.Size = new System.Drawing.Size(56, 18);
		this.RadNoGroup.TabIndex = 3;
		this.RadNoGroup.Text = "None";
		this.RadNoGroup.UseVisualStyleBackColor = true;
		this.RadNoGroup.CheckedChanged += new System.EventHandler(RadNoGroup_CheckedChanged);
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
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLanguage).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
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
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Location = new System.Drawing.Point(1016, 38);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Name = "CmbLanguage";
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbLanguage).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbLanguage).Visible = false;
		this.RadGroupbyParty.AutoSize = true;
		this.RadGroupbyParty.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGroupbyParty.Location = new System.Drawing.Point(873, 18);
		this.RadGroupbyParty.Name = "RadGroupbyParty";
		this.RadGroupbyParty.Size = new System.Drawing.Size(178, 18);
		this.RadGroupbyParty.TabIndex = 2;
		this.RadGroupbyParty.Text = "Group By Offset Account";
		this.RadGroupbyParty.UseVisualStyleBackColor = true;
		this.RadGroupbyParty.CheckedChanged += new System.EventHandler(RadGroupbyParty_CheckedChanged);
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.ForeColor = System.Drawing.Color.Black;
		this.lblLanguage.Location = new System.Drawing.Point(957, 44);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(59, 15);
		this.lblLanguage.TabIndex = 596;
		this.lblLanguage.Text = "Language";
		this.lblLanguage.Visible = false;
		this.RadGroupbyBankName.AutoSize = true;
		this.RadGroupbyBankName.Checked = true;
		this.RadGroupbyBankName.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGroupbyBankName.Location = new System.Drawing.Point(873, 2);
		this.RadGroupbyBankName.Name = "RadGroupbyBankName";
		this.RadGroupbyBankName.Size = new System.Drawing.Size(169, 18);
		this.RadGroupbyBankName.TabIndex = 1;
		this.RadGroupbyBankName.TabStop = true;
		this.RadGroupbyBankName.Text = "Group By Cash Account";
		this.RadGroupbyBankName.UseVisualStyleBackColor = true;
		this.RadGroupbyBankName.CheckedChanged += new System.EventHandler(RadGroupbyBankName_CheckedChanged);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.groupBox1.Controls.Add(this.Branches);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.groupBox1.Controls.Add(this.btnSearch);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDateTo);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(863, 62);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(591, 33);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 4;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(592, 14);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 592;
		this.Branches.Text = "Branches";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(5, 33);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 15);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 590;
		this.label12.Text = "Date Type";
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(370, 33);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(372, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 548;
		this.label7.Text = "Account Title";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1161, 25);
		this.panel6.TabIndex = 6;
		this.SummeryFlowLayoutPanel.AutoScroll = true;
		this.SummeryFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.SummeryFlowLayoutPanel.Location = new System.Drawing.Point(3, 28);
		this.SummeryFlowLayoutPanel.Name = "SummeryFlowLayoutPanel";
		this.SummeryFlowLayoutPanel.Size = new System.Drawing.Size(1161, 501);
		this.SummeryFlowLayoutPanel.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdPayment);
		this.panel9.Controls.Add(this.label4);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(694, 23);
		this.panel9.TabIndex = 8;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdReceipt);
		this.panel10.Controls.Add(this.label3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(467, 25);
		this.panel10.TabIndex = 10;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 119);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1175, 561);
		this.tabControl1.TabIndex = 11;
		this.tabPage1.Controls.Add(this.SummeryFlowLayoutPanel);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1167, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Summary";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1167, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Payment/Receipts Detail";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdReceipt);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(697, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(467, 526);
		this.panel7.TabIndex = 1;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdPayment);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(694, 526);
		this.panel3.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1175, 680);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmCashBalance";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCashBalance_FormClosed);
		base.Load += new System.EventHandler(frmCashBalance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCashBalance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPayment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdReceipt).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLanguage).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
