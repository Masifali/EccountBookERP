// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
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
namespace Architecture.WinApp.Mfg.Reports
{

public class frmSaleOrderHistory_Engr : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtGrid = new DataTable();

	private DataTable grdlst = new DataTable();

	private DataTable dtSalesRegister = new DataTable();

	private DataTable dtBranch = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo CmbItem;

	private Label label2;

	private Label label33;

	private Label label4;

	private Label label3;

	private TextBox txtSrFrom;

	private TextBox txtsrTo;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private Button btnshow;

	internal UltraCombo CmbParentCategory;

	private Label label7;

	internal UltraCombo CmbItemType;

	private Label label9;

	internal UltraCombo CmbItemCategory;

	private Label label8;

	private Label label10;

	private Label label11;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel5;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label12;

	internal GridEX GrdDetail;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label13;

	internal GridEX GrdMain;

	private CheckBox chkincludeDo;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Panel panel6;

	private TabPage tabSummary;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private Label label26;

	private Panel panel7;

	internal UltraCombo cmbItemtypeSumm;

	private Label label16;

	internal UltraCombo CmbCategorySumm;

	private Label label17;

	internal UltraCombo cmbParentcategorySumm;

	private Label label18;

	private Button BtnShowSumm;

	private TextBox txtToDocNoSumm;

	private TextBox txtFromDocNoSumm;

	private DateTimePicker txtToDateSumm;

	private Label label21;

	private DateTimePicker txtFromDateSumm;

	private Label label22;

	internal UltraCombo cmbItemNameSumm;

	private Label label24;

	internal UltraCombo cmbCustomerSumm;

	private Label label25;

	private Panel panel8;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo cmbcitySumm;

	private Label label30;

	internal UltraCombo cmbActivitySumm;

	private Label label14;

	private CtrlGrdBar ctrlGrdBar5;

	private CheckBox ChkDoApprovedIncludSummary;

	public DateTimePicker fromdate;

	public DateTimePicker ToDate;

	public UltraCombo CmbIsApproved;

	public UltraCombo CmbOrderStatus;

	private CtrlGrdBar ctrlGrdBar6;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private CheckBox chkSkipZero;

	private CheckBox chkSkipZeroDetail;

	private ToolStripButton btnRefreshDetail;

	private ToolStripButton btnNewSumm;

	private Label label15;

	internal UltraCombo cmbBranchNameSumm;

	private Label label5;

	internal UltraCombo cmbBranchName;

	private Label label48;

