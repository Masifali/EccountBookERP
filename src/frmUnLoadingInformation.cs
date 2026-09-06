// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Purchase
{

public class frmUnLoadingInformation : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects FormRights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable _dtData = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtEbDetail = new DataTable();

	private DataTable dthistory = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtJob = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtEbCondition = new DataTable();

	private DataTable dtContractors = new DataTable();

	private int? LabId = 0;

	private int? GpId = 0;

	private int? WbId = 0;

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "";

	private bool MultiBranchFeatur;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label20;

	private ComboBox cmbcommtype;

	private Label label26;

	private Label label11;

	private TextBox txtcommremarks;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private ComboBox cmbcommuom;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label2;

	private Label label8;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grd;

	internal UltraCombo cmbstockprty;

	internal UltraCombo cmbrefprty;

	internal UltraCombo cmbcommagent;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private ToolStripButton btnSaleInvoice1;

	private CheckBox ChkBok;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnFrmRefresh;

	private Panel panel10;

	internal GridEX grdDetail;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label27;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator7;

	private Panel panel8;

	private GroupBox groupBox4;

	private Button btnShow;

	internal UltraCombo CmbPaymentTermHistory;

	private Label label52;

	internal UltraCombo CmbCustomerHistory;

	private Label label18;

	private TextBox ToDocNoHistory;

	private Label label46;

	private TextBox FromDocNoHistory;

	private Label label22;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label54;

	private ToolStripButton btnResetHistory;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX grdWages;

	private Panel panel17;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdEmptyBags;

	private Panel panel21;

	private CtrlGrdBar ctrlGrdBar3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo cmbBranchName;

	private Label label66;

	internal UltraCombo CmbWarehouseForUpdateInGrid;

	private Label label51;

	private Button BtnUpdateInGridDetail;

	private Button btnRecordsUpdate;

	internal UltraCombo CmbGpNo;

	private DateTimePicker txtGpDate;

	private TextBox txtNoOfBags;

	private Label label3;

	private Label label4;

	private TextBox txtTransitDays;

	private TextBox txtVehicleNo;

	private Label label6;

	private Label label5;

	private DateTimePicker datBiltyDate;

	private Label labelLine;

	private Label label10;

	private TextBox txtFreight;

	private TextBox txtCity;

	private Label label13;

	private Label label12;

	private GroupBox groupBox5;

	private TextBox txtFactoryWeight;

	private TextBox txtSecondWeight;

	private TextBox txtDiffWeight;

	private TextBox txtNetWeight;

	private Label label40;

	private Label label41;

	private TextBox txtSupplierWeight;

	private Label label42;

	private Label label47;

	private GroupBox groupBox3;

	private TextBox txtFirstStatus;

	private Label label14;

	private TextBox txtFinalStatus;

	private TextBox txtMoisture;

	private Label label16;

	private Label label17;

	private TextBox txtAnalyst;

	private Label label24;

	private Label label35;

	private TextBox txtFirstWeight;

	private Label label37;

	private Label label44;

	private Label label29;

	private Label label43;

	private TextBox txtJobLot;

	private TextBox txtWarehouse;

	private TextBox txtItemName;

	private TextBox txtApprovedBy;

	private Label label15;

	private Label label34;

	private Label label28;

	private Label label39;

	private TextBox txtBiltyNo;

	private Label label1;

	public frmUnLoadingInformation(UserAccount user, DataTable dt, int? _GpId = 0, int? _WbId = 0, int? _LabId = 0)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		_dtData = dt;
		GpId = _GpId;
		WbId = _WbId;
		LabId = _LabId;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1130, 650);
			ScreenName = Conversion.ToString((object)base.Name);
			dtWarehouse.Columns.Add("Id", typeof(int));
			dtWarehouse.Columns.Add("WareHouseName", typeof(string));
			dtJob.Columns.Add("Id", typeof(int));
			dtJob.Columns.Add("JobLotDescription", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("ItemCategoryId", typeof(int));
			dtitem.Columns.Add("itemModalId", typeof(int));
			dtitem.Columns.Add("FinishWeight", typeof(double));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("WarehouseId", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("JobLotId", typeof(int));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackSize", typeof(double));
			dtDetail.Columns.Add("UnloadedQty", typeof(double));
			dtDetail.Columns.Add("Remarks");
			dtWagesDetail.Columns.Add("Id", typeof(int));
			dtWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtWagesDetail.Columns.Add("PackingTypeId", typeof(int));
			dtWagesDetail.Columns.Add("PackingType", typeof(string));
			dtWagesDetail.Columns.Add("PackSize", typeof(double));
			dtWagesDetail.Columns.Add("UnloadedQty", typeof(double));
			dtWagesDetail.Columns.Add("Remarks", typeof(string));
			dtEbDetail.Columns.Add("Id", typeof(int));
			dtEbDetail.Columns.Add("BagConditionId", typeof(int));
			dtEbDetail.Columns.Add("BagsTypeId", typeof(int));
			dtEbDetail.Columns.Add("BagsType", typeof(string));
			dtEbDetail.Columns.Add("PartyBags", typeof(double));
			dtEbDetail.Columns.Add("CompanyBags", typeof(double));
			dtEbDetail.Columns.Add("Remarks", typeof(string));
			CmbGpNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			FormRights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			MultiBranchFeatur = CommonServices.GetERPFeatureById(11);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			GetContractors();
			return new
			{
				ItemData = ItemDbCallAgainstGpWbAndLab()
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
				btnSave.Enabled = FormRights.DoHaveSaveRight;
				btnSaleInvoice1.Enabled = FormRights.DoHavePrintRights;
				btnUpdate.Enabled = FormRights.DoHaveUpdateRights;
				btnDelete.Enabled = FormRights.DoHaveCanDelete;
				ctrlGrdBar1.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar2.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar3.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				ctrlGrdBar4.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
				ctrlGrdBar4.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
				_ = RecId;
				if (GpId > 0)
				{
					BindInformationAgainstGp();
				}
				GatePassBind();
				WarehousesDtFillFromGlobal();
				JobLotBindFromGlobal();
				PackingTypeDtFillFromGlobalAndBind();
				EmptyBagsConditionFill();
				dtitem = result.ItemData;
				AddRowInDetailGrid();
				if (dtDetail != null && dtDetail.Columns.Count > 0)
				{
					grd.DataSource = dtDetail;
					grd.RetrieveStructure();
					grdSettings();
				}
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GatePassBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GpNo", typeof(int));
			dt.Columns.Add("GpDate", typeof(DateTime));
			dt.Columns.Add("WbId", typeof(int));
			dt.Columns.Add("WbNo", typeof(int));
			dt.Columns.Add("LabId", typeof(int));
			dt.Columns.Add("LabNo", typeof(int));
			for (int i = 0; i < _dtData.Rows.Count; i++)
			{
				dt.Rows.Add(_dtData.Rows[i]["Id"], _dtData.Rows[i]["GpSrNo"], Conversion.ToDateTime(_dtData.Rows[i]["GpDate"]).ToString("dd-MMM-yyyy"), _dtData.Rows[i]["WbId"], _dtData.Rows[i]["TicketNo"], _dtData.Rows[i]["LabId"], _dtData.Rows[i]["LabNo"]);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbGpNo, dt, "Id", "GpNo", "Gp No", AllColumns: true, Conversion.ToInt(CmbGpNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbGpNo).DisplayLayout.Bands[0].Columns[1].Width = 100;
			((UltraGridBase)CmbGpNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbGpNo).DisplayLayout.Bands[0].Columns[5].Hidden = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetContractors()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		try
		{
			if (!MultiBranchFeatur)
			{
				SupplierCustomer supplier = new SupplierCustomer();
				supplier.OrganizationId = UserAccount.OrganizationId;
				supplier.CompanyId = UserAccount.CompanyId;
				dtContractors = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			}
			else
			{
				ReportsParameters supplier2 = new ReportsParameters();
				supplier2.OrganizationId = UserAccount.OrganizationId;
				supplier2.CompanyId = UserAccount.CompanyId;
				supplier2.BranchesId = UserAccount.BranchesId;
				dtContractors = ContractorsAllocationToBranch.GetContractorsAllocatedToBranch(supplier2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemDbCallAgainstGpWbAndLab()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			return InvLabAnalysisPurchaseHeader.GetItemsForUnloading(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				GpId = Conversion.ToInt((object)GpId),
				LabId = Conversion.ToInt((object)LabId)
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WarehousesDtFillFromGlobal()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches;
			if (lst != null && lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtWarehouse.Rows.Add(lst[i].WarehouseId, lst[i].WarehouseName);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotBindFromGlobal()
	{
		try
		{
			dtJob.Rows.Clear();
			List<GetJobLotGlIdsandName> lst = clsGlobalVariables.globalJobLot;
			if (lst.Count <= 0)
			{
				return;
			}
			foreach (GetJobLotGlIdsandName item in lst)
			{
				dtJob.Rows.Add(item.Id, item.JobLotDescription);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			dtPackingType = DatatableHelper.PopulateDataTableAndReturn(Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void EmptyBagsConditionFill()
	{
		try
		{
			dtEbCondition = CommonServices.StaticColumnsService("EmptyBagsCondition");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInformationAgainstGp()
	{
		try
		{
			int gpId = Conversion.ToInt((object)GpId);
			int? wbId = Conversion.ToInt((object)WbId);
			int? labId = Conversion.ToInt((object)LabId);
			EnumerableRowCollection<DataRow> filteredRows = from row in _dtData.AsEnumerable()
				where (gpId <= 0 || row.Field<int>("Id") == gpId) && (!(wbId > 0) || row.Field<int>("WbId") == wbId) && (!(labId > 0) || row.Field<int?>("LabId") == labId)
				select row;
			DataTable dt = (filteredRows.Any() ? filteredRows.CopyToDataTable() : _dtData.Clone());
			if (dt.Rows.Count > 0)
			{
				groupBox1.Text = "Gate Inward : " + Conversion.ToString(dt.Rows[0]["Priority"]);
				CmbGpNo.Value = Conversion.ToString(dt.Rows[0]["Id"]);
				txtGpDate.Value = Conversion.ToDateTime(dt.Rows[0]["InDateTimeStamp"]);
				txtNoOfBags.Text = Conversion.ToString(dt.Rows[0]["NoOfBags"]);
				txtVehicleNo.Text = Conversion.ToString(dt.Rows[0]["VehicleNo"]);
				datBiltyDate.Value = Conversion.ToDateTime(dt.Rows[0]["BiltyDate"]);
				txtCity.Text = Conversion.ToString(dt.Rows[0]["CityName"]);
				txtFreight.Text = Conversion.ToString(dt.Rows[0]["NetPaid"]);
				txtItemName.Text = Conversion.ToString(dt.Rows[0]["ItemName"]);
				txtAnalyst.Text = Conversion.ToString(dt.Rows[0]["AnalystName"]);
				txtWarehouse.Text = Conversion.ToString(dt.Rows[0]["LabWareHouseName"]);
				txtJobLot.Text = Conversion.ToString(dt.Rows[0]["LabJobLot"]);
				txtFirstStatus.Text = Conversion.ToString(dt.Rows[0]["LabStatus"]);
				txtApprovedBy.Text = Conversion.ToString(dt.Rows[0]["LabApprovedUserName"]);
				txtFinalStatus.Text = Conversion.ToString(dt.Rows[0]["LabApprovalStatus"]);
				txtMoisture.Text = Conversion.ToString(dt.Rows[0]["Moisture"]);
				txtFirstWeight.Text = Conversion.ToString(dt.Rows[0]["FirstWeight"]);
				txtSecondWeight.Text = Conversion.ToString(dt.Rows[0]["SecondWeight"]);
				txtNetWeight.Text = Conversion.ToString(dt.Rows[0]["NetWbWeight"]);
				txtSupplierWeight.Text = Conversion.ToString(dt.Rows[0]["SupplierWeight"]);
				txtFactoryWeight.Text = Conversion.ToString(dt.Rows[0]["FactoryWeight"]);
				txtDiffWeight.Text = Conversion.ToString(dt.Rows[0]["DifferenceWeight"]);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbGpNo).ActiveRow == null || Conversion.ToInt(CmbGpNo.Value) == 0)
		{
			MessageBox.Show("GpNo is Required");
			CmbGpNo.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			GpId = 0;
			WbId = 0;
			LabId = 0;
			((Control)(object)cmbcommagent).Text = string.Empty;
			cmbcommtype.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			cmbcommuom.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			dtDetail.Rows.Clear();
			AddRowInDetailGrid();
			grd.DataSource = dtDetail;
			if (grd.GetDataRows().Count() > 0)
			{
				grdSettings();
			}
			dtWagesDetail.Rows.Clear();
			grdWages.DataSource = dtWagesDetail;
			dtEbDetail.Rows.Clear();
			grdEmptyBags.DataSource = dtEbDetail;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
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
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "WarehousesWithBranches", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			PackingTypeDtFillFromGlobalAndBind();
			EmptyBagsConditionFill();
			JobLotBindFromGlobal();
			WarehousesDtFillFromGlobal();
			dtitem = ItemDbCallAgainstGpWbAndLab();
			GetContractors();
			GridDetailCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmUnLoadingInformation_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "ctrlGrdBar2")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInDetailGrid()
	{
		try
		{
			dtDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
			grd.DataSource = dtDetail;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && (e.Column.Key == "ItemId" || e.Column.Key == "PackingTypeId" || e.Column.Key == "PackSize"))
			{
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int packingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
				double packSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow row in rows)
				{
					if ((int)row.RowType == 0 && row != item)
					{
						int existingItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
						int existingPackingTypeId = Conversion.ToInt(row.Cells["PackingTypeId"].Value);
						double existingPackSize = Conversion.ToDouble(row.Cells["PackSize"].Value);
						if (itemId == existingItemId && packingTypeId == existingPackingTypeId && packSize == existingPackSize)
						{
							MessageBox.Show("Duplicate item with same Pack Size and Packing Type is not allowed.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							item.BeginEdit();
							item.Cells["PackSize"].Value = 0;
							item.EndEdit();
						}
					}
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowsInWagesAndEmptyBagGrid()
	{
		try
		{
			DataTable dtUnlodingDetail = (DataTable)grd.DataSource;
			var enumerable = from r in dtUnlodingDetail.AsEnumerable()
				group r by new
				{
					PackingTypeId = r.Field<int>("PackingTypeId"),
					PackSize = r.Field<double>("PackSize")
				};
			dtWagesDetail.Rows.Clear();
			dtEbDetail.Rows.Clear();
			foreach (var g2 in enumerable)
			{
				string desc2 = (from r in dtPackingType.AsEnumerable()
					where r.Field<int>("Id") == g2.Key.PackingTypeId
					select r.Field<string>("Description")).FirstOrDefault() ?? string.Empty;
				dtWagesDetail.Rows.Add(0, 0, g2.Key.PackingTypeId, desc2, g2.Key.PackSize, g2.Sum((DataRow x) => x.Field<double>("UnloadedQty")), "");
				grdWages.DataSource = dtWagesDetail;
				grdWages.RetrieveStructure();
				grdWagesSettings();
			}
			foreach (var g in from r in dtUnlodingDetail.AsEnumerable()
				group r by new
				{
					PackingTypeId = r.Field<int>("PackingTypeId")
				})
			{
				string desc = (from r in dtPackingType.AsEnumerable()
					where r.Field<int>("Id") == g.Key.PackingTypeId
					select r.Field<string>("Description")).FirstOrDefault() ?? string.Empty;
				dtEbDetail.Rows.Add(0, 0, g.Key.PackingTypeId, desc, g.Sum((DataRow x) => x.Field<double>("UnloadedQty")), 0, "");
				grdEmptyBags.DataSource = dtEbDetail;
				grdEmptyBags.RetrieveStructure();
				grdEmptyBagSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailCombo()
	{
		try
		{
			grd.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WarehouseId"].HasValueList = true;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)5;
			grd.RootTable.Columns["WarehouseId"].LimitToList = true;
			grd.RootTable.Columns["WarehouseId"].Caption = "Ware House";
			grd.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ItemId"].HasValueList = true;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)5;
			grd.RootTable.Columns["ItemId"].LimitToList = true;
			grd.RootTable.Columns["ItemId"].Caption = "Item Name";
			grd.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "ItemId", "ItemName");
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].Caption = "Job Lot";
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)5;
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)5;
			grd.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grd.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "Description");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"WarehouseId",
					(EditType)4
				},
				{
					"ItemId",
					(EditType)4
				},
				{
					"JobLotId",
					(EditType)4
				},
				{
					"PackingTypeId",
					(EditType)4
				},
				{
					"PackSize",
					(EditType)1
				},
				{
					"UnloadedQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editCol);
			GridDetailCombo();
			grd.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["JobLotId"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingTypeId"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["UnloadedQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		if ((e.Column.Key == "UnloadedQty" || e.Column.Key == "PackSize") && !CommonServices.IsPositiveNumeric(e))
		{
			throw new Exception("Please Type Only positive Numeric Value");
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Add")
				{
					AddRowInDetailGrid();
				}
				else if (e.Column.Key == "Delete")
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInWagesGrid()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				int PackingTypeId = Conversion.ToInt(grdWages.CurrentRow.Cells["PackingTypeId"].Value);
				string PackingType = Conversion.ToString(grdWages.CurrentRow.Cells["PackingType"].Value);
				double PackSize = Conversion.ToDouble(grdWages.CurrentRow.Cells["PackSize"].Value);
				double totalQtyInDetail = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("PackingTypeId") == PackingTypeId && r.Field<double>("PackSize") == PackSize
					select r).Sum((DataRow r) => r.Field<double>("UnloadedQty"));
				double UnlodedQty = (from r in dtWagesDetail.AsEnumerable()
					where r.Field<int>("PackingTypeId") == PackingTypeId && r.Field<double>("PackSize") == PackSize
					select r).Sum((DataRow r) => r.Field<double>("UnloadedQty"));
				if (totalQtyInDetail == UnlodedQty)
				{
					throw new Exception("Not furthur breakable");
				}
				double rem = totalQtyInDetail - UnlodedQty;
				dtWagesDetail.Rows.Add(0, 0, PackingTypeId, PackingType, PackSize, rem, "");
				grdWages.DataSource = dtWagesDetail;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridWagesCombo()
	{
		try
		{
			grdWages.RootTable.Columns["ContractorId"].ColumnType = (ColumnType)0;
			grdWages.RootTable.Columns["ContractorId"].HasValueList = true;
			grdWages.RootTable.Columns["ContractorId"].EditType = (EditType)5;
			grdWages.RootTable.Columns["ContractorId"].LimitToList = true;
			grdWages.RootTable.Columns["ContractorId"].Caption = "Contractore";
			grdWages.RootTable.Columns["ContractorId"].ValueList.PopulateValueList((IEnumerable)dtContractors.DefaultView, "Id", "CompanyName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PackingTypeId" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"ContractorId",
					(EditType)4
				},
				{
					"UnloadedQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdWages, 2, 3, hiddenColumns, editCol);
			GridWagesCombo();
			grdWages.RootTable.Columns["ContractorId"].Width = Constants.InventoryConstants.SupplierName + 50;
			grdWages.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdWages.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdWages.RootTable.Columns["UnloadedQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdWages.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			grdWages.RootTable.Columns.Add("Delete");
			grdWages.RootTable.Columns["Delete"].Key = "Delete";
			grdWages.RootTable.Columns["Delete"].Caption = "X";
			grdWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdWages.RootTable.Columns["Delete"].Width = 20;
			grdWages.RootTable.Columns.Add("Add");
			grdWages.RootTable.Columns["Add"].Key = "Add";
			grdWages.RootTable.Columns["Add"].Caption = "+";
			grdWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdWages.RootTable.Columns["Add"].ButtonText = "+";
			grdWages.RootTable.Columns["Add"].Width = 20;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdWages.UpdateData();
			GridEXRow item = grdWages.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "UnloadedQty")
			{
				int packingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
				double packSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
				double num = Conversion.ToDouble(item.Cells["UnloadedQty"].Value);
				double totalQty = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("PackingTypeId") == packingTypeId && r.Field<double>("PackSize") == packSize
					select r.Field<double?>("UnloadedQty").GetValueOrDefault()).Sum();
				DataRow currentRow = ((DataRowView)item.DataRow).Row;
				double wagesQty = (from r in dtWagesDetail.AsEnumerable()
					where r != currentRow && r.Field<int>("PackingTypeId") == packingTypeId && r.Field<double>("PackSize") == packSize
					select r.Field<double?>("UnloadedQty").GetValueOrDefault()).Sum();
				if (num + wagesQty > totalQty)
				{
					double allowedQty = totalQty - wagesQty;
					item.Cells["UnloadedQty"].Value = ((allowedQty < 0.0) ? 0.0 : allowedQty);
				}
			}
			grdWages.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWages_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "UnloadedQty" && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only positive Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdWages.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Add")
			{
				AddRowInWagesGrid();
			}
			else if (e.Column.Key == "Delete")
			{
				int packingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
				double packSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
				if (dtWagesDetail.AsEnumerable().Count((DataRow r) => r.Field<int>("PackingTypeId") == packingTypeId && r.Field<double>("PackSize") == packSize) <= 1)
				{
					MessageBox.Show("This row cannot be deleted because it is the only entry for this Packing Type and Pack Size.", "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInEmptyBagGrid()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				int PackingTypeId = Conversion.ToInt(grdEmptyBags.CurrentRow.Cells["BagsTypeId"].Value);
				string PackingType = Conversion.ToString(grdEmptyBags.CurrentRow.Cells["BagsType"].Value);
				double totalQtyInDetail = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("PackingTypeId") == PackingTypeId
					select r).Sum((DataRow r) => r.Field<double>("UnloadedQty"));
				double PartyQty = (from r in dtEbDetail.AsEnumerable()
					where r.Field<int>("BagsTypeId") == PackingTypeId
					select r).Sum((DataRow r) => r.Field<double>("PartyBags"));
				double CompanyQty = (from r in dtEbDetail.AsEnumerable()
					where r.Field<int>("BagsTypeId") == PackingTypeId
					select r).Sum((DataRow r) => r.Field<double>("CompanyBags"));
				if (totalQtyInDetail == PartyQty + CompanyQty)
				{
					throw new Exception("Not furthur breakable");
				}
				double rem = totalQtyInDetail - (PartyQty + CompanyQty);
				dtEbDetail.Rows.Add(0, 0, PackingTypeId, PackingType, rem, 0, "");
				grdEmptyBags.DataSource = dtEbDetail;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridEmptyBagCombo()
	{
		try
		{
			grdEmptyBags.RootTable.Columns["BagConditionId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["BagConditionId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["BagConditionId"].EditType = (EditType)5;
			grdEmptyBags.RootTable.Columns["BagConditionId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["BagConditionId"].Caption = "Bag Condition";
			grdEmptyBags.RootTable.Columns["BagConditionId"].ValueList.PopulateValueList((IEnumerable)dtEbCondition.DefaultView, "Id", "type");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBagSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "BagsTypeId" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"BagConditionId",
					(EditType)4
				},
				{
					"PartyBags",
					(EditType)1
				},
				{
					"CompanyBags",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBags, 2, 3, hiddenColumns, editCol);
			GridEmptyBagCombo();
			grdEmptyBags.RootTable.Columns["BagConditionId"].Width = Constants.InventoryConstants.PackingType;
			grdEmptyBags.RootTable.Columns["BagsType"].Width = Constants.InventoryConstants.PackingType;
			grdEmptyBags.RootTable.Columns["PartyBags"].Width = Constants.InventoryConstants.QtyConstant;
			grdEmptyBags.RootTable.Columns["CompanyBags"].Width = Constants.InventoryConstants.QtyConstant;
			grdEmptyBags.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 100;
			grdEmptyBags.RootTable.Columns.Add("Delete");
			grdEmptyBags.RootTable.Columns["Delete"].Key = "Delete";
			grdEmptyBags.RootTable.Columns["Delete"].Caption = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonText = "X";
			grdEmptyBags.RootTable.Columns["Delete"].Width = 20;
			grdEmptyBags.RootTable.Columns.Add("Add");
			grdEmptyBags.RootTable.Columns["Add"].Key = "Add";
			grdEmptyBags.RootTable.Columns["Add"].Caption = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Add"].ButtonText = "+";
			grdEmptyBags.RootTable.Columns["Add"].Width = 20;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdEmptyBags.UpdateData();
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "PartyBags" || e.Column.Key == "CompanyBags")
			{
				int packingTypeId = Conversion.ToInt(item.Cells["BagsTypeId"].Value);
				double PartyQty = Conversion.ToDouble(item.Cells["PartyBags"].Value);
				double CompanyQty = Conversion.ToDouble(item.Cells["CompanyBags"].Value);
				double totalQty = (from r in dtDetail.AsEnumerable()
					where r.Field<int>("PackingTypeId") == packingTypeId
					select r.Field<double?>("UnloadedQty").GetValueOrDefault()).Sum();
				DataRow currentRow = ((DataRowView)item.DataRow).Row;
				double EbPartyQty = (from r in dtEbDetail.AsEnumerable()
					where r != currentRow && r.Field<int>("BagsTypeId") == packingTypeId
					select r.Field<double?>("PartyBags").GetValueOrDefault()).Sum();
				double EbCompanyQty = (from r in dtEbDetail.AsEnumerable()
					where r != currentRow && r.Field<int>("BagsTypeId") == packingTypeId
					select r.Field<double?>("CompanyBags").GetValueOrDefault()).Sum();
				if (PartyQty + EbPartyQty + CompanyQty + EbCompanyQty > totalQty)
				{
					double allowedQty = totalQty - (PartyQty + EbPartyQty + CompanyQty + EbCompanyQty);
					item.Cells[e.Column.Key].Value = ((allowedQty < 0.0) ? 0.0 : allowedQty);
				}
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "PartyBags" || e.Column.Key == "CompanyBags") && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only positive Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdEmptyBags.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Add")
			{
				AddRowInEmptyBagGrid();
			}
			else if (e.Column.Key == "Delete")
			{
				int packingTypeId = Conversion.ToInt(item.Cells["BagsTypeId"].Value);
				if (dtEbDetail.AsEnumerable().Count((DataRow r) => r.Field<int>("BagsTypeId") == packingTypeId) <= 1)
				{
					MessageBox.Show("This row cannot be deleted because it is the only entry for this Packing Type and Pack Size.", "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert(bool IsAutoUpdate = false, int RecUpdateId = 0)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			UnloadingInformation obj = new UnloadingInformation();
			if (!IsAutoUpdate)
			{
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					obj.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			if (grd.GetRows().Count() <= 0)
			{
				((Control)(object)grd).Focus();
				MessageBox.Show("Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (grdWages.GetRows().Count() <= 0)
			{
				((Control)(object)grdWages).Focus();
				MessageBox.Show("Wages Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (grdEmptyBags.GetRows().Count() <= 0)
			{
				((Control)(object)grd).Focus();
				MessageBox.Show("Empty bags Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 930;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.GatePassId = Conversion.ToInt((object)GpId);
			obj.WbId = Conversion.ToInt((object)WbId);
			obj.LabId = Conversion.ToInt((object)LabId);
			obj.GpNo = Conversion.ToInt((object)((Control)(object)CmbGpNo).Text);
			obj.GpDate = Conversion.ToDateTime(((UltraDropDownBase)CmbGpNo).SelectedRow.Cells["GpDate"].Value);
			obj.VehicleNo = txtVehicleNo.Text;
			obj.BiltyNo = txtBiltyNo.Text;
			double UnloadedQtyDetail = GridEX_Helper.GetColumnSum(grd, "UnloadedQty");
			double UnloadedQtyWages = GridEX_Helper.GetColumnSum(grdWages, "UnloadedQty");
			double columnSum = GridEX_Helper.GetColumnSum(grdEmptyBags, "PartyBags");
			double CompanyBags = GridEX_Helper.GetColumnSum(grdEmptyBags, "CompanyBags");
			double TotalEbQty = columnSum + CompanyBags;
			if (UnloadedQtyDetail != UnloadedQtyWages && UnloadedQtyDetail != TotalEbQty)
			{
				throw new Exception("Total detail Unloaded Qty not equal to Wages Qty!");
			}
			if (UnloadedQtyDetail != TotalEbQty)
			{
				throw new Exception("Total detail Unloaded Qty not equal to Empty Bags Qty!");
			}
			obj.UnloadingInformationDetailList = new List<UnloadingInformationDetail>();
			obj.UnloadingInformationWagesDetailList = new List<UnloadingInformationWagesDetail>();
			obj.UnloadingInformationEmptyBagDetailList = new List<UnloadingInformationEmptyBagDetail>();
			if (grd.RowCount > 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow r3 in dataRows)
				{
					UnloadingInformationDetail Sd = new UnloadingInformationDetail();
					Sd.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					Sd.ActionTypeId = ((Sd.Id <= 0) ? 1 : 2);
					Sd.WareHouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
					Sd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Sd.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Sd.PackingTypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					Sd.PackSize = Conversion.ToDouble(r3.Cells["PackSize"].Value);
					Sd.UnloadedQty = Conversion.ToDouble(r3.Cells["UnloadedQty"].Value);
					Sd.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					Sd.GatePassId = obj.GatePassId;
					Sd.WbId = obj.WbId;
					Sd.LabId = obj.LabId;
					if (Sd.WareHouseId == 0)
					{
						throw new Exception("Warehouse Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					if (Sd.ItemId == 0)
					{
						throw new Exception("Item Required In Row #" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					if (Sd.JobLotId == 0)
					{
						throw new Exception("Job Lot Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					if (Sd.PackingTypeId == 0)
					{
						throw new Exception("Packing Type Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					if (Sd.UnloadedQty == 0.0)
					{
						throw new Exception("Unloeded Qty Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					if (Sd.PackSize == 0.0)
					{
						throw new Exception("Pack Size Required In Row#" + (r3.RowIndex + 1) + " In Detail Grid");
					}
					obj.UnloadingInformationDetailList.Add(Sd);
				}
				dataRows = grdWages.GetDataRows();
				foreach (GridEXRow r2 in dataRows)
				{
					UnloadingInformationWagesDetail d2 = new UnloadingInformationWagesDetail();
					d2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					d2.ActionTypeId = ((d2.Id <= 0) ? 1 : 2);
					d2.ContractorId = Conversion.ToInt(r2.Cells["ContractorId"].Value);
					d2.BagsTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
					d2.PackSize = Conversion.ToDouble(r2.Cells["PackSize"].Value);
					d2.UnloadedQty = Conversion.ToDouble(r2.Cells["UnloadedQty"].Value);
					d2.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					if (d2.ContractorId == 0)
					{
						throw new Exception("Contractor Required In Row#" + (r2.RowIndex + 1) + " In Wages Grid");
					}
					if (d2.UnloadedQty == 0.0)
					{
						throw new Exception("Unloaded Qty Required In Row #" + (r2.RowIndex + 1) + " In Wages Grid");
					}
					d2.GatePassId = obj.GatePassId;
					d2.WbId = obj.WbId;
					d2.LabId = obj.LabId;
					obj.UnloadingInformationWagesDetailList.Add(d2);
				}
				dataRows = grdEmptyBags.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					UnloadingInformationEmptyBagDetail d = new UnloadingInformationEmptyBagDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.ActionTypeId = ((d.Id <= 0) ? 1 : 2);
					d.BagsConditionId = Conversion.ToInt(r.Cells["BagConditionId"].Value);
					d.BagsTypeId = Conversion.ToInt(r.Cells["BagsTypeId"].Value);
					d.PartyBags = Conversion.ToDouble(r.Cells["PartyBags"].Value);
					d.CompanyBags = Conversion.ToDouble(r.Cells["CompanyBags"].Value);
					d.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					d.GatePassId = obj.GatePassId;
					d.WbId = obj.WbId;
					d.LabId = obj.LabId;
					if (d.BagsConditionId == 0)
					{
						throw new Exception("Bags Condition Required In Row#" + (r.RowIndex + 1) + " In Empty Bag Grid");
					}
					if (d.PartyBags == 0.0 && d.CompanyBags == 0.0)
					{
						throw new Exception("Party Or Company bags Required In Row #" + (r.RowIndex + 1) + " In Empty Bag Grid");
					}
					obj.UnloadingInformationEmptyBagDetailList.Add(d);
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<UnloadingInformation>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				UnloadingInformation.Save(obj);
				if (IsAutoUpdate)
				{
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 930, 0);
					}
					Reset();
					return;
				}
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully against GpNo [" + obj.GpNo + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + obj.GpNo + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 930, 0);
					}
				}
				Reset();
				return;
			}
			throw new Exception("Detail Grid Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = Conversion.ToInt((object)ID);
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(RecId);
			AttachmentsValues = invSI.AttachmentsValues;
			CustomAttachmentsValues = invSI.CustomAttachmentsValues;
			dtDetail.Rows.Clear();
			for (int i = 0; i < invSI.InvSaleInvoiceDetaillist.Count; i++)
			{
				dtDetail.Rows.Add(invSI.InvSaleInvoiceDetaillist[i].Id, invSI.InvSaleInvoiceDetaillist[i].SaleOrderId, invSI.InvSaleInvoiceDetaillist[i].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[i].SaleOrder, invSI.InvSaleInvoiceDetaillist[i].OrderDate, invSI.InvSaleInvoiceDetaillist[i].WarehouseId, invSI.InvSaleInvoiceDetaillist[i].ItemId, invSI.InvSaleInvoiceDetaillist[i].ItemCode, invSI.InvSaleInvoiceDetaillist[i].ItemName, invSI.InvSaleInvoiceDetaillist[i].ItemUOMId, invSI.InvSaleInvoiceDetaillist[i].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[i].PackEquivalent, invSI.InvSaleInvoiceDetaillist[i].ItemVariantId, invSI.InvSaleInvoiceDetaillist[i].VariantDescription, invSI.InvSaleInvoiceDetaillist[i].CastingTypeId, invSI.InvSaleInvoiceDetaillist[i].ProductionStageId, invSI.InvSaleInvoiceDetaillist[i].RemarksDetail, invSI.InvSaleInvoiceDetaillist[i].ItemQty, invSI.InvSaleInvoiceDetaillist[i].WeightFinishGoods, invSI.InvSaleInvoiceDetaillist[i].ItemRate - invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[i].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[i].RateUOM, invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].ItemRate, invSI.InvSaleInvoiceDetaillist[i].DiscountTypeId, invSI.InvSaleInvoiceDetaillist[i].ItemDiscount, invSI.InvSaleInvoiceDetaillist[i].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[i].ItemAmount, invSI.InvSaleInvoiceDetaillist[i].FcyAmount, invSI.InvSaleInvoiceDetaillist[i].TaxNameId, invSI.InvSaleInvoiceDetaillist[i].TaxPercent, invSI.InvSaleInvoiceDetaillist[i].TaxAmount, invSI.InvSaleInvoiceDetaillist[i].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[i].FreightAmount, invSI.InvSaleInvoiceDetaillist[i].BillAmount, invSI.InvSaleInvoiceDetaillist[i].GpDate.ToShortDateString(), invSI.InvSaleInvoiceDetaillist[i].GpNo, invSI.InvSaleInvoiceDetaillist[i].VehicleNo, invSI.InvSaleInvoiceDetaillist[i].CityId);
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings();
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 1661,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "1661"
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtPaymentTerm = new DataTable();
			dtPaymentTerm.Columns.Add("Id", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PaymentTerm")
				{
					dtPaymentTerm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Customer", ZeroIndex: false);
			DDL.BindDDL(dtPaymentTerm, CmbPaymentTermHistory, "Id", "PaymentTerm", "Payment Term", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
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
			FromDateHistory.Focus();
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters SI = new ReportsParameters();
			SI.OrganizationId = UserAccount.OrganizationId;
			SI.CompanyId = UserAccount.CompanyId;
			SI.DocumentTypeId = 1661;
			SI.CanViewAllRecord = FormRights.DoHaveCanViewAllRecordRights;
			SI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			SI.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					SI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					SI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			SI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			SI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			SI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			SI.PaymenetTermId = Conversion.ToInt(CmbPaymentTermHistory.Value);
			DataTable dtGetAll = InvSaleInvoice.FormHistory(SI);
			if (dtGetAll.Rows.Count > 0)
			{
				if (dthistory.Columns.Count == 0)
				{
					dthistory.Columns.Clear();
					dthistory.Columns.Add("Id", typeof(int));
					dthistory.Columns.Add("VoucherHeadId", typeof(int));
					dthistory.Columns.Add("DocNo", typeof(int));
					dthistory.Columns.Add("DocDate", typeof(DateTime));
					dthistory.Columns.Add("DueDate", typeof(DateTime));
					dthistory.Columns.Add("ManualBillNo");
					dthistory.Columns.Add("SupplierCustomerId", typeof(int));
					dthistory.Columns.Add("CustomerName");
					dthistory.Columns.Add("BillAmount", typeof(double));
					dthistory.Columns.Add("EntryUser");
					dthistory.Columns.Add("EntryDate", typeof(DateTime));
					dthistory.Columns.Add("ModifyUser");
					dthistory.Columns.Add("ModifyDate", typeof(DateTime));
					dthistory.Columns.Add("ApprovedUser");
					dthistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dthistory.Columns.Add("NoOfAttachments", typeof(int));
					dthistory.Columns.Add("Remarks");
				}
				dthistory.Clear();
				for (int i = 0; i < dtGetAll.Rows.Count; i++)
				{
					dthistory.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["VoucherHeadId"], dtGetAll.Rows[i]["DocNo"], Conversion.ToDateTime(dtGetAll.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToDateTime(dtGetAll.Rows[i]["DueDate"]).ToShortDateString(), dtGetAll.Rows[i]["ManualBillNo"], dtGetAll.Rows[i]["SupplierCustomerId"], dtGetAll.Rows[i]["CustomerName"], dtGetAll.Rows[i]["BillAmount"], dtGetAll.Rows[i]["UserName"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["ModifyUserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ApprovedUserName"], dtGetAll.Rows[i]["ApprovedDate"], dtGetAll.Rows[i]["NoOfAttachments"], dtGetAll.Rows[i]["RemarksHeader"]);
				}
				grdHistory.DataSource = dthistory;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
				grdDetail.ClearStructure();
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
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdHistory.RootTable.Columns["ManualBillNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["BillAmount"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdHistory, 3, 3);
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("Print-1661");
			grdHistory.RootTable.Columns["Print-1661"].Caption = "Print-1661";
			grdHistory.RootTable.Columns["Print-1661"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print-1661"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print-1661"].ButtonText = "Print-1661";
			grdHistory.RootTable.Columns["Print-1661"].Position = 1;
			grdHistory.RootTable.Columns["Print-1661"].Width = 80;
			grdHistory.RootTable.Columns.Add("Print-1661A");
			grdHistory.RootTable.Columns["Print-1661A"].Caption = "Print-1661A";
			grdHistory.RootTable.Columns["Print-1661A"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print-1661A"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print-1661A"].ButtonText = "Print-1661A";
			grdHistory.RootTable.Columns["Print-1661A"].Position = 2;
			grdHistory.RootTable.Columns["Print-1661A"].Width = 80;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Position = 3;
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(id);
				}
				else if (e.Column.Key == "Print-1661")
				{
					int SupCustId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(item.Cells["DocDate"].Value);
					CommonServices.SaleInvoicePrint1661(id, SupCustId, 0, Date, Date);
				}
				else if (e.Column.Key == "Print-1661A")
				{
					GeneratePrint1661A(id);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(item.Cells["VoucherHeadId"].Value), 1661);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1661);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow Row = grdHistory.CurrentRow;
			if (Row != null && (int)Row.RowType == 0 && Row.Cells.Count > 0)
			{
				BindDetailOfHeaderId(Conversion.ToInt(Row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailOfHeaderId(int Id)
	{
		try
		{
			InvSaleInvoice invSI = InvSaleInvoice.GetByID(Id);
			if (invSI.InvSaleInvoiceDetaillist != null && invSI.InvSaleInvoiceDetaillist.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderDetailId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("OrderDate", typeof(DateTime));
				dt.Columns.Add("Warehouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUomId", typeof(int));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("PackEquivalent", typeof(double));
				dt.Columns.Add("VariantId", typeof(int));
				dt.Columns.Add("VariantDescription", typeof(string));
				dt.Columns.Add("CastingType", typeof(string));
				dt.Columns.Add("ProductionStage", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUomId", typeof(int));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("RateEquivalent", typeof(double));
				dt.Columns.Add("AddLessRate", typeof(double));
				dt.Columns.Add("NetRate", typeof(double));
				dt.Columns.Add("DiscountType", typeof(string));
				dt.Columns.Add("Discount%", typeof(double));
				dt.Columns.Add("DiscountAmount", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("TaxType", typeof(string));
				dt.Columns.Add("Tax%", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("Expense", typeof(double));
				dt.Columns.Add("Freights", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("CityName");
				for (int i = 0; i < invSI.InvSaleInvoiceDetaillist.Count; i++)
				{
					dt.Rows.Add(invSI.InvSaleInvoiceDetaillist[i].Id, invSI.InvSaleInvoiceDetaillist[i].SaleOrderId, invSI.InvSaleInvoiceDetaillist[i].SaleOrderDetailId, invSI.InvSaleInvoiceDetaillist[i].SaleOrder, invSI.InvSaleInvoiceDetaillist[i].OrderDate, invSI.InvSaleInvoiceDetaillist[i].WareHouseName, invSI.InvSaleInvoiceDetaillist[i].ItemId, invSI.InvSaleInvoiceDetaillist[i].ItemCode, invSI.InvSaleInvoiceDetaillist[i].ItemName, invSI.InvSaleInvoiceDetaillist[i].ItemUOMId, invSI.InvSaleInvoiceDetaillist[i].UOMCodeItem, invSI.InvSaleInvoiceDetaillist[i].PackEquivalent, invSI.InvSaleInvoiceDetaillist[i].ItemVariantId, invSI.InvSaleInvoiceDetaillist[i].VariantDescription, invSI.InvSaleInvoiceDetaillist[i].CastingType, invSI.InvSaleInvoiceDetaillist[i].ProductionProcessStage, invSI.InvSaleInvoiceDetaillist[i].RemarksDetail, invSI.InvSaleInvoiceDetaillist[i].ItemQty, invSI.InvSaleInvoiceDetaillist[i].WeightFinishGoods, invSI.InvSaleInvoiceDetaillist[i].ItemRate - invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].UomScheduleIdRate, invSI.InvSaleInvoiceDetaillist[i].UOMCodeRate, invSI.InvSaleInvoiceDetaillist[i].RateUOM, invSI.InvSaleInvoiceDetaillist[i].RateCut, invSI.InvSaleInvoiceDetaillist[i].ItemRate, invSI.InvSaleInvoiceDetaillist[i].DiscountType, invSI.InvSaleInvoiceDetaillist[i].ItemDiscount, invSI.InvSaleInvoiceDetaillist[i].ItemDiscountAmount, invSI.InvSaleInvoiceDetaillist[i].ItemAmount, invSI.InvSaleInvoiceDetaillist[i].FcyAmount, invSI.InvSaleInvoiceDetaillist[i].TaxName, invSI.InvSaleInvoiceDetaillist[i].TaxPercent, invSI.InvSaleInvoiceDetaillist[i].TaxAmount, invSI.InvSaleInvoiceDetaillist[i].ExpenseAmount, invSI.InvSaleInvoiceDetaillist[i].FreightAmount, invSI.InvSaleInvoiceDetaillist[i].BillAmount, invSI.InvSaleInvoiceDetaillist[i].GpDate.ToShortDateString(), invSI.InvSaleInvoiceDetaillist[i].GpNo, invSI.InvSaleInvoiceDetaillist[i].VehicleNo, invSI.InvSaleInvoiceDetaillist[i].CityName);
					grdDetail.DataSource = dt;
					grdDetail.RetrieveStructure();
					grdDetailSettings();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["VariantId"].Visible = false;
			grdDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdDetail.RootTable.Columns["Freights"].Visible = false;
			grdDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["PackUom"].Caption = "Uom";
			grdDetail.RootTable.Columns["VariantDescription"].Caption = "Modal Description";
			grdDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["NetWeight"].Caption = "Item FG Weight";
			grdDetail.RootTable.Columns["Remarks"].Caption = "Specification/Remarks";
			GridEXRow row = grdDetail.GetRow(0);
			int OrderId = ((row != null) ? Conversion.ToInt(row.Cells["OrderId"].Value) : 0);
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				CommonServices.GridColumnSettings(item, 3);
			}
			grdDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grdDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["AddLessRate"].Width = Constants.InventoryConstants.Address;
			grdDetail.RootTable.Columns["NetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["DiscountType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["Discount%"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["DiscountAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TaxType"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Freights"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdDetail.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdDetail.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdDetail.RootTable.Columns["ProductionStage"].Width = Constants.InventoryConstants.ProductionStage;
			grdDetail.RootTable.Columns["OrderNo"].Visible = OrderId > 0;
			grdDetail.RootTable.Columns["OrderDate"].Visible = OrderId > 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbPaymentTermHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
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
			GetAll();
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
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryComboFill();
				return;
			}
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((UltraGridBase)CmbCustomerHistory).DataSource = null;
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtDetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dthistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdWages, dtWagesDetail);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdEmptyBags, dtEbDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipDetail_Click(object sender, EventArgs e)
	{
	}

	private void GeneratePrint1661A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 1661;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaleinvoice2_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GeneratePrint1661A(RecId);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			btnRecordsUpdate.Visible = false;
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
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F)
			{
				btnRecordsUpdate.Visible = true;
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
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnFrmRefresh_Click(null, null);
				}
				else if (!e.Control || e.KeyCode != Keys.F5)
				{
					if (e.Control && e.KeyCode == Keys.F10)
					{
						AT.Show();
					}
					else if (e.Control && e.KeyCode == Keys.P)
					{
						btnSlipDetail_Click(null, null);
					}
					else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
					{
						btnSlipDetail_Click(null, null);
					}
					else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
					{
						btnSaleinvoice2_Click(null, null);
					}
					else if (e.Alt && e.KeyCode != Keys.NumPad3 && e.KeyCode != Keys.D3)
					{
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 1661");
			dt.Rows.Add("Alt+2", "For Print Slip 1661A");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus Grids");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Add")
				{
					DataRow dr = dtDetail.Rows[item.RowIndex];
					dtDetail.Rows.Add(dr.ItemArray);
					grd.DataSource = dtDetail;
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
					}
				}
				if (grd.CurrentColumn.Key == "Delete" && item != null && (int)item.RowType == 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					item.Delete();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				DataRow dr2 = dtDetail.Rows[item.RowIndex];
				dtDetail.Rows.Add(dr2.ItemArray);
				grd.DataSource = dtDetail;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				item.Delete();
			}
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
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (FormRights.DoHaveUpdateRights)
					{
						RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
						ReadById(RecId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Print-1661")
				{
					int SupCustId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					DateTime Date = Conversion.ToDateTime(item.Cells["DocDate"].Value);
					CommonServices.SaleInvoicePrint1661(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), SupCustId, 0, Date, Date);
				}
				else if (grdHistory.CurrentColumn.Key == "Print-1661A")
				{
					GeneratePrint1661A(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(item.Cells["VoucherHeadId"].Value), 1661);
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 1661);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (FormRights.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && FormRights.DoHavePrintRights)
			{
				int SupCustId2 = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				DateTime Date2 = Conversion.ToDateTime(item.Cells["DocDate"].Value);
				CommonServices.SaleInvoicePrint1661(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), SupCustId2, 0, Date2, Date2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateInGridDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetDataRows().Count() > 0)
			{
				AddRowsInWagesAndEmptyBagGrid();
				if (((UltraGridBase)CmbWarehouseForUpdateInGrid).ActiveRow == null || Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value) == 0)
				{
					GridEXRow[] GridRows = grd.GetDataRows();
					if (Conversion.ToInt(GridRows[0].Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in Detail Grid Row No: 01");
					}
					Conversion.ToInt(GridRows[0].Cells["WarehouseId"].Value);
				}
				else if (((UltraGridBase)CmbWarehouseForUpdateInGrid).ActiveRow != null && Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value) != 0)
				{
					int ContrId = Conversion.ToInt(CmbWarehouseForUpdateInGrid.Value);
					GridEXRow[] dataRows = grd.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = ContrId;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("Detail Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecordsUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dtIds = InvSaleInvoice.GetSaleInvoiceIdsForAutoUpdation(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, 1661);
			if (dtIds != null && dtIds.Rows.Count > 0)
			{
				bool ShouldStop = false;
				for (int i = 0; i < dtIds.Rows.Count; i++)
				{
					int RecId = Conversion.ToInt(dtIds.Rows[i]["Id"]);
					ReadById(RecId);
					if (!ShouldStop)
					{
						break;
					}
				}
				if (ShouldStop)
				{
					MessageBox.Show("Records Update Successfully");
				}
				btnRecordsUpdate.Visible = false;
				return;
			}
			btnRecordsUpdate.Visible = false;
			throw new Exception("Ids Not Found For Update");
		}
		catch (Exception ex)
		{
			btnRecordsUpdate.Visible = false;
			MessageBox.Show(ex.Message);
		}
	}

	private void frmUnLoadingInformation_Load(object sender, EventArgs e)
	{
	}

	private void CmbGpNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbGpNo.Value);
			if (Id != GpId && grd.RowCount > 0)
			{
				if (MessageBox.Show("Are you sure to change record? if yes then the detail entry will be lost", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					CmbGpNo.Value = GpId;
					return;
				}
				dtDetail.Rows.Clear();
				AddRowInDetailGrid();
				grd.DataSource = dtDetail;
				dtWagesDetail.Rows.Clear();
				grdWages.DataSource = null;
				dtEbDetail.Rows.Clear();
				grdEmptyBags.DataSource = null;
			}
			if (Id > 0)
			{
				GpId = Conversion.ToInt(CmbGpNo.Value);
				WbId = Conversion.ToInt(((UltraDropDownBase)CmbGpNo).SelectedRow.Cells["WbId"].Value);
				LabId = Conversion.ToInt(((UltraDropDownBase)CmbGpNo).SelectedRow.Cells["LabId"].Value);
				BindInformationAgainstGp();
				dtitem = ItemDbCallAgainstGpWbAndLab();
				GridDetailCombo();
				return;
			}
			GpId = 0;
			LabId = 0;
			WbId = 0;
			groupBox1.Text = "Gate Inward : ";
			txtGpDate.Value = DateTime.Now;
			txtNoOfBags.Text = "";
			txtVehicleNo.Text = "";
			datBiltyDate.Value = DateTime.Now;
			txtCity.Text = "";
			txtFreight.Text = "";
			txtItemName.Text = "";
			txtAnalyst.Text = "";
			txtWarehouse.Text = "";
			txtJobLot.Text = "";
			txtFirstStatus.Text = "";
			txtApprovedBy.Text = "";
			txtFinalStatus.Text = "";
			txtMoisture.Text = "";
			txtFirstWeight.Text = "";
			txtSecondWeight.Text = "";
			txtNetWeight.Text = "";
			txtSupplierWeight.Text = "";
			txtFactoryWeight.Text = "";
			txtDiffWeight.Text = "";
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
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Expected O, but got Unknown
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Expected O, but got Unknown
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Expected O, but got Unknown
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_161d: Expected O, but got Unknown
		//IL_162a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1634: Expected O, but got Unknown
		//IL_1641: Unknown result type (might be due to invalid IL or missing references)
		//IL_164b: Expected O, but got Unknown
		//IL_190d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1917: Expected O, but got Unknown
		//IL_1924: Unknown result type (might be due to invalid IL or missing references)
		//IL_192e: Expected O, but got Unknown
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Expected O, but got Unknown
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c11: Expected O, but got Unknown
		//IL_1c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c28: Expected O, but got Unknown
		//IL_1c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3f: Expected O, but got Unknown
		//IL_5f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f5d: Expected O, but got Unknown
		//IL_5f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f8b: Expected O, but got Unknown
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWages_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance51 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance61 = new Appearance();
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
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.frmUnLoadingInformation));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.txtFirstWeight = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtSecondWeight = new System.Windows.Forms.TextBox();
		this.txtDiffWeight = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdWages = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.CmbWarehouseForUpdateInGrid = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.BtnUpdateInGridDetail = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtJobLot = new System.Windows.Forms.TextBox();
		this.txtWarehouse = new System.Windows.Forms.TextBox();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtFirstStatus = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtFinalStatus = new System.Windows.Forms.TextBox();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtAnalyst = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.txtTransitDays = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtGpDate = new System.Windows.Forms.DateTimePicker();
		this.CmbGpNo = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.txtCity = new System.Windows.Forms.TextBox();
		this.labelLine = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbcommuom = new System.Windows.Forms.ComboBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbcommtype = new System.Windows.Forms.ComboBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbstockprty = new UltraCombo();
		this.cmbcommagent = new UltraCombo();
		this.cmbrefprty = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnRecordsUpdate = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSaleInvoice1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.label66 = new System.Windows.Forms.Label();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbPaymentTermHistory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label54 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWages).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseForUpdateInGrid).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1556, 666);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1556, 666);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.groupBox5);
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1548, 637);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.groupBox5.Controls.Add(this.txtFirstWeight);
		this.groupBox5.Controls.Add(this.label37);
		this.groupBox5.Controls.Add(this.txtFactoryWeight);
		this.groupBox5.Controls.Add(this.txtSecondWeight);
		this.groupBox5.Controls.Add(this.txtDiffWeight);
		this.groupBox5.Controls.Add(this.txtNetWeight);
		this.groupBox5.Controls.Add(this.label40);
		this.groupBox5.Controls.Add(this.label41);
		this.groupBox5.Controls.Add(this.txtSupplierWeight);
		this.groupBox5.Controls.Add(this.label42);
		this.groupBox5.Controls.Add(this.label44);
		this.groupBox5.Controls.Add(this.label47);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 435);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(474, 121);
		this.groupBox5.TabIndex = 119;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Weigh Bridge";
		this.txtFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWeight.Enabled = false;
		this.txtFirstWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstWeight.Location = new System.Drawing.Point(96, 20);
		this.txtFirstWeight.Name = "txtFirstWeight";
		this.txtFirstWeight.Size = new System.Drawing.Size(139, 23);
		this.txtFirstWeight.TabIndex = 119;
		this.txtFirstWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(5, 23);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(71, 15);
		this.label37.TabIndex = 120;
		this.label37.Text = "First Weight";
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(337, 54);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(128, 23);
		this.txtFactoryWeight.TabIndex = 10;
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondWeight.Enabled = false;
		this.txtSecondWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondWeight.Location = new System.Drawing.Point(96, 54);
		this.txtSecondWeight.Name = "txtSecondWeight";
		this.txtSecondWeight.Size = new System.Drawing.Size(139, 23);
		this.txtSecondWeight.TabIndex = 10;
		this.txtSecondWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiffWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiffWeight.Enabled = false;
		this.txtDiffWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffWeight.Location = new System.Drawing.Point(337, 86);
		this.txtDiffWeight.Name = "txtDiffWeight";
		this.txtDiffWeight.Size = new System.Drawing.Size(128, 23);
		this.txtDiffWeight.TabIndex = 10;
		this.txtDiffWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(96, 86);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(139, 23);
		this.txtNetWeight.TabIndex = 10;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(239, 88);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(62, 15);
		this.label40.TabIndex = 118;
		this.label40.Text = "Difference";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(239, 57);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(88, 15);
		this.label41.TabIndex = 118;
		this.label41.Text = "Factory Weight";
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Enabled = false;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWeight.Location = new System.Drawing.Point(337, 20);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(128, 23);
		this.txtSupplierWeight.TabIndex = 10;
		this.txtSupplierWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(5, 88);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(68, 15);
		this.label42.TabIndex = 118;
		this.label42.Text = "Net Weight";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(5, 57);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(89, 15);
		this.label44.TabIndex = 118;
		this.label44.Text = "Second Weight";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(239, 23);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(93, 15);
		this.label47.TabIndex = 118;
		this.label47.Text = "Supplier Weight";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel5.Controls.Add(this.panel21);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdWages);
		this.panel5.Controls.Add(this.panel17);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(478, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1067, 579);
		this.panel5.TabIndex = 3;
		this.grdEmptyBags.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 417);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(1067, 162);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.UpdatingCell += new UpdatingCellEventHandler(grdEmptyBags_UpdatingCell);
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label34);
		this.panel21.Controls.Add(this.ctrlGrdBar4);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 395);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1067, 22);
		this.panel21.TabIndex = 1;
		this.label34.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(5, 1);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(87, 19);
		this.label34.TabIndex = 164;
		this.label34.Text = "Empty Bags";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1027, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		((ScrollableBase)this.grdWages).BackColor = System.Drawing.Color.White;
		this.grdWages.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWages.DesignTimeLayout = grdWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWages).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWages).Location = new System.Drawing.Point(0, 235);
		((System.Windows.Forms.Control)(object)this.grdWages).Name = "grdWages";
		((ScrollableBase)this.grdWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWages).Size = new System.Drawing.Size(1067, 160);
		((System.Windows.Forms.Control)(object)this.grdWages).TabIndex = 3;
		this.grdWages.TotalRow = (InheritableBoolean)1;
		this.grdWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWages).VisualStyle = (VisualStyle)4;
		this.grdWages.CellUpdated += new ColumnActionEventHandler(grdWages_CellUpdated);
		this.grdWages.UpdatingCell += new UpdatingCellEventHandler(grdWages_UpdatingCell);
		this.grdWages.ColumnButtonClick += new ColumnActionEventHandler(grdWages_ColumnButtonClick);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label28);
		this.panel17.Controls.Add(this.ctrlGrdBar3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 213);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1067, 22);
		this.panel17.TabIndex = 0;
		this.label28.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(5, 1);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(129, 19);
		this.label28.TabIndex = 163;
		this.label28.Text = "Contractor Wages";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1028, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdWages;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 101;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1067, 187);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label39);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid);
		this.panel7.Controls.Add(this.label51);
		this.panel7.Controls.Add(this.BtnUpdateInGridDetail);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1067, 26);
		this.panel7.TabIndex = 42;
		this.label39.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.Teal;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(5, 3);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(124, 19);
		this.label39.TabIndex = 163;
		this.label39.Text = "UnLoading Detail";
		this.CmbWarehouseForUpdateInGrid.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbWarehouseForUpdateInGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseForUpdateInGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseForUpdateInGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseForUpdateInGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseForUpdateInGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Location = new System.Drawing.Point(770, 0);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Name = "CmbWarehouseForUpdateInGrid";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseForUpdateInGrid).TabIndex = 160;
		this.label51.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.Teal;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(638, 4);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(130, 19);
		this.label51.TabIndex = 162;
		this.label51.Text = "WareHouse Name";
		this.BtnUpdateInGridDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateInGridDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateInGridDetail.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateInGridDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateInGridDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateInGridDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateInGridDetail.Location = new System.Drawing.Point(952, 1);
		this.BtnUpdateInGridDetail.Name = "BtnUpdateInGridDetail";
		this.BtnUpdateInGridDetail.Size = new System.Drawing.Size(72, 23);
		this.BtnUpdateInGridDetail.TabIndex = 161;
		this.BtnUpdateInGridDetail.Text = "Update";
		this.BtnUpdateInGridDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateInGridDetail.Click += new System.EventHandler(BtnUpdateInGridDetail_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1028, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.txtCommAgentId);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.cmbcommuom);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtSupplierGLId);
		this.panel4.Controls.Add(this.cmbcommtype);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrefprty);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(475, 579);
		this.panel4.TabIndex = 0;
		this.groupBox3.Controls.Add(this.txtApprovedBy);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.txtJobLot);
		this.groupBox3.Controls.Add(this.txtWarehouse);
		this.groupBox3.Controls.Add(this.txtItemName);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtFirstStatus);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtFinalStatus);
		this.groupBox3.Controls.Add(this.txtMoisture);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.txtAnalyst);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 171);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(468, 206);
		this.groupBox3.TabIndex = 119;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Quality Lab";
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Enabled = false;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovedBy.Location = new System.Drawing.Point(337, 130);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.Size = new System.Drawing.Size(128, 23);
		this.txtApprovedBy.TabIndex = 125;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(340, 110);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 15);
		this.label15.TabIndex = 126;
		this.label15.Text = "Approved By";
		this.txtJobLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtJobLot.Enabled = false;
		this.txtJobLot.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtJobLot.Location = new System.Drawing.Point(6, 130);
		this.txtJobLot.Name = "txtJobLot";
		this.txtJobLot.Size = new System.Drawing.Size(316, 23);
		this.txtJobLot.TabIndex = 124;
		this.txtWarehouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWarehouse.Enabled = false;
		this.txtWarehouse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWarehouse.Location = new System.Drawing.Point(6, 85);
		this.txtWarehouse.Name = "txtWarehouse";
		this.txtWarehouse.Size = new System.Drawing.Size(316, 23);
		this.txtWarehouse.TabIndex = 123;
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Enabled = false;
		this.txtItemName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(6, 39);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(316, 23);
		this.txtItemName.TabIndex = 122;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.ForeColor = System.Drawing.Color.Black;
		this.label43.Location = new System.Drawing.Point(5, 111);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(99, 15);
		this.label43.TabIndex = 121;
		this.label43.Text = "Job/Lot Assigned";
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.White;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.Black;
		this.label29.Location = new System.Drawing.Point(5, 66);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(108, 15);
		this.label29.TabIndex = 119;
		this.label29.Text = "Warehouse Routed";
		this.txtFirstStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstStatus.Enabled = false;
		this.txtFirstStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstStatus.Location = new System.Drawing.Point(337, 85);
		this.txtFirstStatus.Name = "txtFirstStatus";
		this.txtFirstStatus.Size = new System.Drawing.Size(128, 23);
		this.txtFirstStatus.TabIndex = 10;
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(5, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 15);
		this.label14.TabIndex = 0;
		this.label14.Text = "Item Name";
		this.txtFinalStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinalStatus.Enabled = false;
		this.txtFinalStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinalStatus.Location = new System.Drawing.Point(337, 176);
		this.txtFinalStatus.Name = "txtFinalStatus";
		this.txtFinalStatus.Size = new System.Drawing.Size(128, 23);
		this.txtFinalStatus.TabIndex = 10;
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Enabled = false;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(6, 176);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(316, 23);
		this.txtMoisture.TabIndex = 10;
		this.txtMoisture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(340, 156);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(68, 15);
		this.label16.TabIndex = 118;
		this.label16.Text = "Final Status";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(340, 65);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(65, 15);
		this.label17.TabIndex = 118;
		this.label17.Text = "First Status";
		this.txtAnalyst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnalyst.Enabled = false;
		this.txtAnalyst.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnalyst.Location = new System.Drawing.Point(337, 39);
		this.txtAnalyst.Name = "txtAnalyst";
		this.txtAnalyst.Size = new System.Drawing.Size(128, 23);
		this.txtAnalyst.TabIndex = 10;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(5, 156);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 118;
		this.label24.Text = "Moisture";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(340, 19);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(46, 15);
		this.label35.TabIndex = 118;
		this.label35.Text = "Analyst";
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1230, 93);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(47, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.groupBox1.Controls.Add(this.txtBiltyNo);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.datBiltyDate);
		this.groupBox1.Controls.Add(this.txtTransitDays);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.txtGpDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNo);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtFreight);
		this.groupBox1.Controls.Add(this.txtCity);
		this.groupBox1.Controls.Add(this.labelLine);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtNoOfBags);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(468, 165);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Gate Inward : ";
		this.datBiltyDate.CustomFormat = "dd-MMM-yy";
		this.datBiltyDate.Enabled = false;
		this.datBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datBiltyDate.Location = new System.Drawing.Point(134, 84);
		this.datBiltyDate.Name = "datBiltyDate";
		this.datBiltyDate.Size = new System.Drawing.Size(113, 22);
		this.datBiltyDate.TabIndex = 4;
		this.txtTransitDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransitDays.Enabled = false;
		this.txtTransitDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransitDays.Location = new System.Drawing.Point(368, 84);
		this.txtTransitDays.Name = "txtTransitDays";
		this.txtTransitDays.Size = new System.Drawing.Size(99, 23);
		this.txtTransitDays.TabIndex = 10;
		this.txtTransitDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(6, 84);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(120, 23);
		this.txtVehicleNo.TabIndex = 10;
		this.txtVehicleNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.txtGpDate.CustomFormat = "dd-MMM-yy hh:mm tt";
		this.txtGpDate.Enabled = false;
		this.txtGpDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGpDate.Location = new System.Drawing.Point(134, 37);
		this.txtGpDate.Name = "txtGpDate";
		this.txtGpDate.Size = new System.Drawing.Size(213, 22);
		this.txtGpDate.TabIndex = 4;
		this.CmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Location = new System.Drawing.Point(6, 35);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Name = "CmbGpNo";
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbGpNo).Leave += new System.EventHandler(CmbGpNo_Leave);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(6, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass No";
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Enabled = false;
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(368, 137);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(99, 23);
		this.txtFreight.TabIndex = 10;
		this.txtFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCity.Enabled = false;
		this.txtCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCity.Location = new System.Drawing.Point(6, 137);
		this.txtCity.Name = "txtCity";
		this.txtCity.Size = new System.Drawing.Size(345, 23);
		this.txtCity.TabIndex = 10;
		this.labelLine.BackColor = System.Drawing.Color.DodgerBlue;
		this.labelLine.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLine.ForeColor = System.Drawing.Color.DodgerBlue;
		this.labelLine.Location = new System.Drawing.Point(7, 112);
		this.labelLine.Name = "labelLine";
		this.labelLine.Size = new System.Drawing.Size(457, 1);
		this.labelLine.TabIndex = 118;
		this.labelLine.Tag = "";
		this.labelLine.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(368, 120);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 118;
		this.label13.Text = "Freight";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(368, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 118;
		this.label6.Text = "Transits Days";
		this.txtNoOfBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfBags.Enabled = false;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(368, 37);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(99, 23);
		this.txtNoOfBags.TabIndex = 10;
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 120);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(53, 15);
		this.label12.TabIndex = 118;
		this.label12.Text = "Location";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(134, 67);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 118;
		this.label10.Text = "Bilty Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 67);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 15);
		this.label5.TabIndex = 118;
		this.label5.Text = "Vehicle No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(134, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(100, 15);
		this.label4.TabIndex = 118;
		this.label4.Text = "Gate In DateTime";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(368, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 15);
		this.label3.TabIndex = 118;
		this.label3.Text = "Item Qty";
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(1478, 69);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(61, 21);
		this.txtcommremarks.TabIndex = 18;
		this.txtcommremarks.TabStop = false;
		this.txtcommremarks.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1400, 72);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(57, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.cmbcommuom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommuom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommuom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommuom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.FormattingEnabled = true;
		this.cmbcommuom.Items.AddRange(new object[7] { "1", "5", "25", "40", "50", "60", "100" });
		this.cmbcommuom.Location = new System.Drawing.Point(1478, 41);
		this.cmbcommuom.Name = "cmbcommuom";
		this.cmbcommuom.Size = new System.Drawing.Size(61, 23);
		this.cmbcommuom.TabIndex = 14;
		this.cmbcommuom.TabStop = false;
		this.cmbcommuom.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1281, 45);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label21.Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1158, 72);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(1347, 41);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(48, 22);
		this.txtcommrate.TabIndex = 13;
		this.txtcommrate.TabStop = false;
		this.txtcommrate.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1400, 45);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(76, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm UOM";
		this.label19.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1347, 68);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(48, 22);
		this.txtcommamount.TabIndex = 15;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1158, 45);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Ref Party";
		this.label2.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1230, 116);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(47, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.FormattingEnabled = true;
		this.cmbcommtype.Items.AddRange(new object[3] { "Flat", "Percentage", "Comm Weight" });
		this.cmbcommtype.Location = new System.Drawing.Point(1478, 14);
		this.cmbcommtype.Name = "cmbcommtype";
		this.cmbcommtype.Size = new System.Drawing.Size(61, 23);
		this.cmbcommtype.TabIndex = 12;
		this.cmbcommtype.TabStop = false;
		this.cmbcommtype.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1281, 72);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1400, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1230, 67);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(47, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(1347, 13);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(48, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		this.cmbrefprty.AllowNull = (DefaultableBoolean)1;
		this.cmbrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Location = new System.Drawing.Point(1230, 40);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Name = "cmbrefprty";
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Size = new System.Drawing.Size(47, 24);
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbrefprty).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1281, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnRecordsUpdate);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1542, 24);
		this.panel3.TabIndex = 1;
		this.btnRecordsUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnRecordsUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecordsUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecordsUpdate.ForeColor = System.Drawing.Color.White;
		this.btnRecordsUpdate.Location = new System.Drawing.Point(276, 1);
		this.btnRecordsUpdate.Name = "btnRecordsUpdate";
		this.btnRecordsUpdate.Size = new System.Drawing.Size(75, 22);
		this.btnRecordsUpdate.TabIndex = 44;
		this.btnRecordsUpdate.Text = "Records Update";
		this.btnRecordsUpdate.UseVisualStyleBackColor = false;
		this.btnRecordsUpdate.Visible = false;
		this.btnRecordsUpdate.Click += new System.EventHandler(btnRecordsUpdate_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(0, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(276, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Purchase UnLoading Information";
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(1493, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(46, 19);
		this.ChkBok.TabIndex = 17;
		this.ChkBok.TabStop = false;
		this.ChkBok.Text = "Slip";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1542, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.toolStripSeparator3, this.btnNew, this.btnFrmRefresh, this.toolStripSeparator2, this.btnSave, this.btnDelete, this.btnUpdate, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator5,
			this.btnSaleInvoice1, this.toolStripSeparator7, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1542, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnFrmRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnSaleInvoice1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSaleInvoice1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaleInvoice1.Name = "btnSaleInvoice1";
		this.btnSaleInvoice1.Size = new System.Drawing.Size(100, 24);
		this.btnSaleInvoice1.Text = "1661-Print";
		this.btnSaleInvoice1.Click += new System.EventHandler(btnSlipDetail_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel10);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabHistory.Controls.Add(this.panel8);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1548, 606);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 138);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1542, 305);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label27);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel10.Location = new System.Drawing.Point(3, 443);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1542, 18);
		this.panel10.TabIndex = 2;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 2);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(187, 14);
		this.label27.TabIndex = 25;
		this.label27.Text = "Detail Of Selected Above row";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel14.Location = new System.Drawing.Point(3, 111);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1542, 27);
		this.panel14.TabIndex = 29;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1501, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(171, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "Sale Invoice History";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 461);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1542, 142);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 3;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add(this.groupBox4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 31);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1542, 80);
		this.panel8.TabIndex = 0;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.label66);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnShow);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory);
		this.groupBox4.Controls.Add(this.label52);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label18);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.label46);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Controls.Add(this.label54);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(7, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(993, 74);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(86, 15);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(887, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(6, 21);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(79, 15);
		this.label66.TabIndex = 90;
		this.label66.Text = "Branch Name";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(887, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(806, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(806, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(742, 44);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 24);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.CmbPaymentTermHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Location = new System.Drawing.Point(591, 43);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Name = "CmbPaymentTermHistory";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermHistory).TabIndex = 6;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(511, 49);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(84, 15);
		this.label52.TabIndex = 14;
		this.label52.Text = "Payment Term";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(591, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(511, 21);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(58, 15);
		this.label18.TabIndex = 12;
		this.label18.Text = "Customer";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(416, 45);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(340, 49);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(61, 15);
		this.label46.TabIndex = 10;
		this.label46.Text = "To DocNo";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(416, 17);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(340, 21);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 15);
		this.label22.TabIndex = 8;
		this.label22.Text = "From DocNo";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(212, 45);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(86, 45);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(125, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 49);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(79, 15);
		this.label54.TabIndex = 5;
		this.label54.Text = "From,To Date";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1542, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.BtnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1542, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(55, 24);
		this.btnResetHistory.Text = "New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(70, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Enabled = false;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(248, 84);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(98, 23);
		this.txtBiltyNo.TabIndex = 119;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(248, 66);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 15);
		this.label1.TabIndex = 120;
		this.label1.Text = "Bilty No";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1556, 666);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmUnLoadingInformation";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmUnLoadingInformation_FormClosed);
		base.Load += new System.EventHandler(frmUnLoadingInformation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWages).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseForUpdateInGrid).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefprty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
