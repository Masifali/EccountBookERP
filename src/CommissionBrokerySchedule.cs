using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.CmTr.TransactionForms
{

public class CommissionBrokerySchedule : Form
{
	public UserAccount UserAccount = new UserAccount();

	private List<CommTradeRevenueExpenseScheduleDetail> lstRemoveRecord = new List<CommTradeRevenueExpenseScheduleDetail>();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtDetail = new DataTable();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private DataTable dtrevenueexpenxe = new DataTable();

	private DataTable dttarnstype = new DataTable();

	private DataTable dtChargesIncentive = new DataTable();

	private DataTable dtCalParameter = new DataTable();

	private DataTable dtPayTerm = new DataTable();

	private int RecId;

	private int FormType;

	private DataTable dtSupplier = new DataTable();

	private DataTable dtgrdComboAll = new DataTable();

	private string ScreenName = "";

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnRefresh;

	private ToolStripButton BtnShortCutKeys;

	private Panel panel17;

	private Label label1;

	private Panel panel1;

	private Label label3;

	private TextBox txtpolicycode;

	internal UltraCombo cmbsuppliername;

	private Label label7;

	private Panel panel12;

	private Label label30;

	internal GridEX grd;

	private GroupBox groupBox1;

	private Label label2;

	private RichTextBox txtRemarksHeader;

	private Label label4;

	private TabControl tabControl1;

	private TabPage Form;

	private TabPage History;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistoryDetail;

	private Panel panel2;

	private Label label5;

	internal GridEX grdHistory;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoaddAllHistory;

	private Label label6;

	private ToolStripButton btnDelete;

	public CommissionBrokerySchedule(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void CommissionBrokerySchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			ScreenName = Conversion.ToString(base.Tag);
			if (Conversion.ToString(base.Tag) == "CommissionBrokerySchedule")
			{
				FormType = 1;
			}
			else if (Conversion.ToString(base.Tag) == "BrokerSchedule")
			{
				FormType = 2;
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			BtnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = formright.DoHaveCanDelete;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtrevenueexpenxe.Columns.Add("Id");
			dtrevenueexpenxe.Columns.Add("Name");
			dttarnstype.Columns.Add("Id");
			dttarnstype.Columns.Add("Name");
			dtChargesIncentive.Columns.Add("Id");
			dtChargesIncentive.Columns.Add("Name");
			dtCalParameter.Columns.Add("Id");
			dtCalParameter.Columns.Add("Name");
			dtPayTerm.Columns.Add("Id");
			dtPayTerm.Columns.Add("Name");
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("IsRevenueExpenseId", typeof(int));
			dtDetail.Columns.Add("EffectiveDate", typeof(DateTime));
			dtDetail.Columns.Add("TransactionTypeId", typeof(int));
			dtDetail.Columns.Add("ChargesIncentiveAccountId", typeof(int));
			dtDetail.Columns.Add("PaymentTermId", typeof(int));
			dtDetail.Columns.Add("DaysFrom", typeof(int));
			dtDetail.Columns.Add("DaysTo", typeof(int));
			dtDetail.Columns.Add("CalculationParameterId", typeof(int));
			dtDetail.Columns.Add("Rate", typeof(double));
			dtDetail.Columns.Add("RateUom", typeof(double));
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			AddRowInGrid();
			CBGrdidSetting();
			suppliercustomer();
			cmbsuppliername.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int suppId = Conversion.ToInt(cmbsuppliername.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbsuppliername, "Id", "CompanyName", "Customer Name", true);
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbsuppliername.Value = suppId;
				}
				else
				{
					((Control)(object)cmbsuppliername).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void grdDetailComboFill()
	{
		try
		{
			dtgrdComboAll = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(0, 0);
			if (dtgrdComboAll.Rows.Count > 0)
			{
				dtrevenueexpenxe.Rows.Clear();
				dttarnstype.Rows.Clear();
				dtChargesIncentive.Rows.Clear();
				dtCalParameter.Rows.Clear();
				dtPayTerm.Rows.Clear();
				for (int i = 0; i < dtgrdComboAll.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtgrdComboAll.Rows[i]["InvLookupTypeId"]) == 1)
					{
						dtrevenueexpenxe.Rows.Add(dtgrdComboAll.Rows[i]["Id"], dtgrdComboAll.Rows[i]["LookupName"]);
					}
					else if (Conversion.ToInt(dtgrdComboAll.Rows[i]["InvLookupTypeId"]) == 2)
					{
						dttarnstype.Rows.Add(dtgrdComboAll.Rows[i]["Id"], dtgrdComboAll.Rows[i]["LookupName"]);
					}
					else if (Conversion.ToInt(dtgrdComboAll.Rows[i]["InvLookupTypeId"]) == 3)
					{
						dtChargesIncentive.Rows.Add(dtgrdComboAll.Rows[i]["Id"], dtgrdComboAll.Rows[i]["LookupName"]);
					}
					else if (Conversion.ToInt(dtgrdComboAll.Rows[i]["InvLookupTypeId"]) == 4)
					{
						dtCalParameter.Rows.Add(dtgrdComboAll.Rows[i]["Id"], dtgrdComboAll.Rows[i]["LookupName"]);
					}
					else if (Conversion.ToInt(dtgrdComboAll.Rows[i]["InvLookupTypeId"]) == 5)
					{
						dtPayTerm.Rows.Add(dtgrdComboAll.Rows[i]["Id"], dtgrdComboAll.Rows[i]["LookupName"]);
					}
				}
			}
			grd.RootTable.Columns["IsRevenueExpenseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["IsRevenueExpenseId"].HasValueList = true;
			grd.RootTable.Columns["IsRevenueExpenseId"].LimitToList = true;
			grd.RootTable.Columns["IsRevenueExpenseId"].EditType = (EditType)0;
			grd.RootTable.Columns["IsRevenueExpenseId"].Caption = "Revenue Or Expense";
			grd.RootTable.Columns["IsRevenueExpenseId"].ValueList.PopulateValueList((IEnumerable)dtrevenueexpenxe.DefaultView, "Id", "name");
			grd.RootTable.Columns["TransactionTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["TransactionTypeId"].HasValueList = true;
			grd.RootTable.Columns["TransactionTypeId"].LimitToList = true;
			grd.RootTable.Columns["TransactionTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["TransactionTypeId"].Caption = "Transaction Type";
			grd.RootTable.Columns["TransactionTypeId"].ValueList.PopulateValueList((IEnumerable)dttarnstype.DefaultView, "Id", "name");
			grd.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PaymentTermId"].HasValueList = true;
			grd.RootTable.Columns["PaymentTermId"].LimitToList = true;
			grd.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grd.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grd.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtPayTerm.DefaultView, "Id", "name");
			grd.RootTable.Columns["ChargesIncentiveAccountId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ChargesIncentiveAccountId"].HasValueList = true;
			grd.RootTable.Columns["ChargesIncentiveAccountId"].LimitToList = true;
			grd.RootTable.Columns["ChargesIncentiveAccountId"].EditType = (EditType)4;
			grd.RootTable.Columns["ChargesIncentiveAccountId"].Caption = "Charges Incentive Account";
			grd.RootTable.Columns["ChargesIncentiveAccountId"].ValueList.PopulateValueList((IEnumerable)dtChargesIncentive.DefaultView, "Id", "name");
			grd.RootTable.Columns["CalculationParameterId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CalculationParameterId"].HasValueList = true;
			grd.RootTable.Columns["CalculationParameterId"].LimitToList = true;
			grd.RootTable.Columns["CalculationParameterId"].EditType = (EditType)4;
			grd.RootTable.Columns["CalculationParameterId"].Caption = "Calculation Parameter";
			grd.RootTable.Columns["CalculationParameterId"].ValueList.PopulateValueList((IEnumerable)dtCalParameter.DefaultView, "Id", "name");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CBGrdidSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].Visible = false;
			grdDetailComboFill();
			if (FormType == 2)
			{
				grd.RootTable.Columns["ChargesIncentiveAccountId"].EditType = (EditType)0;
			}
			grd.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			grd.RootTable.Columns["Add"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid()
	{
		try
		{
			if (FormType == 1)
			{
				dtDetail.Rows.Add(0, FormType, DateTime.Now, 4, 0, 0, 0, 0, 0, 0, 0);
			}
			else if (FormType == 2)
			{
				dtDetail.Rows.Add(0, FormType, DateTime.Now, 4, 6, 0, 0, 0, 0, 0, 0);
			}
			grd.DataSource = dtDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					CommTradeRevenueExpenseScheduleDetail vd = new CommTradeRevenueExpenseScheduleDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.EffectiveDate = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
					vd.TransactionTypeId = Conversion.ToInt(r.Cells["TransactionTypeId"].Value);
					vd.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vd.ChargesIncentiveId = Conversion.ToInt(r.Cells["ChargesIncentiveAccountId"].Value);
					vd.CalculationParameterId = Conversion.ToInt(r.Cells["CalculationParameterId"].Value);
					vd.DaysFrom = Conversion.ToInt(r.Cells["DaysFrom"].Value);
					vd.DaysTo = Conversion.ToInt(r.Cells["DaysTo"].Value);
					vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					vd.RateUom = Conversion.ToDecimal(r.Cells["RateUom"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
					if (grd.GetRows().Count() == 0)
					{
						AddRowInGrid();
					}
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			if (e.Column.Key == "Rate" && Conversion.ToString((object)grd.CurrentRow.Cells["CalculationParameterId"].Text) == "Percent")
			{
				double Percentage = 0.0;
				Percentage = Conversion.ToDouble(grd.CurrentRow.Cells["Rate"].Value);
				if (Percentage > 100.0)
				{
					grd.CurrentRow.Cells["Rate"].Value = 100;
					MessageBox.Show("Percentage Rate Can not be Greater than 100...");
					return;
				}
				if (Percentage > 2.0)
				{
					MessageBox.Show("Percentage is Greater than 2 Please Make Sure it...", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			if (e.Column.Key == "RateUom" && Conversion.ToDouble(grd.CurrentRow.Cells["RateUom"].Value) > 100.0)
			{
				grd.CurrentRow.Cells["RateUom"].Value = 100;
				MessageBox.Show("RateUom Can not be Greater than 100...");
				return;
			}
			_ = e.Column.Key == "CalculationParameterId";
			if ((e.Column.Key == "DaysFrom" || e.Column.Key == "DaysTo") && Conversion.ToInt(grd.CurrentRow.Cells["DaysTo"].Value) != 0 && Conversion.ToInt(grd.CurrentRow.Cells["DaysFrom"].Value) > Conversion.ToInt(grd.CurrentRow.Cells["DaysTo"].Value))
			{
				grd.CurrentRow.Cells["DaysFrom"].Value = Conversion.ToInt(grd.CurrentRow.Cells["DaysTo"].Value) - 1;
				MessageBox.Show("DaysFrom Can not be Greater than DaysTo...");
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
			RecId = 0;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			txtpolicycode.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			cmbsuppliername.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			dtDetail.Rows.Clear();
			grd.DataSource = null;
			AddRowInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionBrokerySchedule_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			suppliercustomer();
			grdDetailComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarksHeader_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "tabControl1")
			{
				cmbsuppliername.Focus();
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbsuppliername).ActiveRow == null)
		{
			MessageBox.Show("Customer/Supplier Field is Required");
			cmbsuppliername.Focus();
			return false;
		}
		if (txtpolicycode.Text.Trim() == string.Empty || txtpolicycode.Text.Trim() == "0" || txtpolicycode.Text.Trim() == null)
		{
			MessageBox.Show("Policy Code Field is Required");
			txtpolicycode.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			CommTradeRevenueExpenseSchedule po = new CommTradeRevenueExpenseSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			po.DocumentTypeId = 1700;
			po.BranchId = Conversion.ToInt((object)UserAccount.BranchesId);
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.EntryUserId = UserAccount.ID;
			po.ModifyUserId = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ModifyDate = DateTime.Now;
			po.ApprovedDate = DateTime.Now;
			po.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			po.PolicyCode = Conversion.ToString((object)txtpolicycode.Text);
			po.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			po.ScreenName = Conversion.ToString((object)ScreenName);
			po.CommTradeRevenueExpenseScheduleList = new List<CommTradeRevenueExpenseScheduleDetail>();
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					CommTradeRevenueExpenseScheduleDetail vd = new CommTradeRevenueExpenseScheduleDetail();
					if (Conversion.ToInt(r.Cells["IsRevenueExpenseId"].Value) > 0)
					{
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.IsRevenueExpense = Conversion.ToInt(r.Cells["IsRevenueExpenseId"].Value);
						if (vd.Id > 0)
						{
							vd.ActionTypeId = 2;
						}
						else
						{
							vd.ActionTypeId = 1;
						}
						if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(r.Cells["EffectiveDate"].Value)))
						{
							vd.EffectiveDate = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
							if (Conversion.ToInt(r.Cells["TransactionTypeId"].Value) > 0)
							{
								vd.TransactionTypeId = Conversion.ToInt(r.Cells["TransactionTypeId"].Value);
								if (Conversion.ToInt(r.Cells["PaymentTermId"].Value) > 0)
								{
									vd.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
									if (Conversion.ToInt(r.Cells["ChargesIncentiveAccountId"].Value) > 0)
									{
										vd.ChargesIncentiveId = Conversion.ToInt(r.Cells["ChargesIncentiveAccountId"].Value);
										if (Conversion.ToInt(r.Cells["CalculationParameterId"].Value) > 0)
										{
											vd.CalculationParameterId = Conversion.ToInt(r.Cells["CalculationParameterId"].Value);
											if (Conversion.ToInt(r.Cells["DaysFrom"].Value) == 0 && Conversion.ToString((object)r.Cells["PaymentTermId"].Text) == "Credit")
											{
												throw new Exception("Days From Field Required In Row No " + (r.RowIndex + 1));
											}
											vd.DaysFrom = Conversion.ToInt(r.Cells["DaysFrom"].Value);
											if (Conversion.ToInt(r.Cells["DaysTo"].Value) == 0 && Conversion.ToInt(r.Cells["PaymentTermId"].Value) == 2)
											{
												throw new Exception("Days To Field Required In Row No " + (r.RowIndex + 1));
											}
											vd.DaysTo = Conversion.ToInt(r.Cells["DaysTo"].Value);
											if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0)
											{
												vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
												if (Conversion.ToDouble(r.Cells["RateUom"].Value) == 0.0 && Conversion.ToString((object)r.Cells["CalculationParameterId"].Text) == "Weight")
												{
													throw new Exception("RateUom Field Required In Row No " + (r.RowIndex + 1));
												}
												vd.RateUom = Conversion.ToDecimal(r.Cells["RateUom"].Value);
												po.CommTradeRevenueExpenseScheduleList.Add(vd);
												continue;
											}
											throw new Exception("Rate Field Required In Row No " + (r.RowIndex + 1));
										}
										throw new Exception("Calculation Parameter Field Required In Row No " + (r.RowIndex + 1));
									}
									throw new Exception("Charges/Incentive Account Field Required In Row No " + (r.RowIndex + 1));
								}
								throw new Exception("Payment Term Field Required In Row No " + (r.RowIndex + 1));
							}
							throw new Exception("Transaction Type Field Required In Row No " + (r.RowIndex + 1));
						}
						throw new Exception("EffectiveDate Field Required In Row No " + (r.RowIndex + 1));
					}
					throw new Exception("IsRevenueExpense Field Required In Row No " + (r.RowIndex + 1));
				}
				if (lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						po.CommTradeRevenueExpenseScheduleList.Add(lstRemoveRecord[i]);
					}
				}
				po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				int success = CommTradeRevenueExpenseSchedule.Save(po);
				po.Id = success;
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully.... ");
				}
				else
				{
					MessageBox.Show("Data Save Successfully.... ");
				}
				Reset();
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
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
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeRevenueExpenseSchedule.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Record Delete Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("No Record found To Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			CommTradeRevenueExpenseSchedule po = new CommTradeRevenueExpenseSchedule();
			RecId = Id;
			po = CommTradeRevenueExpenseSchedule.ReadById(Id);
			cmbsuppliername.Value = Conversion.ToInt((object)po.SupplierCustomerId);
			txtpolicycode.Text = Conversion.ToString((object)po.PolicyCode);
			txtRemarksHeader.Text = Conversion.ToString((object)po.RemarksHeader);
			tabControl1.SelectedIndex = 0;
			dtDetail.Clear();
			for (int i = 0; i < po.CommTradeRevenueExpenseScheduleList.Count; i++)
			{
				dtDetail.Rows.Add(po.CommTradeRevenueExpenseScheduleList[i].Id, po.CommTradeRevenueExpenseScheduleList[i].IsRevenueExpense, po.CommTradeRevenueExpenseScheduleList[i].EffectiveDate, po.CommTradeRevenueExpenseScheduleList[i].TransactionTypeId, po.CommTradeRevenueExpenseScheduleList[i].ChargesIncentiveId, po.CommTradeRevenueExpenseScheduleList[i].PaymentTermId, po.CommTradeRevenueExpenseScheduleList[i].DaysFrom, po.CommTradeRevenueExpenseScheduleList[i].DaysTo, po.CommTradeRevenueExpenseScheduleList[i].CalculationParameterId, po.CommTradeRevenueExpenseScheduleList[i].Rate, po.CommTradeRevenueExpenseScheduleList[i].RateUom);
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			CBGrdidSetting();
			btnSave.Visible = false;
			btnUpdate.Visible = true;
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
			if (tabControl1.SelectedIndex == 1)
			{
				gridhistoryfill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaddAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.BranchesId = UserAccount.BranchesId;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.DocumentTypeId = 1700;
			reports.NoOfRecords = NoOfRecords;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			DataTable dtGridHistory = new DataTable();
			dtGridHistory = CommTradeRevenueExpenseSchedule.FormHistory(reports);
			if (dtGridHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("PolicyCode", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("ApprovedUserName", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridHistory.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridHistory.Rows[i]["RecordNo"], dtGridHistory.Rows[i]["Id"], dtGridHistory.Rows[i]["DocumentTypeId"], dtGridHistory.Rows[i]["SupplierCustomerId"], dtGridHistory.Rows[i]["SupplierName"], dtGridHistory.Rows[i]["PolicyCode"], dtGridHistory.Rows[i]["RemarksHeader"], dtGridHistory.Rows[i]["EntryUserName"], dtGridHistory.Rows[i]["EntryDate"], dtGridHistory.Rows[i]["ModifyUserName"], dtGridHistory.Rows[i]["ModifyDate"], dtGridHistory.Rows[i]["IsApproved"], dtGridHistory.Rows[i]["ApprovedUserName"], dtGridHistory.Rows[i]["ApprovedDate"]);
				}
				grdHistory.DataSource = dtcol;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
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

	private void HistoryGridSettings()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Caption = "Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
				grdHistory.RootTable.Columns["Print"].Width = 50;
			}
			if (formright.DoHavePrintRights && formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.RootTable.Columns["Print"].Position = 1;
				grdHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GridDetailBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			DataTable po = new DataTable();
			po = CommTradeRevenueExpenseSchedule.FormHistoryDetail(Conversion.ToInt(r.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dtdetailhistory = new DataTable();
				dtdetailhistory.Columns.Add("Id", typeof(int));
				dtdetailhistory.Columns.Add("IsRevenueExpense", typeof(string));
				dtdetailhistory.Columns.Add("EffectiveDate", typeof(DateTime));
				dtdetailhistory.Columns.Add("TransactionType", typeof(string));
				dtdetailhistory.Columns.Add("Charges/IncentiveAccount", typeof(string));
				dtdetailhistory.Columns.Add("PaymentTerm", typeof(string));
				dtdetailhistory.Columns.Add("DaysFrom", typeof(int));
				dtdetailhistory.Columns.Add("DaysTo", typeof(int));
				dtdetailhistory.Columns.Add("CalculationParameter", typeof(string));
				dtdetailhistory.Columns.Add("Rate", typeof(double));
				dtdetailhistory.Columns.Add("RateUom", typeof(double));
				for (int i = 0; i < po.Rows.Count; i++)
				{
					dtdetailhistory.Rows.Add(po.Rows[i]["Id"], po.Rows[i]["IsRevenueExpense"], po.Rows[i]["EffectiveDate"], po.Rows[i]["TransactionType"], po.Rows[i]["ChargesIncentive"], po.Rows[i]["PaymentTermId"], po.Rows[i]["DaysFrom"], po.Rows[i]["DaysTo"], po.Rows[i]["CalculationParameter"], po.Rows[i]["Rate"], po.Rows[i]["RateUom"]);
				}
				grdHistoryDetail.DataSource = dtdetailhistory;
				grdHistoryDetail.RetrieveStructure();
				GridHistoryDetailSetting();
			}
		}
		catch (Exception)
		{
		}
	}

	private void GridHistoryDetailSetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				Reset();
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				return;
			}
			throw new Exception("You Don't have Update Right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionBrokerySchedule_KeyDown(object sender, KeyEventArgs e)
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
				}
				else
				{
					tabControl1.SelectedIndex = 1;
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
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 80;
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbsuppliername.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbsuppliername.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Name");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void BtnShortCutKeys_Click(object sender, EventArgs e)
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r2 = grdHistory.CurrentRow;
				if (r2 != null && (int)r2.RowType == 0 && grdHistory.CurrentColumn != null)
				{
					Conversion.ToInt((object)r2.Cells["Id"].Value.ToString());
					if (grdHistory.CurrentColumn.Key == "Edit")
					{
						if (!formright.DoHaveUpdateRights)
						{
							throw new Exception("You Don't have Update Right");
						}
						Reset();
						RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
						ReadById(RecId);
					}
					if (grdHistory.CurrentColumn.Key == "Print" && !formright.DoHavePrintRights)
					{
						throw new Exception("You Don't have Print Right");
					}
				}
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				return;
			}
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("You Don't have Update Right");
				}
				Reset();
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
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
			Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (grd.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					CommTradeRevenueExpenseScheduleDetail vd = new CommTradeRevenueExpenseScheduleDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.EffectiveDate = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
					vd.TransactionTypeId = Conversion.ToInt(r.Cells["TransactionTypeId"].Value);
					vd.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vd.ChargesIncentiveId = Conversion.ToInt(r.Cells["ChargesIncentiveAccountId"].Value);
					vd.CalculationParameterId = Conversion.ToInt(r.Cells["CalculationParameterId"].Value);
					vd.DaysFrom = Conversion.ToInt(r.Cells["DaysFrom"].Value);
					vd.DaysTo = Conversion.ToInt(r.Cells["DaysTo"].Value);
					vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					vd.RateUom = Conversion.ToDecimal(r.Cells["RateUom"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (grd.CurrentColumn.Key == "Add")
			{
				AddRowInGrid();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Expected O, but got Unknown
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_1904: Unknown result type (might be due to invalid IL or missing references)
		//IL_190e: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.TransactionForms.CommissionBrokerySchedule));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarksHeader = new System.Windows.Forms.RichTextBox();
		this.cmbsuppliername = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtpolicycode = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.History = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoaddAllHistory = new System.Windows.Forms.ToolStripButton();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip1.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		this.panel12.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.BtnPrint, this.BtnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(61, 24);
		this.BtnPrint.Text = "Print";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutKeys.Image");
		this.BtnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeys.Name = "BtnShortCutKeys";
		this.BtnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutKeys.Text = "ShortCut Keys";
		this.BtnShortCutKeys.Click += new System.EventHandler(BtnShortCutKeys_Click);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar1);
		this.panel17.Controls.Add(this.label2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 144);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(950, 24);
		this.panel17.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(897, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(51, 22);
		this.ctrlGrdBar1.TabIndex = 44;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 168);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(950, 463);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 40;
		this.label2.Text = "Detail";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(253, 19);
		this.label1.TabIndex = 40;
		this.label1.Text = "Commission Brokery Schedule";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 54);
		this.panel1.TabIndex = 4;
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtpolicycode);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(943, 82);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtRemarksHeader.Location = new System.Drawing.Point(428, 18);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(509, 54);
		this.txtRemarksHeader.TabIndex = 2;
		this.txtRemarksHeader.Text = "";
		this.txtRemarksHeader.Leave += new System.EventHandler(txtRemarksHeader_Leave);
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(103, 18);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(267, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(376, 24);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 15;
		this.label4.Text = "Remarks";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 53);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 15;
		this.label3.Text = "Policy Code";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 24);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 14;
		this.label7.Text = "Customer Name";
		this.txtpolicycode.BackColor = System.Drawing.Color.White;
		this.txtpolicycode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtpolicycode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpolicycode.Location = new System.Drawing.Point(103, 49);
		this.txtpolicycode.Name = "txtpolicycode";
		this.txtpolicycode.Size = new System.Drawing.Size(267, 23);
		this.txtpolicycode.TabIndex = 1;
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.label30);
		this.panel12.Controls.Add(this.groupBox1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 57);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 87);
		this.panel12.TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(0, 14);
		this.label30.TabIndex = 40;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 7;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.Form.Controls.Add(this.panel17);
		this.Form.Controls.Add(this.panel12);
		this.Form.Controls.Add(this.panel1);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(3);
		this.Form.Size = new System.Drawing.Size(956, 634);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.History.Controls.Add(this.panel2);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.History.Controls.Add(this.panel3);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(3);
		this.History.Size = new System.Drawing.Size(956, 634);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 459);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(950, 172);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 11;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 435);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 24);
		this.panel2.TabIndex = 10;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(232, 17);
		this.label5.TabIndex = 40;
		this.label5.Text = "Detail of the Selected Above row";
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 57);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(950, 378);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 9;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar3);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 54);
		this.panel3.TabIndex = 8;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(898, 28);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 4, 7, 4);
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(46, 25);
		this.ctrlGrdBar3.TabIndex = 44;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoaddAllHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoaddAllHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaddAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoaddAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaddAllHistory.Name = "btnLoaddAllHistory";
		this.btnLoaddAllHistory.Size = new System.Drawing.Size(82, 24);
		this.btnLoaddAllHistory.Text = "&Load All";
		this.btnLoaddAllHistory.Click += new System.EventHandler(btnLoaddAllHistory_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 19);
		this.label6.TabIndex = 40;
		this.label6.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommissionBrokerySchedule";
		this.Text = "CommissionBrokerySchedule";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CommissionBrokerySchedule_FormClosed);
		base.Load += new System.EventHandler(CommissionBrokerySchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommissionBrokerySchedule_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
