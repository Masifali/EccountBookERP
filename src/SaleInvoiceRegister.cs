// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.pcc.Sale;
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
namespace Architecture.WinApp.pcc.Reports
{

public class SaleInvoiceRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtRegisteredRefParty = new DataTable();

	private DataTable dtcol = new DataTable();

	private bool HasSubsidiaryFeature;

	public DataTable dtGrid = new DataTable();

	public DataTable dtGridSummary = new DataTable();

	public DataTable dtBindCombo = new DataTable();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage2;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel7;

	private GroupBox groupBox2;

	internal UltraCombo CmbApprovalStatusSumm;

	private Label label28;

	internal UltraCombo cmbItemtypeSumm;

	private Label label26;

	internal UltraCombo cmbRefPartyDetailSumm;

	private Label label16;

	private Label label19;

	internal UltraCombo cmbCustomerSumm;

	internal UltraCombo cmbDistrictSumm;

	private Label label30;

	internal UltraCombo cmbcitySumm;

	private Label label17;

	private Label label31;

	internal UltraCombo cmbReportType;

	internal UltraCombo cmbItemNameSumm;

	private Label label18;

	private Label label29;

	internal UltraCombo cmbJobLotSumm;

	private DateTimePicker txtFromDateSumm;

	private Label label32;

	private Label label21;

	private DateTimePicker txtToDateSumm;

	private TextBox txtFromDocNoSumm;

	private TextBox txtToDocNoSumm;

	private Button BtnShowSumm;

	private Label label20;

	internal UltraCombo cmbParentcategorySumm;

	private Label label27;

	internal UltraCombo CmbCategorySumm;

	private Label label33;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnRefreshSummary;

	private ToolStripButton btnPrintSumm;

	internal UltraCombo CmbDateTypeSumm;

	private Label label40;

	private ToolStripButton BtnNewSummary;

	private CheckBox chkvarient;

	private CheckBox chkCity;

	private TextBox txtavgrate;

	private Label label2;

	private Label label1;

	internal UltraCombo cmbsaleman;

	private Label cmbsaleman1;

	internal UltraCombo CmbPartyGroup;

	private Label label3;

	private Label label4;

	internal UltraCombo CmbRegisteredRefParty;

	private ToolStripButton btnShortcut;

