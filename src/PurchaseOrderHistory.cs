// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
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
namespace Architecture.WinApp.InventoryReports
{

public class PurchaseOrderHistory : Form
{
	public DateTime FromDate;

	public DateTime ToDate;

	public int SupplierId;

	public bool updateFromDate = true;

	private bool FormInitialized;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtDetailData = new DataTable();

	private DataTable grdDetailTableForLayout = new DataTable();

	private DataTable dtSummary = new DataTable();

	private DataTable dtOrderSummaryRegister = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable globaldt = new DataTable();

	private WaitForm waitForm = new WaitForm();

	private bool ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Label label27;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Label label1;

	private ToolStripButton btnPurchsaeOrderDetail;

	private Label label4;

	private ToolStripButton toolStripButton2;

	private CtrlGrdBar ctrlGrdBar3;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private GroupBox groupBox1;

	private Panel panel4;

	private Label label7;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshSummar;

	private ToolStripButton btnSummaryPrint;

	private Panel panel7;

	private GroupBox groupBox2;

	internal UltraCombo cmbItemtypeSumm;

	private Label label25;

	internal UltraCombo cmbCustomerSumm;

	private Label label24;

	internal UltraCombo cmbActivitySumm;

	internal UltraCombo cmbItemNameSumm;

	private Label label14;

	private Label label22;

	internal UltraCombo cmbJobLotSumm;

	private Label label32;

	private Label label21;

	internal UltraCombo cmbDistrictSumm;

	private Label label31;

	private TextBox txtFromDocNoSumm;

	internal UltraCombo cmbcitySumm;

	private TextBox txtToDocNoSumm;

	private Label label30;

	private Button BtnShowSumm;

	private Label label18;

	internal UltraCombo cmbParentcategorySumm;

	private Label label17;

	internal UltraCombo CmbCategorySumm;

	internal UltraCombo cmbCropSumm;

	private Label label16;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label9;

	internal GridEX GridSummary;

	private CheckBox chkIncludeCity;

	private CheckBox chkIncludePackUom;

	private CheckBox chkSkipZero;

	private ToolStripButton btnNewSummary;

	private ToolStripButton btnShortCutKeySumamryTab;

	private ToolStripButton btnShortCutKeys;

	internal UltraCombo cmbBranchName;

	private Label label48;

	internal UltraCombo cmbBranchNameSumm;

	private Label label5;

	public DateTimePicker datFromDate;

	public DateTimePicker datToDate;

	private Label label11;

	private Label label12;

	public DateTimePicker datFromDateSumm;

	public DateTimePicker datToDateSumm;

	private TextBox txtOutstandingValue;

	private TextBox txtExpiringDays;

	private TextBox txtInProcessExcess;

	private TextBox txtOutstandingInprocess;

	private TextBox txtCanceled;

	private Panel PanelCanceledSummary;

	private Label label28;

	private TextBox txtCanceledSummary;

	private Panel PanelExpiringSummary;

	private Label label29;

	private TextBox txtExpiringDaysSummary;

	private Panel PanelInProcessSummary;

	private Label label33;

	private TextBox txtInProcessSummary;

	private Panel PanelOutstandingInProcessSummary;

	private Label label34;

	private TextBox txtOustandingInProcessSummary;

	private Panel PanelOutstandingSummary;

	private Label label35;

	private TextBox txtOustandingSummary;

	private Label label36;

	private Label label6;

	private RadioButton RadCancelSumm;

	private RadioButton RadUnApprovedOnlySumm;

	private RadioButton radAllApprovedAndUnApprovedSumm;

	private RadioButton RadApprovedOnlySumm;

	private RadioButton RadCompleteSumm;

	private RadioButton RadAllOpenCompCancelSumm;

	private RadioButton RadOpenSumm;

	private Panel PanelCanceled;

	private Label label3;

	private Panel PanelExpiringDays;

	private Label label10;

	private Panel PanelInProcess;

	private Label label13;

	private Panel PanelOutstandingInProcess;

	private Label label37;

	private Panel PanelOutstanding;

	private Label labelInterval01;

	public RadioButton RadUnApprovedOnlyDetail;

	public RadioButton radAllApprovedAndUnApprovedDetail;

	public RadioButton RadApprovedOnlyDetail;

	public RadioButton RadCancelDetail;

	public RadioButton RadCompleteDetail;

	public RadioButton RadOpenDetail;

	public RadioButton RadAllOpenCompCancelDetail;

	private Label label15;

	public RadioButton RdGroupByCategory;

	public RadioButton rdGroupByItemType;

	private Label label19;

	public UltraCombo CmbItem;

	public UltraCombo CmbSupplier;

	public TextBox txtSrTo;

	public TextBox txtSrFrom;

	public UltraCombo CmbParentCatgory;

	public Button btnshow;

	private ToolStripButton btnCompleteOrders;

	private ToolStripButton btnOpenOrders;

	private ToolStripButton btnCancelOrders;

	public UltraCombo CmbBookingPerson;

	private Label label20;

	internal UltraCombo cmbBookingPersonSumm;

	private Label label26;

