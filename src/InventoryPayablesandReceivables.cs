// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.Account_Reports
{

public class InventoryPayablesandReceivables : Form
{
	private WaitForm wf = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtRecord = new DataTable();

	private DataTable dtParentCat = new DataTable();

	private DataTable dtGroups = new DataTable();

	public int ReportTypeStatus;

	private int[] AryColWidth;

	public DateTime FromDate = DateTime.Now;

	public DateTime ToDate = DateTime.Now;

	public int CompanyIdFromBreakUp;

	public string BranchesIds = "";

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Label label33;

	private Label label27;

	private Label label1;

	private Label label30;

	private Label label29;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	internal UltraCombo CmbCustomGroup;

	internal UltraCombo CmbBranches;

	internal UltraCombo cmbCustomerGroup;

	private Button btnshow;

	private ToolStripButton btnPrintCurrent;

	private RadioButton rdPurchaseandSale;

	private RadioButton rdPurchase;

	internal UltraCombo CmbTradeType;

	private Label label3;

	private Label label4;

	private TextBox txtClosingDebit;

	private TextBox txtClosingCredit;

	private CheckBox ChkPostedTransaction;

	public RadioButton rdSale;

	public CheckBox ChkPayables;

	public CheckBox ChkReceivables;

	private ToolStripButton btnPrint134A;

	private GroupBox groupBox1;

	public InventoryPayablesandReceivables(UserAccount user)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.P)
		{
			btnPrintCurrent_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
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

	private void CustomerGroupBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGroups.Rows.Clear();
			CustomerGroup obj = new CustomerGroup();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtGroups = CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation(obj);
			if (dtGroups.Rows.Count > 0)
			{
				DDL.BindDDL(dtGroups, cmbCustomerGroup, "Id", "Description", "CustomerGroup", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbCustomerGroup).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbCustomerGroup.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbCustomerGroup.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TradeTypeFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtParentCat.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Activity = "InventoryParentCategories";
			dtParentCat = Item.InventoryParentCategories(obj);
			if (dtParentCat.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCat, CmbTradeType, "Id", "InvParentCateDescription", "InvParentCateDescription", ZeroIndex: false);
				((UltraGridBase)CmbTradeType).Rows[0].Activate();
				((UltraGridBase)CmbTradeType).DisplayLayout.Bands[0].Columns[1].Width = 336;
				UltraGridColumn obj2 = ((UltraGridBase)CmbTradeType).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbTradeType.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbTradeType.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbTradeType.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbTradeType.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomeGroupsDefine()
	{
		try
		{
			DataTable dtCustome = CommonServices.CustomeGroupsDefine(1);
			if (dtCustome.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustome, CmbCustomGroup, "Id", "AcLookUpsDescription", "Custome Group", false);
				((UltraGridBase)CmbCustomGroup).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			UserAccount.CompanyId = ((!UserAccount.IsHeadOffice) ? UserAccount.CompanyId : ((CompanyIdFromBreakUp > 0) ? CompanyIdFromBreakUp : UserAccount.CompanyId));
			BranchFeature = CommonServices.GetERPFeatureById(17);
			BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
			if (!BranchFeature)
			{
				label30.Visible = false;
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
			CustomeGroupsDefine();
			CustomerGroupBind();
			TradeTypeFill();
			datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			if (ReportTypeStatus == 0)
			{
				((Control)(object)grd).Name = "Payablesgrd";
				datFromDate.Value = FromDate;
				datToDate.Value = ToDate;
				rdPurchase.Checked = true;
				ChkPayables.Checked = true;
			}
			if (ReportTypeStatus == 1)
			{
				((Control)(object)grd).Name = "Receivablesgrd";
				datFromDate.Value = FromDate;
				datToDate.Value = ToDate;
				rdSale.Checked = true;
				ChkReceivables.Checked = true;
			}
			GetData();
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

	private void reset()
	{
		((Control)(object)cmbCustomerGroup).Text = string.Empty;
		((Control)(object)CmbTradeType).Text = string.Empty;
		rdPurchase.Checked = true;
		txtClosingCredit.Text = string.Empty;
		txtClosingDebit.Text = string.Empty;
		grd.ClearStructure();
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			wf.Show();
			GetData();
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

	private void GetData()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			string ParentsIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)CmbTradeType).Text, dtParentCat, "InvParentCateDescription");
			string GroupIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbCustomerGroup).Text, dtGroups, "Description");
			string BranchIds = InfragisticsHelper.GetBranchesIdsByFeature(BranchFeature, BranchFeatureConsolidated, CmbBranches, "BranchName", wf);
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.UserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.AccountTypeIds = Conversion.ToString((object)GroupIds);
			obj.Ids = Conversion.ToString((object)ParentsIds);
			obj.Debit = Conversion.ToDouble((object)txtClosingDebit.Text);
			obj.Credit = Conversion.ToDouble((object)txtClosingCredit.Text);
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			if (rdPurchase.Checked)
			{
				obj.TranTypes = 1;
			}
			if (rdSale.Checked)
			{
				obj.TranTypes = 2;
			}
			if (rdPurchaseandSale.Checked)
			{
				obj.TranTypes = 3;
			}
			if (ChkReceivables.Checked)
			{
				obj.ReportTypeId = 1;
			}
			if (ChkPayables.Checked)
			{
				obj.ReportTypeId = 2;
			}
			if (ChkReceivables.Checked && ChkPayables.Checked)
			{
				obj.ReportTypeId = 3;
			}
			if (ChkPostedTransaction.Checked)
			{
				obj.IsApproved = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			obj.BranchesIds = ((BranchIds != "") ? BranchIds : "");
			obj.CustomerGroupId = Conversion.ToInt(CmbCustomGroup.Value);
			dtRecord = GeneralReprots.InventoryPayablesandReceivables(obj);
			if (dtRecord.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("BranchesId", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("Ac1LevelTitle", typeof(string));
				dt.Columns.Add("Ac2LevelTitle", typeof(string));
				dt.Columns.Add("Ac3LevelTitle", typeof(string));
				dt.Columns.Add("AccountClass", typeof(string));
				dt.Columns.Add("Ac4LevelCode", typeof(string));
				dt.Columns.Add("Ac4LevelTitle", typeof(string));
				dt.Columns.Add("AccountType", typeof(string));
				dt.Columns.Add("ObDebit", typeof(double));
				dt.Columns.Add("ObCredit", typeof(double));
				dt.Columns.Add("CurrDebit", typeof(double));
				dt.Columns.Add("CurrCredit", typeof(double));
				dt.Columns.Add("ClDebit", typeof(double));
				dt.Columns.Add("ClCredit", typeof(double));
				dt.Clear();
				dt.BeginLoadData();
				foreach (DataRow row in dtRecord.Rows)
				{
					dt.Rows.Add(Conversion.ToInt(row["BranchesId"]), Conversion.ToString(row["BranchName"]), Conversion.ToInt(row["AccountId"]), Conversion.ToString(row["Ac1LevelTitle"]), Conversion.ToString(row["Ac2LevelTitle"]), Conversion.ToString(row["Ac3LevelTitle"]), Conversion.ToString(row["ClassName"]), Conversion.ToString(row["Ac4LevelCode"]), Conversion.ToString(row["Ac4LevelTitle"]), Conversion.ToString(row["AccountType"]), Conversion.ToDouble(row["ObDebit"]), Conversion.ToDouble(row["ObCredit"]), Conversion.ToDouble(row["CurrDebit"]), Conversion.ToDouble(row["CurrCredit"]), Conversion.ToDouble(row["ClDebit"]), Conversion.ToDouble(row["ClCredit"]));
				}
				dt.EndLoadData();
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
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["Ac4LevelCode"].Caption = "Account Code";
			grd.RootTable.Columns["Ac4LevelTitle"].Caption = "Account Title";
			grd.RootTable.Columns["Ac3LevelTitle"].Caption = "Parent Account";
			grd.RootTable.Columns["ObDebit"].Caption = "Opening Debit";
			grd.RootTable.Columns["ObCredit"].Caption = "Opening Credit";
			grd.RootTable.Columns["CurrDebit"].Caption = "Debit";
			grd.RootTable.Columns["CurrCredit"].Caption = "Credit";
			grd.RootTable.Columns["ClDebit"].Caption = "Closing Debit";
			grd.RootTable.Columns["ClCredit"].Caption = "Closing Credit";
			grd.RootTable.Columns["BranchName"].Visible = BranchFeature && ((Control)(object)CmbBranches).Text != string.Empty;
			grd.RootTable.Columns["BranchesId"].Visible = false;
			grd.RootTable.Columns["AccountId"].Visible = false;
			grd.RootTable.Columns["Ac1LevelTitle"].Visible = false;
			grd.RootTable.Columns["Ac2LevelTitle"].Visible = false;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["Ac1LevelTitle"].Width = 150;
			grd.RootTable.Columns["Ac2LevelTitle"].Width = 150;
			grd.RootTable.Columns["Ac3LevelTitle"].Width = 150;
			grd.RootTable.Columns["Ac4LevelCode"].Width = 90;
			grd.RootTable.Columns["AccountClass"].Width = 80;
			grd.RootTable.Columns["AccountType"].Width = 80;
			grd.RootTable.Columns["Ac4LevelTitle"].Width = 300;
			grd.RootTable.Columns["ObDebit"].Width = 100;
			grd.RootTable.Columns["ObCredit"].Width = 100;
			grd.RootTable.Columns["CurrDebit"].Width = 100;
			grd.RootTable.Columns["CurrCredit"].Width = 100;
			grd.RootTable.Columns["ClDebit"].Width = 100;
			grd.RootTable.Columns["ClCredit"].Width = 100;
			grd.RootTable.Columns["ObDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ObCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ObDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ObCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CurrCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ClCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ObDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ObCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CurrCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ClCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ObDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ObCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["CurrCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["ClCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["Ac4LevelCode"].ColumnType = (ColumnType)5;
			grd.RootTable.SortKeys.Add(grd.RootTable.Columns["Ac4LevelTitle"], (SortOrder)1);
			ctrlGrdBar1_Load(null, null);
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
			wf.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnPrintCurrent_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRecord.Rows.Count > 0 && dtRecord != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtRecord, "134-InventoryPayablesandReceivables.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkPayables_Click(object sender, EventArgs e)
	{
	}

	private void ChkReceivables_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			CommonServices.GoToGeneralLedgerFromLinkedEvent(BranchId: Conversion.ToInt(currentRow.Cells["BranchesId"].Value), AccountId: Conversion.ToInt(currentRow.Cells["AccountId"].Value), FromDate: datFromDate.Value, ToDate: datToDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtClosingDebit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtClosingCredit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InventoryPayablesandReceivables_FormClosing(object sender, FormClosingEventArgs e)
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

	private void handlePrint134AVisibility()
	{
		try
		{
			if (rdPurchase.Checked || rdPurchaseandSale.Checked)
			{
				btnPrint134A.Visible = true;
			}
			else
			{
				btnPrint134A.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdPurchase_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			handlePrint134AVisibility();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSale_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			handlePrint134AVisibility();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdPurchaseandSale_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			handlePrint134AVisibility();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint134A_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtRecord.Rows.Count > 0 && dtRecord != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtRecord, "134A-InventoryPayablesandReceivables.rpt");
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
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ChkPostedTransaction = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtClosingDebit = new System.Windows.Forms.TextBox();
		this.txtClosingCredit = new System.Windows.Forms.TextBox();
		this.ChkPayables = new System.Windows.Forms.CheckBox();
		this.ChkReceivables = new System.Windows.Forms.CheckBox();
		this.rdPurchaseandSale = new System.Windows.Forms.RadioButton();
		this.rdSale = new System.Windows.Forms.RadioButton();
		this.rdPurchase = new System.Windows.Forms.RadioButton();
		this.CmbTradeType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbCustomGroup = new UltraCombo();
		this.CmbBranches = new UltraCombo();
		this.cmbCustomerGroup = new UltraCombo();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrintCurrent = new System.Windows.Forms.ToolStripButton();
		this.btnPrint134A = new System.Windows.Forms.ToolStripButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTradeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel3.Controls.Add(this.label30);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1102, 30);
		this.panel3.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1061, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1102, 472);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(286, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Inventory Payables && Receivables";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1102, 551);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 79);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1102, 472);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1102, 79);
		this.panel5.TabIndex = 0;
		this.ChkPostedTransaction.AutoSize = true;
		this.ChkPostedTransaction.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPostedTransaction.Location = new System.Drawing.Point(875, 44);
		this.ChkPostedTransaction.Name = "ChkPostedTransaction";
		this.ChkPostedTransaction.Size = new System.Drawing.Size(147, 19);
		this.ChkPostedTransaction.TabIndex = 12;
		this.ChkPostedTransaction.Text = "Approved Transactions";
		this.ChkPostedTransaction.UseVisualStyleBackColor = true;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(477, 37);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(78, 30);
		this.label4.TabIndex = 114;
		this.label4.Text = "Closing Debit\r\n&& Credit";
		this.txtClosingDebit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingDebit.Location = new System.Drawing.Point(556, 41);
		this.txtClosingDebit.Name = "txtClosingDebit";
		this.txtClosingDebit.Size = new System.Drawing.Size(98, 23);
		this.txtClosingDebit.TabIndex = 5;
		this.txtClosingDebit.Text = "0";
		this.txtClosingDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingDebit_KeyPress);
		this.txtClosingCredit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtClosingCredit.Location = new System.Drawing.Point(656, 41);
		this.txtClosingCredit.Name = "txtClosingCredit";
		this.txtClosingCredit.Size = new System.Drawing.Size(102, 23);
		this.txtClosingCredit.TabIndex = 6;
		this.txtClosingCredit.Text = "0";
		this.txtClosingCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtClosingCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtClosingCredit_KeyPress);
		this.ChkPayables.AutoSize = true;
		this.ChkPayables.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPayables.Location = new System.Drawing.Point(875, 12);
		this.ChkPayables.Name = "ChkPayables";
		this.ChkPayables.Size = new System.Drawing.Size(72, 19);
		this.ChkPayables.TabIndex = 10;
		this.ChkPayables.Text = "Payables";
		this.ChkPayables.UseVisualStyleBackColor = true;
		this.ChkPayables.Click += new System.EventHandler(ChkPayables_Click);
		this.ChkReceivables.AutoSize = true;
		this.ChkReceivables.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkReceivables.Location = new System.Drawing.Point(875, 28);
		this.ChkReceivables.Name = "ChkReceivables";
		this.ChkReceivables.Size = new System.Drawing.Size(87, 19);
		this.ChkReceivables.TabIndex = 11;
		this.ChkReceivables.Text = "Receivables";
		this.ChkReceivables.UseVisualStyleBackColor = true;
		this.ChkReceivables.CheckedChanged += new System.EventHandler(ChkReceivables_CheckedChanged);
		this.rdPurchaseandSale.AutoSize = true;
		this.rdPurchaseandSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPurchaseandSale.Location = new System.Drawing.Point(763, 44);
		this.rdPurchaseandSale.Name = "rdPurchaseandSale";
		this.rdPurchaseandSale.Size = new System.Drawing.Size(110, 19);
		this.rdPurchaseandSale.TabIndex = 9;
		this.rdPurchaseandSale.TabStop = true;
		this.rdPurchaseandSale.Text = "Purchase && Sale";
		this.rdPurchaseandSale.UseVisualStyleBackColor = true;
		this.rdPurchaseandSale.CheckedChanged += new System.EventHandler(rdPurchaseandSale_CheckedChanged);
		this.rdSale.AutoSize = true;
		this.rdSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSale.Location = new System.Drawing.Point(763, 28);
		this.rdSale.Name = "rdSale";
		this.rdSale.Size = new System.Drawing.Size(47, 19);
		this.rdSale.TabIndex = 8;
		this.rdSale.TabStop = true;
		this.rdSale.Text = "Sale";
		this.rdSale.UseVisualStyleBackColor = true;
		this.rdSale.CheckedChanged += new System.EventHandler(rdSale_CheckedChanged);
		this.rdPurchase.AutoSize = true;
		this.rdPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdPurchase.Location = new System.Drawing.Point(763, 12);
		this.rdPurchase.Name = "rdPurchase";
		this.rdPurchase.Size = new System.Drawing.Size(73, 19);
		this.rdPurchase.TabIndex = 7;
		this.rdPurchase.TabStop = true;
		this.rdPurchase.Text = "Purchase";
		this.rdPurchase.UseVisualStyleBackColor = true;
		this.rdPurchase.CheckedChanged += new System.EventHandler(rdPurchase_CheckedChanged);
		this.CmbTradeType.AllowNull = (DefaultableBoolean)1;
		this.CmbTradeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTradeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTradeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTradeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTradeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbTradeType).Location = new System.Drawing.Point(556, 12);
		((System.Windows.Forms.Control)(object)this.CmbTradeType).Name = "CmbTradeType";
		((System.Windows.Forms.Control)(object)this.CmbTradeType).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradeType).TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(477, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(61, 15);
		this.label3.TabIndex = 109;
		this.label3.Text = "TradeType";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1023, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 26);
		this.btnshow.TabIndex = 13;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbCustomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Location = new System.Drawing.Point(294, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Name = "CmbCustomGroup";
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomGroup).TabIndex = 2;
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
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
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
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(376, 2);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 0;
		this.cmbCustomerGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Location = new System.Drawing.Point(294, 39);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Name = "cmbCustomerGroup";
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerGroup).TabIndex = 3;
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(72, 41);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(123, 23);
		this.datToDate.TabIndex = 1;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(198, 45);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(94, 15);
		this.label29.TabIndex = 106;
		this.label29.Text = "Inventory Group";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(72, 13);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(123, 23);
		this.datFromDate.TabIndex = 0;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(328, 8);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(46, 15);
		this.label30.TabIndex = 105;
		this.label30.Text = "Branch";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 45);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "Date To";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(4, 17);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "Date From";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(198, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 15);
		this.label1.TabIndex = 104;
		this.label1.Text = "Custom Group";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1102, 608);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1102, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnPrintCurrent, this.btnPrint134A });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1102, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrintCurrent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCurrent.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCurrent.Name = "btnPrintCurrent";
		this.btnPrintCurrent.Size = new System.Drawing.Size(92, 24);
		this.btnPrintCurrent.Text = "134-Print";
		this.btnPrintCurrent.Click += new System.EventHandler(btnPrintCurrent_Click);
		this.btnPrint134A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint134A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint134A.Name = "btnPrint134A";
		this.btnPrint134A.Size = new System.Drawing.Size(101, 24);
		this.btnPrint134A.Text = "134A-Print";
		this.btnPrint134A.Visible = false;
		this.btnPrint134A.Click += new System.EventHandler(btnPrint134A_Click);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.ChkPostedTransaction);
		this.groupBox1.Controls.Add(this.ChkReceivables);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.ChkPayables);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.txtClosingDebit);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerGroup);
		this.groupBox1.Controls.Add(this.txtClosingCredit);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomGroup);
		this.groupBox1.Controls.Add(this.rdPurchaseandSale);
		this.groupBox1.Controls.Add(this.rdSale);
		this.groupBox1.Controls.Add(this.rdPurchase);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTradeType);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1085, 71);
		this.groupBox1.TabIndex = 115;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1102, 608);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InventoryPayablesandReceivables";
		this.Text = "Inventory Payables & Recivables";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InventoryPayablesandReceivables_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.CmbTradeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerGroup).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