	public frmSaleOrderHistory_Engr(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 800);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			dtSalesRegister.Columns.Add("Id", typeof(int));
			dtSalesRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtSalesRegister.Columns.Add("DocDate", typeof(DateTime));
			dtSalesRegister.Columns.Add("DocNo", typeof(int));
			dtSalesRegister.Columns.Add("PartyName", typeof(string));
			dtSalesRegister.Columns.Add("ItemCode", typeof(string));
			dtSalesRegister.Columns.Add("ItemName", typeof(string));
			dtSalesRegister.Columns.Add("PackUom", typeof(string));
			dtSalesRegister.Columns.Add("OrderQty", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedQty", typeof(double));
			dtSalesRegister.Columns.Add("BalQty", typeof(double));
			dtSalesRegister.Columns.Add("OrderAmount", typeof(double));
			dtSalesRegister.Columns.Add("DispatchedAmount", typeof(double));
			dtSalesRegister.Columns.Add("BalAmount", typeof(double));
			dtSalesRegister.Columns.Add("AvgRate", typeof(double));
			dtSalesRegister.Columns.Add("PercentOfTotal", typeof(double));
			dtSalesRegister.Columns.Add("CityName", typeof(string));
			StatusFill();
			OrderStatusFill();
			BranchesFill();
			AllDropDownBind();
			ActivityFill();
			fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			txtFromDateSumm.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			PrintButtonManage();
			tabControl1.SelectedIndex = 1;
			if (((Control)(object)CmbOrderStatus).Text == string.Empty)
			{
				((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			}
			if (((Control)(object)CmbIsApproved).Text == string.Empty)
			{
				((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind(int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.CostCenterId = CostCenterId;
			obj.DocumentTypeIds = "1656";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtAll = SaleOrder.GetDataForDropDownFromSaleOrder(obj);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Customer"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowswh = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "District"
					select r;
				if (!rowswh.Any())
				{
					dtAll.Clone();
				}
				else
				{
					rowswh.CopyToDataTable();
				}
				EnumerableRowCollection<DataRow> rowsJob = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "JobLot"
					select r;
				if (!rowsJob.Any())
				{
					dtAll.Clone();
				}
				else
				{
					rowsJob.CopyToDataTable();
				}
				EnumerableRowCollection<DataRow> rowsCity = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "City"
					select r;
				DataTable dtCity = (rowsCity.Any() ? rowsCity.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsCrp = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Crop"
					select r;
				if (!rowsCrp.Any())
				{
					dtAll.Clone();
				}
				else
				{
					rowsCrp.CopyToDataTable();
				}
				EnumerableRowCollection<DataRow> rowspt = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "PackingType"
					select r;
				if (!rowspt.Any())
				{
					dtAll.Clone();
				}
				else
				{
					rowspt.CopyToDataTable();
				}
				EnumerableRowCollection<DataRow> rowsPc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ParentCategory"
					select r;
				DataTable dtParentCat = (rowsPc.Any() ? rowsPc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsIc = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemCategory"
					select r;
				DataTable dtCategory = (rowsIc.Any() ? rowsIc.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsItmType = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "ItemType"
					select r;
				DataTable dtItemType = (rowsItmType.Any() ? rowsItmType.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsRefParty = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "RefPartyDetail"
					select r;
				if (!rowsRefParty.Any())
				{
					dtAll.Clone();
				}
				else
				{
					rowsRefParty.CopyToDataTable();
				}
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParty, cmbCustomerSumm, "Id", "ReferenceName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)cmbCustomerSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbCustomerSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItem, cmbItemNameSumm, "Id", "ReferenceName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCity, cmbcitySumm, "Id", "ReferenceName", "City Name", ZeroIndex: true);
				((UltraGridBase)cmbcitySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbcitySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, cmbParentcategorySumm, "Id", "ReferenceName", "Parent Category", ZeroIndex: true);
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtParentCat, CmbParentCategory, "Id", "ReferenceName", "Parent Category", ZeroIndex: true);
				((UltraGridBase)CmbParentCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbItemCategory, "Id", "ReferenceName", "Item Category", ZeroIndex: true);
				((UltraGridBase)CmbItemCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtCategory, CmbCategorySumm, "Id", "ReferenceName", "Item Category", ZeroIndex: true);
				((UltraGridBase)CmbCategorySumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, CmbItemType, "Id", "ReferenceName", "Item Type", ZeroIndex: true);
				((UltraGridBase)CmbItemType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDL(dtItemType, cmbItemtypeSumm, "Id", "ReferenceName", "Item Type", ZeroIndex: true);
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			((UltraGridBase)cmbBranchNameSumm).DataSource = null;
			dtBranch = SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 1656);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
				DDL.BindDDL(dtBranch, cmbBranchNameSumm, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)cmbBranchNameSumm).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbBranchNameSumm.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchNameSumm.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchNameSumm).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void resetDetailRegister()
	{
		try
		{
			chkincludeDo.Checked = false;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			txtSrFrom.Clear();
			txtsrTo.Clear();
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			fromdate.Focus();
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		resetDetailRegister();
	}

	private void frmSaleOrderHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefreshDetail_Click(object sender, EventArgs e)
	{
		try
		{
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApprove");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApproved, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Cancel");
			dt.Rows.Add(3, "Complete");
			DDL.BindDDL(dt, CmbOrderStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdsaleorder.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdsaleorder.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
				DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
				if (grdsaleorder.CurrentColumn.Key == "IsApproved")
				{
					ApproveRecord(Id);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Complete")
				{
					CompleteStatus(Id);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Cancel")
				{
					CancelStatus(Id, DispatchQty);
				}
				else if (grdsaleorder.CurrentColumn.Key == "Update")
				{
					UpdateExpiryDate(Id, date);
				}
				else if (grdsaleorder.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
				}
				else if (grdsaleorder.CurrentColumn.Key == "DocNo")
				{
					CommonServices.SaleOrderSlipEngr_1656(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdMain).Name);
			if (fs != null)
			{
				GrdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetail).Name);
			if (fs != null)
			{
				GrdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdlst.Rows.Count == 0 || grdlst == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(grdlst, "1656_SaleOrderDetailRegister.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (tabControl2.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.F5)
				{
					fromdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					print_Click(null, null);
				}
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshDetail_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						((Control)(object)grdsaleorder).Focus();
					}
					else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GrdDetail).Focus();
					}
					else
					{
						((Control)(object)GrdMain).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.T)
				{
					tabControl2.SelectedIndex = 1;
					txtFromDateSumm.Focus();
				}
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
				{
					Close();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtFromDateSumm.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrintSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnShowSumm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewSumm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshSummary_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.T)
				{
					tabControl2.SelectedIndex = 0;
					fromdate.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveRecord(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CompleteStatus(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Complete Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Status";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Complete Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CancelStatus(int Id, double dispatchQty)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			if (dispatchQty > 0.0)
			{
				MessageBox.Show("Record Not Cancel because Dispatch Qty gratter than zero");
				resetDetailRegister();
			}
			else if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Cancel";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Cancel Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateExpiryDate(int Id, DateTime postData)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Update ExpiryDate?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "UpdateExpiryDate";
				item.EntryUser = UserAccount.ID;
				item.PostDate = postData;
				val.AllApprovalLists.Add(item);
				SaleOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Update ExpiryDate Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridHisory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = ToDate.Value;
			obj.DocumentTypeId = 1656;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtsrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			obj.Status = Conversion.ToString((object)((Control)(object)CmbOrderStatus).Text);
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (chkincludeDo.Checked)
			{
				obj.ActionId = 1;
			}
			if (chkSkipZeroDetail.Checked)
			{
				obj.SkipZero = 1;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				grdlst = SaleOrder.SaleOrderDetailRegister_Mfg(obj);
				if (grdlst.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("DocumentTypeId", typeof(int));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("OrderSupCustId", typeof(int));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("ReferenceNo", typeof(string));
					dt.Columns.Add("PaymentTerm", typeof(string));
					dt.Columns.Add("DeliveryTerm", typeof(string));
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("ItemCode", typeof(string));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("ItemDescription", typeof(string));
					dt.Columns.Add("PackUom", typeof(string));
					dt.Columns.Add("VariantDescription", typeof(string));
					dt.Columns.Add("CastingType", typeof(string));
					dt.Columns.Add("ItemQty", typeof(double));
					dt.Columns.Add("DispatchQty", typeof(double));
					dt.Columns.Add("BalQty", typeof(double));
					dt.Columns.Add("ItemRate", typeof(double));
					dt.Columns.Add("ItemAmount", typeof(double));
					dt.Columns.Add("DispatchAmount", typeof(double));
					dt.Columns.Add("BalAmount", typeof(double));
					dt.Columns.Add("CityName", typeof(string));
					dt.Columns.Add("DetailRemarks", typeof(string));
					dt.Columns.Add("DueDays", typeof(int));
					dt.Columns.Add("DueDate", typeof(DateTime));
					dt.Columns.Add("OrderStatus", typeof(string));
					dt.Columns.Add("ApprovedStatus", typeof(string));
					dt.Columns.Add("ExpiryDate", typeof(DateTime));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("EntryUser", typeof(string));
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ModifyUser", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUser", typeof(string));
					dt.Columns.Add("Remarks", typeof(string));
					dt.Clear();
					foreach (DataRow r in grdlst.Rows)
					{
						dt.Rows.Add(r["Id"], r["DocumentTypeId"], r["DocNo"], Convert.ToDateTime(r["DocDate"]).ToString("dd-MMM-yyyy"), r["OrderSupCustId"], r["CustomerName"], r["SupplierRefNo"], r["TermsDescription"], r["DeliveryTerm"], r["OrderItemId"], r["ItemCode"], r["ItemName"], r["ItemDiscription"], r["UOMCodeItm"], r["VarientDescription"], r["CastingType"], r["OrderItemQty"], r["DispatchQty"], r["BalQty"], r["OrderItemRate"], r["Amount"], r["DispatchAmount"], r["BalAmount"], r["CityName"], r["OrderRemarks"], r["OrderDueDays"], r["OrderDueDate"], r["OrderStatus"], r["ApprovedStatus"], r["OrderExpiryDate"], r["EntryDate"], r["UserNameEusr"], r["ModifyDate"], r["UserNameMusr"], r["PostDate"], r["UserNameAusr"], r["RemarksHeader"]);
					}
					grdsaleorder.DataSource = dt;
					grdsaleorder.RetrieveStructure();
					GridSettings();
					DataTable dtMain = new DataTable();
					dtMain.Columns.Add("Id", typeof(string));
					dtMain.Columns.Add("DocumentTypeId", typeof(int));
					dtMain.Columns.Add("DocNo", typeof(int));
					dtMain.Columns.Add("DocDate", typeof(DateTime));
					dtMain.Columns.Add("SupplierCustomerId", typeof(int));
					dtMain.Columns.Add("CustomerName", typeof(string));
					dtMain.Columns.Add("OrderStatus", typeof(string));
					dtMain.Columns.Add("ApprovedStatus", typeof(string));
					dtMain.Columns.Add("PaymentTerm", typeof(string));
					dtMain.Columns.Add("DueDays", typeof(int));
					dtMain.Columns.Add("DueDate", typeof(DateTime));
					dtMain.Columns.Add("DeliveryTerm", typeof(string));
					dtMain.Columns.Add("ItemQty", typeof(double));
					dtMain.Columns.Add("DispatchQty", typeof(double));
					dtMain.Columns.Add("BalQty", typeof(double));
					dtMain.Columns.Add("OrderAmount", typeof(double));
					dtMain.Columns.Add("DispatchAmount", typeof(double));
					dtMain.Columns.Add("BalAmount", typeof(double));
					dtMain.Columns.Add("ExpiryDate", typeof(DateTime));
					dtMain.Columns.Add("EntryDate", typeof(DateTime));
					dtMain.Columns.Add("EntryUser", typeof(string));
					dtMain.Columns.Add("ModifyDate", typeof(DateTime));
					dtMain.Columns.Add("ModifyUser", typeof(string));
					dtMain.Columns.Add("ApprovedDate", typeof(DateTime));
					dtMain.Columns.Add("ApprovedUser", typeof(string));
					dtMain.Columns.Add("Remarks", typeof(string));
					HashSet<string> uniqueIds = new HashSet<string>();
					foreach (DataRow row in grdlst.Rows)
					{
						string id = Conversion.ToString(row["Id"]);
						if (!uniqueIds.Contains(id))
						{
							uniqueIds.Add(id);
							string filter = "Id = '" + id + "'";
							double itemQty = Conversion.ToDouble(grdlst.Compute("SUM(OrderItemQty)", filter));
							double dispQty = Conversion.ToDouble(grdlst.Compute("SUM(DispatchQty)", filter));
							double balQty = Conversion.ToDouble(grdlst.Compute("SUM(BalQty)", filter));
							double amount = Conversion.ToDouble(grdlst.Compute("SUM(Amount)", filter));
							double dispAmount = Conversion.ToDouble(grdlst.Compute("SUM(DispatchAmount)", filter));
							double balAmount = Conversion.ToDouble(grdlst.Compute("SUM(BalAmount)", filter));
							dtMain.Rows.Add(id, row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["OrderSupCustId"], row["CustomerName"], row["OrderStatus"], row["ApprovedStatus"], row["TermsDescription"], row["OrderDueDays"], row["OrderDueDate"], row["DeliveryTerm"], itemQty, dispQty, balQty, amount, dispAmount, balAmount, row["OrderExpiryDate"], row["EntryDate"], row["UserNameEusr"], row["ModifyDate"], row["UserNameMusr"], row["PostDate"], row["UserNameAusr"], row["RemarksHeader"]);
						}
					}
					GrdMain.DataSource = dtMain;
					GrdMain.RetrieveStructure();
					MainGridSetting();
				}
				else
				{
					grdsaleorder.DataSource = null;
					GrdMain.ClearStructure();
					GrdDetail.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdsaleorder.RootTable.Columns["OrderSupCustId"].Visible = false;
			grdsaleorder.RootTable.Columns["ItemId"].Visible = false;
			grdsaleorder.RootTable.Columns["PackUom"].Caption = "Uom";
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			grdsaleorder.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdsaleorder, 2, 3);
			grdsaleorder.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdsaleorder.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdsaleorder.RootTable.Columns["ReferenceNo"].Width = Constants.InventoryConstants.RecordNo;
			grdsaleorder.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdsaleorder.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdsaleorder.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdsaleorder.RootTable.Columns["ItemDescription"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdsaleorder.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.Variant;
			grdsaleorder.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			grdsaleorder.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdsaleorder.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdsaleorder.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["DispatchAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdsaleorder.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.StringConstant;
			grdsaleorder.RootTable.Columns["DetailRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdsaleorder.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdsaleorder.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["OrderStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdsaleorder.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdsaleorder.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdsaleorder.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdsaleorder.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdsaleorder.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			if (((Control)(object)CmbIsApproved).Text == "Approve")
			{
				grdsaleorder.RootTable.Columns.Add("Complete");
				grdsaleorder.RootTable.Columns["Complete"].Caption = "Complete";
				grdsaleorder.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Complete"].ButtonText = "Complete";
				grdsaleorder.RootTable.Columns["Complete"].Width = 80;
				grdsaleorder.RootTable.Columns.Add("Cancel");
				grdsaleorder.RootTable.Columns["Cancel"].Caption = "Cancel";
				grdsaleorder.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Cancel"].ButtonText = "Cancel";
				grdsaleorder.RootTable.Columns["Cancel"].Width = 60;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				grdsaleorder.RootTable.Columns.Add("IsApproved");
				grdsaleorder.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				grdsaleorder.RootTable.Columns["IsApproved"].Width = 80;
				grdsaleorder.RootTable.Columns["IsApproved"].Visible = false;
			}
			if (((Control)(object)CmbOrderStatus).Text == "Open" && ((Control)(object)CmbIsApproved).Text == "Approve")
			{
				grdsaleorder.AllowEdit = (InheritableBoolean)1;
				grdsaleorder.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
				grdsaleorder.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
				grdsaleorder.RootTable.Columns.Add("Update");
				grdsaleorder.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
				grdsaleorder.RootTable.Columns["Update"].Width = 120;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		gridHisory();
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Complete")
			{
				CompleteStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDouble(grdsaleorder.CurrentRow.Cells["DispatchQty"].Value));
			}
			if (e.Column.Key == "Update")
			{
				UpdateExpiryDate(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(grdsaleorder.CurrentRow.Cells["ExpiryDate"].Value));
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grdsaleorder.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				CommonServices.SaleOrderSlipEngr_1656(Conversion.ToInt(item.Cells["Id"].Value));
			}
			else if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["OrderSupCustId"].Value)), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MainGridSetting()
	{
		try
		{
			GrdMain.RootTable.Columns["Id"].Visible = false;
			GrdMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdMain.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdMain.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GrdMain.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(GrdMain, 2, 3);
			GrdMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			GrdMain.RootTable.Columns["OrderStatus"].Width = Constants.InventoryConstants.StatusConstant;
			GrdMain.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant;
			GrdMain.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdMain.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			GrdMain.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdMain.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdMain.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdMain.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdMain.RootTable.Columns["OrderAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdMain.RootTable.Columns["DispatchAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdMain.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdMain.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdMain.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdMain.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			if (((Control)(object)CmbIsApproved).Text == "Approve" && ((Control)(object)CmbOrderStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					GrdMain.RootTable.Columns.Add("Complete");
					GrdMain.RootTable.Columns["Complete"].Caption = "Complete";
					GrdMain.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Complete"].ButtonText = "Complete";
					GrdMain.RootTable.Columns["Complete"].Width = 80;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					GrdMain.RootTable.Columns.Add("Cancel");
					GrdMain.RootTable.Columns["Cancel"].Caption = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Cancel"].ButtonText = "Cancel";
					GrdMain.RootTable.Columns["Cancel"].Width = 60;
				}
				if (formright.CanChangeOrderExpiryDate)
				{
					GrdMain.AllowEdit = (InheritableBoolean)1;
					GrdMain.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
					GrdMain.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
					GrdMain.RootTable.Columns.Add("Update");
					GrdMain.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
					GrdMain.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
					GrdMain.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
					GrdMain.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
					GrdMain.RootTable.Columns["Update"].Width = 120;
				}
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApprove")
			{
				GrdMain.RootTable.Columns.Add("IsApproved");
				GrdMain.RootTable.Columns["IsApproved"].Caption = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdMain.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				GrdMain.RootTable.Columns["IsApproved"].ButtonText = "IsApproved";
				GrdMain.RootTable.Columns["IsApproved"].Width = 60;
				GrdMain.RootTable.Columns["IsApproved"].Visible = false;
			}
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value));
			}
			else if (e.Column.Key == "Complete")
			{
				if (((Control)(object)CmbIsApproved).Text == "Approve")
				{
					CompleteStatus(Conversion.ToInt((object)GrdMain.CurrentRow.Cells["Id"].Value.ToString()));
				}
			}
			else if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt((object)GrdMain.CurrentRow.Cells["Id"].Value.ToString()), Conversion.ToDouble(GrdMain.CurrentRow.Cells["DispatchQty"].Value));
			}
			else if (e.Column.Key == "Update")
			{
				UpdateExpiryDate(Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value), Conversion.ToDateTime(GrdMain.CurrentRow.Cells["ExpiryDate"].Value));
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GrdMain.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GrdMain.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GrdMain.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				CommonServices.SaleOrderSlipEngr_1656(Conversion.ToInt(item.Cells["Id"].Value));
			}
			else if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GrdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GrdMain.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				double DispatchQty = Conversion.ToDouble(r.Cells["DispatchQty"].Value);
				DateTime date = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
				if (GrdMain.CurrentColumn.Key == "IsApproved")
				{
					ApproveRecord(Id);
				}
				else if (GrdMain.CurrentColumn.Key == "Complete")
				{
					CompleteStatus(Id);
				}
				else if (GrdMain.CurrentColumn.Key == "Cancel")
				{
					CancelStatus(Id, DispatchQty);
				}
				else if (GrdMain.CurrentColumn.Key == "Update")
				{
					UpdateExpiryDate(Id, date);
				}
				else if (GrdMain.CurrentColumn.Key == "CustomerName")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value)), fromdate.Value, ToDate.Value);
				}
				else if (GrdMain.CurrentColumn.Key == "DocNo")
				{
					CommonServices.SaleOrderSlipEngr_1656(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("ItemId");
				dtDetail.Columns.Add("ItemCode");
				dtDetail.Columns.Add("ItemName");
				dtDetail.Columns.Add("Uom");
				dtDetail.Columns.Add("VariantDescription");
				dtDetail.Columns.Add("CastingType");
				dtDetail.Columns.Add("ItemQty", typeof(double));
				dtDetail.Columns.Add("DispatchQty", typeof(double));
				dtDetail.Columns.Add("BalQty", typeof(double));
				dtDetail.Columns.Add("ItemRate", typeof(double));
				dtDetail.Columns.Add("ItemAmount", typeof(double));
				dtDetail.Columns.Add("DispatchAmount", typeof(double));
				dtDetail.Columns.Add("BalAmount", typeof(double));
				dtDetail.Columns.Add("TaxType", typeof(string));
				dtDetail.Columns.Add("Tax%", typeof(double));
				dtDetail.Columns.Add("TaxAmount", typeof(double));
				dtDetail.Columns.Add("CityName");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["VarientDescription"], resultRows.Rows[i]["CastingType"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalQty"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["DispatchAmount"], resultRows.Rows[i]["BalAmount"], resultRows.Rows[i]["TaxName"], resultRows.Rows[i]["TaxPercent"], resultRows.Rows[i]["TaxAmount"], resultRows.Rows[i]["CityName"]);
				}
				GrdDetail.DataSource = dtDetail;
				GrdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GrdDetail.RootTable.Columns["ItemId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(GrdDetail, 2, 3);
			GrdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.StringConstant;
			GrdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GrdDetail.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			GrdDetail.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			GrdDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			GrdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["DispatchQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetail.RootTable.Columns["DispatchAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetail.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetail.RootTable.Columns["TaxType"].Width = Constants.InventoryConstants.ItemType;
			GrdDetail.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.PercentOfTotal;
			GrdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.StringConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GrdDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["SaleGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GrdDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GrdDetail.CurrentColumn != null && GrdDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["SaleGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSrFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsrTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ActivityFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Order Register");
			dt.Rows.Add(2, "Order Summary By Item");
			dt.Rows.Add(3, "Order Summary By Item & Pack Size");
			dt.Rows.Add(4, "Order Summary By Item & City");
			dt.Rows.Add(5, "Order Summary By Item,Pack Size & City");
			dt.Rows.Add(6, "Order Summary By Customer");
			dt.Rows.Add(7, "Order Summary By Customer & OrderNo");
			dt.Rows.Add(8, "Order Summary By Customer & Item");
			dt.Rows.Add(9, "Order Summary By Customer & Pack Size");
			dt.Rows.Add(10, "Order Summary By Customer & City");
			dt.Rows.Add(11, "Order Summary By Customer,Item & City");
			DDL.BindDDL(dt, cmbActivitySumm, "Id", "Name", "Activity", ZeroIndex: false);
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSummary_Click(object sender, EventArgs e)
	{
		try
		{
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewSumm_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDateSumm.Value = DateTime.Now;
			txtToDateSumm.Value = DateTime.Now;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
			txtFromDateSumm.Focus();
			DataGridHistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbActivitySumm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryFill()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow == null || string.IsNullOrEmpty(((Control)(object)cmbActivitySumm).Text))
			{
				throw new Exception("Please Select and Activity first...");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDateSumm.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSumm.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSumm.Text);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.ReportType = ((Control)(object)cmbActivitySumm).Text;
			if (ChkDoApprovedIncludSummary.Checked)
			{
				obj.ActionId = 1;
			}
			obj.ApprovedFilter = "All";
			if (chkSkipZero.Checked)
			{
				obj.SkipZero = 1;
			}
			dtGrid = SaleOrder.SaleOrderEng_SummaryRegister(obj);
			if (dtGrid.Rows.Count > 0)
			{
				((Control)(object)DataGridHistory).Name = ((Control)(object)cmbActivitySumm).Text.Trim().Replace(" ", "");
				dtSalesRegister.Rows.Clear();
				foreach (DataRow r in dtGrid.Rows)
				{
					dtSalesRegister.Rows.Add(r["Id"], r["DocumentTypeId"], Conversion.ToDateTime(r["DocDate"]).ToShortDateString(), r["DocNo"], r["Customer"], r["ItemCode"], r["ItemName"], r["UOMCode"], r["OrderQty"], r["DispatchQty"], r["BalQty"], r["OrderAmount"], r["DispatchAmount"], r["BalAmount"], r["AvgRate"], r["PrcntOfTotal"], r["CityName"]);
				}
				DataGridHistory.DataSource = dtSalesRegister;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["PackUom"].Caption = "Uom";
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 3);
				item.FilterEditType = (FilterEditType)((!(item.Type == typeof(DateTime))) ? 1 : 6);
				item.Visible = false;
			}
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistory.RootTable.Columns["OrderQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["DispatchedQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["OrderAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["DispatchedAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["PercentOfTotal"].Width = Constants.InventoryConstants.PercentOfTotal;
			DataGridHistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.StringConstant;
			Dictionary<string, List<string>> visibleColumnsMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
			{
				{
					"Order Register",
					new List<string>
					{
						"DocDate", "DocNo", "PartyName", "ItemCode", "ItemName", "PackUom", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount",
						"DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal", "CityName"
					}
				},
				{
					"Order Summary By Item",
					new List<string> { "ItemCode", "ItemName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal" }
				},
				{
					"Order Summary By Item & Pack Size",
					new List<string>
					{
						"ItemCode", "ItemName", "PackUom", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate",
						"PercentOfTotal"
					}
				},
				{
					"Order Summary By Item & City",
					new List<string>
					{
						"ItemCode", "ItemName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal",
						"CityName"
					}
				},
				{
					"Order Summary By Item,Pack Size & City",
					new List<string>
					{
						"ItemCode", "ItemName", "PackUom", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate",
						"PercentOfTotal", "CityName"
					}
				},
				{
					"Order Summary By Customer",
					new List<string> { "PartyName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal" }
				},
				{
					"Order Summary By Customer & OrderNo",
					new List<string>
					{
						"PartyName", "DocDate", "DocNo", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate",
						"PercentOfTotal"
					}
				},
				{
					"Order Summary By Customer & Item",
					new List<string>
					{
						"PartyName", "ItemCode", "ItemName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate",
						"PercentOfTotal"
					}
				},
				{
					"Order Summary By Customer & City",
					new List<string> { "PartyName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal", "CityName" }
				},
				{
					"Order Summary By Customer,Item & City",
					new List<string>
					{
						"PartyName", "ItemCode", "ItemName", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate",
						"PercentOfTotal", "CityName"
					}
				},
				{
					"Order Summary By Customer & Pack Size",
					new List<string> { "PartyName", "PackUom", "OrderQty", "DispatchedQty", "BalQty", "OrderAmount", "DispatchedAmount", "BalAmount", "AvgRate", "PercentOfTotal" }
				}
			};
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbActivitySumm).Text) && visibleColumnsMap.TryGetValue(((Control)(object)cmbActivitySumm).Text.Trim(), out var columnsToShow))
			{
				foreach (string colName in columnsToShow)
				{
					if (DataGridHistory.RootTable.Columns.Contains(colName))
					{
						DataGridHistory.RootTable.Columns[colName].Visible = true;
					}
				}
			}
			ctrlGrdBar5_Load(null, null);
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
			txtFromDateSumm.Focus();
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrintSummary_Click(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string ReportName = Conversion.ToString((object)btnPrintSumm.Text) + ".rpt";
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, ReportName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (((Control)(object)cmbActivitySumm).Text == "Order Register")
				{
					btnPrintSumm.Text = "1656_01_OrderRegister";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & Pack Size")
				{
					btnPrintSumm.Text = "1656_02_OrderSummaryByItem&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item,Pack Size & City")
				{
					btnPrintSumm.Text = "1656_03_OrderSummaryByItem,PackSize&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Pack Size")
				{
					btnPrintSumm.Text = "1656_04_OrderSummaryByCustomer&PackSize";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
				{
					btnPrintSumm.Text = "1656_05_OrderSummaryByItem";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item & City")
				{
					btnPrintSumm.Text = "1656_06_OrderSummaryByItem&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer")
				{
					btnPrintSumm.Text = "1656_07_OrderSummaryByCustomer";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Sales Summary By Customer & City")
				{
					btnPrintSumm.Text = "1656_08_OrderSummaryByCustomer&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & Item")
				{
					btnPrintSumm.Text = "1656_09_OrderSummaryByCustomer&Item";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer,Item & City")
				{
					btnPrintSumm.Text = "1656_10_OrderSummaryByCustomer,Item&City";
				}
				else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Customer & OrderNo")
				{
					btnPrintSumm.Text = "1656_11_OrderSummaryByCustomer&Order";
				}
			}
			else
			{
				btnPrintSumm.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Expected O, but got Unknown
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Expected O, but got Unknown
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Expected O, but got Unknown
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Expected O, but got Unknown
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Expected O, but got Unknown
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Expected O, but got Unknown
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Expected O, but got Unknown
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_10db: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e5: Expected O, but got Unknown
		//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fc: Expected O, but got Unknown
		//IL_14bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c5: Expected O, but got Unknown
		//IL_14e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f3: Expected O, but got Unknown
		//IL_17a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17af: Expected O, but got Unknown
		//IL_4ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ed9: Expected O, but got Unknown
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance165 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Mfg.Reports.frmSaleOrderHistory_Engr));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdMain = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.GrdDetail = new GridEX();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.chkSkipZeroDetail = new System.Windows.Forms.CheckBox();
		this.CmbItemType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.chkincludeDo = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.CmbOrderStatus = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.CmbItemCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.CmbParentCategory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtsrTo = new System.Windows.Forms.TextBox();
		this.CmbIsApproved = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshDetail = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.tabSummary = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.label26 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchNameSumm = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbItemtypeSumm = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.cmbParentcategorySumm = new UltraCombo();
		this.CmbCategorySumm = new UltraCombo();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbCustomerSumm = new UltraCombo();
		this.ChkDoApprovedIncludSummary = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbActivitySumm = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbcitySumm = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewSumm = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabSummary.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(980, 661);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabSummary);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 26);
		this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(980, 635);
		this.tabControl2.TabIndex = 5;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Margin = new System.Windows.Forms.Padding(4);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(4);
		this.tabDetail.Size = new System.Drawing.Size(972, 606);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail Register";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(4, 4);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 598);
		this.panel6.TabIndex = 6;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 165);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 433);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.label6);
		this.tabPage1.Location = new System.Drawing.Point(4, 24);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
		this.tabPage1.Size = new System.Drawing.Size(956, 405);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(4, 27);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(948, 374);
		this.panel5.TabIndex = 25;
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		this.grdsaleorder.AlternatingColors = true;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		this.grdsaleorder.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(948, 374);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(907, 5);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 22);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 4);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(948, 23);
		this.label6.TabIndex = 1;
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdMain);
		this.tabPage2.Controls.Add(this.label13);
		this.tabPage2.Controls.Add(this.ctrlGrdBar6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetail);
		this.tabPage2.Controls.Add(this.label12);
		this.tabPage2.Controls.Add(this.ctrlGrdBar4);
		this.tabPage2.Controls.Add(this.ctrlGrdBar2);
		this.tabPage2.Location = new System.Drawing.Point(4, 24);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
		this.tabPage2.Size = new System.Drawing.Size(956, 405);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Brief";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdMain.AllowEdit = (InheritableBoolean)2;
		this.GrdMain.AlternatingColors = true;
		((ScrollableBase)this.GrdMain).BackColor = System.Drawing.Color.White;
		GrdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdMain.DesignTimeLayout = GrdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdMain.FilterMode = (FilterMode)2;
		this.GrdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdMain.GroupTotals = (GroupTotals)2;
		this.GrdMain.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdMain).Location = new System.Drawing.Point(4, 27);
		((System.Windows.Forms.Control)(object)this.GrdMain).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.GrdMain).Name = "GrdMain";
		((ScrollableBase)this.GrdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdMain).Size = new System.Drawing.Size(948, 202);
		((System.Windows.Forms.Control)(object)this.GrdMain).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.GrdMain).Tag = "MasterGrid";
		this.GrdMain.TotalRow = (InheritableBoolean)1;
		this.GrdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdMain).VisualStyle = (VisualStyle)4;
		this.GrdMain.ColumnButtonClick += new ColumnActionEventHandler(GrdMain_ColumnButtonClick);
		this.GrdMain.SelectionChanged += new System.EventHandler(GrdMain_SelectionChanged);
		this.GrdMain.LinkClicked += new ColumnActionEventHandler(GrdMain_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdMain_KeyDown);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 229);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(948, 18);
		this.label13.TabIndex = 27;
		this.label13.Text = "Detail";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(910, 5);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
		this.ctrlGrdBar6.MyGrid = this.GrdMain;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar6.TabIndex = 30;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.GrdDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdDetail.AlternatingColors = true;
		((ScrollableBase)this.GrdDetail).BackColor = System.Drawing.Color.White;
		GrdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetail.DesignTimeLayout = GrdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdDetail.FilterMode = (FilterMode)2;
		this.GrdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetail.GroupByBoxVisible = false;
		this.GrdDetail.GroupTotals = (GroupTotals)2;
		this.GrdDetail.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Location = new System.Drawing.Point(4, 247);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Name = "GrdDetail";
		((ScrollableBase)this.GrdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Size = new System.Drawing.Size(948, 154);
		((System.Windows.Forms.Control)(object)this.GrdDetail).TabIndex = 1;
		this.GrdDetail.TotalRow = (InheritableBoolean)1;
		this.GrdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetail).VisualStyle = (VisualStyle)4;
		this.GrdDetail.LinkClicked += new ColumnActionEventHandler(GrdDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdDetail_KeyDown);
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Dock = System.Windows.Forms.DockStyle.Top;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(4, 4);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(948, 23);
		this.label12.TabIndex = 2;
		this.label12.Text = "Main";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.ctrlGrdBar4.FormName = null;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(225, 185);
		this.ctrlGrdBar4.TabIndex = 33;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(0, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(225, 185);
		this.ctrlGrdBar2.TabIndex = 31;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 137);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.chkSkipZeroDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.chkincludeDo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatus);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtSrFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtsrTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 3);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
		this.groupBox1.Size = new System.Drawing.Size(961, 131);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Criteria's";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(78, 16);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(4, 22);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 151;
		this.label48.Text = "Branch Name";
		this.chkSkipZeroDetail.AutoSize = true;
		this.chkSkipZeroDetail.Checked = true;
		this.chkSkipZeroDetail.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZeroDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZeroDetail.Location = new System.Drawing.Point(669, 104);
		this.chkSkipZeroDetail.Margin = new System.Windows.Forms.Padding(4);
		this.chkSkipZeroDetail.Name = "chkSkipZeroDetail";
		this.chkSkipZeroDetail.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZeroDetail.TabIndex = 12;
		this.chkSkipZeroDetail.Text = "Skip Zero";
		this.chkSkipZeroDetail.UseVisualStyleBackColor = true;
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(356, 72);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(595, 22);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 123;
		this.label1.Text = "Customer Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance186).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(690, 16);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 9;
		this.chkincludeDo.AutoSize = true;
		this.chkincludeDo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkincludeDo.Location = new System.Drawing.Point(750, 104);
		this.chkincludeDo.Margin = new System.Windows.Forms.Padding(4);
		this.chkincludeDo.Name = "chkincludeDo";
		this.chkincludeDo.Size = new System.Drawing.Size(138, 19);
		this.chkincludeDo.TabIndex = 13;
		this.chkincludeDo.Text = "Include approved Do";
		this.chkincludeDo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.chkincludeDo.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(267, 106);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Item Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(595, 50);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(95, 15);
		this.label11.TabIndex = 141;
		this.label11.Text = "Approved Status";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(356, 100);
		((System.Windows.Forms.Control)(object)this.CmbItem).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 8;
		this.CmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Location = new System.Drawing.Point(690, 72);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Name = "CmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).TabIndex = 11;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 50);
		this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(595, 78);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 140;
		this.label10.Text = "Status";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(78, 46);
		this.fromdate.Margin = new System.Windows.Forms.Padding(4);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(183, 22);
		this.fromdate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 78);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "To Date";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(267, 78);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "Item Type";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(78, 74);
		this.ToDate.Margin = new System.Windows.Forms.Padding(4);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(183, 22);
		this.ToDate.TabIndex = 2;
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(356, 44);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 106);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "So From,To";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(267, 50);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 138;
		this.label8.Text = "Item Category";
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(78, 102);
		this.txtSrFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(91, 23);
		this.txtSrFrom.TabIndex = 3;
		this.txtSrFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrFrom_KeyPress);
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(356, 16);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(267, 22);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 138;
		this.label7.Text = "Parent Category";
		this.txtsrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsrTo.Location = new System.Drawing.Point(171, 102);
		this.txtsrTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtsrTo.Name = "txtsrTo";
		this.txtsrTo.Size = new System.Drawing.Size(90, 23);
		this.txtsrTo.TabIndex = 4;
		this.txtsrTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsrTo_KeyPress);
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(690, 44);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 10;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(897, 100);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 14;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnRefreshDetail, this.print });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(964, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefreshDetail.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshDetail.Name = "btnRefreshDetail";
		this.btnRefreshDetail.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshDetail.Text = "&Refresh";
		this.btnRefreshDetail.Click += new System.EventHandler(btnRefreshDetail_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(150, 24);
		this.print.Text = "1656_PO_Register";
		this.print.Click += new System.EventHandler(print_Click);
		this.tabSummary.Controls.Add(this.panel9);
		this.tabSummary.Controls.Add(this.panel7);
		this.tabSummary.Controls.Add(this.panel8);
		this.tabSummary.Location = new System.Drawing.Point(4, 25);
		this.tabSummary.Margin = new System.Windows.Forms.Padding(4);
		this.tabSummary.Name = "tabSummary";
		this.tabSummary.Padding = new System.Windows.Forms.Padding(4);
		this.tabSummary.Size = new System.Drawing.Size(972, 606);
		this.tabSummary.TabIndex = 1;
		this.tabSummary.Text = "Summary";
		this.tabSummary.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Controls.Add(this.label26);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(4, 184);
		this.panel9.Margin = new System.Windows.Forms.Padding(4);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(964, 418);
		this.panel9.TabIndex = 4;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(920, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.AlternatingColors = true;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(964, 394);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Dock = System.Windows.Forms.DockStyle.Top;
		this.label26.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(0, 0);
		this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(964, 24);
		this.label26.TabIndex = 3;
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(4, 41);
		this.panel7.Margin = new System.Windows.Forms.Padding(4);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(964, 143);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.chkSkipZero);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add(this.ChkDoApprovedIncludSummary);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivitySumm);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 5);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
		this.groupBox2.Size = new System.Drawing.Size(953, 133);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.cmbBranchNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Location = new System.Drawing.Point(82, 17);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Name = "cmbBranchNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 162;
		this.label5.Text = "Branch Name";
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(360, 71);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 7;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(360, 99);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 8;
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(360, 15);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(360, 43);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 6;
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(82, 50);
		this.txtFromDateSumm.Margin = new System.Windows.Forms.Padding(4);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(183, 22);
		this.txtFromDateSumm.TabIndex = 1;
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(82, 78);
		this.txtToDateSumm.Margin = new System.Windows.Forms.Padding(4);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(183, 22);
		this.txtToDateSumm.TabIndex = 2;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(82, 106);
		this.txtFromDocNoSumm.Margin = new System.Windows.Forms.Padding(4);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(91, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtFromDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNo_KeyPress);
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(174, 106);
		this.txtToDocNoSumm.Margin = new System.Windows.Forms.Padding(4);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(91, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.txtToDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.Location = new System.Drawing.Point(653, 103);
		this.chkSkipZero.Margin = new System.Windows.Forms.Padding(4);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 12;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(560, 21);
		this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(93, 15);
		this.label25.TabIndex = 123;
		this.label25.Text = "Customer Name";
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(653, 15);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 9;
		this.ChkDoApprovedIncludSummary.AutoSize = true;
		this.ChkDoApprovedIncludSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDoApprovedIncludSummary.Location = new System.Drawing.Point(730, 103);
		this.ChkDoApprovedIncludSummary.Margin = new System.Windows.Forms.Padding(4);
		this.ChkDoApprovedIncludSummary.Name = "ChkDoApprovedIncludSummary";
		this.ChkDoApprovedIncludSummary.Size = new System.Drawing.Size(138, 19);
		this.ChkDoApprovedIncludSummary.TabIndex = 13;
		this.ChkDoApprovedIncludSummary.Text = "Include approved Do";
		this.ChkDoApprovedIncludSummary.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(268, 105);
		this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 125;
		this.label24.Text = "Item Name";
		this.cmbActivitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbActivitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Location = new System.Drawing.Point(653, 71);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Name = "cmbActivitySumm";
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).TabIndex = 11;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(560, 77);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(70, 15);
		this.label14.TabIndex = 155;
		this.label14.Text = "Report Type";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 82);
		this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(63, 15);
		this.label15.TabIndex = 127;
		this.label15.Text = "From Date";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 54);
		this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(48, 15);
		this.label22.TabIndex = 127;
		this.label22.Text = "To Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(5, 110);
		this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(79, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From Doc No";
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(653, 43);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 10;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(560, 49);
		this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(62, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City Name";
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(889, 99);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(57, 26);
		this.BtnShowSumm.TabIndex = 14;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(268, 21);
		this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(91, 15);
		this.label18.TabIndex = 138;
		this.label18.Text = "Parent Category";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(268, 49);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 138;
		this.label17.Text = "Item Category";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(268, 77);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 138;
		this.label16.Text = "Item Type";
		this.panel8.Controls.Add(this.toolStrip1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(4, 4);
		this.panel8.Margin = new System.Windows.Forms.Padding(4);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(964, 37);
		this.panel8.TabIndex = 2;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewSumm, this.btnRefreshSummary, this.btnPrintSumm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(964, 37);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewSumm.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSumm.Name = "btnNewSumm";
		this.btnNewSumm.Size = new System.Drawing.Size(68, 34);
		this.btnNewSumm.Text = "Reset";
		this.btnNewSumm.Click += new System.EventHandler(btnNewSumm_Click);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 34);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 34);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSummary_Click);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(980, 26);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(156, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Sale Order Report";
		base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(980, 661);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmSaleOrderHistory_Engr";
		this.Text = "Eccountbook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleOrderHistory_FormClosing);
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabSummary.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