	public SaleInvoiceRegister(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(903, 703);
			dtRegisteredRefParty.Columns.Add("Id");
			dtRegisteredRefParty.Columns.Add("name");
			dtcol.Columns.Add("Id", typeof(int));
			dtcol.Columns.Add("DocDate", typeof(DateTime));
			dtcol.Columns.Add("DocNo", typeof(int));
			dtcol.Columns.Add("DocumentTypeId", typeof(string));
			dtcol.Columns.Add("DocumentType", typeof(string));
			dtcol.Columns.Add("SupplierCustomerId", typeof(int));
			dtcol.Columns.Add("CustomerName", typeof(string));
			dtcol.Columns.Add("ReferenceNo", typeof(string));
			dtcol.Columns.Add("ManualBillNo", typeof(string));
			dtcol.Columns.Add("VisitedByName", typeof(string));
			dtcol.Columns.Add("DueDays", typeof(int));
			dtcol.Columns.Add("DueDate", typeof(DateTime));
			dtcol.Columns.Add("DeliveryTerm", typeof(string));
			dtcol.Columns.Add("BuildingStorey", typeof(string));
			dtcol.Columns.Add("BuildingHeight", typeof(string));
			dtcol.Columns.Add("BuildingArea", typeof(string));
			dtcol.Columns.Add("ReferencePartyName", typeof(string));
			dtcol.Columns.Add("ReferencePartyCellNo", typeof(string));
			dtcol.Columns.Add("ReferencePartyAddress", typeof(string));
			dtcol.Columns.Add("RefSalesMan", typeof(string));
			dtcol.Columns.Add("CommissionType", typeof(string));
			dtcol.Columns.Add("CommissionRate", typeof(decimal));
			dtcol.Columns.Add("ExFactoryAmount", typeof(decimal));
			dtcol.Columns.Add("NetDiscountAmount", typeof(decimal));
			dtcol.Columns.Add("LabourAmount", typeof(decimal));
			dtcol.Columns.Add("TransporterName", typeof(string));
			dtcol.Columns.Add("CarriageAmount", typeof(decimal));
			dtcol.Columns.Add("CommissionAmount", typeof(decimal));
			dtcol.Columns.Add("BillAmount", typeof(decimal));
			dtcol.Columns.Add("WareHouseName", typeof(string));
			dtcol.Columns.Add("ItemId", typeof(int));
			dtcol.Columns.Add("ItemCode", typeof(string));
			dtcol.Columns.Add("ItemName", typeof(string));
			dtcol.Columns.Add("Variant", typeof(string));
			dtcol.Columns.Add("VariantEquivalent", typeof(decimal));
			dtcol.Columns.Add("VariantQty", typeof(decimal));
			dtcol.Columns.Add("CityName", typeof(string));
			dtcol.Columns.Add("ItemQty", typeof(decimal));
			dtcol.Columns.Add("NetWeight", typeof(decimal));
			dtcol.Columns.Add("ItemAmount", typeof(decimal));
			dtcol.Columns.Add("ItemDiscountType", typeof(string));
			dtcol.Columns.Add("ItemDiscountRate", typeof(decimal));
			dtcol.Columns.Add("ItemAmountWithDisc", typeof(decimal));
			dtcol.Columns.Add("ItemNetAmount", typeof(decimal));
			dtcol.Columns.Add("AvgRate", typeof(decimal));
			dtcol.Columns.Add("PrcntOfTotalQty", typeof(decimal));
			dtcol.Columns.Add("PrcntOfTotalAmount", typeof(decimal));
			dtcol.Columns.Add("CurrentGlBalance", typeof(decimal));
			HasSubsidiaryFeature = CommonServices.GetERPFeatureById(4);
			CmbDateTypeSumm.Focus();
			ComboFill();
			ParameterFill();
			StatusFill();
			ReportTypeFill();
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
			dt.Rows.Add(1, "UnApproved");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbApprovalStatusSumm, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbApprovalStatusSumm).Rows[2].Activate();
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
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDateTypeSumm, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)CmbDateTypeSumm).Rows[1].Activate();
			}
			else
			{
				((Control)(object)CmbDateTypeSumm).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDateTypeSumm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(CmbDateTypeSumm.Value) == 1)
			{
				txtFromDateSumm.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 2)
			{
				txtFromDateSumm.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromDateSumm.Value = first;
				txtToDateSumm.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromDateSumm.Value = first2;
				txtToDateSumm.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(CmbDateTypeSumm.Value) == 5)
			{
				txtFromDateSumm.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtToDateSumm.Value = DateTime.Today;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		try
		{
			dtRegisteredRefParty.Clear();
			((UltraGridBase)CmbRegisteredRefParty).DataSource = null;
			((Control)(object)CmbRegisteredRefParty).Text = "";
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("name");
			DataTable dtDistrict = new DataTable();
			dtDistrict.Columns.Add("Id");
			dtDistrict.Columns.Add("name");
			DataTable dtCity = new DataTable();
			dtCity.Columns.Add("Id");
			dtCity.Columns.Add("name");
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			DataTable dtRefParty = new DataTable();
			dtRefParty.Columns.Add("Id");
			dtRefParty.Columns.Add("name");
			DataTable dtsaleman = new DataTable();
			dtsaleman.Columns.Add("Id");
			dtsaleman.Columns.Add("name");
			DataTable dtCustomerGroup = new DataTable();
			dtCustomerGroup.Columns.Add("Id");
			dtCustomerGroup.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = InvSaleInvoice.DropDownFillFromInvSaleInvoice(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtParentCategory.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ItemType")
				{
					dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "JobLot")
				{
					dtJob.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "District")
				{
					dtDistrict.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "City")
				{
					dtCity.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Customer")
				{
					dtSupplieerCustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "ReferenceParty")
				{
					dtRefParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RegisteredReferenceParty")
				{
					dtRegisteredRefParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RefSalesMan")
				{
					dtsaleman.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "CustomerGroup")
				{
					dtCustomerGroup.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtParentCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCategory, cmbParentcategorySumm, "Id", "name", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbParentcategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategories.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategories, CmbCategorySumm, "Id", "name", "Item Categories", ZeroIndex: false);
				((UltraGridBase)CmbCategorySumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, cmbItemtypeSumm, "Id", "name", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbItemtypeSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemNameSumm, "Id", "name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbItemNameSumm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, cmbJobLotSumm, "Id", "name", "JobLot", ZeroIndex: false);
			}
			if (dtDistrict.Rows.Count > 0)
			{
				DDL.BindDDL(dtDistrict, cmbDistrictSumm, "Id", "name", "District", ZeroIndex: false);
			}
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDL(dtCity, cmbcitySumm, "Id", "name", "City", ZeroIndex: false);
			}
			if (dtSupplieerCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplieerCustomer, cmbCustomerSumm, "Id", "name", "Customer Name", ZeroIndex: false);
			}
			if (dtRefParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtRefParty, cmbRefPartyDetailSumm, "Id", "name", "Ref Party", ZeroIndex: false);
			}
			if (dtRegisteredRefParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtRegisteredRefParty, CmbRegisteredRefParty, "Id", "name", "Sales Person", ZeroIndex: false);
				UltraGridColumn obj2 = ((UltraGridBase)CmbRegisteredRefParty).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				CmbRegisteredRefParty.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbRegisteredRefParty.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
			if (dtsaleman.Rows.Count > 0)
			{
				DDL.BindDDL(dtsaleman, cmbsaleman, "Id", "name", "Sale Man", ZeroIndex: false);
			}
			if (dtCustomerGroup.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomerGroup, CmbPartyGroup, "Id", "name", "Party Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("Sales Register", "Sales Register");
			dtreporttype.Rows.Add("Sales Summary DocNo_Customer & SalesMan", "Sales Summary DocNo_Customer & SalesMan");
			dtreporttype.Rows.Add("Sales Summary By Item_Varient & City", "Sales Summary By Item");
			dtreporttype.Rows.Add("Sales Summary By City & Varient", "Sales Summary By City & Variant");
			dtreporttype.Rows.Add("Sales Summary By Customer_Varient & City", "Sales Summary By Customer");
			dtreporttype.Rows.Add("Sales Summary By Customer_Item_Varient & City", "Sales Summary By Customer_Item");
			dtreporttype.Rows.Add("Sales Summary By ReferenceParty_Varient & City", "Sales Summary By ReferenceParty");
			dtreporttype.Rows.Add("Sales Summary By ReferenceParty_Customer_Varient & City", "Sales Summary By ReferenceParty_Customer");
			dtreporttype.Rows.Add("Sales Summary By ReferenceParty_Item_Varient & City", "Sales Summary By ReferenceParty_Item");
			dtreporttype.Rows.Add("Sales Summary By ReferenceParty_Customer_Item_Varient & City", "Sales Summary By ReferenceParty_Customer_Item");
			DDL.BindDDL(dtreporttype, cmbReportType, "Id", "name", "Report Type", ZeroIndex: false);
			((UltraGridBase)cmbReportType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPartyGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id");
			dtSupplier.Columns.Add("name");
			int selectedId = Conversion.ToInt(CmbPartyGroup.Value);
			foreach (DataRow row2 in from row in dtBindCombo.AsEnumerable()
				where (selectedId == 0 || Conversion.ToInt(row["CustomerGroupId"]) == selectedId) && Conversion.ToString(row["Activity"]) == "Customer"
				select row)
			{
				dtSupplier.Rows.Add(row2["Id"], row2["ReferenceName"]);
			}
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbCustomerSumm, "Id", "name", "Customer Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An error occurred: " + ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewSummary_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshSummary_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					BtnShowSumm_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrintSumm_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtFromDateSumm.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtFromDateSumm.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
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
			ComboFill();
			txtFromDateSumm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void BtnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)CmbApprovalStatusSumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			txtFromDocNoSumm.Clear();
			txtToDocNoSumm.Clear();
			((UltraGridBase)cmbReportType).Rows[1].Activate();
			BtnShowSumm_Click(null, null);
			txtFromDateSumm.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowSumm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar5")
			{
				CmbDateTypeSumm.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleInvoiceRegister_FormClosed(object sender, FormClosedEventArgs e)
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

	private void BtnShowSumm_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				GridSummaryFill();
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbReportType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryFill()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			dtcol.Rows.Clear();
			dtGridSummary.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtFromDateSumm.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDateSumm.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSumm.Text);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.CustomerGroupId = Conversion.ToInt(CmbPartyGroup.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.RefSaleManId = Conversion.ToInt(cmbsaleman.Value);
			obj.ReferencePartyName = Conversion.ToString((object)((Control)(object)cmbRefPartyDetailSumm).Text);
			string RefPartyIds = "";
			if (((Control)(object)CmbRegisteredRefParty).Text != null && ((Control)(object)CmbRegisteredRefParty).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbRegisteredRefParty).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtRegisteredRefParty.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						RefPartyIds = RefPartyIds + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
				obj.RefPartyIds = RefPartyIds;
			}
			obj.ReportType = Conversion.ToString(cmbReportType.Value);
			obj.IsApproved = ((Control)(object)CmbApprovalStatusSumm).Text == "Approve";
			if (((Control)(object)CmbApprovalStatusSumm).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (Conversion.ToDouble((object)txtavgrate.Text) > 0.0)
			{
				obj.ItemRate = Conversion.ToDouble((object)txtavgrate.Text);
			}
			else
			{
				MessageBox.Show("Average Rate Always Greater Than Zero");
			}
			if (chkCity.Checked)
			{
				obj.ActionId = 1;
			}
			if (chkvarient.Checked)
			{
				obj.SkipZero = 1;
			}
			if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By City & Varient" && !chkCity.Checked && !chkvarient.Checked)
			{
				DataGridHistory.ClearStructure();
				throw new Exception("At least one checkbox (AddCity or Varient) must be checked.");
			}
			dtGridSummary = InvSaleInvoice.SaleInvoiceSummaryRegister(obj);
			if (dtGridSummary.Rows.Count > 0)
			{
				for (int i = 0; i < dtGridSummary.Rows.Count; i++)
				{
					string variantValue = null;
					if (chkvarient.Checked)
					{
						variantValue = Conversion.ToString(dtGridSummary.Rows[i]["Varient"]);
					}
					dtcol.Rows.Add(dtGridSummary.Rows[i]["Id"], dtGridSummary.Rows[i]["DocDate"], dtGridSummary.Rows[i]["DocNo"], dtGridSummary.Rows[i]["DocumentTypeId"], dtGridSummary.Rows[i]["DocumentTypeCode"], dtGridSummary.Rows[i]["SupplierCustomerId"], dtGridSummary.Rows[i]["CustomerName"], dtGridSummary.Rows[i]["ReferenceNo"], dtGridSummary.Rows[i]["ManualBillNo"], dtGridSummary.Rows[i]["VisitedByName"], dtGridSummary.Rows[i]["DueDays"], dtGridSummary.Rows[i]["DueDate"], dtGridSummary.Rows[i]["DeliveryTerm"], dtGridSummary.Rows[i]["BuildingStorey"], dtGridSummary.Rows[i]["BuildingHeight"], dtGridSummary.Rows[i]["BuildingArea"], dtGridSummary.Rows[i]["ReferencPartyName"], dtGridSummary.Rows[i]["ReferencPartyCellNo"], dtGridSummary.Rows[i]["ReferencPartyAddress"], dtGridSummary.Rows[i]["RefSalesMan"], dtGridSummary.Rows[i]["CommissionType"], dtGridSummary.Rows[i]["CommissionRate"], dtGridSummary.Rows[i]["ExFactoryAmountHeader"], dtGridSummary.Rows[i]["NetDiscountAmountHeader"], dtGridSummary.Rows[i]["LabourAmountHeader"], dtGridSummary.Rows[i]["TransporterName"], dtGridSummary.Rows[i]["CarriageAmount"], dtGridSummary.Rows[i]["CommissionAmount"], dtGridSummary.Rows[i]["BillAmount"], dtGridSummary.Rows[i]["WareHouseName"], dtGridSummary.Rows[i]["ItemId"], dtGridSummary.Rows[i]["ItemCode"], dtGridSummary.Rows[i]["ItemName"], variantValue, dtGridSummary.Rows[i]["VarientEquivalent"], dtGridSummary.Rows[i]["VarientQty"], dtGridSummary.Rows[i]["CityName"], dtGridSummary.Rows[i]["ItemQty"], dtGridSummary.Rows[i]["NetWeight"], dtGridSummary.Rows[i]["ItemAmount"], dtGridSummary.Rows[i]["ItemDiscountType"], dtGridSummary.Rows[i]["ItemDiscountRate"], dtGridSummary.Rows[i]["ItemAmountWithDisc"], dtGridSummary.Rows[i]["ItemNetAmount"], dtGridSummary.Rows[i]["AvgRate"], dtGridSummary.Rows[i]["PrcntOfTotalQty"], dtGridSummary.Rows[i]["PrcntOfTotalAmount"], dtGridSummary.Rows[i]["CurrentGlBalance"]);
				}
				((Control)(object)DataGridHistory).Name = "grd" + cmbReportType.Value.ToString().Replace(" ", "").Replace("&", "");
				DataGridHistory.DataSource = dtcol;
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
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["CustomerName"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Variant"].Visible = chkvarient.Checked;
			DataGridHistory.RootTable.Columns["VariantEquivalent"].Visible = chkvarient.Checked;
			DataGridHistory.RootTable.Columns["VariantQty"].Visible = chkvarient.Checked;
			DataGridHistory.RootTable.Columns["CityName"].Visible = chkCity.Checked;
			DataGridHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					CommonServices.GridColumnSettings(column);
				}
			}
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["ReferenceNo"].Width = Constants.InventoryConstants.RecordNo;
			DataGridHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["VisitedByName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			DataGridHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			DataGridHistory.RootTable.Columns["BuildingStorey"].Width = Constants.InventoryConstants.Building;
			DataGridHistory.RootTable.Columns["BuildingHeight"].Width = Constants.InventoryConstants.Building;
			DataGridHistory.RootTable.Columns["BuildingArea"].Width = Constants.InventoryConstants.Building;
			DataGridHistory.RootTable.Columns["ReferencePartyName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Width = Constants.InventoryConstants.CellNo;
			DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Width = Constants.InventoryConstants.Address;
			DataGridHistory.RootTable.Columns["RefSalesMan"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["CommissionType"].Width = Constants.InventoryConstants.PackingType;
			DataGridHistory.RootTable.Columns["CommissionRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["ExFactoryAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["NetDiscountAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["LabourAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["TransporterName"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["CarriageAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.DocumentTypeCode;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["Variant"].Width = Constants.InventoryConstants.Variant;
			DataGridHistory.RootTable.Columns["VariantEquivalent"].Width = Constants.InventoryConstants.VariantEquivalent;
			DataGridHistory.RootTable.Columns["VariantQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ItemDiscountType"].Width = Constants.InventoryConstants.DiscountType;
			DataGridHistory.RootTable.Columns["ItemDiscountRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["ItemNetAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Width = Constants.InventoryConstants.PercentOfTotal;
			DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Width = Constants.InventoryConstants.PercentOfTotal;
			DataGridHistory.RootTable.Columns["CurrentGlBalance"].Width = Constants.InventoryConstants.AmountForReportConstant;
			if (Conversion.ToString(cmbReportType.Value) == "Sales Register")
			{
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary DocNo_Customer & SalesMan")
			{
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemCode"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.Columns["Variant"].Visible = false;
				DataGridHistory.RootTable.Columns["VariantEquivalent"].Visible = false;
				DataGridHistory.RootTable.Columns["VariantQty"].Visible = false;
				DataGridHistory.RootTable.Columns["CityName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Item_Varient")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["CustomerName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ExFactoryAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["NetDiscountAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["LabourAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CarriageAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["BillAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["CurrentGlBalance"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Item_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["CustomerName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ExFactoryAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["NetDiscountAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["LabourAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CarriageAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["BillAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["CurrentGlBalance"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By City & Varient")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["CustomerName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ExFactoryAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["NetDiscountAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["LabourAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CarriageAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["BillAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemCode"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["CurrentGlBalance"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Customer_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemCode"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Customer_Item_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Customer_Item_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemQty"].Visible = false;
				DataGridHistory.RootTable.Columns["NetWeight"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Item_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["CustomerName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemQty"].Visible = false;
				DataGridHistory.RootTable.Columns["NetWeight"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CurrentGlBalance"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Customer_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemCode"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemQty"].Visible = false;
				DataGridHistory.RootTable.Columns["NetWeight"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Visible = false;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Varient & City")
			{
				DataGridHistory.RootTable.Columns["DocDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DocNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DocumentType"].Visible = false;
				DataGridHistory.RootTable.Columns["CustomerName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferenceNo"].Visible = false;
				DataGridHistory.RootTable.Columns["ManualBillNo"].Visible = false;
				DataGridHistory.RootTable.Columns["VisitedByName"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyAddress"].Visible = false;
				DataGridHistory.RootTable.Columns["ReferencePartyCellNo"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDays"].Visible = false;
				DataGridHistory.RootTable.Columns["DueDate"].Visible = false;
				DataGridHistory.RootTable.Columns["DeliveryTerm"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingStorey"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingHeight"].Visible = false;
				DataGridHistory.RootTable.Columns["BuildingArea"].Visible = false;
				DataGridHistory.RootTable.Columns["RefSalesMan"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionType"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionRate"].Visible = false;
				DataGridHistory.RootTable.Columns["CommissionAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["TransporterName"].Visible = false;
				DataGridHistory.RootTable.Columns["WareHouseName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemCode"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemName"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemQty"].Visible = false;
				DataGridHistory.RootTable.Columns["NetWeight"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountType"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemDiscountRate"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemAmountWithDisc"].Visible = false;
				DataGridHistory.RootTable.Columns["ItemNetAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["AvgRate"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalQty"].Visible = false;
				DataGridHistory.RootTable.Columns["PrcntOfTotalAmount"].Visible = false;
				DataGridHistory.RootTable.Columns["CurrentGlBalance"].Visible = false;
			}
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			txtFromDateSumm.Focus();
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			int DocumentTypeId = Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
			int SupplierCustomerId = Conversion.ToInt((object)r.Cells["SupplierCustomerId"].Value.ToString());
			if (e.Column.Key == "DocNo")
			{
				switch (DocumentTypeId)
				{
				case 1856:
				{
					tblUserRights screenRights = clsGlobalVariables.ScreenViewReights.FirstOrDefault((tblUserRights x) => x.ScreenName.Trim() == "SaleInvoiceDirectConcrete");
					if ((screenRights != null && Conversion.ToBool((object)screenRights.Value)) || UserAccount.RoleName == "Admin")
					{
						SaleInvoiceDirectConcrete saleInvoiceDirectConcrete = new SaleInvoiceDirectConcrete(UserAccount);
						saleInvoiceDirectConcrete.ShowDialog();
						saleInvoiceDirectConcrete.BringToFront();
						saleInvoiceDirectConcrete.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You don't have the rights to view this form.");
					}
					break;
				}
				case 1861:
				{
					tblUserRights screenRights2 = clsGlobalVariables.ScreenViewReights.FirstOrDefault((tblUserRights x) => x.ScreenName.Trim() == "SaleInvoiceAgainstGDNConcrete");
					if ((screenRights2 != null && Conversion.ToBool((object)screenRights2.Value)) || UserAccount.RoleName == "Admin")
					{
						SaleInvoiceAgainstGDNConcrete saleInvoiceAgainstGDNConcrete = new SaleInvoiceAgainstGDNConcrete(UserAccount);
						saleInvoiceAgainstGDNConcrete.ShowDialog();
						saleInvoiceAgainstGDNConcrete.BringToFront();
						saleInvoiceAgainstGDNConcrete.ReadById(Id);
					}
					else
					{
						MessageBox.Show("You don't have the rights to view this form.");
					}
					break;
				}
				}
			}
			if (e.Column.Key == "CustomerName")
			{
				int GlId = CommonServices.GetGlAccountIdBySupplierCustomerId(SupplierCustomerId);
				if (HasSubsidiaryFeature)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, txtFromDateSumm.Value, txtToDateSumm.Value, SupplierCustomerId);
				}
				else
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, txtFromDateSumm.Value, txtToDateSumm.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtavgrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoSumm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoSumm_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrintSumm_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string reportName = Conversion.ToString((object)btnPrintSumm.Text);
			if (dtGridSummary == null || dtGridSummary.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			if (string.IsNullOrWhiteSpace(reportName))
			{
				throw new Exception("Invalid report name.");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGridSummary, reportName + ".rpt");
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
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrintSumm.Visible = true;
				if (Conversion.ToString(cmbReportType.Value) == "Sales Register")
				{
					btnPrintSumm.Text = "1861_01-SalesRegister";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary DocNo_Customer & SalesMan")
				{
					btnPrintSumm.Text = "1861_17-SalesSummaryDocNo_Customer&SalesMan";
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Item_Varient & City")
				{
					if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_03-SalesSummaryByItem";
					}
					else if (chkCity.Checked && chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_16-SalesSummaryByItemCity&Varient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_15-SalesSummaryByItem&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_02-SalesSummaryByItem&Varient";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By City & Varient")
				{
					btnPrintSumm.Text = "1861_04-SalesSummaryByCity&Varient";
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_04-SalesSummaryByCity&Varient";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_05-SalesSummaryByVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_06-SalesSummaryByCity";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Customer_Varient & City")
				{
					if (chkCity.Checked && chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_07-SalesSummaryByCustomerVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_08-SalesSummaryByCustomerVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_09-SalesSummaryByCustomerCity";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_13-SalesSummaryByCustomer";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By Customer_Item_Varient & City")
				{
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_10-SalesSummaryByCustomerItemVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_11-SalesSummaryByCustomerItemVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_12-SalesSummaryByCustomerItem&City";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_14-SalesSummaryByCustomerItem";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Customer_Item_Varient & City")
				{
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_18-SalesSummaryByReferencePartyCustomerItemVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_19-SalesSummaryByReferencePartyCustomerItemVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_20-SalesSummaryByReferencePartyCustomerItem&City";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_21-SalesSummaryByReferencePartyCustomerItem";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Item_Varient & City")
				{
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_22-SalesSummaryByReferencePartyItemVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_23-SalesSummaryByReferencePartyItemVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_24-SalesSummaryByReferencePartyItem&City";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_25-SalesSummaryByReferencePartyItem";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Customer_Varient & City")
				{
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_26-SalesSummaryByReferencePartyCustomerVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_27-SalesSummaryByReferencePartyCustomerVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_28-SalesSummaryByReferencePartyCustomer&City";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_29-SalesSummaryByReferencePartyCustomer";
					}
				}
				else if (Conversion.ToString(cmbReportType.Value) == "Sales Summary By ReferenceParty_Varient & City")
				{
					if (chkvarient.Checked && chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_30-SalesSummaryByReferencePartyVarient&City";
					}
					else if (chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_31-SalesSummaryByReferencePartyVarient";
					}
					else if (chkCity.Checked)
					{
						btnPrintSumm.Text = "1861_32-SalesSummaryByReferenceParty&City";
					}
					else if (!chkCity.Checked && !chkvarient.Checked)
					{
						btnPrintSumm.Text = "1861_33-SalesSummaryByReferenceParty";
					}
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
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afb: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbRegisteredRefParty = new UltraCombo();
		this.cmbsaleman = new UltraCombo();
		this.CmbApprovalStatusSumm = new UltraCombo();
		this.cmbRefPartyDetailSumm = new UltraCombo();
		this.cmbReportType = new UltraCombo();
		this.cmbCustomerSumm = new UltraCombo();
		this.cmbDistrictSumm = new UltraCombo();
		this.CmbPartyGroup = new UltraCombo();
		this.cmbcitySumm = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.cmbJobLotSumm = new UltraCombo();
		this.chkvarient = new System.Windows.Forms.CheckBox();
		this.cmbsaleman1 = new System.Windows.Forms.Label();
		this.txtavgrate = new System.Windows.Forms.TextBox();
		this.CmbDateTypeSumm = new UltraCombo();
		this.chkCity = new System.Windows.Forms.CheckBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.cmbItemtypeSumm = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbParentcategorySumm = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCategorySumm = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummary = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSumm = new System.Windows.Forms.ToolStripButton();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRegisteredRefParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsaleman).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovalStatusSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(974, 664);
		this.tabControl1.TabIndex = 34;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(966, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Register";
		this.tabPage2.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 276);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(960, 356);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Controls.Add(this.label2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 251);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(960, 25);
		this.panel9.TabIndex = 7;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(913, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(2, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(119, 17);
		this.label2.TabIndex = 138;
		this.label2.Text = "Filtered Records";
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 56);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(960, 195);
		this.panel7.TabIndex = 5;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsaleman);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrictSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyGroup);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.chkvarient);
		this.groupBox2.Controls.Add(this.cmbsaleman1);
		this.groupBox2.Controls.Add(this.txtavgrate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm);
		this.groupBox2.Controls.Add(this.chkCity);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.txtFromDateSumm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtToDateSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(949, 188);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria's";
		this.CmbRegisteredRefParty.AllowNull = (DefaultableBoolean)1;
		this.CmbRegisteredRefParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRegisteredRefParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRegisteredRefParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRegisteredRefParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Location = new System.Drawing.Point(663, 44);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Name = "CmbRegisteredRefParty";
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).Size = new System.Drawing.Size(280, 26);
		((System.Windows.Forms.Control)(object)this.CmbRegisteredRefParty).TabIndex = 15;
		this.cmbsaleman.AllowNull = (DefaultableBoolean)1;
		this.cmbsaleman.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsaleman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsaleman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsaleman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsaleman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Location = new System.Drawing.Point(663, 72);
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Name = "cmbsaleman";
		((System.Windows.Forms.Control)(object)this.cmbsaleman).Size = new System.Drawing.Size(280, 26);
		((System.Windows.Forms.Control)(object)this.cmbsaleman).TabIndex = 16;
		this.CmbApprovalStatusSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbApprovalStatusSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApprovalStatusSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApprovalStatusSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApprovalStatusSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbApprovalStatusSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Location = new System.Drawing.Point(663, 128);
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Name = "CmbApprovalStatusSumm";
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbApprovalStatusSumm).TabIndex = 18;
		this.cmbRefPartyDetailSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbRefPartyDetailSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRefPartyDetailSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRefPartyDetailSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRefPartyDetailSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRefPartyDetailSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Location = new System.Drawing.Point(663, 16);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Name = "cmbRefPartyDetailSumm";
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).Size = new System.Drawing.Size(280, 26);
		((System.Windows.Forms.Control)(object)this.cmbRefPartyDetailSumm).TabIndex = 14;
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(663, 100);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(280, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 17;
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(372, 157);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 13;
		this.cmbDistrictSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbDistrictSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDistrictSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDistrictSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDistrictSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDistrictSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(372, 72);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 10;
		this.CmbPartyGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Location = new System.Drawing.Point(372, 128);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Name = "CmbPartyGroup";
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroup).Leave += new System.EventHandler(CmbPartyGroup_Leave);
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(372, 100);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 11;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(372, 16);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 8;
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(372, 44);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 9;
		this.chkvarient.AutoSize = true;
		this.chkvarient.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkvarient.Location = new System.Drawing.Point(800, 132);
		this.chkvarient.Name = "chkvarient";
		this.chkvarient.Size = new System.Drawing.Size(88, 19);
		this.chkvarient.TabIndex = 20;
		this.chkvarient.Text = "Add Varient";
		this.chkvarient.UseVisualStyleBackColor = true;
		this.cmbsaleman1.AutoSize = true;
		this.cmbsaleman1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsaleman1.Location = new System.Drawing.Point(591, 78);
		this.cmbsaleman1.Name = "cmbsaleman1";
		this.cmbsaleman1.Size = new System.Drawing.Size(56, 15);
		this.cmbsaleman1.TabIndex = 596;
		this.cmbsaleman1.Text = "Sale Man";
		this.txtavgrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtavgrate.Font = new System.Drawing.Font("Verdana", 10f);
		this.txtavgrate.Location = new System.Drawing.Point(663, 158);
		this.txtavgrate.Name = "txtavgrate";
		this.txtavgrate.Size = new System.Drawing.Size(133, 24);
		this.txtavgrate.TabIndex = 19;
		this.txtavgrate.Text = "1";
		this.txtavgrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtavgrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtavgrate_KeyPress);
		this.CmbDateTypeSumm.AllowNull = (DefaultableBoolean)1;
		this.CmbDateTypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDateTypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDateTypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDateTypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDateTypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Location = new System.Drawing.Point(93, 16);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Name = "CmbDateTypeSumm";
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbDateTypeSumm).TabIndex = 0;
		this.CmbDateTypeSumm.ValueChanged += new System.EventHandler(CmbDateTypeSumm_ValueChanged);
		this.chkCity.AutoSize = true;
		this.chkCity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkCity.Location = new System.Drawing.Point(800, 163);
		this.chkCity.Name = "chkCity";
		this.chkCity.Size = new System.Drawing.Size(71, 19);
		this.chkCity.TabIndex = 21;
		this.chkCity.Text = "Add City";
		this.chkCity.UseVisualStyleBackColor = true;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(2, 22);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(60, 15);
		this.label40.TabIndex = 594;
		this.label40.Text = "Date Type";
		this.label28.BackColor = System.Drawing.Color.LightGray;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(591, 157);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(70, 26);
		this.label28.TabIndex = 162;
		this.label28.Text = "Avg Rate";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 8.75f, System.Drawing.FontStyle.Bold);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(888, 128);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(55, 26);
		this.BtnShowSumm.TabIndex = 22;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.BtnShowSumm.Leave += new System.EventHandler(BtnShowSumm_Leave);
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(93, 157);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 7;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.Location = new System.Drawing.Point(591, 134);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(64, 15);
		this.label26.TabIndex = 163;
		this.label26.Text = "        Status";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(591, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(74, 15);
		this.label4.TabIndex = 158;
		this.label4.Text = "Sales Offices";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(304, 163);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(58, 15);
		this.label16.TabIndex = 123;
		this.label16.Text = "Customer";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(591, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(54, 15);
		this.label19.TabIndex = 158;
		this.label19.Text = "Ref Party";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(304, 134);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 149;
		this.label3.Text = "Party Group";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(304, 106);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(30, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City ";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(304, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(35, 15);
		this.label17.TabIndex = 125;
		this.label17.Text = "Item ";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(304, 78);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(591, 106);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(70, 15);
		this.label18.TabIndex = 155;
		this.label18.Text = "Report Type";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(2, 50);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(91, 15);
		this.label29.TabIndex = 127;
		this.label29.Text = "From && To Date";
		this.txtFromDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateSumm.Location = new System.Drawing.Point(93, 46);
		this.txtFromDateSumm.Name = "txtFromDateSumm";
		this.txtFromDateSumm.Size = new System.Drawing.Size(103, 23);
		this.txtFromDateSumm.TabIndex = 1;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(304, 50);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(2, 78);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(82, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From && To No";
		this.txtToDateSumm.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.CustomFormat = "dd-MMM-yy";
		this.txtToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateSumm.Location = new System.Drawing.Point(197, 46);
		this.txtToDateSumm.Name = "txtToDateSumm";
		this.txtToDateSumm.Size = new System.Drawing.Size(103, 23);
		this.txtToDateSumm.TabIndex = 2;
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(93, 74);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(103, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtFromDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoSumm_KeyPress);
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(197, 74);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(103, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.txtToDocNoSumm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoSumm_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(2, 106);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 138;
		this.label20.Text = "Parent Category";
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(93, 100);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(2, 134);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(82, 15);
		this.label27.TabIndex = 138;
		this.label27.Text = "Item Category";
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(93, 128);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 6;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(2, 163);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(32, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "Type";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(960, 53);
		this.panel8.TabIndex = 6;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewSummary, this.btnRefreshSummary, this.btnPrintSumm, this.btnShortcut });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(960, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewSummary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewSummary.Name = "BtnNewSummary";
		this.BtnNewSummary.Size = new System.Drawing.Size(59, 24);
		this.BtnNewSummary.Text = "&New";
		this.BtnNewSummary.Click += new System.EventHandler(BtnNewSummary_Click);
		this.btnRefreshSummary.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummary.Name = "btnRefreshSummary";
		this.btnRefreshSummary.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummary.Text = "&Refresh";
		this.btnRefreshSummary.Click += new System.EventHandler(btnRefreshSummary_Click);
		this.btnPrintSumm.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSumm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSumm.Name = "btnPrintSumm";
		this.btnPrintSumm.Size = new System.Drawing.Size(61, 24);
		this.btnPrintSumm.Text = "Print";
		this.btnPrintSumm.Click += new System.EventHandler(btnPrintSumm_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 31);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(182, 19);
		this.label1.TabIndex = 138;
		this.label1.Text = "Sale Invoice Register";
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(974, 664);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "SaleInvoiceRegister";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SaleInvoiceRegister_FormClosed);
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRegisteredRefParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsaleman).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovalStatusSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefPartyDetailSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDateTypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