	public PurchaseOrderHistory(UserAccount user)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtOrderSummaryRegister.Columns.Add("Id", typeof(int));
			dtOrderSummaryRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtOrderSummaryRegister.Columns.Add("DocType", typeof(string));
			dtOrderSummaryRegister.Columns.Add("DocDate", typeof(DateTime));
			dtOrderSummaryRegister.Columns.Add("DocNo", typeof(int));
			dtOrderSummaryRegister.Columns.Add("BranchName", typeof(string));
			dtOrderSummaryRegister.Columns.Add("ItemCode", typeof(string));
			dtOrderSummaryRegister.Columns.Add("ItemName", typeof(string));
			dtOrderSummaryRegister.Columns.Add("PackUom", typeof(string));
			dtOrderSummaryRegister.Columns.Add("OrderQty", typeof(double));
			dtOrderSummaryRegister.Columns.Add("RcvdQty", typeof(double));
			dtOrderSummaryRegister.Columns.Add("BalQty", typeof(double));
			dtOrderSummaryRegister.Columns.Add("OrderWeight", typeof(double));
			dtOrderSummaryRegister.Columns.Add("RcvdWeight", typeof(double));
			dtOrderSummaryRegister.Columns.Add("BalWeight", typeof(double));
			dtOrderSummaryRegister.Columns.Add("OrderAmount", typeof(double));
			dtOrderSummaryRegister.Columns.Add("RcvdAmount", typeof(double));
			dtOrderSummaryRegister.Columns.Add("BalAmount", typeof(double));
			dtOrderSummaryRegister.Columns.Add("AvgRate", typeof(double));
			dtOrderSummaryRegister.Columns.Add("PercentOfTotal", typeof(double));
			dtOrderSummaryRegister.Columns.Add("CityName", typeof(string));
			dtOrderSummaryRegister.Columns.Add("Supplier", typeof(string));
			dtOrderSummaryRegister.Columns.Add("BookingPerson", typeof(string));
			dtOrderSummaryRegister.Columns.Add("CropYear", typeof(string));
			dtOrderSummaryRegister.Columns.Add("JobLot", typeof(string));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		try
		{
			waitForm.Show(this);
			Task.Run(delegate
			{
				Rightsobjects rightsData = CommonServices.SetRightsValueInRightsObject(base.Name);
				DataTable dropDownData = GetDropDownData();
				DataTable parentCategoryData = GetParentCategoryData();
				DataTable branchesAllocatedToUserFromPurchaseOrder = PurchaseOrder.GetBranchesAllocatedToUserFromPurchaseOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 41);
				ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation"));
				return new
				{
					RightsData = rightsData,
					PcData = parentCategoryData,
					ComboBindData = dropDownData,
					BranchesData = branchesAllocatedToUserFromPurchaseOrder
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
					formright = result.RightsData;
					dtBranch = result.BranchesData;
					BindParentCategory(result.PcData);
					ReportTypeFill();
					BranchesFill(dtBranch);
					AllDropDownBind(result.ComboBindData);
					FormInitialized = true;
					ctrlGrdBar3.mGridChooseFielder.Visible = false;
					ctrlGrdBar3.mGridSaveLayouts.Visible = false;
					ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
					if (Conversion.CheckDateTimeNull(FromDate))
					{
						datToDate.Value = DateTime.Today;
					}
					else
					{
						if (updateFromDate)
						{
							datFromDate.Value = FromDate;
						}
						datToDate.Value = ToDate;
						if (SupplierId > 0)
						{
							CmbSupplier.Value = SupplierId;
						}
						GetDetailData();
					}
					datFromDate.Focus();
				}
				waitForm.Close();
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 625);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			datFromDate.Value = DateTime.Now.AddDays(-7.0);
			datToDate.Value = DateTime.Now;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtSrFrom.Clear();
			txtSrTo.Clear();
			datFromDate.Focus();
			grd.ClearStructure();
			ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation"));
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
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNewSummary_Click(object sender, EventArgs e)
	{
		try
		{
			datFromDateSumm.Value = DateTime.Now;
			datToDateSumm.Value = DateTime.Now;
			txtFromDocNoSumm.Text = string.Empty;
			txtToDocNoSumm.Text = string.Empty;
			((Control)(object)cmbParentcategorySumm).Text = string.Empty;
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((Control)(object)cmbCropSumm).Text = string.Empty;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			GridSummary.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshSummar_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			AllDropDownBind(GetDropDownData());
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Order Register");
			dt.Rows.Add(2, "Order Summary By Item");
			dt.Rows.Add(3, "Order Summary By Supplier");
			dt.Rows.Add(4, "Order Summary By Supplier & Item");
			DDL.BindDDL(dt, cmbActivitySumm, "Id", "Name", "Report Type", ZeroIndex: false);
			((UltraGridBase)cmbActivitySumm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetDropDownData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "41";
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
			return PurchaseOrder.GetDataForDropDownFromPurchaseOrder(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetParentCategoryData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return PurchaseOrder.GetParentCategoriesForPurchaseOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindParentCategory(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbParentCatgory, "Id", "InvParentCateDescription", "Parent Category", ZeroIndex: false);
				DDL.BindDDL(dt, cmbParentcategorySumm, "Id", "InvParentCateDescription", "Parent Category", ZeroIndex: false);
				((UltraGridBase)CmbParentCatgory).Rows[0].Activate();
				((UltraGridBase)cmbParentcategorySumm).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbParentCatgory).Text = string.Empty;
				((UltraGridBase)CmbParentCatgory).DataSource = null;
				((Control)(object)cmbParentcategorySumm).Text = string.Empty;
				((UltraGridBase)cmbParentcategorySumm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable FilterData(DataTable source, string activityType)
	{
		EnumerableRowCollection<DataRow> filteredRows = from row in source.AsEnumerable()
			where row.Field<string>("Activity") == activityType
			select row;
		if (!filteredRows.Any())
		{
			return source.Clone();
		}
		return filteredRows.CopyToDataTable();
	}

	private void BindComboBox(DataTable data, UltraCombo combo, string placeholder, int columnWidth = -1)
	{
		if (data.Rows.Count > 0)
		{
			DDL.BindDDL(data, combo, "Id", "ReferenceName", placeholder, ZeroIndex: false);
			((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)combo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (columnWidth > 0)
			{
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[1].Width = columnWidth;
			}
		}
	}

	private void AllDropDownBind(DataTable dt)
	{
		try
		{
			if (dt != null && dt.Rows.Count > 0)
			{
				DataTable dtItemType = FilterData(dt, "ItemTypes");
				DataTable dtCategory = FilterData(dt, "ItemCategories");
				DataTable dtCrop = FilterData(dt, "CropYear");
				DataTable dtDis = FilterData(dt, "District");
				DataTable dtCity = FilterData(dt, "City");
				DataTable dtJob = FilterData(dt, "JobLot");
				DataTable dtItem = FilterData(dt, "Item");
				DataTable dtParty = FilterData(dt, "Supplier");
				DataTable dtBooking = FilterData(dt, "BookingPerson");
				BindComboBox(dtParty, CmbSupplier, "Supplier Name", 250);
				BindComboBox(dtParty, cmbCustomerSumm, "Supplier Name", 250);
				BindComboBox(dtItem, CmbItem, "Item Name", 250);
				BindComboBox(dtCategory, CmbCategorySumm, "Item Category", 250);
				BindComboBox(dtItemType, cmbItemtypeSumm, "Item Type");
				BindComboBox(dtCrop, cmbCropSumm, "Crop Year");
				BindComboBox(dtJob, cmbJobLotSumm, "Job Lot");
				BindComboBox(dtDis, cmbDistrictSumm, "District");
				BindComboBox(dtCity, cmbcitySumm, "City");
				BindComboBox(dtItem, cmbItemNameSumm, "Item Name", 250);
				BindComboBox(dtBooking, CmbBookingPerson, "Booking Person", 250);
				BindComboBox(dtBooking, cmbBookingPersonSumm, "Booking Person", 250);
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
				((Control)(object)cmbItemNameSumm).Text = string.Empty;
				((UltraGridBase)cmbItemNameSumm).DataSource = null;
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
				((Control)(object)cmbCustomerSumm).Text = string.Empty;
				((UltraGridBase)cmbCustomerSumm).DataSource = null;
				((Control)(object)CmbCategorySumm).Text = string.Empty;
				((UltraGridBase)CmbCategorySumm).DataSource = null;
				((Control)(object)cmbItemtypeSumm).Text = string.Empty;
				((UltraGridBase)cmbItemtypeSumm).DataSource = null;
				((Control)(object)cmbCropSumm).Text = string.Empty;
				((UltraGridBase)cmbCropSumm).DataSource = null;
				((Control)(object)cmbJobLotSumm).Text = string.Empty;
				((UltraGridBase)cmbJobLotSumm).DataSource = null;
				((Control)(object)cmbDistrictSumm).Text = string.Empty;
				((UltraGridBase)cmbDistrictSumm).DataSource = null;
				((Control)(object)cmbcitySumm).Text = string.Empty;
				((UltraGridBase)cmbcitySumm).DataSource = null;
				((Control)(object)CmbBookingPerson).Text = string.Empty;
				((UltraGridBase)CmbBookingPerson).DataSource = null;
				((Control)(object)cmbBookingPersonSumm).Text = string.Empty;
				((UltraGridBase)cmbBookingPersonSumm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesFill(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
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
				DDL.BindDDL(dt, cmbBranchNameSumm, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
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
			else
			{
				((Control)(object)cmbBranchName).Text = string.Empty;
				((UltraGridBase)cmbBranchName).DataSource = null;
				((Control)(object)cmbBranchNameSumm).Text = string.Empty;
				((UltraGridBase)cmbBranchNameSumm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			FilterData();
			BindDetailData(GetDetailData());
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private DataTable GetDetailData(int Activity = 0)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			label36.Visible = false;
			dtDetailData.Rows.Clear();
			globaldt.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			if (datToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.BookingPersonId = Conversion.ToInt(CmbBookingPerson.Value);
			obj.ParentCategoryId = Conversion.ToInt(CmbParentCatgory.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			if (RadOpenDetail.Checked)
			{
				obj.Status = "Open";
			}
			else if (RadCompleteDetail.Checked)
			{
				obj.Status = "Complete";
			}
			else if (RadCancelDetail.Checked)
			{
				obj.Status = "Cancel";
			}
			if (RadApprovedOnlyDetail.Checked)
			{
				obj.IsApproved = true;
			}
			else if (RadUnApprovedOnlyDetail.Checked)
			{
				obj.IsApproved = false;
			}
			else if (radAllApprovedAndUnApprovedDetail.Checked)
			{
				obj.ApprovedFilter = "All";
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
				obj.ActivityId = Activity;
				switch (Activity)
				{
				case 1:
					label36.Text = "OutStanding && Open";
					break;
				case 2:
					label36.Text = "OutStanding && InProcess";
					break;
				case 3:
					label36.Text = "InProcess && Excess Received";
					break;
				case 4:
					label36.Text = "Expiring Next 7 Days";
					break;
				case 5:
					label36.Text = "Canceled";
					break;
				}
				label36.Visible = Activity > 0;
				globaldt = PurchaseOrder.PurchaseOrderHistory(obj);
				return globaldt;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindDetailData(DataTable dt)
	{
		try
		{
			grdDetailTableForLayout.Rows.Clear();
			dtDetailData.Rows.Clear();
			dtDetailData = dt.Copy();
			DataTable grdlst = dt.Copy();
			DataTable data = new DataTable();
			data.Columns.Add("Id", typeof(int));
			data.Columns.Add("DocumentTypeId", typeof(int));
			data.Columns.Add("BranchName", typeof(string));
			data.Columns.Add("ItemCategory", typeof(string));
			data.Columns.Add("ItemType", typeof(string));
			data.Columns.Add("DocDate", typeof(DateTime));
			data.Columns.Add("DocNo", typeof(int));
			data.Columns.Add("OrderSupCustId", typeof(int));
			data.Columns.Add("SupplierName", typeof(string));
			data.Columns.Add("BookingPerson", typeof(string));
			data.Columns.Add("CommissionAgent", typeof(string));
			data.Columns.Add("DeliveryTerm", typeof(string));
			data.Columns.Add("DeliveryDays", typeof(int));
			data.Columns.Add("DueDate", typeof(DateTime));
			data.Columns.Add("PurchaseGLAC", typeof(int));
			data.Columns.Add("ItemId", typeof(int));
			data.Columns.Add("ItemName", typeof(string));
			data.Columns.Add("Crop", typeof(string));
			data.Columns.Add("PackUom", typeof(string));
			data.Columns.Add("Qty", typeof(double));
			data.Columns.Add("RcvdQty", typeof(double));
			data.Columns.Add("RejQty", typeof(double));
			data.Columns.Add("BalQty", typeof(double));
			data.Columns.Add("Weight", typeof(double));
			data.Columns.Add("RcvdWeight", typeof(double));
			data.Columns.Add("BalWeight", typeof(double));
			data.Columns.Add("Rate", typeof(double));
			data.Columns.Add("OrderAmount", typeof(double));
			data.Columns.Add("ReceivedAmount", typeof(double));
			data.Columns.Add("BalAmount", typeof(double));
			data.Columns.Add("PPBagRate", typeof(double));
			data.Columns.Add("JuteBagRate", typeof(double));
			data.Columns.Add("PPBagWeight", typeof(double));
			data.Columns.Add("JuteBagWeight", typeof(double));
			data.Columns.Add("ApprovedBy", typeof(string));
			data.Columns.Add("ApprovedDate", typeof(DateTime));
			data.Columns.Add("ExpiryDate", typeof(DateTime));
			data.Columns.Add("ActionRemarks", typeof(string));
			data.Clear();
			grdDetailTableForLayout = data.Clone();
			if (grdlst.Rows.Count > 0)
			{
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					data.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["BranchName"], grdlst.Rows[i]["ItemCategory"], grdlst.Rows[i]["TypeDescription"], Convert.ToDateTime(grdlst.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), grdlst.Rows[i]["DocNo"], grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["BookingPerson"], grdlst.Rows[i]["CompanyNameSpCAgent"], grdlst.Rows[i]["DeliveryTerm"], grdlst.Rows[i]["DeliveryDays"], grdlst.Rows[i]["OrderDueDate"], grdlst.Rows[i]["PurchaseGLAC"], grdlst.Rows[i]["OrderItemId"], grdlst.Rows[i]["ItemName"], grdlst.Rows[i]["Crop"], grdlst.Rows[i]["UOMCodeItm"], grdlst.Rows[i]["OrderItemQty"], grdlst.Rows[i]["ReceivedQty"], grdlst.Rows[i]["RejQty"], grdlst.Rows[i]["BalQty"], grdlst.Rows[i]["NetWeight"], grdlst.Rows[i]["ReceivedWeight"], grdlst.Rows[i]["BalWeight"], grdlst.Rows[i]["OrderItemRate"], grdlst.Rows[i]["Amount"], grdlst.Rows[i]["ReceivedAmount"], grdlst.Rows[i]["BalAmount"], grdlst.Rows[i]["PPBagRate"], grdlst.Rows[i]["JuteBagRate"], grdlst.Rows[i]["PPBagWeight"], grdlst.Rows[i]["JuteBagWeight"], grdlst.Rows[i]["UserNameAusr"], Conversion.ToDateTime(grdlst.Rows[i]["PostDate"]).ToShortDateString(), Convert.ToDateTime(grdlst.Rows[i]["OrderExpiryDate"]).ToString("dd-MMM-yy"));
				}
				grdDetailTableForLayout = data.Copy();
				grd.DataSource = data;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FilterData()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			PanelOutstanding.Visible = false;
			PanelOutstandingInProcess.Visible = false;
			PanelInProcess.Visible = false;
			PanelExpiringDays.Visible = false;
			PanelCanceled.Visible = false;
			DataTable dtGrid = PurchaseOrder.GetOrderStatus(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 41
			});
			if (dtGrid.Rows.Count > 0)
			{
				PanelOutstanding.Visible = true;
				PanelOutstandingInProcess.Visible = true;
				PanelInProcess.Visible = true;
				PanelExpiringDays.Visible = true;
				PanelCanceled.Visible = true;
				double OutStanding = 0.0;
				double OutStandingInProcess = 0.0;
				double InProcessExcess = 0.0;
				double ExpiringDays = 0.0;
				double Canceled = 0.0;
				OutStanding = Conversion.ToDouble(dtGrid.Rows[0]["OrderQty"]);
				OutStandingInProcess = Conversion.ToDouble(dtGrid.Rows[1]["OrderQty"]);
				InProcessExcess = Conversion.ToDouble(dtGrid.Rows[2]["OrderQty"]);
				ExpiringDays = Conversion.ToDouble(dtGrid.Rows[3]["OrderQty"]);
				Canceled = Conversion.ToDouble(dtGrid.Rows[4]["OrderQty"]);
				txtOutstandingValue.Text = OutStanding.ToString(clsGlobalVariables.stringFormatboth);
				txtOutstandingInprocess.Text = OutStandingInProcess.ToString(clsGlobalVariables.stringFormatboth);
				txtInProcessExcess.Text = InProcessExcess.ToString(clsGlobalVariables.stringFormatboth);
				txtExpiringDays.Text = ExpiringDays.ToString(clsGlobalVariables.stringFormatboth);
				txtCanceled.Text = Canceled.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				waitForm.Close();
				txtOutstandingValue.Text = string.Empty;
				txtOutstandingInprocess.Text = string.Empty;
				txtInProcessExcess.Text = string.Empty;
				txtExpiringDays.Text = string.Empty;
				txtCanceled.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["OrderSupCustId"].Visible = false;
			grd.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["RejQty"].Visible = false;
			grd.RootTable.Columns["ItemCategory"].Visible = false;
			grd.RootTable.Columns["ItemType"].Visible = false;
			grd.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ExpiryDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["PackUom"].Caption = "Pack Size";
			grd.RootTable.Columns["Qty"].Caption = "Order Qty";
			grd.RootTable.Columns["RcvdQty"].Caption = "Received Qty";
			grd.RootTable.Columns["BalQty"].Caption = "Balance Qty";
			grd.RootTable.Columns["Weight"].Caption = "Order Weight";
			grd.RootTable.Columns["RcvdWeight"].Caption = "Received Weight";
			grd.RootTable.Columns["BalWeight"].Caption = "Balance Weight";
			CommonServices.GridWrappingAndColumnSettings(grd, 2);
			grd.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["SupplierName"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["BookingPerson"].Width = Constants.InventoryConstants.ReferenceParty;
			grd.RootTable.Columns["CommissionAgent"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["DeliveryTerm"].Width = Constants.ExportConstants.TermConstant;
			grd.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["Crop"].Width = Constants.ExportConstants.CropYearConstant;
			grd.RootTable.Columns["PackUom"].Width = Constants.ExportConstants.UomConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["RcvdQty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["RejQty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["BalQty"].Width = Constants.ExportConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["RcvdWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["BalWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["OrderAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["ReceivedAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["BalAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["PPBagRate"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["JuteBagRate"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["PPBagWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["JuteBagWeight"].Width = Constants.ExportConstants.WeightConstant;
			grd.RootTable.Columns["ApprovedBy"].Width = Constants.ExportConstants.ApprovedByConstant;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ExpiryDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ActionRemarks"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["DeliveryDays"].Width = Constants.ExportConstants.DueDays;
			grd.RootTable.Columns["DueDate"].Width = Constants.ExportConstants.DateConstant + 10;
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["DocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grd.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grd.RootTable.Columns["BranchName"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderSupCustId"].EditType = (EditType)0;
			grd.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			grd.RootTable.Columns["CommissionAgent"].EditType = (EditType)0;
			grd.RootTable.Columns["DeliveryTerm"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["Crop"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["RcvdQty"].EditType = (EditType)0;
			grd.RootTable.Columns["RejQty"].EditType = (EditType)0;
			grd.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grd.RootTable.Columns["Weight"].EditType = (EditType)0;
			grd.RootTable.Columns["RcvdWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["ReceivedAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BalAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["PPBagRate"].EditType = (EditType)0;
			grd.RootTable.Columns["JuteBagRate"].EditType = (EditType)0;
			grd.RootTable.Columns["PPBagWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["JuteBagWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ApprovedBy"].EditType = (EditType)0;
			grd.RootTable.Columns["ApprovedDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ExpiryDate"].EditType = (EditType)0;
			grd.RootTable.Columns["DeliveryDays"].EditType = (EditType)0;
			grd.RootTable.Columns["DueDate"].EditType = (EditType)0;
			grd.RootTable.Columns["ActionRemarks"].EditType = (EditType)1;
			grd.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DocumentTypeId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OrderSupCustId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["SupplierName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["CommissionAgent"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DeliveryTerm"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PurchaseGLAC"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Crop"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RcvdQty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RejQty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BalQty"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["RcvdWeight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BalWeight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["OrderAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ReceivedAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["BalAmount"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PPBagRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["JuteBagRate"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PPBagWeight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["JuteBagWeight"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ApprovedBy"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["DeliveryDays"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["ApprovedDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["ExpiryDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["DueDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["RcvdQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RcvdQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RcvdQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["RcvdQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["RejQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RejQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RejQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["RejQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.####";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["RcvdWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RcvdWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RcvdWeight"].FormatString = "#,##0.####";
			grd.RootTable.Columns["RcvdWeight"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].FormatString = "#,##0.####";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = "#,##0.####";
			grd.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["OrderAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["ReceivedAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ReceivedAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ReceivedAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["ReceivedAmount"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns.Add("Slip");
			grd.RootTable.Columns["Slip"].Caption = "Slip203";
			grd.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Slip"].ButtonText = "Slip203";
			grd.RootTable.Columns["Slip"].Width = 60;
			grd.RootTable.Columns["Slip"].Position = 0;
			grd.RootTable.Columns.Add("Open");
			grd.RootTable.Columns["Open"].Caption = "Open";
			grd.RootTable.Columns["Open"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Open"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Open"].ButtonText = "Open";
			grd.RootTable.Columns["Open"].Width = 40;
			grd.RootTable.Columns.Add("Complete");
			grd.RootTable.Columns["Complete"].Caption = "Complete";
			grd.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Complete"].ButtonText = "Complete";
			grd.RootTable.Columns["Complete"].Width = 80;
			grd.RootTable.Columns.Add("Cancel");
			grd.RootTable.Columns["Cancel"].Caption = "Cancel";
			grd.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Cancel"].ButtonText = "Cancel";
			grd.RootTable.Columns["Cancel"].Width = 60;
			grd.RootTable.Columns.Add("Update");
			grd.RootTable.Columns["Update"].Caption = "UpdateExpiryDate";
			grd.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Update"].ButtonText = "UpdateExpiryDate";
			grd.RootTable.Columns["Update"].Width = 120;
			grd.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
			grd.RootTable.Columns["Open"].Visible = false;
			grd.RootTable.Columns["Complete"].Visible = false;
			grd.RootTable.Columns["Cancel"].Visible = false;
			grd.RootTable.Columns["Update"].Visible = false;
			btnOpenOrders.Visible = false;
			btnCancelOrders.Visible = false;
			btnCompleteOrders.Visible = false;
			if (CommonServices.CheckDocumentInApprovalPolicy(41))
			{
				grd.RootTable.Columns.Add("ApprovalDetail");
				grd.RootTable.Columns["ApprovalDetail"].Caption = "Approval Detail";
				grd.RootTable.Columns["ApprovalDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["ApprovalDetail"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["ApprovalDetail"].ButtonText = "Approval Detail";
				grd.RootTable.Columns["ApprovalDetail"].Width = 100;
				grd.RootTable.Columns["ApprovalDetail"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.FrozenColumns = 1;
			}
			((Control)(object)grd).Refresh();
			if (RdGroupByCategory.Checked)
			{
				grd.RootTable.Groups.Add("ItemCategory");
			}
			else if (rdGroupByItemType.Checked)
			{
				grd.RootTable.Groups.Add("ItemType");
			}
			ctrlGrdBar3_Load(null, null);
			if (RadApprovedOnlyDetail.Checked && !RadOpenDetail.Checked && !RadAllOpenCompCancelDetail.Checked && formright.CanChangeOrderStatusToOpen)
			{
				grd.RootTable.Columns["Open"].Visible = true;
				btnOpenOrders.Visible = true;
				btnCompleteOrders.Visible = false;
				btnCancelOrders.Visible = false;
			}
			else if (RadApprovedOnlyDetail.Checked && RadOpenDetail.Checked)
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					grd.RootTable.Columns["Complete"].Visible = true;
					btnCompleteOrders.Visible = true;
				}
				else
				{
					grd.RootTable.Columns["Complete"].Visible = false;
					btnOpenOrders.Visible = false;
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					grd.RootTable.Columns["Cancel"].Visible = true;
					btnCancelOrders.Visible = true;
				}
				else
				{
					grd.RootTable.Columns["Cancel"].Visible = false;
					btnCancelOrders.Visible = false;
				}
				if (formright.CanChangeOrderExpiryDate)
				{
					grd.RootTable.Columns["Update"].Visible = true;
				}
				else
				{
					grd.RootTable.Columns["Update"].Visible = false;
				}
				grd.RootTable.Columns["Open"].Visible = false;
				btnOpenOrders.Visible = false;
			}
			if (grd.RootTable.Columns["Complete"].Visible || grd.RootTable.Columns["Cancel"].Visible || grd.RootTable.Columns["Update"].Visible || grd.RootTable.Columns["Open"].Visible)
			{
				grd.RootTable.Columns.Add("Select");
				grd.RootTable.Columns["Select"].ActAsSelector = true;
				grd.RootTable.Columns["Select"].UseHeaderSelector = true;
				grd.RootTable.Columns["Select"].Width = 40;
				grd.RootTable.Columns["ActionRemarks"].EditType = (EditType)1;
			}
			else
			{
				grd.RootTable.Columns["ActionRemarks"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			throw new Exception(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "DocNo")
			{
				if (UserAccount.RoleName == "Admin")
				{
					new ActivityDetails(UserAccount, Conversion.ToInt(item.Cells["Id"].Value)).Show();
				}
				else if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "ActivityDetails").ToList();
					if (newList.Count <= 0)
					{
						MessageBox.Show("you don't have Screen Rights");
						return;
					}
					if (!Conversion.ToBool((object)newList[0].Value.ToString()))
					{
						MessageBox.Show("you don't have Screen Rights");
						return;
					}
					new ActivityDetails(UserAccount, Conversion.ToInt(item.Cells["Id"].Value)).Show();
				}
			}
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(item.Cells["OrderSupCustId"].Value)), datFromDate.Checked ? datFromDate.Value : DateTime.Now.AddDays(-30.0), datToDate.Value);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "VoucherCode")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Cells["Id"].Value.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Slip")
			{
				GenerateReport();
			}
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grd.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grd.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
			if (e.Column.Key == "Complete")
			{
				CompleteStatus(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Open")
			{
				OpenStatus(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Cancel")
			{
				CancelStatus(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Update")
			{
				UpdateExpiryStatus(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
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
			if (tabControl1.SelectedIndex == 1)
			{
				datFromDate.Focus();
			}
			else if (tabControl1.SelectedIndex == 0)
			{
				datFromDate.Focus();
			}
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
				FilterDataSummary();
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

	private void FilterDataSummary()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			PanelOutstandingSummary.Visible = false;
			PanelOutstandingInProcessSummary.Visible = false;
			PanelInProcessSummary.Visible = false;
			PanelExpiringSummary.Visible = false;
			PanelCanceledSummary.Visible = false;
			DataTable dtGrid = PurchaseOrder.GetOrderStatus(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 41
			});
			if (dtGrid.Rows.Count > 0)
			{
				PanelOutstandingSummary.Visible = true;
				PanelOutstandingInProcessSummary.Visible = true;
				PanelInProcessSummary.Visible = true;
				PanelExpiringSummary.Visible = true;
				PanelCanceledSummary.Visible = true;
				double OutStandingSummary = 0.0;
				double OutStandingInProcessSummary = 0.0;
				double InProcessExcessSummary = 0.0;
				double ExpiringDaysSummary = 0.0;
				double CanceledSummary = 0.0;
				OutStandingSummary = Conversion.ToDouble(dtGrid.Rows[0]["OrderQty"]);
				OutStandingInProcessSummary = Conversion.ToDouble(dtGrid.Rows[1]["OrderQty"]);
				InProcessExcessSummary = Conversion.ToDouble(dtGrid.Rows[2]["OrderQty"]);
				ExpiringDaysSummary = Conversion.ToDouble(dtGrid.Rows[3]["OrderQty"]);
				CanceledSummary = Conversion.ToDouble(dtGrid.Rows[4]["OrderQty"]);
				txtOustandingSummary.Text = OutStandingSummary.ToString(clsGlobalVariables.stringFormatboth);
				txtOustandingInProcessSummary.Text = OutStandingInProcessSummary.ToString(clsGlobalVariables.stringFormatboth);
				txtInProcessSummary.Text = InProcessExcessSummary.ToString(clsGlobalVariables.stringFormatboth);
				txtExpiringDaysSummary.Text = ExpiringDaysSummary.ToString(clsGlobalVariables.stringFormatboth);
				txtCanceledSummary.Text = CanceledSummary.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtOustandingSummary.Text = string.Empty;
				txtOustandingInProcessSummary.Text = string.Empty;
				txtInProcessSummary.Text = string.Empty;
				txtExpiringDaysSummary.Text = string.Empty;
				txtCanceledSummary.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSummaryFill(int Activity = 0)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			waitForm.Show(this);
			label15.Visible = false;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoSumm.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoSumm.Text);
			obj.ParentCategoryId = Conversion.ToInt(cmbParentcategorySumm.Value);
			obj.ItemCategoryId = Conversion.ToInt(CmbCategorySumm.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtypeSumm.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSumm.Value);
			obj.BookingPersonId = Conversion.ToInt(cmbBookingPersonSumm.Value);
			obj.ItemId = Conversion.ToInt(cmbItemNameSumm.Value);
			obj.CropYear = ((((UltraGridBase)cmbCropSumm).ActiveRow != null && Conversion.ToInt(cmbCropSumm.Value) > 0) ? ((Control)(object)cmbCropSumm).Text : null);
			obj.JobLotId = Conversion.ToInt(cmbJobLotSumm.Value);
			obj.CityId = Conversion.ToInt(cmbcitySumm.Value);
			obj.DistrictId = Conversion.ToInt(cmbDistrictSumm.Value);
			obj.Activity = ((Control)(object)cmbActivitySumm).Text;
			if (datFromDateSumm.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDateSumm.Value);
			}
			if (datToDateSumm.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDateSumm.Value);
			}
			if (RadOpenSumm.Checked)
			{
				obj.Status = "Open";
			}
			else if (RadCompleteSumm.Checked)
			{
				obj.Status = "Complete";
			}
			else if (RadCancelSumm.Checked)
			{
				obj.Status = "Cancel";
			}
			if (RadApprovedOnlySumm.Checked)
			{
				obj.IsApproved = true;
			}
			else if (RadUnApprovedOnlySumm.Checked)
			{
				obj.IsApproved = false;
			}
			else if (radAllApprovedAndUnApprovedSumm.Checked)
			{
				obj.ApprovedFilter = "All";
			}
			if (chkIncludePackUom.Checked)
			{
				obj.PackSizeTo = 1;
			}
			if (chkIncludeCity.Checked)
			{
				obj.AppId = 1;
			}
			if (chkSkipZero.Checked)
			{
				obj.ActionId = 1;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchNameSumm).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchNameSumm).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				obj.ActivityId = Activity;
				switch (Activity)
				{
				case 1:
					label15.Text = "OutStanding && Open";
					break;
				case 2:
					label15.Text = "OutStanding && InProcess";
					break;
				case 3:
					label15.Text = "InProcess && Excess Received";
					break;
				case 4:
					label15.Text = "Expiring Next 7 Days";
					break;
				case 5:
					label15.Text = "Canceled";
					break;
				}
				label15.Visible = Activity > 0;
				dtSummary = PurchaseOrder.PurchaseOrderSummaryRegister(obj);
				if (dtSummary.Rows.Count > 0)
				{
					if (((UltraGridBase)cmbActivitySumm).ActiveRow != null)
					{
						if (((Control)(object)cmbActivitySumm).Text == "Order Register")
						{
							((Control)(object)GridSummary).Name = "OrderRegister";
							btnSummaryPrint.Text = "OrderRegister-391";
							dtOrderSummaryRegister.Rows.Clear();
							for (int i = 0; i < dtSummary.Rows.Count; i++)
							{
								dtOrderSummaryRegister.Rows.Add(dtSummary.Rows[i]["Id"], dtSummary.Rows[i]["DocumentTypeId"], dtSummary.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(dtSummary.Rows[i]["DocDate"]).ToShortDateString(), dtSummary.Rows[i]["DocNo"], dtSummary.Rows[i]["BranchName"], dtSummary.Rows[i]["ItemCode"], dtSummary.Rows[i]["ItemName"], dtSummary.Rows[i]["UOMCode"], dtSummary.Rows[i]["OrderQty"], dtSummary.Rows[i]["ReceivedQty"], dtSummary.Rows[i]["BalQty"], dtSummary.Rows[i]["OrderWeight"], dtSummary.Rows[i]["ReceivedWeight"], dtSummary.Rows[i]["BalWeight"], dtSummary.Rows[i]["OrderAmount"], dtSummary.Rows[i]["ReceivedAmount"], dtSummary.Rows[i]["BalAmount"], dtSummary.Rows[i]["AvgRate"], dtSummary.Rows[i]["PrcntOfTotal"], dtSummary.Rows[i]["CityName"], dtSummary.Rows[i]["Supplier"], dtSummary.Rows[i]["BookingPerson"], dtSummary.Rows[i]["CropBatch"], dtSummary.Rows[i]["JobLotCode"]);
							}
							GridSummary.DataSource = dtOrderSummaryRegister;
							GridSummary.RootTable.Columns["Id"].Visible = false;
							GridSummary.RootTable.Columns["DocumentTypeId"].Visible = false;
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
						{
							((Control)(object)GridSummary).Name = "OrderSummaryByItem";
							btnSummaryPrint.Text = "OrderSummaryByItem-392";
							dtOrderSummaryRegister.Rows.Clear();
							for (int j = 0; j < dtSummary.Rows.Count; j++)
							{
								dtOrderSummaryRegister.Rows.Add(0, 0, "", DateTime.Now, 0, dtSummary.Rows[j]["BranchName"], dtSummary.Rows[j]["ItemCode"], dtSummary.Rows[j]["ItemName"], dtSummary.Rows[j]["UOMCode"], dtSummary.Rows[j]["OrderQty"], dtSummary.Rows[j]["ReceivedQty"], dtSummary.Rows[j]["BalQty"], dtSummary.Rows[j]["OrderWeight"], dtSummary.Rows[j]["ReceivedWeight"], dtSummary.Rows[j]["BalWeight"], dtSummary.Rows[j]["OrderAmount"], dtSummary.Rows[j]["ReceivedAmount"], dtSummary.Rows[j]["BalAmount"], dtSummary.Rows[j]["AvgRate"], dtSummary.Rows[j]["PrcntOfTotal"], dtSummary.Rows[j]["CityName"], "", "", "", "");
							}
							GridSummary.DataSource = dtOrderSummaryRegister;
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Supplier")
						{
							((Control)(object)GridSummary).Name = "OrderSummaryBySupplier";
							btnSummaryPrint.Text = "OrderSummaryBySupplier-393";
							dtOrderSummaryRegister.Rows.Clear();
							for (int k = 0; k < dtSummary.Rows.Count; k++)
							{
								dtOrderSummaryRegister.Rows.Add(0, 0, "", DateTime.Now, 0, dtSummary.Rows[k]["BranchName"], "", "", dtSummary.Rows[k]["UOMCode"], dtSummary.Rows[k]["OrderQty"], dtSummary.Rows[k]["ReceivedQty"], dtSummary.Rows[k]["BalQty"], dtSummary.Rows[k]["OrderWeight"], dtSummary.Rows[k]["ReceivedWeight"], dtSummary.Rows[k]["BalWeight"], dtSummary.Rows[k]["OrderAmount"], dtSummary.Rows[k]["ReceivedAmount"], dtSummary.Rows[k]["BalAmount"], dtSummary.Rows[k]["AvgRate"], dtSummary.Rows[k]["PrcntOfTotal"], dtSummary.Rows[k]["CityName"], dtSummary.Rows[k]["Supplier"], "", "", "");
							}
							GridSummary.DataSource = dtOrderSummaryRegister;
						}
						else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Supplier & Item")
						{
							((Control)(object)GridSummary).Name = "OrderSummaryBySupplierAndItem";
							btnSummaryPrint.Text = "OrderSummaryBySupplierAndItem-394";
							dtOrderSummaryRegister.Rows.Clear();
							for (int l = 0; l < dtSummary.Rows.Count; l++)
							{
								dtOrderSummaryRegister.Rows.Add(0, 0, "", DateTime.Now, 0, dtSummary.Rows[l]["BranchName"], dtSummary.Rows[l]["ItemCode"], dtSummary.Rows[l]["ItemName"], dtSummary.Rows[l]["UOMCode"], dtSummary.Rows[l]["OrderQty"], dtSummary.Rows[l]["ReceivedQty"], dtSummary.Rows[l]["BalQty"], dtSummary.Rows[l]["OrderWeight"], dtSummary.Rows[l]["ReceivedWeight"], dtSummary.Rows[l]["BalWeight"], dtSummary.Rows[l]["OrderAmount"], dtSummary.Rows[l]["ReceivedAmount"], dtSummary.Rows[l]["BalAmount"], dtSummary.Rows[l]["AvgRate"], dtSummary.Rows[l]["PrcntOfTotal"], dtSummary.Rows[l]["CityName"], dtSummary.Rows[l]["Supplier"], "", "", "");
							}
							GridSummary.DataSource = dtOrderSummaryRegister;
						}
						GridSummary.RetrieveStructure();
						DataGridHistorySetting();
					}
					else
					{
						GridSummary.DataSource = null;
					}
				}
				else
				{
					waitForm.Close();
					GridSummary.DataSource = null;
				}
				return;
			}
			cmbBranchNameSumm.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			GridSummary.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			GridEX_Helper.GridWrappingAndColumnSettings(GridSummary, 2, 3);
			GridSummary.RootTable.Columns["RcvdQty"].Caption = "Received Qty";
			GridSummary.RootTable.Columns["BalQty"].Caption = "Balance Qty";
			GridSummary.RootTable.Columns["RcvdWeight"].Caption = "Received Weight";
			GridSummary.RootTable.Columns["BalWeight"].Caption = "Balance Weight";
			GridSummary.RootTable.Columns["RcvdAmount"].Caption = "Received Amount";
			GridSummary.RootTable.Columns["BalAmount"].Caption = "Balance Amount";
			GridSummary.RootTable.Columns["AvgRate"].Caption = "Average Rate";
			GridSummary.RootTable.Columns["PercentOfTotal"].Caption = "% Of Total";
			GridSummary.RootTable.Columns["Id"].Visible = false;
			GridSummary.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridSummary.RootTable.Columns["DocType"].Visible = false;
			GridSummary.RootTable.Columns["DocDate"].Visible = false;
			GridSummary.RootTable.Columns["DocNo"].Visible = false;
			GridSummary.RootTable.Columns["BranchName"].Visible = false;
			GridSummary.RootTable.Columns["Supplier"].Visible = false;
			GridSummary.RootTable.Columns["BookingPerson"].Visible = false;
			GridSummary.RootTable.Columns["ItemCode"].Visible = false;
			GridSummary.RootTable.Columns["ItemName"].Visible = false;
			GridSummary.RootTable.Columns["PackUom"].Visible = false;
			GridSummary.RootTable.Columns["CropYear"].Visible = false;
			GridSummary.RootTable.Columns["JobLot"].Visible = false;
			GridSummary.RootTable.Columns["OrderQty"].Visible = false;
			GridSummary.RootTable.Columns["RcvdQty"].Visible = false;
			GridSummary.RootTable.Columns["BalQty"].Visible = false;
			GridSummary.RootTable.Columns["OrderWeight"].Visible = false;
			GridSummary.RootTable.Columns["RcvdWeight"].Visible = false;
			GridSummary.RootTable.Columns["BalWeight"].Visible = false;
			GridSummary.RootTable.Columns["OrderAmount"].Visible = false;
			GridSummary.RootTable.Columns["RcvdAmount"].Visible = false;
			GridSummary.RootTable.Columns["BalAmount"].Visible = false;
			GridSummary.RootTable.Columns["AvgRate"].Visible = false;
			GridSummary.RootTable.Columns["PercentOfTotal"].Visible = false;
			GridSummary.RootTable.Columns["CityName"].Visible = false;
			if (((Control)(object)cmbActivitySumm).Text == "Order Register")
			{
				GridSummary.RootTable.Columns["DocType"].Visible = true;
				GridSummary.RootTable.Columns["DocDate"].Visible = true;
				GridSummary.RootTable.Columns["DocNo"].Visible = true;
				GridSummary.RootTable.Columns["BookingPerson"].Visible = true;
				GridSummary.RootTable.Columns["BranchName"].Visible = true;
				GridSummary.RootTable.Columns["ItemCode"].Visible = true;
				GridSummary.RootTable.Columns["ItemName"].Visible = true;
				GridSummary.RootTable.Columns["PackUom"].Visible = true;
				GridSummary.RootTable.Columns["CropYear"].Visible = true;
				GridSummary.RootTable.Columns["JobLot"].Visible = true;
				GridSummary.RootTable.Columns["OrderQty"].Visible = true;
				GridSummary.RootTable.Columns["RcvdQty"].Visible = true;
				GridSummary.RootTable.Columns["BalQty"].Visible = true;
				GridSummary.RootTable.Columns["OrderWeight"].Visible = true;
				GridSummary.RootTable.Columns["RcvdWeight"].Visible = true;
				GridSummary.RootTable.Columns["BalWeight"].Visible = true;
				GridSummary.RootTable.Columns["OrderAmount"].Visible = true;
				GridSummary.RootTable.Columns["RcvdAmount"].Visible = true;
				GridSummary.RootTable.Columns["BalAmount"].Visible = true;
				GridSummary.RootTable.Columns["AvgRate"].Visible = true;
				GridSummary.RootTable.Columns["PercentOfTotal"].Visible = true;
				GridSummary.RootTable.Columns["CityName"].Visible = true;
			}
			else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Item")
			{
				GridSummary.RootTable.Columns["BranchName"].Visible = true;
				GridSummary.RootTable.Columns["ItemCode"].Visible = true;
				GridSummary.RootTable.Columns["ItemName"].Visible = true;
				GridSummary.RootTable.Columns["PackUom"].Visible = true;
				GridSummary.RootTable.Columns["OrderQty"].Visible = true;
				GridSummary.RootTable.Columns["RcvdQty"].Visible = true;
				GridSummary.RootTable.Columns["BalQty"].Visible = true;
				GridSummary.RootTable.Columns["OrderWeight"].Visible = true;
				GridSummary.RootTable.Columns["RcvdWeight"].Visible = true;
				GridSummary.RootTable.Columns["BalWeight"].Visible = true;
				GridSummary.RootTable.Columns["OrderAmount"].Visible = true;
				GridSummary.RootTable.Columns["RcvdAmount"].Visible = true;
				GridSummary.RootTable.Columns["BalAmount"].Visible = true;
				GridSummary.RootTable.Columns["AvgRate"].Visible = true;
				GridSummary.RootTable.Columns["PercentOfTotal"].Visible = true;
				GridSummary.RootTable.Columns["CityName"].Visible = true;
			}
			else if (((Control)(object)cmbActivitySumm).Text == "Order Summary By Supplier")
			{
				GridSummary.RootTable.Columns["BranchName"].Visible = true;
				GridSummary.RootTable.Columns["Supplier"].Visible = true;
				GridSummary.RootTable.Columns["PackUom"].Visible = true;
				GridSummary.RootTable.Columns["OrderQty"].Visible = true;
				GridSummary.RootTable.Columns["RcvdQty"].Visible = true;
				GridSummary.RootTable.Columns["BalQty"].Visible = true;
				GridSummary.RootTable.Columns["OrderWeight"].Visible = true;
				GridSummary.RootTable.Columns["RcvdWeight"].Visible = true;
				GridSummary.RootTable.Columns["BalWeight"].Visible = true;
				GridSummary.RootTable.Columns["OrderAmount"].Visible = true;
				GridSummary.RootTable.Columns["RcvdAmount"].Visible = true;
				GridSummary.RootTable.Columns["BalAmount"].Visible = true;
				GridSummary.RootTable.Columns["AvgRate"].Visible = true;
				GridSummary.RootTable.Columns["PercentOfTotal"].Visible = true;
				GridSummary.RootTable.Columns["CityName"].Visible = true;
			}
			else
			{
				GridSummary.RootTable.Columns["BranchName"].Visible = true;
				GridSummary.RootTable.Columns["Supplier"].Visible = true;
				GridSummary.RootTable.Columns["ItemCode"].Visible = true;
				GridSummary.RootTable.Columns["ItemName"].Visible = true;
				GridSummary.RootTable.Columns["PackUom"].Visible = true;
				GridSummary.RootTable.Columns["OrderQty"].Visible = true;
				GridSummary.RootTable.Columns["RcvdQty"].Visible = true;
				GridSummary.RootTable.Columns["BalQty"].Visible = true;
				GridSummary.RootTable.Columns["OrderWeight"].Visible = true;
				GridSummary.RootTable.Columns["RcvdWeight"].Visible = true;
				GridSummary.RootTable.Columns["BalWeight"].Visible = true;
				GridSummary.RootTable.Columns["OrderAmount"].Visible = true;
				GridSummary.RootTable.Columns["RcvdAmount"].Visible = true;
				GridSummary.RootTable.Columns["BalAmount"].Visible = true;
				GridSummary.RootTable.Columns["AvgRate"].Visible = true;
				GridSummary.RootTable.Columns["PercentOfTotal"].Visible = true;
				GridSummary.RootTable.Columns["CityName"].Visible = true;
			}
			GridSummary.RootTable.Columns["DocType"].Width = Constants.ExportConstants.CarrierTypeConstant;
			GridSummary.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			GridSummary.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			GridSummary.RootTable.Columns["BranchName"].Width = Constants.ExportConstants.StringConstant;
			GridSummary.RootTable.Columns["BookingPerson"].Width = Constants.InventoryConstants.ReferenceParty;
			GridSummary.RootTable.Columns["Supplier"].Width = Constants.ExportConstants.StringConstant;
			GridSummary.RootTable.Columns["ItemCode"].Width = Constants.ExportConstants.TermConstant;
			GridSummary.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.StringConstant;
			GridSummary.RootTable.Columns["PackUom"].Width = Constants.ExportConstants.UomConstant;
			GridSummary.RootTable.Columns["CropYear"].Width = Constants.ExportConstants.CropYearConstant;
			GridSummary.RootTable.Columns["JobLot"].Width = Constants.ExportConstants.JobLotConstant;
			GridSummary.RootTable.Columns["OrderQty"].Width = Constants.ExportConstants.QtyConstant;
			GridSummary.RootTable.Columns["RcvdQty"].Width = Constants.ExportConstants.QtyConstant;
			GridSummary.RootTable.Columns["BalQty"].Width = Constants.ExportConstants.QtyConstant;
			GridSummary.RootTable.Columns["OrderWeight"].Width = Constants.ExportConstants.WeightConstant;
			GridSummary.RootTable.Columns["RcvdWeight"].Width = Constants.ExportConstants.WeightConstant;
			GridSummary.RootTable.Columns["BalWeight"].Width = Constants.ExportConstants.WeightConstant;
			GridSummary.RootTable.Columns["OrderAmount"].Width = Constants.ExportConstants.AmountConstant;
			GridSummary.RootTable.Columns["RcvdAmount"].Width = Constants.ExportConstants.AmountConstant;
			GridSummary.RootTable.Columns["BalAmount"].Width = Constants.ExportConstants.AmountConstant;
			GridSummary.RootTable.Columns["AvgRate"].Width = Constants.ExportConstants.ExRateConstant;
			GridSummary.RootTable.Columns["PercentOfTotal"].Width = Constants.ExportConstants.PercentOfTotal;
			GridSummary.RootTable.Columns["CityName"].Width = Constants.ExportConstants.TermConstant;
			GridSummary.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			GridSummary.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			GridSummary.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdQty"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["RcvdQty"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["RcvdQty"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdQty"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["OrderWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["RcvdWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["RcvdWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdWeight"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["BalWeight"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["OrderAmount"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["OrderAmount"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdAmount"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["RcvdAmount"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["RcvdAmount"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["RcvdAmount"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["BalAmount"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0.###";
			GridSummary.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["AvgRate"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["PercentOfTotal"].AggregateFunction = (AggregateFunction)2;
			GridSummary.RootTable.Columns["PercentOfTotal"].TextAlignment = (TextAlignment)3;
			GridSummary.RootTable.Columns["PercentOfTotal"].FormatString = "#,##0.###";
			GridSummary.RootTable.Columns["PercentOfTotal"].TotalFormatString = "#,##0.###";
			if (((Control)(object)cmbActivitySumm).Text == "Order Register" && CommonServices.CheckDocumentInApprovalPolicy(41))
			{
				GridSummary.RootTable.Columns.Add("ApprovalDetail");
				GridSummary.RootTable.Columns["ApprovalDetail"].Caption = "Approval Detail";
				GridSummary.RootTable.Columns["ApprovalDetail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GridSummary.RootTable.Columns["ApprovalDetail"].ButtonStyle = (ButtonStyle)5;
				GridSummary.RootTable.Columns["ApprovalDetail"].ButtonText = "Approval Detail";
				GridSummary.RootTable.Columns["ApprovalDetail"].Width = 100;
				GridSummary.RootTable.Columns["ApprovalDetail"].Position = 0;
				GridSummary.FrozenColumns = 1;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSummary_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GridSummary.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GridSummary.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GridSummary.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSummaryPrint_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			if (dtSummary == null || dtSummary.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			if (btnSummaryPrint.Text == "OrderRegister-391")
			{
				rpt.ShowReportWithDataTable(dtSummary, "391-OrderRegister.rpt");
			}
			else if (btnSummaryPrint.Text == "OrderSummaryByItem-392")
			{
				rpt.ShowReportWithDataTable(dtSummary, "392-OrderSummaryByItem.rpt");
			}
			else if (btnSummaryPrint.Text == "OrderSummaryBySupplier-393")
			{
				rpt.ShowReportWithDataTable(dtSummary, "393-OrderSummaryBySupplier.rpt");
			}
			else if (btnSummaryPrint.Text == "OrderSummaryBySupplierAndItem-394")
			{
				rpt.ShowReportWithDataTable(dtSummary, "394-OrderSummaryByCustomerandItem.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompleteStatus(int Id)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		try
		{
			if (!RadApprovedOnlyDetail.Checked || !formright.CanChangeOrderStatusToComplete)
			{
				return;
			}
			string Remarks = "";
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
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
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Complete Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenStatus(int Id)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		try
		{
			if (!RadApprovedOnlyDetail.Checked || !formright.CanChangeOrderStatusToOpen)
			{
				return;
			}
			string Remarks = "";
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Open Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = "Open";
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Open Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CancelStatus(int Id)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
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
				item.Comments = Remarks;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record Cancel Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateExpiryStatus(int Id)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		try
		{
			string Remarks = "";
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Remarks = Conversion.ToString((object)row.Cells["ActionRemarks"].Text);
				if (Remarks == string.Empty && !ActionRemarksRequiredNotOnPurchaseOrderStatusUpdatation)
				{
					MessageBox.Show("Action Remarks Required");
					return;
				}
			}
			if (MessageBox.Show("Are you sure to ExpiryDate Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
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
				item.Comments = Remarks;
				item.PostDate = Conversion.ToDateTime(grd.CurrentRow.Cells["ExpiryDate"].Value);
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Update ExpiryDate Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				toolStripButton2_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3) && btnOpenOrders.Visible)
			{
				btnOpenOrders_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4) && btnCompleteOrders.Visible)
			{
				btnCompleteOrders_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5) && btnCancelOrders.Visible)
			{
				btnCancelOrders_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDate.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeysForDetailTab();
			}
		}
		if (tabControl1.SelectedIndex == 1)
		{
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShowSumm_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GridSummary).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDateSumm.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnSummaryPrint_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeysForSummaryTab();
			}
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				tabControl1.SelectedIndex = 1;
				datFromDateSumm.Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
				datFromDate.Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefreshSummar_Click(null, null);
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
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (grd.CurrentColumn.Key == "Complete")
			{
				CompleteStatus(Id);
			}
			else if (grd.CurrentColumn.Key == "Cancel")
			{
				CancelStatus(Id);
			}
			else if (grd.CurrentColumn.Key == "Open")
			{
				OpenStatus(Id);
			}
			else if (grd.CurrentColumn.Key == "Update")
			{
				UpdateExpiryStatus(Id);
			}
			else if (grd.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(r.Cells["OrderSupCustId"].Value)), datFromDate.Checked ? datFromDate.Value : DateTime.Now.AddDays(-30.0), datToDate.Value);
			}
			else if (grd.CurrentColumn.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), datFromDate.Value, datToDate.Value);
			}
			else if (grd.CurrentColumn.Key == "DocNo")
			{
				new SaleActivityDetails(UserAccount, Id).Show();
			}
			else if (grd.CurrentColumn.Key == "Slip")
			{
				GenerateReport();
			}
			else if (grd.CurrentColumn.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (grd.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(grd.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSummary_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GridSummary.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GridSummary.CurrentColumn != null && GridSummary.CurrentColumn.Key == "ApprovalDetail")
			{
				frmApprovalCommentory obj = new frmApprovalCommentory(UserAccount);
				if (GridSummary.CurrentRow.Cells.Count > 0)
				{
					obj.RefDocumentTypeId = Conversion.ToInt(GridSummary.CurrentRow.Cells["DocumentTypeId"].Value);
					obj.RefDocNoId = Conversion.ToInt(GridSummary.CurrentRow.Cells["Id"].Value);
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysForDetailTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeysForDetailTab()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show History");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Esc", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 206");
			dt.Rows.Add("Alt+2", "For Print 205");
			dt.Rows.Add("Alt+3", "For Open Multi Records");
			dt.Rows.Add("Alt+4", "For Complete Multi Record");
			dt.Rows.Add("Alt+5", "For Cancel Multi Records");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on From Date");
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

	private void btnShortCutKeySumamryTab_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysForSummaryTab();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeysForSummaryTab()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show History");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on From Date");
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

	private void GenerateReport()
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (globaldt == null || globaldt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(globaldt, "206-RptPurchaseOrderRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (globaldt == null || globaldt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(globaldt, "205-InvRptPurchaseOrderDetailRegisterRice.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridSummary).Name);
			if (fs != null)
			{
				GridSummary.LoadLayoutFile((Stream)fs);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, grdDetailTableForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtSrFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSrTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllDropDownBind(GetDropDownData());
				return;
			}
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)CmbSupplier).DataSource = null;
			((Control)(object)CmbItem).Text = string.Empty;
			((UltraGridBase)CmbItem).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchNameSumm_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchNameSumm).Text != string.Empty && ((UltraGridBase)cmbBranchNameSumm).ActiveRow != null)
			{
				AllDropDownBind(GetDropDownData());
				return;
			}
			((Control)(object)CmbCategorySumm).Text = string.Empty;
			((UltraGridBase)CmbCategorySumm).DataSource = null;
			((Control)(object)cmbItemtypeSumm).Text = string.Empty;
			((UltraGridBase)cmbItemtypeSumm).DataSource = null;
			((Control)(object)cmbCropSumm).Text = string.Empty;
			((UltraGridBase)cmbCropSumm).DataSource = null;
			((Control)(object)cmbDistrictSumm).Text = string.Empty;
			((UltraGridBase)cmbDistrictSumm).DataSource = null;
			((Control)(object)cmbcitySumm).Text = string.Empty;
			((UltraGridBase)cmbcitySumm).DataSource = null;
			((Control)(object)cmbJobLotSumm).Text = string.Empty;
			((UltraGridBase)cmbJobLotSumm).DataSource = null;
			((Control)(object)cmbItemNameSumm).Text = string.Empty;
			((UltraGridBase)cmbItemNameSumm).DataSource = null;
			((Control)(object)cmbCustomerSumm).Text = string.Empty;
			((UltraGridBase)cmbCustomerSumm).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelOutstanding_Click(object sender, EventArgs e)
	{
		try
		{
			BindDetailData(GetDetailData(1));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelOutstandingInProcess_Click(object sender, EventArgs e)
	{
		try
		{
			BindDetailData(GetDetailData(2));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelInProcess_Click(object sender, EventArgs e)
	{
		try
		{
			BindDetailData(GetDetailData(3));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelExpiringDays_Click(object sender, EventArgs e)
	{
		try
		{
			BindDetailData(GetDetailData(4));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelCanceled_Click(object sender, EventArgs e)
	{
		try
		{
			BindDetailData(GetDetailData(5));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelOutstandingSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GridSummaryFill(1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelOutstandingInProcessSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GridSummaryFill(2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelInProcessSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GridSummaryFill(3);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelExpiringSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GridSummaryFill(4);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PanelCanceledSummary_Click(object sender, EventArgs e)
	{
		try
		{
			GridSummaryFill(5);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdGroupByCategory_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetDataRows().Count() > 0)
			{
				((JanusCollectionBase)grd.RootTable.Groups).Clear();
				if (RdGroupByCategory.Checked)
				{
					grd.RootTable.Groups.Add("ItemCategory");
				}
				else if (rdGroupByItemType.Checked)
				{
					grd.RootTable.Groups.Add("ItemType");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnOpenOrders_Click(object sender, EventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		try
		{
			if (!RadApprovedOnlyDetail.Checked || !formright.CanChangeOrderStatusToOpen)
			{
				return;
			}
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows To Proceed!");
			}
			if (MessageBox.Show("Are you sure to Open Status?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grd.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.EntryUser = UserAccount.ID;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Open";
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Open Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCompleteOrders_Click(object sender, EventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		try
		{
			if (!RadApprovedOnlyDetail.Checked || !formright.CanChangeOrderStatusToComplete)
			{
				return;
			}
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows To Proceed!");
			}
			if (MessageBox.Show("Are you sure to Complete Status?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grd.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.EntryUser = UserAccount.ID;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Status";
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Completed Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelOrders_Click(object sender, EventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		try
		{
			if (!RadApprovedOnlyDetail.Checked || !formright.CanChangeOrderStatusToCancel)
			{
				return;
			}
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Check Rows To Proceed!");
			}
			if (MessageBox.Show("Are you sure to Cancel Status?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			RemarksPopUp remarksPopUp = new RemarksPopUp();
			remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
			remarksPopUp.ShowDialog();
			string Remarks = remarksPopUp.txtRemarks.Text;
			if (Remarks == null || Remarks == string.Empty)
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (grd.GetCheckedRows().Count() > 0)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.EntryUser = UserAccount.ID;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "Cancel";
					item.Comments = Remarks;
					obj.AllApprovalLists.Add(item);
				}
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Orders Cancel Successfully");
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		if (grd.RowCount <= 0)
		{
			return;
		}
		GridEXColumn Closing = grd.RootTable.Columns["ExpiryDate"];
		GridEXColumn DeliveryDays = grd.RootTable.Columns["DeliveryDays"];
		if (Closing != null && DeliveryDays != null)
		{
			GridEXCell obj = e.Row.Cells[((JanusCollectionElement)Closing).Index];
			GridEXCell deliveryCell = e.Row.Cells[((JanusCollectionElement)DeliveryDays).Index];
			if (Conversion.ToDateTime(obj.Value).Date < DateTime.Now)
			{
				deliveryCell.FormatStyle = new GridEXFormatStyle();
				deliveryCell.FormatStyle.ForeColor = Color.Red;
				deliveryCell.FormatStyle.FontBold = (TriState)2;
			}
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
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Expected O, but got Unknown
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Expected O, but got Unknown
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Expected O, but got Unknown
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Expected O, but got Unknown
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Expected O, but got Unknown
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Expected O, but got Unknown
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Expected O, but got Unknown
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Expected O, but got Unknown
		//IL_0f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f62: Expected O, but got Unknown
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f79: Expected O, but got Unknown
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f90: Expected O, but got Unknown
		//IL_46e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_46ef: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.InventoryReports.PurchaseOrderHistory));
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		GridEXLayout GridSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
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
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtSrTo = new System.Windows.Forms.TextBox();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.rdGroupByItemType = new System.Windows.Forms.RadioButton();
		this.RdGroupByCategory = new System.Windows.Forms.RadioButton();
		this.label36 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PanelCanceled = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCanceled = new System.Windows.Forms.TextBox();
		this.PanelExpiringDays = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.txtExpiringDays = new System.Windows.Forms.TextBox();
		this.PanelInProcess = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.txtInProcessExcess = new System.Windows.Forms.TextBox();
		this.PanelOutstandingInProcess = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.txtOutstandingInprocess = new System.Windows.Forms.TextBox();
		this.PanelOutstanding = new System.Windows.Forms.Panel();
		this.txtOutstandingValue = new System.Windows.Forms.TextBox();
		this.labelInterval01 = new System.Windows.Forms.Label();
		this.RadCancelDetail = new System.Windows.Forms.RadioButton();
		this.RadCompleteDetail = new System.Windows.Forms.RadioButton();
		this.RadOpenDetail = new System.Windows.Forms.RadioButton();
		this.RadAllOpenCompCancelDetail = new System.Windows.Forms.RadioButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadUnApprovedOnlyDetail = new System.Windows.Forms.RadioButton();
		this.CmbParentCatgory = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.radAllApprovedAndUnApprovedDetail = new System.Windows.Forms.RadioButton();
		this.RadApprovedOnlyDetail = new System.Windows.Forms.RadioButton();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.cmbBranchName = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPurchsaeOrderDetail = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnOpenOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCompleteOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCancelOrders = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.label15 = new System.Windows.Forms.Label();
		this.GridSummary = new GridEX();
		this.label9 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.PanelCanceledSummary = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.txtCanceledSummary = new System.Windows.Forms.TextBox();
		this.PanelExpiringSummary = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.txtExpiringDaysSummary = new System.Windows.Forms.TextBox();
		this.PanelInProcessSummary = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.txtInProcessSummary = new System.Windows.Forms.TextBox();
		this.PanelOutstandingInProcessSummary = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.txtOustandingInProcessSummary = new System.Windows.Forms.TextBox();
		this.RadCancelSumm = new System.Windows.Forms.RadioButton();
		this.PanelOutstandingSummary = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.txtOustandingSummary = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbCustomerSumm = new UltraCombo();
		this.cmbActivitySumm = new UltraCombo();
		this.cmbItemNameSumm = new UltraCombo();
		this.cmbParentcategorySumm = new UltraCombo();
		this.datToDateSumm = new System.Windows.Forms.DateTimePicker();
		this.datFromDateSumm = new System.Windows.Forms.DateTimePicker();
		this.cmbBranchNameSumm = new UltraCombo();
		this.txtFromDocNoSumm = new System.Windows.Forms.TextBox();
		this.txtToDocNoSumm = new System.Windows.Forms.TextBox();
		this.cmbItemtypeSumm = new UltraCombo();
		this.CmbCategorySumm = new UltraCombo();
		this.RadUnApprovedOnlySumm = new System.Windows.Forms.RadioButton();
		this.BtnShowSumm = new System.Windows.Forms.Button();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.chkIncludeCity = new System.Windows.Forms.CheckBox();
		this.chkIncludePackUom = new System.Windows.Forms.CheckBox();
		this.radAllApprovedAndUnApprovedSumm = new System.Windows.Forms.RadioButton();
		this.RadApprovedOnlySumm = new System.Windows.Forms.RadioButton();
		this.label5 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbDistrictSumm = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbcitySumm = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbJobLotSumm = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbCropSumm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.RadCompleteSumm = new System.Windows.Forms.RadioButton();
		this.RadAllOpenCompCancelSumm = new System.Windows.Forms.RadioButton();
		this.RadOpenSumm = new System.Windows.Forms.RadioButton();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewSummary = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshSummar = new System.Windows.Forms.ToolStripButton();
		this.btnSummaryPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeySumamryTab = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.CmbBookingPerson = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbBookingPersonSumm = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.PanelCanceled.SuspendLayout();
		this.PanelExpiringDays.SuspendLayout();
		this.PanelInProcess.SuspendLayout();
		this.PanelOutstandingInProcess.SuspendLayout();
		this.PanelOutstanding.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCatgory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridSummary).BeginInit();
		this.panel7.SuspendLayout();
		this.PanelCanceledSummary.SuspendLayout();
		this.PanelExpiringSummary.SuspendLayout();
		this.PanelInProcessSummary.SuspendLayout();
		this.PanelOutstandingInProcessSummary.SuspendLayout();
		this.PanelOutstandingSummary.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropSumm).BeginInit();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBookingPersonSumm).BeginInit();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1364, 27);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(208, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Purchase Order Register";
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 195);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1350, 401);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick_1);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(704, 98);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 23);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtSrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrTo.Location = new System.Drawing.Point(315, 45);
		this.txtSrTo.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrTo.Name = "txtSrTo";
		this.txtSrTo.Size = new System.Drawing.Size(139, 23);
		this.txtSrTo.TabIndex = 4;
		this.txtSrTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrTo_KeyPress);
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(315, 17);
		this.txtSrFrom.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(139, 23);
		this.txtSrFrom.TabIndex = 3;
		this.txtSrFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSrFrom_KeyPress);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance191;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(545, 15);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 6;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(545, 43);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 7;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(457, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(224, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 106;
		this.label4.Text = "Po No From";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(457, 49);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Supplier Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 49);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1364, 652);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 27);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1364, 625);
		this.tabControl1.TabIndex = 22;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.toolStrip3);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1356, 599);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.rdGroupByItemType);
		this.panel4.Controls.Add(this.RdGroupByCategory);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.ctrlGrdBar3);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 163);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1350, 32);
		this.panel4.TabIndex = 22;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(388, 8);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(65, 16);
		this.label19.TabIndex = 26;
		this.label19.Text = "Group By";
		this.rdGroupByItemType.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupByItemType.ForeColor = System.Drawing.Color.White;
		this.rdGroupByItemType.Location = new System.Drawing.Point(549, 1);
		this.rdGroupByItemType.Name = "rdGroupByItemType";
		this.rdGroupByItemType.Size = new System.Drawing.Size(63, 30);
		this.rdGroupByItemType.TabIndex = 25;
		this.rdGroupByItemType.Text = "Item Type";
		this.rdGroupByItemType.UseVisualStyleBackColor = true;
		this.RdGroupByCategory.Checked = true;
		this.RdGroupByCategory.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdGroupByCategory.ForeColor = System.Drawing.Color.White;
		this.RdGroupByCategory.Location = new System.Drawing.Point(463, 1);
		this.RdGroupByCategory.Name = "RdGroupByCategory";
		this.RdGroupByCategory.Size = new System.Drawing.Size(85, 30);
		this.RdGroupByCategory.TabIndex = 24;
		this.RdGroupByCategory.TabStop = true;
		this.RdGroupByCategory.Text = "Item Category";
		this.RdGroupByCategory.UseVisualStyleBackColor = true;
		this.RdGroupByCategory.CheckedChanged += new System.EventHandler(RdGroupByCategory_CheckedChanged);
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.Teal;
		this.label36.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(153, 8);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(53, 16);
		this.label36.TabIndex = 23;
		this.label36.Text = "label36";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1308, 5);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(1, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(132, 18);
		this.label7.TabIndex = 2;
		this.label7.Text = "Filtered Records";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel2.Controls.Add(this.PanelCanceled);
		this.panel2.Controls.Add(this.PanelExpiringDays);
		this.panel2.Controls.Add(this.PanelInProcess);
		this.panel2.Controls.Add(this.PanelOutstandingInProcess);
		this.panel2.Controls.Add(this.PanelOutstanding);
		this.panel2.Controls.Add(this.RadCancelDetail);
		this.panel2.Controls.Add(this.RadCompleteDetail);
		this.panel2.Controls.Add(this.RadOpenDetail);
		this.panel2.Controls.Add(this.RadAllOpenCompCancelDetail);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1350, 133);
		this.panel2.TabIndex = 0;
		this.PanelCanceled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelCanceled.Controls.Add(this.label3);
		this.PanelCanceled.Controls.Add(this.txtCanceled);
		this.PanelCanceled.Location = new System.Drawing.Point(1248, 5);
		this.PanelCanceled.Name = "PanelCanceled";
		this.PanelCanceled.Size = new System.Drawing.Size(99, 54);
		this.PanelCanceled.TabIndex = 166;
		this.PanelCanceled.Visible = false;
		this.PanelCanceled.Click += new System.EventHandler(PanelCanceled_Click);
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(97, 21);
		this.label3.TabIndex = 107;
		this.label3.Text = "Canceled";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtCanceled.BackColor = System.Drawing.Color.White;
		this.txtCanceled.Enabled = false;
		this.txtCanceled.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtCanceled.Location = new System.Drawing.Point(1, 22);
		this.txtCanceled.Margin = new System.Windows.Forms.Padding(2);
		this.txtCanceled.Name = "txtCanceled";
		this.txtCanceled.ReadOnly = true;
		this.txtCanceled.Size = new System.Drawing.Size(95, 29);
		this.txtCanceled.TabIndex = 0;
		this.txtCanceled.Click += new System.EventHandler(PanelCanceled_Click);
		this.PanelExpiringDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelExpiringDays.Controls.Add(this.label10);
		this.PanelExpiringDays.Controls.Add(this.txtExpiringDays);
		this.PanelExpiringDays.Location = new System.Drawing.Point(1036, 62);
		this.PanelExpiringDays.Name = "PanelExpiringDays";
		this.PanelExpiringDays.Size = new System.Drawing.Size(210, 51);
		this.PanelExpiringDays.TabIndex = 167;
		this.PanelExpiringDays.Visible = false;
		this.PanelExpiringDays.Click += new System.EventHandler(PanelExpiringDays_Click);
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Dock = System.Windows.Forms.DockStyle.Top;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(0, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(208, 17);
		this.label10.TabIndex = 107;
		this.label10.Text = "Expiring Next 7 Days";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtExpiringDays.BackColor = System.Drawing.Color.White;
		this.txtExpiringDays.Enabled = false;
		this.txtExpiringDays.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtExpiringDays.Location = new System.Drawing.Point(1, 18);
		this.txtExpiringDays.Margin = new System.Windows.Forms.Padding(2);
		this.txtExpiringDays.Name = "txtExpiringDays";
		this.txtExpiringDays.ReadOnly = true;
		this.txtExpiringDays.Size = new System.Drawing.Size(206, 29);
		this.txtExpiringDays.TabIndex = 0;
		this.txtExpiringDays.Click += new System.EventHandler(PanelExpiringDays_Click);
		this.PanelInProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInProcess.Controls.Add(this.label13);
		this.PanelInProcess.Controls.Add(this.txtInProcessExcess);
		this.PanelInProcess.Location = new System.Drawing.Point(1036, 5);
		this.PanelInProcess.Name = "PanelInProcess";
		this.PanelInProcess.Size = new System.Drawing.Size(210, 54);
		this.PanelInProcess.TabIndex = 168;
		this.PanelInProcess.Visible = false;
		this.PanelInProcess.Click += new System.EventHandler(PanelInProcess_Click);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(208, 21);
		this.label13.TabIndex = 107;
		this.label13.Text = "InProcess && Excess Received";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInProcessExcess.BackColor = System.Drawing.Color.White;
		this.txtInProcessExcess.Enabled = false;
		this.txtInProcessExcess.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtInProcessExcess.Location = new System.Drawing.Point(1, 22);
		this.txtInProcessExcess.Margin = new System.Windows.Forms.Padding(2);
		this.txtInProcessExcess.Name = "txtInProcessExcess";
		this.txtInProcessExcess.ReadOnly = true;
		this.txtInProcessExcess.Size = new System.Drawing.Size(206, 29);
		this.txtInProcessExcess.TabIndex = 0;
		this.txtInProcessExcess.Click += new System.EventHandler(PanelInProcess_Click);
		this.PanelOutstandingInProcess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelOutstandingInProcess.Controls.Add(this.label37);
		this.PanelOutstandingInProcess.Controls.Add(this.txtOutstandingInprocess);
		this.PanelOutstandingInProcess.Location = new System.Drawing.Point(867, 62);
		this.PanelOutstandingInProcess.Name = "PanelOutstandingInProcess";
		this.PanelOutstandingInProcess.Size = new System.Drawing.Size(166, 51);
		this.PanelOutstandingInProcess.TabIndex = 169;
		this.PanelOutstandingInProcess.Visible = false;
		this.PanelOutstandingInProcess.Click += new System.EventHandler(PanelOutstandingInProcess_Click);
		this.label37.BackColor = System.Drawing.Color.Teal;
		this.label37.Dock = System.Windows.Forms.DockStyle.Top;
		this.label37.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(0, 0);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(164, 17);
		this.label37.TabIndex = 107;
		this.label37.Text = "Outstanding && InProcess";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOutstandingInprocess.BackColor = System.Drawing.Color.White;
		this.txtOutstandingInprocess.Enabled = false;
		this.txtOutstandingInprocess.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtOutstandingInprocess.Location = new System.Drawing.Point(1, 19);
		this.txtOutstandingInprocess.Margin = new System.Windows.Forms.Padding(2);
		this.txtOutstandingInprocess.Name = "txtOutstandingInprocess";
		this.txtOutstandingInprocess.ReadOnly = true;
		this.txtOutstandingInprocess.Size = new System.Drawing.Size(162, 29);
		this.txtOutstandingInprocess.TabIndex = 0;
		this.txtOutstandingInprocess.Click += new System.EventHandler(PanelOutstandingInProcess_Click);
		this.PanelOutstanding.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelOutstanding.Controls.Add(this.txtOutstandingValue);
		this.PanelOutstanding.Controls.Add(this.labelInterval01);
		this.PanelOutstanding.Location = new System.Drawing.Point(867, 5);
		this.PanelOutstanding.Name = "PanelOutstanding";
		this.PanelOutstanding.Size = new System.Drawing.Size(166, 54);
		this.PanelOutstanding.TabIndex = 165;
		this.PanelOutstanding.Visible = false;
		this.PanelOutstanding.Click += new System.EventHandler(PanelOutstanding_Click);
		this.txtOutstandingValue.BackColor = System.Drawing.Color.White;
		this.txtOutstandingValue.Enabled = false;
		this.txtOutstandingValue.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtOutstandingValue.Location = new System.Drawing.Point(1, 22);
		this.txtOutstandingValue.Margin = new System.Windows.Forms.Padding(2);
		this.txtOutstandingValue.Name = "txtOutstandingValue";
		this.txtOutstandingValue.ReadOnly = true;
		this.txtOutstandingValue.Size = new System.Drawing.Size(162, 29);
		this.txtOutstandingValue.TabIndex = 0;
		this.txtOutstandingValue.Click += new System.EventHandler(PanelOutstanding_Click);
		this.labelInterval01.BackColor = System.Drawing.Color.Teal;
		this.labelInterval01.Dock = System.Windows.Forms.DockStyle.Top;
		this.labelInterval01.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelInterval01.ForeColor = System.Drawing.Color.White;
		this.labelInterval01.Location = new System.Drawing.Point(0, 0);
		this.labelInterval01.Name = "labelInterval01";
		this.labelInterval01.Size = new System.Drawing.Size(164, 21);
		this.labelInterval01.TabIndex = 107;
		this.labelInterval01.Text = "Outstanding && Open";
		this.labelInterval01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.RadCancelDetail.AutoSize = true;
		this.RadCancelDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCancelDetail.Location = new System.Drawing.Point(777, 68);
		this.RadCancelDetail.Name = "RadCancelDetail";
		this.RadCancelDetail.Size = new System.Drawing.Size(68, 17);
		this.RadCancelDetail.TabIndex = 4;
		this.RadCancelDetail.Text = "Cancel";
		this.RadCancelDetail.UseVisualStyleBackColor = true;
		this.RadCompleteDetail.AutoSize = true;
		this.RadCompleteDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCompleteDetail.Location = new System.Drawing.Point(777, 50);
		this.RadCompleteDetail.Name = "RadCompleteDetail";
		this.RadCompleteDetail.Size = new System.Drawing.Size(86, 17);
		this.RadCompleteDetail.TabIndex = 3;
		this.RadCompleteDetail.Text = "Complete";
		this.RadCompleteDetail.UseVisualStyleBackColor = true;
		this.RadOpenDetail.AutoSize = true;
		this.RadOpenDetail.Checked = true;
		this.RadOpenDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadOpenDetail.Location = new System.Drawing.Point(777, 31);
		this.RadOpenDetail.Name = "RadOpenDetail";
		this.RadOpenDetail.Size = new System.Drawing.Size(58, 17);
		this.RadOpenDetail.TabIndex = 2;
		this.RadOpenDetail.TabStop = true;
		this.RadOpenDetail.Text = "Open";
		this.RadOpenDetail.UseVisualStyleBackColor = true;
		this.RadAllOpenCompCancelDetail.AutoSize = true;
		this.RadAllOpenCompCancelDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAllOpenCompCancelDetail.Location = new System.Drawing.Point(777, 13);
		this.RadAllOpenCompCancelDetail.Name = "RadAllOpenCompCancelDetail";
		this.RadAllOpenCompCancelDetail.Size = new System.Drawing.Size(42, 17);
		this.RadAllOpenCompCancelDetail.TabIndex = 1;
		this.RadAllOpenCompCancelDetail.Text = "All";
		this.RadAllOpenCompCancelDetail.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBookingPerson);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.RadUnApprovedOnlyDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCatgory);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.radAllApprovedAndUnApprovedDetail);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.RadApprovedOnlyDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtSrTo);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.txtSrFrom);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(769, 126);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadUnApprovedOnlyDetail.AutoSize = true;
		this.RadUnApprovedOnlyDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadUnApprovedOnlyDetail.Location = new System.Drawing.Point(595, 101);
		this.RadUnApprovedOnlyDetail.Name = "RadUnApprovedOnlyDetail";
		this.RadUnApprovedOnlyDetail.Size = new System.Drawing.Size(109, 17);
		this.RadUnApprovedOnlyDetail.TabIndex = 11;
		this.RadUnApprovedOnlyDetail.Text = "Un Approved";
		this.RadUnApprovedOnlyDetail.UseVisualStyleBackColor = true;
		this.CmbParentCatgory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCatgory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCatgory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCatgory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatgory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCatgory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCatgory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCatgory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCatgory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCatgory).Location = new System.Drawing.Point(315, 70);
		((System.Windows.Forms.Control)(object)this.CmbParentCatgory).Name = "CmbParentCatgory";
		((System.Windows.Forms.Control)(object)this.CmbParentCatgory).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCatgory).TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(224, 76);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(91, 15);
		this.label12.TabIndex = 150;
		this.label12.Text = "Parent Category";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 76);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(48, 15);
		this.label11.TabIndex = 148;
		this.label11.Text = "To Date";
		this.radAllApprovedAndUnApprovedDetail.AutoSize = true;
		this.radAllApprovedAndUnApprovedDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radAllApprovedAndUnApprovedDetail.Location = new System.Drawing.Point(460, 101);
		this.radAllApprovedAndUnApprovedDetail.Name = "radAllApprovedAndUnApprovedDetail";
		this.radAllApprovedAndUnApprovedDetail.Size = new System.Drawing.Size(42, 17);
		this.radAllApprovedAndUnApprovedDetail.TabIndex = 9;
		this.radAllApprovedAndUnApprovedDetail.Text = "All";
		this.radAllApprovedAndUnApprovedDetail.UseVisualStyleBackColor = true;
		this.RadApprovedOnlyDetail.AutoSize = true;
		this.RadApprovedOnlyDetail.Checked = true;
		this.RadApprovedOnlyDetail.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadApprovedOnlyDetail.Location = new System.Drawing.Point(504, 101);
		this.RadApprovedOnlyDetail.Name = "RadApprovedOnlyDetail";
		this.RadApprovedOnlyDetail.Size = new System.Drawing.Size(88, 17);
		this.RadApprovedOnlyDetail.TabIndex = 10;
		this.RadApprovedOnlyDetail.TabStop = true;
		this.RadApprovedOnlyDetail.Text = "Approved";
		this.RadApprovedOnlyDetail.UseVisualStyleBackColor = true;
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(85, 72);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(135, 23);
		this.datToDate.TabIndex = 2;
		this.datFromDate.Checked = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(85, 45);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(135, 23);
		this.datFromDate.TabIndex = 1;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance184).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(85, 15);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 21);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 147;
		this.label48.Text = "Branch Name";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(224, 49);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(56, 15);
		this.label6.TabIndex = 106;
		this.label6.Text = "Po No To";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnPurchsaeOrderDetail, this.toolStripButton2, this.btnShortCutKeys, this.btnOpenOrders, this.btnCompleteOrders, this.btnCancelOrders });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1350, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPurchsaeOrderDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchsaeOrderDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchsaeOrderDetail.Name = "btnPurchsaeOrderDetail";
		this.btnPurchsaeOrderDetail.Size = new System.Drawing.Size(115, 24);
		this.btnPurchsaeOrderDetail.Text = "206-PoDetail";
		this.btnPurchsaeOrderDetail.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(168, 24);
		this.toolStripButton2.Text = "205-PoDetailRegister";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnOpenOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnOpenOrders.Image = (System.Drawing.Image)resources.GetObject("btnOpenOrders.Image");
		this.btnOpenOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOpenOrders.Name = "btnOpenOrders";
		this.btnOpenOrders.Size = new System.Drawing.Size(45, 24);
		this.btnOpenOrders.Text = "Open";
		this.btnOpenOrders.Visible = false;
		this.btnOpenOrders.Click += new System.EventHandler(btnOpenOrders_Click);
		this.btnCompleteOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCompleteOrders.Image = (System.Drawing.Image)resources.GetObject("btnCompleteOrders.Image");
		this.btnCompleteOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCompleteOrders.Name = "btnCompleteOrders";
		this.btnCompleteOrders.Size = new System.Drawing.Size(72, 24);
		this.btnCompleteOrders.Text = "Complete";
		this.btnCompleteOrders.Visible = false;
		this.btnCompleteOrders.Click += new System.EventHandler(btnCompleteOrders_Click);
		this.btnCancelOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCancelOrders.Image = (System.Drawing.Image)resources.GetObject("btnCancelOrders.Image");
		this.btnCancelOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCancelOrders.Name = "btnCancelOrders";
		this.btnCancelOrders.Size = new System.Drawing.Size(55, 24);
		this.btnCancelOrders.Text = "Cancel";
		this.btnCancelOrders.Visible = false;
		this.btnCancelOrders.Click += new System.EventHandler(btnCancelOrders_Click);
		this.tabPage2.Controls.Add(this.label15);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridSummary);
		this.tabPage2.Controls.Add(this.label9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.toolStrip1);
		this.tabPage2.Controls.Add(this.ctrlGrdBar2);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1356, 599);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Summary";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(83, 208);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 16);
		this.label15.TabIndex = 161;
		this.label15.Text = "label15";
		this.GridSummary.AllowEdit = (InheritableBoolean)2;
		this.GridSummary.AlternatingColors = true;
		((ScrollableBase)this.GridSummary).BackColor = System.Drawing.Color.White;
		this.GridSummary.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridSummary_DesignTimeLayout.LayoutString = resources.GetString("GridSummary_DesignTimeLayout.LayoutString");
		this.GridSummary.DesignTimeLayout = GridSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridSummary.FilterMode = (FilterMode)2;
		this.GridSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridSummary.GroupTotals = (GroupTotals)2;
		this.GridSummary.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GridSummary).Location = new System.Drawing.Point(3, 228);
		((System.Windows.Forms.Control)(object)this.GridSummary).Name = "GridSummary";
		((ScrollableBase)this.GridSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridSummary).Size = new System.Drawing.Size(1350, 368);
		((System.Windows.Forms.Control)(object)this.GridSummary).TabIndex = 1;
		this.GridSummary.TotalRow = (InheritableBoolean)1;
		this.GridSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridSummary).VisualStyle = (VisualStyle)4;
		this.GridSummary.ColumnButtonClick += new ColumnActionEventHandler(GridSummary_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.GridSummary).KeyDown += new System.Windows.Forms.KeyEventHandler(GridSummary_KeyDown);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 204);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(1350, 24);
		this.label9.TabIndex = 3;
		this.label9.Text = "Summary ";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.PanelCanceledSummary);
		this.panel7.Controls.Add(this.PanelExpiringSummary);
		this.panel7.Controls.Add(this.PanelInProcessSummary);
		this.panel7.Controls.Add(this.PanelOutstandingInProcessSummary);
		this.panel7.Controls.Add(this.RadCancelSumm);
		this.panel7.Controls.Add(this.PanelOutstandingSummary);
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Controls.Add(this.RadCompleteSumm);
		this.panel7.Controls.Add(this.RadAllOpenCompCancelSumm);
		this.panel7.Controls.Add(this.RadOpenSumm);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1350, 174);
		this.panel7.TabIndex = 0;
		this.PanelCanceledSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelCanceledSummary.Controls.Add(this.label28);
		this.PanelCanceledSummary.Controls.Add(this.txtCanceledSummary);
		this.PanelCanceledSummary.Location = new System.Drawing.Point(1204, 61);
		this.PanelCanceledSummary.Name = "PanelCanceledSummary";
		this.PanelCanceledSummary.Size = new System.Drawing.Size(142, 51);
		this.PanelCanceledSummary.TabIndex = 161;
		this.PanelCanceledSummary.Visible = false;
		this.PanelCanceledSummary.Click += new System.EventHandler(PanelCanceledSummary_Click);
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Dock = System.Windows.Forms.DockStyle.Top;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(0, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(140, 17);
		this.label28.TabIndex = 107;
		this.label28.Text = "Canceled";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtCanceledSummary.BackColor = System.Drawing.Color.White;
		this.txtCanceledSummary.Enabled = false;
		this.txtCanceledSummary.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtCanceledSummary.Location = new System.Drawing.Point(1, 19);
		this.txtCanceledSummary.Margin = new System.Windows.Forms.Padding(2);
		this.txtCanceledSummary.Name = "txtCanceledSummary";
		this.txtCanceledSummary.ReadOnly = true;
		this.txtCanceledSummary.Size = new System.Drawing.Size(138, 29);
		this.txtCanceledSummary.TabIndex = 0;
		this.txtCanceledSummary.Click += new System.EventHandler(PanelCanceledSummary_Click);
		this.PanelExpiringSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelExpiringSummary.Controls.Add(this.label29);
		this.PanelExpiringSummary.Controls.Add(this.txtExpiringDaysSummary);
		this.PanelExpiringSummary.Location = new System.Drawing.Point(1204, 4);
		this.PanelExpiringSummary.Name = "PanelExpiringSummary";
		this.PanelExpiringSummary.Size = new System.Drawing.Size(142, 54);
		this.PanelExpiringSummary.TabIndex = 162;
		this.PanelExpiringSummary.Visible = false;
		this.PanelExpiringSummary.Click += new System.EventHandler(PanelExpiringSummary_Click);
		this.label29.BackColor = System.Drawing.Color.Teal;
		this.label29.Dock = System.Windows.Forms.DockStyle.Top;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(0, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(140, 21);
		this.label29.TabIndex = 107;
		this.label29.Text = "Expiring Next 7 Days";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtExpiringDaysSummary.BackColor = System.Drawing.Color.White;
		this.txtExpiringDaysSummary.Enabled = false;
		this.txtExpiringDaysSummary.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtExpiringDaysSummary.Location = new System.Drawing.Point(1, 22);
		this.txtExpiringDaysSummary.Margin = new System.Windows.Forms.Padding(2);
		this.txtExpiringDaysSummary.Name = "txtExpiringDaysSummary";
		this.txtExpiringDaysSummary.ReadOnly = true;
		this.txtExpiringDaysSummary.Size = new System.Drawing.Size(138, 29);
		this.txtExpiringDaysSummary.TabIndex = 0;
		this.txtExpiringDaysSummary.Click += new System.EventHandler(PanelExpiringSummary_Click);
		this.PanelInProcessSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelInProcessSummary.Controls.Add(this.label33);
		this.PanelInProcessSummary.Controls.Add(this.txtInProcessSummary);
		this.PanelInProcessSummary.Location = new System.Drawing.Point(995, 115);
		this.PanelInProcessSummary.Name = "PanelInProcessSummary";
		this.PanelInProcessSummary.Size = new System.Drawing.Size(204, 51);
		this.PanelInProcessSummary.TabIndex = 163;
		this.PanelInProcessSummary.Visible = false;
		this.PanelInProcessSummary.Click += new System.EventHandler(PanelInProcessSummary_Click);
		this.label33.BackColor = System.Drawing.Color.Teal;
		this.label33.Dock = System.Windows.Forms.DockStyle.Top;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(0, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(202, 17);
		this.label33.TabIndex = 107;
		this.label33.Text = "InProcess && Excess Received";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInProcessSummary.BackColor = System.Drawing.Color.White;
		this.txtInProcessSummary.Enabled = false;
		this.txtInProcessSummary.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtInProcessSummary.Location = new System.Drawing.Point(1, 19);
		this.txtInProcessSummary.Margin = new System.Windows.Forms.Padding(2);
		this.txtInProcessSummary.Name = "txtInProcessSummary";
		this.txtInProcessSummary.ReadOnly = true;
		this.txtInProcessSummary.Size = new System.Drawing.Size(200, 29);
		this.txtInProcessSummary.TabIndex = 0;
		this.txtInProcessSummary.Click += new System.EventHandler(PanelInProcessSummary_Click);
		this.PanelOutstandingInProcessSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelOutstandingInProcessSummary.Controls.Add(this.label34);
		this.PanelOutstandingInProcessSummary.Controls.Add(this.txtOustandingInProcessSummary);
		this.PanelOutstandingInProcessSummary.Location = new System.Drawing.Point(995, 61);
		this.PanelOutstandingInProcessSummary.Name = "PanelOutstandingInProcessSummary";
		this.PanelOutstandingInProcessSummary.Size = new System.Drawing.Size(204, 51);
		this.PanelOutstandingInProcessSummary.TabIndex = 164;
		this.PanelOutstandingInProcessSummary.Visible = false;
		this.PanelOutstandingInProcessSummary.Click += new System.EventHandler(PanelOutstandingInProcessSummary_Click);
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Dock = System.Windows.Forms.DockStyle.Top;
		this.label34.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(0, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(202, 17);
		this.label34.TabIndex = 107;
		this.label34.Text = "Outstanding && InProcess";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOustandingInProcessSummary.BackColor = System.Drawing.Color.White;
		this.txtOustandingInProcessSummary.Enabled = false;
		this.txtOustandingInProcessSummary.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtOustandingInProcessSummary.Location = new System.Drawing.Point(1, 19);
		this.txtOustandingInProcessSummary.Margin = new System.Windows.Forms.Padding(2);
		this.txtOustandingInProcessSummary.Name = "txtOustandingInProcessSummary";
		this.txtOustandingInProcessSummary.ReadOnly = true;
		this.txtOustandingInProcessSummary.Size = new System.Drawing.Size(200, 29);
		this.txtOustandingInProcessSummary.TabIndex = 0;
		this.txtOustandingInProcessSummary.Click += new System.EventHandler(PanelOutstandingInProcessSummary_Click);
		this.RadCancelSumm.AutoSize = true;
		this.RadCancelSumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCancelSumm.Location = new System.Drawing.Point(910, 71);
		this.RadCancelSumm.Name = "RadCancelSumm";
		this.RadCancelSumm.Size = new System.Drawing.Size(68, 17);
		this.RadCancelSumm.TabIndex = 4;
		this.RadCancelSumm.Text = "Cancel";
		this.RadCancelSumm.UseVisualStyleBackColor = true;
		this.PanelOutstandingSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelOutstandingSummary.Controls.Add(this.label35);
		this.PanelOutstandingSummary.Controls.Add(this.txtOustandingSummary);
		this.PanelOutstandingSummary.Location = new System.Drawing.Point(995, 4);
		this.PanelOutstandingSummary.Name = "PanelOutstandingSummary";
		this.PanelOutstandingSummary.Size = new System.Drawing.Size(204, 54);
		this.PanelOutstandingSummary.TabIndex = 160;
		this.PanelOutstandingSummary.Visible = false;
		this.PanelOutstandingSummary.Click += new System.EventHandler(PanelOutstandingSummary_Click);
		this.label35.BackColor = System.Drawing.Color.Teal;
		this.label35.Dock = System.Windows.Forms.DockStyle.Top;
		this.label35.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(0, 0);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(202, 21);
		this.label35.TabIndex = 107;
		this.label35.Text = "Outstanding && Open";
		this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOustandingSummary.BackColor = System.Drawing.Color.White;
		this.txtOustandingSummary.Enabled = false;
		this.txtOustandingSummary.Font = new System.Drawing.Font("Verdana", 13f, System.Drawing.FontStyle.Bold);
		this.txtOustandingSummary.Location = new System.Drawing.Point(1, 22);
		this.txtOustandingSummary.Margin = new System.Windows.Forms.Padding(2);
		this.txtOustandingSummary.Name = "txtOustandingSummary";
		this.txtOustandingSummary.ReadOnly = true;
		this.txtOustandingSummary.Size = new System.Drawing.Size(200, 29);
		this.txtOustandingSummary.TabIndex = 0;
		this.txtOustandingSummary.Click += new System.EventHandler(PanelOutstandingSummary_Click);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivitySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemNameSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm);
		this.groupBox2.Controls.Add(this.datToDateSumm);
		this.groupBox2.Controls.Add(this.datFromDateSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm);
		this.groupBox2.Controls.Add(this.txtFromDocNoSumm);
		this.groupBox2.Controls.Add(this.RadUnApprovedOnlySumm);
		this.groupBox2.Controls.Add(this.txtToDocNoSumm);
		this.groupBox2.Controls.Add(this.RadApprovedOnlySumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm);
		this.groupBox2.Controls.Add(this.radAllApprovedAndUnApprovedSumm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategorySumm);
		this.groupBox2.Controls.Add(this.chkSkipZero);
		this.groupBox2.Controls.Add(this.BtnShowSumm);
		this.groupBox2.Controls.Add(this.chkIncludeCity);
		this.groupBox2.Controls.Add(this.chkIncludePackUom);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrictSumm);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcitySumm);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotSumm);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropSumm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(900, 165);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter Criteria";
		this.cmbCustomerSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Location = new System.Drawing.Point(698, 44);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Name = "cmbCustomerSumm";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSumm).TabIndex = 13;
		this.cmbActivitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbActivitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Location = new System.Drawing.Point(698, 98);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Name = "cmbActivitySumm";
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivitySumm).TabIndex = 15;
		this.cmbItemNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemNameSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Location = new System.Drawing.Point(698, 16);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Name = "cmbItemNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemNameSumm).TabIndex = 12;
		this.cmbParentcategorySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbParentcategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentcategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentcategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentcategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentcategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Location = new System.Drawing.Point(106, 100);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Name = "cmbParentcategorySumm";
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentcategorySumm).TabIndex = 5;
		this.datToDateSumm.CustomFormat = "dd-MMM-yy";
		this.datToDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateSumm.Location = new System.Drawing.Point(238, 46);
		this.datToDateSumm.Name = "datToDateSumm";
		this.datToDateSumm.ShowCheckBox = true;
		this.datToDateSumm.Size = new System.Drawing.Size(131, 23);
		this.datToDateSumm.TabIndex = 2;
		this.datFromDateSumm.CustomFormat = "dd-MMM-yy";
		this.datFromDateSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateSumm.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDateSumm.Location = new System.Drawing.Point(106, 46);
		this.datFromDateSumm.Name = "datFromDateSumm";
		this.datFromDateSumm.ShowCheckBox = true;
		this.datFromDateSumm.Size = new System.Drawing.Size(131, 23);
		this.datFromDateSumm.TabIndex = 1;
		this.cmbBranchNameSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchNameSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchNameSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchNameSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchNameSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Location = new System.Drawing.Point(106, 16);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Name = "cmbBranchNameSumm";
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchNameSumm).Leave += new System.EventHandler(cmbBranchNameSumm_Leave);
		this.txtFromDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoSumm.Location = new System.Drawing.Point(106, 74);
		this.txtFromDocNoSumm.Name = "txtFromDocNoSumm";
		this.txtFromDocNoSumm.Size = new System.Drawing.Size(131, 23);
		this.txtFromDocNoSumm.TabIndex = 3;
		this.txtToDocNoSumm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoSumm.Location = new System.Drawing.Point(238, 74);
		this.txtToDocNoSumm.Name = "txtToDocNoSumm";
		this.txtToDocNoSumm.Size = new System.Drawing.Size(131, 23);
		this.txtToDocNoSumm.TabIndex = 4;
		this.cmbItemtypeSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtypeSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtypeSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtypeSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtypeSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemtypeSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Location = new System.Drawing.Point(433, 16);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Name = "cmbItemtypeSumm";
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtypeSumm).TabIndex = 7;
		this.CmbCategorySumm.AllowNull = (DefaultableBoolean)1;
		this.CmbCategorySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategorySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategorySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategorySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategorySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Location = new System.Drawing.Point(106, 128);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Name = "CmbCategorySumm";
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).Size = new System.Drawing.Size(263, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategorySumm).TabIndex = 6;
		this.RadUnApprovedOnlySumm.AutoSize = true;
		this.RadUnApprovedOnlySumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadUnApprovedOnlySumm.Location = new System.Drawing.Point(729, 146);
		this.RadUnApprovedOnlySumm.Name = "RadUnApprovedOnlySumm";
		this.RadUnApprovedOnlySumm.Size = new System.Drawing.Size(109, 17);
		this.RadUnApprovedOnlySumm.TabIndex = 21;
		this.RadUnApprovedOnlySumm.Text = "Un Approved";
		this.RadUnApprovedOnlySumm.UseVisualStyleBackColor = true;
		this.BtnShowSumm.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSumm.FlatAppearance.BorderSize = 0;
		this.BtnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSumm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowSumm.ForeColor = System.Drawing.Color.White;
		this.BtnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSumm.Location = new System.Drawing.Point(838, 127);
		this.BtnShowSumm.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSumm.Name = "BtnShowSumm";
		this.BtnShowSumm.Size = new System.Drawing.Size(57, 26);
		this.BtnShowSumm.TabIndex = 22;
		this.BtnShowSumm.Text = "Show";
		this.BtnShowSumm.UseVisualStyleBackColor = false;
		this.BtnShowSumm.Click += new System.EventHandler(BtnShowSumm_Click);
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.Location = new System.Drawing.Point(757, 128);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 18;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.chkIncludeCity.AutoSize = true;
		this.chkIncludeCity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludeCity.Location = new System.Drawing.Point(685, 128);
		this.chkIncludeCity.Name = "chkIncludeCity";
		this.chkIncludeCity.Size = new System.Drawing.Size(72, 19);
		this.chkIncludeCity.TabIndex = 17;
		this.chkIncludeCity.Text = "CityWise";
		this.chkIncludeCity.UseVisualStyleBackColor = true;
		this.chkIncludePackUom.AutoSize = true;
		this.chkIncludePackUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludePackUom.Location = new System.Drawing.Point(609, 128);
		this.chkIncludePackUom.Name = "chkIncludePackUom";
		this.chkIncludePackUom.Size = new System.Drawing.Size(77, 19);
		this.chkIncludePackUom.TabIndex = 16;
		this.chkIncludePackUom.Text = "PackUom";
		this.chkIncludePackUom.UseVisualStyleBackColor = true;
		this.radAllApprovedAndUnApprovedSumm.AutoSize = true;
		this.radAllApprovedAndUnApprovedSumm.Checked = true;
		this.radAllApprovedAndUnApprovedSumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radAllApprovedAndUnApprovedSumm.Location = new System.Drawing.Point(608, 146);
		this.radAllApprovedAndUnApprovedSumm.Name = "radAllApprovedAndUnApprovedSumm";
		this.radAllApprovedAndUnApprovedSumm.Size = new System.Drawing.Size(42, 17);
		this.radAllApprovedAndUnApprovedSumm.TabIndex = 19;
		this.radAllApprovedAndUnApprovedSumm.TabStop = true;
		this.radAllApprovedAndUnApprovedSumm.Text = "All";
		this.radAllApprovedAndUnApprovedSumm.UseVisualStyleBackColor = true;
		this.RadApprovedOnlySumm.AutoSize = true;
		this.RadApprovedOnlySumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadApprovedOnlySumm.Location = new System.Drawing.Point(647, 146);
		this.RadApprovedOnlySumm.Name = "RadApprovedOnlySumm";
		this.RadApprovedOnlySumm.Size = new System.Drawing.Size(88, 17);
		this.RadApprovedOnlySumm.TabIndex = 20;
		this.RadApprovedOnlySumm.Text = "Approved";
		this.RadApprovedOnlySumm.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 162;
		this.label5.Text = "Branch Name";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(608, 50);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(86, 15);
		this.label25.TabIndex = 123;
		this.label25.Text = "Supplier Name";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(4, 50);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(91, 15);
		this.label22.TabIndex = 127;
		this.label22.Text = "From && To Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(4, 78);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(104, 15);
		this.label21.TabIndex = 129;
		this.label21.Text = "From && To DocNo";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(608, 104);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(70, 15);
		this.label14.TabIndex = 155;
		this.label14.Text = "Report Type";
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
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Location = new System.Drawing.Point(433, 100);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Name = "cmbDistrictSumm";
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrictSumm).TabIndex = 10;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(608, 22);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 15);
		this.label24.TabIndex = 125;
		this.label24.Text = "Item Name";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(372, 106);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 151;
		this.label31.Text = "District";
		this.cmbcitySumm.AllowNull = (DefaultableBoolean)1;
		this.cmbcitySumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcitySumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcitySumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcitySumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcitySumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Location = new System.Drawing.Point(433, 128);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Name = "cmbcitySumm";
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbcitySumm).TabIndex = 11;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(372, 134);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(62, 15);
		this.label30.TabIndex = 149;
		this.label30.Text = "City Name";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(4, 106);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(91, 15);
		this.label18.TabIndex = 138;
		this.label18.Text = "Parent Category";
		this.cmbJobLotSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance204;
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
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Location = new System.Drawing.Point(433, 72);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Name = "cmbJobLotSumm";
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotSumm).TabIndex = 9;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 134);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 138;
		this.label17.Text = "Item Category";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(372, 78);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(46, 15);
		this.label32.TabIndex = 153;
		this.label32.Text = "Job Lot";
		this.cmbCropSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbCropSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Location = new System.Drawing.Point(433, 44);
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Name = "cmbCropSumm";
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropSumm).TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(372, 22);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 138;
		this.label16.Text = "Item Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(372, 50);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(57, 15);
		this.label8.TabIndex = 143;
		this.label8.Text = "Crop Year";
		this.RadCompleteSumm.AutoSize = true;
		this.RadCompleteSumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCompleteSumm.Location = new System.Drawing.Point(910, 53);
		this.RadCompleteSumm.Name = "RadCompleteSumm";
		this.RadCompleteSumm.Size = new System.Drawing.Size(86, 17);
		this.RadCompleteSumm.TabIndex = 3;
		this.RadCompleteSumm.Text = "Complete";
		this.RadCompleteSumm.UseVisualStyleBackColor = true;
		this.RadAllOpenCompCancelSumm.AutoSize = true;
		this.RadAllOpenCompCancelSumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAllOpenCompCancelSumm.Location = new System.Drawing.Point(910, 16);
		this.RadAllOpenCompCancelSumm.Name = "RadAllOpenCompCancelSumm";
		this.RadAllOpenCompCancelSumm.Size = new System.Drawing.Size(42, 17);
		this.RadAllOpenCompCancelSumm.TabIndex = 1;
		this.RadAllOpenCompCancelSumm.Text = "All";
		this.RadAllOpenCompCancelSumm.UseVisualStyleBackColor = true;
		this.RadOpenSumm.AutoSize = true;
		this.RadOpenSumm.Checked = true;
		this.RadOpenSumm.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadOpenSumm.Location = new System.Drawing.Point(910, 34);
		this.RadOpenSumm.Name = "RadOpenSumm";
		this.RadOpenSumm.Size = new System.Drawing.Size(58, 17);
		this.RadOpenSumm.TabIndex = 2;
		this.RadOpenSumm.TabStop = true;
		this.RadOpenSumm.Text = "Open";
		this.RadOpenSumm.UseVisualStyleBackColor = true;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewSummary, this.btnRefreshSummar, this.btnSummaryPrint, this.btnShortCutKeySumamryTab });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1350, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewSummary.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewSummary.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewSummary.Name = "btnNewSummary";
		this.btnNewSummary.Size = new System.Drawing.Size(68, 24);
		this.btnNewSummary.Text = "&Reset";
		this.btnNewSummary.Click += new System.EventHandler(btnNewSummary_Click);
		this.btnRefreshSummar.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshSummar.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSummar.Name = "btnRefreshSummar";
		this.btnRefreshSummar.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshSummar.Text = "&Refresh";
		this.btnRefreshSummar.Click += new System.EventHandler(btnRefreshSummar_Click);
		this.btnSummaryPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSummaryPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSummaryPrint.Name = "btnSummaryPrint";
		this.btnSummaryPrint.Size = new System.Drawing.Size(61, 24);
		this.btnSummaryPrint.Text = "Print";
		this.btnSummaryPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSummaryPrint.Click += new System.EventHandler(btnSummaryPrint_Click);
		this.btnShortCutKeySumamryTab.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeySumamryTab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeySumamryTab.Name = "btnShortCutKeySumamryTab";
		this.btnShortCutKeySumamryTab.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeySumamryTab.Text = "ShortCut Keys";
		this.btnShortCutKeySumamryTab.Click += new System.EventHandler(btnShortCutKeySumamryTab_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1312, 243);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
		this.ctrlGrdBar2.MyGrid = this.GridSummary;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar2.TabIndex = 160;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.CmbBookingPerson.AllowNull = (DefaultableBoolean)1;
		this.CmbBookingPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBookingPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBookingPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBookingPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Location = new System.Drawing.Point(545, 70);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Name = "CmbBookingPerson";
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).TabIndex = 8;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(457, 76);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(90, 15);
		this.label20.TabIndex = 151;
		this.label20.Text = "Booking Person";
		this.cmbBookingPersonSumm.AllowNull = (DefaultableBoolean)1;
		this.cmbBookingPersonSumm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBookingPersonSumm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBookingPersonSumm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBookingPersonSumm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBookingPersonSumm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Location = new System.Drawing.Point(698, 71);
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Name = "cmbBookingPersonSumm";
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbBookingPersonSumm).TabIndex = 14;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(608, 77);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(90, 15);
		this.label26.TabIndex = 164;
		this.label26.Text = "Booking Person";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1364, 652);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchaseOrderHistory";
		this.Text = "Ecount Book ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseOrderHistory_FormClosing);
		base.Load += new System.EventHandler(PurchaseOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseOrderHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.PanelCanceled.ResumeLayout(false);
		this.PanelCanceled.PerformLayout();
		this.PanelExpiringDays.ResumeLayout(false);
		this.PanelExpiringDays.PerformLayout();
		this.PanelInProcess.ResumeLayout(false);
		this.PanelInProcess.PerformLayout();
		this.PanelOutstandingInProcess.ResumeLayout(false);
		this.PanelOutstandingInProcess.PerformLayout();
		this.PanelOutstanding.ResumeLayout(false);
		this.PanelOutstanding.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCatgory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridSummary).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.PanelCanceledSummary.ResumeLayout(false);
		this.PanelCanceledSummary.PerformLayout();
		this.PanelExpiringSummary.ResumeLayout(false);
		this.PanelExpiringSummary.PerformLayout();
		this.PanelInProcessSummary.ResumeLayout(false);
		this.PanelInProcessSummary.PerformLayout();
		this.PanelOutstandingInProcessSummary.ResumeLayout(false);
		this.PanelOutstandingInProcessSummary.PerformLayout();
		this.PanelOutstandingSummary.ResumeLayout(false);
		this.PanelOutstandingSummary.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentcategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchNameSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtypeSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategorySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrictSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcitySumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotSumm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropSumm).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBookingPersonSumm).EndInit();
		base.ResumeLayout(false);
	}
}

}
