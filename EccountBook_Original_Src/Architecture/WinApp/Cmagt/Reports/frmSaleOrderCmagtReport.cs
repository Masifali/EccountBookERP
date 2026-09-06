using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.CommissionAgent;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Cmagt.Reports;

public class frmSaleOrderCmagtReport : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGridFromDb = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtHistoryCombo = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	private int PrintId;

	private bool HasMultiCurrencyFeature = true;

	private string DocumentTypeIds = "1053";

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdfrm;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private Label label7;

	private Label label3;

	private Label label1;

	internal UltraCombo cmbCategory;

	private Button btnshow;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label11;

	internal UltraCombo cmbReportType;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Label label23;

	private ToolStripButton btnPrint;

	private Label label30;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnshortcutkeys;

	private Label label2;

	internal UltraCombo CmbStatus;

	internal UltraCombo CmbCommissionAgent;

	internal UltraCombo CmbBuyer;

	private Label label14;

	internal UltraCombo CmbParentItem;

	public UltraCombo CmbIsApproved;

	private Label label4;

	private ToolStripButton btnOpenOrders;

	private ToolStripButton btnCompleteOrders;

	private ToolStripButton btnCancelOrders;

	private Label label6;

	internal UltraCombo CmbShipToParty;

	private Label label5;

	internal UltraCombo CmbDeliveryToParty;

	private ToolStripDropDownButton tsDrowdownSaleCmgt;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label46;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private RadioButton drdocdate;

	public frmSaleOrderCmagtReport(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
		InitializeComponentMethod();
	}

	private void frmStockReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			ReportsLoad();
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
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			return new
			{
				ComboData = ComboDbCall()
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				formright = CommonServices.SetRightsValueInRightsObject(base.Name);
				var result = t.Result;
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
				ReportTypeFill();
				PrintButtonManage();
				ComboBind(result.ComboData);
				BindDropdownsAgainstParentCategory();
				StatusFill();
				if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
				{
					txtDateFrom.Value = FromDate;
					txtDateTo.Value = ToDate;
				}
				else
				{
					txtDateFrom.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					txtDateTo.Value = DateTime.Now;
				}
				FormInitialized = true;
				txtDateFrom.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboDbCall();
			if (((Control)(object)CmbParentItem).Text != null && ((Control)(object)CmbParentItem).Text != string.Empty)
			{
				BindDropdownsAgainstParentCategory();
				return;
			}
			ComboBind(dtHistoryCombo);
			BindDropdownsAgainstParentCategory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ComboDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryCombo.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistoryCombo = saleOrderMaster.GetDataForDropDown(obj);
			return dtHistoryCombo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtCA = new DataTable();
			dtCA.Columns.Add("Id", typeof(int));
			dtCA.Columns.Add("Name");
			DataTable dtParentCategory = dtCA.Clone();
			DataTable dtCategory = dtCA.Clone();
			DataTable dtItem = dtCA.Clone();
			DataTable dtBuyer = dtCA.Clone();
			DataTable dtStatus = dtCA.Clone();
			DataTable dtToParty = dtCA.Clone();
			DataTable dtToShip = dtCA.Clone();
			if (dtcombo.Rows.Count > 0)
			{
				_ = from r in dtcombo.AsEnumerable()
					where Conversion.ToString(r["Activity"]) == "GetMostUsedParentCategoryId"
					select r;
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					Conversion.ToInt(dtcombo.Rows[i]["ParentCategoryId"]);
					string activity = Convert.ToString(dtcombo.Rows[i]["Activity"]);
					switch (activity)
					{
					case "CommissionAgent":
						dtCA.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "buyerName":
						dtBuyer.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "DeliveryToPartyName":
						dtToParty.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "DeliverToAddress":
						dtToShip.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "Item":
						dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "Status":
						dtStatus.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					case "ItemCategories":
						dtCategory.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
						break;
					}
					if (activity == "ParentCategories")
					{
						dtParentCategory.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbCategory, dtCategory, "Id", "Name", "Category", AllColumns: true, Conversion.ToInt(cmbCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(cmbItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgent, dtCA, "Id", "Name", "Commission Agent", AllColumns: true, Conversion.ToInt(CmbCommissionAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBuyer, dtBuyer, "Id", "Name", "Buyer", AllColumns: true, Conversion.ToInt(CmbBuyer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryToParty, dtToParty, "Id", "Name", "Deliver To Party", AllColumns: true, Conversion.ToInt(CmbDeliveryToParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbShipToParty, dtToShip, "Id", "Name", "Deliver To Address", AllColumns: true, Conversion.ToInt(CmbShipToParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbStatus, dtStatus, "Id", "Name", "Status", AllColumns: true, Conversion.ToInt(CmbStatus.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtStatus != null && dtStatus.Rows.Count > 0)
			{
				((UltraGridBase)CmbStatus).Rows[0].Activate();
			}
			DDL.BindDDL(dtParentCategory, CmbParentItem, "Id", "name", "Parent Categories", ZeroIndex: false);
			((UltraGridBase)CmbParentItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridBand band = ((UltraGridBase)CmbParentItem).DisplayLayout.Bands[0];
			if (!((KeyedSubObjectsCollectionBase)band.Columns).Exists("Selected"))
			{
				UltraGridColumn obj = band.Columns.Add("Selected");
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
			}
			CmbParentItem.LimitToList = false;
			CmbParentItem.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbParentItem.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbParentItem.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbParentItem.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDropdownsAgainstParentCategory()
	{
		try
		{
			string ParentIds = "";
			if (!string.IsNullOrEmpty(((Control)(object)CmbParentItem).Text))
			{
				DataTable dt = (DataTable)((UltraGridBase)CmbParentItem).DataSource;
				string[] array = ((Control)(object)CmbParentItem).Text.Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dt.Select("name='" + author.Trim() + "'");
					if (dr.Length != 0)
					{
						ParentIds = ParentIds + dr[0]["Id"].ToString() + ",";
					}
				}
				if (ParentIds.EndsWith(","))
				{
					ParentIds = ParentIds.TrimEnd(',');
				}
			}
			DataTable dtCA = new DataTable();
			dtCA.Columns.Add("Id", typeof(int));
			dtCA.Columns.Add("Name");
			dtCA.Clear();
			DataTable dtCategory = dtCA.Clone();
			DataTable dtItem = dtCA.Clone();
			DataTable dtBuyer = dtCA.Clone();
			DataTable dtStatus = dtCA.Clone();
			DataTable dtDToParty = dtCA.Clone();
			DataTable dtShipToParty = dtCA.Clone();
			HashSet<int> CaIds = new HashSet<int>();
			HashSet<int> BuyerIds = new HashSet<int>();
			HashSet<int> dToIds = new HashSet<int>();
			HashSet<int> shipToIds = new HashSet<int>();
			HashSet<int> ItemIds = new HashSet<int>();
			HashSet<int> StatusIds = new HashSet<int>();
			HashSet<int> CategoryIds = new HashSet<int>();
			IEnumerable<DataRow> rowsToBind;
			if (!string.IsNullOrEmpty(ParentIds))
			{
				string[] parentIdArray = ParentIds.Split(',');
				rowsToBind = from r in dtHistoryCombo.AsEnumerable()
					where parentIdArray.Contains(r["ParentCategoryId"].ToString())
					select r;
			}
			else
			{
				rowsToBind = dtHistoryCombo.AsEnumerable();
			}
			foreach (DataRow item in rowsToBind)
			{
				string activity = item["Activity"].ToString();
				int id = Conversion.ToInt(item["Id"]);
				string name = item["ReferenceName"].ToString();
				switch (activity)
				{
				case "CommissionAgent":
					if (CaIds.Add(id))
					{
						dtCA.Rows.Add(id, name);
					}
					break;
				case "buyerName":
					if (BuyerIds.Add(id))
					{
						dtBuyer.Rows.Add(id, name);
					}
					break;
				case "DeliveryToPartyName":
					if (dToIds.Add(id))
					{
						dtDToParty.Rows.Add(id, name);
					}
					break;
				case "DeliverToAddress":
					if (shipToIds.Add(id))
					{
						dtShipToParty.Rows.Add(id, name);
					}
					break;
				case "Item":
					if (ItemIds.Add(id))
					{
						dtItem.Rows.Add(id, name);
					}
					break;
				case "Status":
					if (StatusIds.Add(id))
					{
						dtStatus.Rows.Add(id, name);
					}
					break;
				case "ItemCategories":
					if (CategoryIds.Add(id))
					{
						dtCategory.Rows.Add(id, name);
					}
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbCategory, dtCategory, "Id", "Name", "Category", AllColumns: true, Conversion.ToInt(cmbCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(cmbItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgent, dtCA, "Id", "Name", "Commission Agent", AllColumns: true, Conversion.ToInt(CmbCommissionAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBuyer, dtBuyer, "Id", "Name", "Buyer", AllColumns: true, Conversion.ToInt(CmbBuyer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryToParty, dtDToParty, "Id", "Name", "Delivery To Party", AllColumns: true, Conversion.ToInt(CmbDeliveryToParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbShipToParty, dtShipToParty, "Id", "Name", "Ship To Address", AllColumns: true, Conversion.ToInt(CmbShipToParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbStatus, dtStatus, "Id", "Name", "Status", AllColumns: true, Conversion.ToInt(CmbStatus.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtStatus != null && dtStatus.Rows.Count > 0)
			{
				((UltraGridBase)CmbStatus).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbParentItem_Leave(object sender, EventArgs e)
	{
		try
		{
			BindDropdownsAgainstParentCategory();
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
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApproved, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbIsApproved).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add(1, "Detail");
			dtreporttype.Rows.Add(2, "DocWise_Summary");
			dtreporttype.Rows.Add(3, "ItemCommissionAndBuyerWise_Summary");
			dtreporttype.Rows.Add(4, "ItemWise_Summary");
			InfragisticsHelper.BindAndRetainSelection(cmbReportType, dtreporttype, "Id", "name", "Report Type");
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
			GridBind();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
			DataTable dt = (DataTable)((UltraGridBase)CmbParentItem).DataSource;
			obj.ParentCategoryIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)CmbParentItem).Text, dt, "name");
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.CommissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
			obj.BuyerId = Conversion.ToInt(CmbBuyer.Value);
			obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToParty.Value);
			obj.ShipToAddress = Conversion.ToString((object)((Control)(object)CmbShipToParty).Text);
			obj.StatusId = Conversion.ToInt(CmbStatus.Value);
			if (string.IsNullOrEmpty(((Control)(object)CmbIsApproved).Text) || ((Control)(object)CmbIsApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			else
			{
				obj.IsApproved = ((Control)(object)CmbIsApproved).Text == "Approved" || (!(((Control)(object)CmbIsApproved).Text == "UnApproved") && obj.IsApproved);
			}
			if (drdocdate.Checked)
			{
				obj.FromDate = txtDateFrom.Value;
				obj.ToDate = txtDateTo.Value;
			}
			else if (rdentrydate.Checked)
			{
				obj.EntryDateFrom = txtDateFrom.Value;
				obj.EntryDateTo = txtDateTo.Value;
			}
			else if (rdmodifydate.Checked)
			{
				obj.ModifyDateFrom = txtDateFrom.Value;
				obj.ModifyDateTo = txtDateTo.Value;
			}
			obj.Activity = Conversion.ToString((object)((Control)(object)cmbReportType).Text);
			dtGridFromDb = saleOrderMaster.SaleOrder_Report(obj);
			DataTable dataTable = dtGridFromDb;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				grdfrm.DataSource = null;
			}
			DataTable dataTable2 = dtGrid;
			if (dataTable2 != null && dataTable2.Columns.Count == 0)
			{
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("CommissionAgentId", typeof(int));
				dtGrid.Columns.Add("CommissionAgentName", typeof(string));
				dtGrid.Columns.Add("BuyerId", typeof(int));
				dtGrid.Columns.Add("BuyerName", typeof(string));
				dtGrid.Columns.Add("DeliveryToParty", typeof(string));
				dtGrid.Columns.Add("ShipToAddress", typeof(string));
				dtGrid.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dtGrid.Columns.Add("DeliveryDays", typeof(int));
				dtGrid.Columns.Add("Status", typeof(string));
				dtGrid.Columns.Add("ItemId", typeof(int));
				dtGrid.Columns.Add("ItemCode", typeof(string));
				dtGrid.Columns.Add("ItemName", typeof(string));
				dtGrid.Columns.Add("ItemQty", typeof(decimal));
				dtGrid.Columns.Add("GdnQty", typeof(decimal));
				dtGrid.Columns.Add("BalanceQtyAgainstGdn", typeof(decimal));
				dtGrid.Columns.Add("MappedQty", typeof(decimal));
				dtGrid.Columns.Add("BalanceQtyAgainstMapping", typeof(decimal));
				dtGrid.Columns.Add("ItemWeight", typeof(decimal));
				dtGrid.Columns.Add("GdnGrossWeight", typeof(decimal));
				dtGrid.Columns.Add("GdnEbUnit", typeof(decimal));
				dtGrid.Columns.Add("GdnEbTotal", typeof(decimal));
				dtGrid.Columns.Add("GdnAddLess", typeof(decimal));
				dtGrid.Columns.Add("GdnWeight", typeof(decimal));
				dtGrid.Columns.Add("BalanceWeightAgainstGdn", typeof(decimal));
				dtGrid.Columns.Add("MappedWeight", typeof(decimal));
				dtGrid.Columns.Add("BalanceWeightAgainstMapping", typeof(decimal));
				dtGrid.Columns.Add("ItemRate", typeof(decimal));
				dtGrid.Columns.Add("ItemAmount", typeof(decimal));
				dtGrid.Columns.Add("Tax%", typeof(double));
				dtGrid.Columns.Add("TaxAmount", typeof(decimal));
				dtGrid.Columns.Add("TotalAmount", typeof(decimal));
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUserName", typeof(string));
				dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
				dtGrid.Columns.Add("ModifyUserName", typeof(string));
				dtGrid.Columns.Add("ApprovedDate", typeof(DateTime));
				dtGrid.Columns.Add("ApprovalUserName", typeof(string));
				dtGrid.Columns.Add("IsApproved", typeof(bool));
				dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				dtGrid.Columns.Add("IsWhtApplied", typeof(bool));
				dtGrid.Columns.Add("IsbuyerOtherChargesAllowed", typeof(bool));
				dtGrid.Columns.Add("EBWeightDeductionTerm", typeof(string));
				dtGrid.Columns.Add("RemarksHeader", typeof(string));
				dtGrid.Columns.Add("QualitySpecification", typeof(string));
				dtGrid.Columns.Add("RemarksDetail", typeof(string));
				dtGrid.Columns.Add("ValidityDate", typeof(DateTime));
			}
			dtGrid.Clear();
			foreach (DataRow dr in dtGridFromDb.Rows)
			{
				((Control)(object)grdfrm).Name = ((Control)(object)cmbReportType).Text;
				dtGrid.Rows.Add(dr["saleOrderMasterId"], dr["DocNo"], dr["DocDate"], dr["CommissionAgentId"], dr["CommissionAgentName"], dr["BuyerId"], dr["BuyerName"], dr["DeliveryToPartyName"], dr["ShipToAddress"], dr["DeliveryStartDate"], dr["DeliveryDays"], dr["Status"], dr["ItemId"], dr["ItemCode"], dr["ItemName"], dr["ItemQty"], dr["GdnQty"], dr["BalanceQty"], dr["MappedQty"], dr["BalanceQtyAgainstMapping"], dr["ItemWeight"], dr["GdnGrossWeight"], dr["GdnEbUnit"], dr["GdnEbTotal"], dr["GdnAddLess"], dr["GdnWeight"], dr["BalanceWeight"], dr["MappedWeight"], dr["BalanceWeightAgainstMapping"], dr["ItemRate"], dr["ItemAmount"], dr["TaxPercent"], dr["TaxAmount"], dr["TotalAmount"], dr["EntryDate"], dr["EntryUserName"], dr["ModifyDate"], dr["ModifyUserName"], dr["ApprovedDate"], dr["ApprovalUserName"], dr["IsApproved"], dr["NoOfAttachments"], dr["IsWhtApplied"], dr["IsbuyerOtherChargesAllowed"], dr["EBWeightDeductionTerm"], dr["RemarksHeader"], dr["QualitySpecification"], dr["RemarksDetail"], dr["ValidityDate"]);
			}
			grdfrm.DataSource = dtGrid;
			grdfrm.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "CommissionAgentId", "BuyerId", "IsWhtApplied", "IsbuyerOtherChargesAllowed", "ItemId" };
			string type = ((Control)(object)cmbReportType).Text.Trim();
			if (type == "DocWise_Summary")
			{
				hiddenColumns.AddRange(new string[7] { "ItemName", "ItemCode", "ItemRate", "Tax%", "QualitySpecification", "GdnEbUnit", "RemarksDetail" });
			}
			if (type == "ItemCommissionAndBuyerWise_Summary")
			{
				hiddenColumns.AddRange(new string[25]
				{
					"ItemRate", "Tax%", "QualitySpecification", "RemarksDetail", "DocNo", "DocDate", "Status", "DeliveryToParty", "GdnEbUnit", "ShipToAddress",
					"RemarksHeader", "ValidityDate", "DeliveryStartDate", "DeliveryDays", "IsWhtApplied", "IsbuyerOtherChargesAllowed", "EBWeightDeductionTerm", "EntryDate", "EntryUserName", "ModifyDate",
					"ModifyUserName", "ApprovedDate", "ApprovalUserName", "IsApproved", "NoOfAttachments"
				});
			}
			if (type == "ItemWise_Summary")
			{
				hiddenColumns.AddRange(new string[29]
				{
					"ItemRate", "Tax%", "QualitySpecification", "RemarksDetail", "CommissionAgentId", "CommissionAgentName", "BuyerId", "BuyerName", "DeliveryToParty", "ShipToAddress",
					"DocNo", "DocDate", "Status", "GdnEbUnit", "RemarksHeader", "ValidityDate", "DeliveryStartDate", "DeliveryDays", "IsWhtApplied", "IsbuyerOtherChargesAllowed",
					"EBWeightDeductionTerm", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName", "ApprovedDate", "ApprovalUserName", "IsApproved", "NoOfAttachments"
				});
			}
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (type == "Detail")
			{
				bool isApproved2 = ((Control)(object)CmbIsApproved).Text == "Approved";
				bool isOpen2 = ((Control)(object)CmbStatus).Text == "Open";
				bool hasStatus2 = ((Control)(object)CmbStatus).Text != "";
				btnCompleteOrders.Visible = isApproved2 && isOpen2 && formright.CanChangeOrderStatusToComplete;
				btnCancelOrders.Visible = isApproved2 && isOpen2 && formright.CanChangeOrderStatusToCancel;
				btnOpenOrders.Visible = isApproved2 && hasStatus2 && !isOpen2 && formright.CanChangeOrderStatusToOpen;
				if (isApproved2 && isOpen2)
				{
					if (formright.CanChangeOrderStatusToComplete)
					{
						GridEX_Helper.AddButton(grdfrm, "Complete", "Complete", 80);
					}
					if (formright.CanChangeOrderStatusToCancel)
					{
						GridEX_Helper.AddButton(grdfrm, "Cancel", "Cancel", 60);
					}
					if (formright.CanChangeOrderExpiryDate)
					{
						editableColumns.Add("ValidityDate", (EditType)2);
						GridEX_Helper.AddButton(grdfrm, "Update", "Update Expiry Date", 120);
					}
				}
				if (isApproved2 && hasStatus2 && !isOpen2 && formright.CanChangeOrderStatusToOpen)
				{
					GridEX_Helper.AddButton(grdfrm, "Open", "Open", 40);
				}
				if (isApproved2 && (formright.CanChangeOrderStatusToComplete || formright.CanChangeOrderStatusToCancel || formright.CanChangeOrderStatusToOpen))
				{
					GridEX_Helper.AddSelectButton(grdfrm, "Select", 40, 0);
					grdfrm.FrozenColumns = 1;
				}
			}
			else
			{
				btnCompleteOrders.Visible = false;
				btnCancelOrders.Visible = false;
				btnOpenOrders.Visible = false;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 3, 2, hiddenColumns, editableColumns);
			if ((type == "Detail" || type == "DocWise_Summary") && grdfrm.RootTable.Columns.Contains("NoOfAttachments"))
			{
				grdfrm.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			}
			if (grdfrm.RootTable.Columns.Contains("EBWeightDeductionTerm"))
			{
				grdfrm.RootTable.Columns["EBWeightDeductionTerm"].Caption = "EB Weight Deduction Term";
			}
			grdfrm.RootTable.Columns["ValidityDate"].Caption = "Expiry Date";
			grdfrm.RootTable.Columns["DeliveryToParty"].Caption = "Deliver To Party";
			grdfrm.RootTable.Columns["ShipToAddress"].Caption = "Deliver To Address";
			foreach (KeyValuePair<string, int> kvp in new Dictionary<string, int>
			{
				["DocNo"] = 60,
				["DocDate"] = 80,
				["CommissionAgentName"] = 140,
				["BuyerName"] = 140,
				["DeliveryToParty"] = 140,
				["ShipToAddress"] = 140,
				["ValidityDate"] = 80,
				["DeliveryStartDate"] = 80,
				["DeliveryDays"] = 60,
				["IsWhtApplied"] = 80,
				["IsbuyerOtherChargesAllowed"] = 80,
				["EBWeightDeductionTerm"] = 130,
				["Status"] = 70,
				["RemarksHeader"] = 140,
				["ItemName"] = 160,
				["ItemCode"] = 60,
				["ItemQty"] = 75,
				["GdnQty"] = 70,
				["BalanceQtyAgainstGdn"] = 80,
				["MappedQty"] = 70,
				["BalanceQtyAgainstMapping"] = 90,
				["ItemWeight"] = 80,
				["GdnGrossWeight"] = 80,
				["GdnEbUnit"] = 55,
				["GdnEbTotal"] = 70,
				["GdnAddLess"] = 55,
				["GdnWeight"] = 80,
				["BalanceWeightAgainstGdn"] = 80,
				["MappedWeight"] = 80,
				["BalanceWeightAgainstMapping"] = 100,
				["ItemRate"] = 70,
				["ItemAmount"] = 90,
				["Tax%"] = 55,
				["TaxAmount"] = 90,
				["TotalAmount"] = 100,
				["QualitySpecification"] = 130,
				["RemarksDetail"] = 140,
				["EntryDate"] = 80,
				["EntryUserName"] = 100,
				["ModifyDate"] = 80,
				["ModifyUserName"] = 100,
				["ApprovedDate"] = 80,
				["ApprovalUserName"] = 100,
				["IsApproved"] = 80,
				["NoOfAttachments"] = 80
			})
			{
				if (grdfrm.RootTable.Columns.Contains(kvp.Key))
				{
					grdfrm.RootTable.Columns[kvp.Key].Width = kvp.Value;
				}
			}
			if (type == "Detail" || type == "DocWise_Summary")
			{
				GridEX_Helper.AddButton(grdfrm, "Print", "Print", 40, 0);
				grdfrm.FrozenColumns = 1;
			}
			UpdateButtonColumns();
			ctrlGrdBar1_Load(null, null);
			if (type == "Detail")
			{
				bool isApproved = ((Control)(object)CmbIsApproved).Text == "Approved";
				bool isOpen = ((Control)(object)CmbStatus).Text == "Open";
				bool hasStatus = ((Control)(object)CmbStatus).Text != "";
				btnCompleteOrders.Visible = isApproved && isOpen && formright.CanChangeOrderStatusToComplete;
				btnCancelOrders.Visible = isApproved && isOpen && formright.CanChangeOrderStatusToCancel;
				btnOpenOrders.Visible = isApproved && hasStatus && !isOpen && formright.CanChangeOrderStatusToOpen;
				if (isApproved && isOpen)
				{
					if (formright.CanChangeOrderStatusToComplete)
					{
						grdfrm.RootTable.Columns["Complete"].Visible = true;
						btnCompleteOrders.Visible = true;
					}
					else
					{
						grdfrm.RootTable.Columns["Complete"].Visible = false;
						btnOpenOrders.Visible = false;
					}
					if (formright.CanChangeOrderStatusToCancel)
					{
						grdfrm.RootTable.Columns["Cancel"].Visible = true;
						btnCancelOrders.Visible = true;
					}
					else
					{
						grdfrm.RootTable.Columns["Cancel"].Visible = false;
						btnCancelOrders.Visible = false;
					}
					if (formright.CanChangeOrderExpiryDate)
					{
						grdfrm.RootTable.Columns["Update"].Visible = true;
					}
					else
					{
						grdfrm.RootTable.Columns["Update"].Visible = false;
					}
					if (grdfrm.RootTable.Columns.Contains("Open"))
					{
						grdfrm.RootTable.Columns["Open"].Visible = false;
						btnOpenOrders.Visible = false;
					}
				}
				if (isApproved && hasStatus && !isOpen && formright.CanChangeOrderStatusToOpen)
				{
					if (grdfrm.RootTable.Columns.Contains("Open"))
					{
						grdfrm.RootTable.Columns["Open"].Visible = true;
						grdfrm.RootTable.Columns["Open"].Width = 45;
						btnOpenOrders.Visible = true;
					}
					if (grdfrm.RootTable.Columns.Contains("Complete"))
					{
						grdfrm.RootTable.Columns["Complete"].Visible = false;
						btnCompleteOrders.Visible = false;
					}
					if (grdfrm.RootTable.Columns.Contains("Cancel"))
					{
						grdfrm.RootTable.Columns["Cancel"].Visible = false;
						btnCancelOrders.Visible = false;
					}
					if (grdfrm.RootTable.Columns.Contains("Update"))
					{
						grdfrm.RootTable.Columns["Update"].Visible = false;
					}
				}
			}
			else
			{
				btnCompleteOrders.Visible = false;
				btnCancelOrders.Visible = false;
				btnOpenOrders.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateButtonColumns()
	{
		if (grdfrm.RootTable.Columns.Contains("Select"))
		{
			grdfrm.RootTable.Columns["Select"].Width = 40;
		}
		if (grdfrm.RootTable.Columns.Contains("Complete"))
		{
			grdfrm.RootTable.Columns["Complete"].Width = 80;
		}
		if (grdfrm.RootTable.Columns.Contains("Cancel"))
		{
			grdfrm.RootTable.Columns["Cancel"].Width = 60;
		}
		if (grdfrm.RootTable.Columns.Contains("Open"))
		{
			grdfrm.RootTable.Columns["Open"].Width = 40;
		}
		if (grdfrm.RootTable.Columns.Contains("Update"))
		{
			grdfrm.RootTable.Columns["Update"].Width = 120;
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1053);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Print")
				{
					CrystalReportPrint_Helper.saleOrderSlip_1053(Id);
				}
				else if (e.Column.Key == "Complete")
				{
					UpdateOrderStatus(Id, "Complete", grdfrm);
				}
				else if (e.Column.Key == "Open")
				{
					UpdateOrderStatus(Id, "Open", grdfrm);
				}
				else if (e.Column.Key == "Cancel")
				{
					UpdateOrderStatus(Id, "Cancel", grdfrm);
				}
				else if (e.Column.Key == "Update")
				{
					UpdateOrderStatus(Id, "Expiry", grdfrm);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateOrderStatus(int id, string reqType, GridEX grid)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0125: Expected O, but got Unknown
		try
		{
			GridEXRow row = grid.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				RemarksPopUp remarksPopUp = new RemarksPopUp();
				remarksPopUp.StartPosition = FormStartPosition.CenterScreen;
				remarksPopUp.BringToFront();
				remarksPopUp.ShowDialog();
				string Remarks = remarksPopUp.txtRemarks.Text;
				if (Remarks == null || Remarks == string.Empty)
				{
					MessageBox.Show("Action Remarks Required");
				}
				else if (MessageBox.Show("Are you sure to " + reqType + " Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					saleOrderMaster.SaleOrderHeader_UpdateStatus(new ApprovalList
					{
						AllApprovalLists = new List<ApprovalList>
						{
							new ApprovalList
							{
								OrganizationId = UserAccount.OrganizationId,
								CompanyId = UserAccount.CompanyId,
								DocumentTypeId = 1053,
								Id = id,
								ReqType = reqType,
								EntryUser = UserAccount.ID,
								PostDate = ((reqType == "Expiry") ? Conversion.ToDateTime(row.Cells["ValidityDate"].Value) : DateTime.Now),
								Comments = Remarks
							}
						}
					});
					reqType = ((reqType == "Expiry") ? "Expiry Date Updated" : reqType);
					MessageBox.Show("Record " + reqType + " Successfully");
					GridBind();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateMultiOrderStatus(string reqType)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		try
		{
			if (!grdfrm.GetCheckedRows().Cast<GridEXRow>().Any())
			{
				MessageBox.Show("Please Check Rows first");
				return;
			}
			RemarksPopUp popUp = new RemarksPopUp();
			popUp.StartPosition = FormStartPosition.CenterScreen;
			popUp.ShowDialog();
			string remarks = popUp.txtRemarks.Text;
			if (string.IsNullOrEmpty(remarks))
			{
				MessageBox.Show("Action Remarks Required");
			}
			else if (MessageBox.Show("Are you sure to " + reqType + " Status?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				saleOrderMaster.SaleOrderHeader_UpdateStatus(new ApprovalList
				{
					AllApprovalLists = grdfrm.GetCheckedRows().Cast<GridEXRow>().Select((Func<GridEXRow, ApprovalList>)((GridEXRow r) => new ApprovalList
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 1053,
						EntryUser = UserAccount.ID,
						Id = Conversion.ToInt(r.Cells["Id"].Value),
						PostDate = DateTime.Now,
						ReqType = reqType,
						Comments = remarks
					}))
						.ToList()
				});
				MessageBox.Show("Record " + reqType + " Successfully");
				GridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)CmbParentItem).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)CmbStatus).Text = string.Empty;
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			((Control)(object)CmbBuyer).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
			grdfrm.ClearStructure();
			txtDateFrom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleOrderCmagtReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdfrm, dtGrid);
			}
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
			tsDrowdownSaleCmgt.Visible = false;
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrint.Visible = true;
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Detail")
				{
					tsDrowdownSaleCmgt.Visible = true;
					btnPrint.Text = "1053_01_SaleOrderDetailReport";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "DocWise_Summary")
				{
					btnPrint.Text = "1053_02_SaleOrderDocWise_SummaryReport";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "ItemCommissionAndBuyerWise_Summary")
				{
					btnPrint.Text = "1053_03_SaleOrderItemCommissionAndBuyerWise_SummaryReport";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "ItemWise_Summary")
				{
					btnPrint.Text = "1053_04_SaleOrderItemWise_SummaryReport";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridFromDb.Rows.Count == 0 || dtGridFromDb == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGridFromDb, btnPrint.Text + ".rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleOrderCmagtReport_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtDateFrom.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Main Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Date Type");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
	{
		MakeShortCutKeys();
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdfrm.CurrentColumn != null)
			{
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
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
		try
		{
			UpdateMultiOrderStatus("Open");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCompleteOrders_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateMultiOrderStatus("Complete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelOrders_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateMultiOrderStatus("Cancel");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_FormattingRow(object sender, RowLoadEventArgs e)
	{
	}

	private void tsDrowdownSaleCmgt_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridFromDb.Rows.Count == 0 || dtGridFromDb == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGridFromDb, e.ClickedItem.Text + ".rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			string[] files = CommonServices.DynamicReportsLoad("CommissionAgentSaleOrder");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDrowdownSaleCmgt.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
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
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Expected O, but got Unknown
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.Reports.frmSaleOrderCmagtReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbShipToParty = new UltraCombo();
		this.CmbDeliveryToParty = new UltraCombo();
		this.CmbBuyer = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.CmbIsApproved = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.CmbCommissionAgent = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbParentItem = new UltraCombo();
		this.cmbCategory = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tsDrowdownSaleCmgt = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnOpenOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCompleteOrders = new System.Windows.Forms.ToolStripButton();
		this.btnCancelOrders = new System.Windows.Forms.ToolStripButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1284, 561);
		this.panel1.TabIndex = 0;
		grdfrm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><CellLayoutMode>UseColumnSets</CellLayoutMode><ColumnSetRowCount>1</ColumnSetRowCount><GroupCondition /><HeaderLines>2</HeaderLines></RootTable></GridEXLayoutData>";
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 171);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1284, 390);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.FormattingRow += new RowLoadEventHandler(grdfrm_FormattingRow);
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 148);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1284, 23);
		this.panel4.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1239, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.rdmodifydate);
		this.panel3.Controls.Add(this.rdentrydate);
		this.panel3.Controls.Add(this.drdocdate);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Controls.Add(this.txtOrderNoTo);
		this.panel3.Controls.Add(this.txtOrderNoFrom);
		this.panel3.Controls.Add(this.label46);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShipToParty);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyer);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.label30);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtDateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1284, 121);
		this.panel3.TabIndex = 0;
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(80, 36);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(115, 22);
		this.txtDateFrom.TabIndex = 0;
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(196, 64);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(115, 23);
		this.txtOrderNoTo.TabIndex = 605;
		this.txtOrderNoTo.Tag = "int";
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(80, 64);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(115, 23);
		this.txtOrderNoFrom.TabIndex = 604;
		this.txtOrderNoFrom.Tag = "int";
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(5, 60);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(51, 30);
		this.label46.TabIndex = 606;
		this.label46.Text = "Doc No\r\nFrom,To";
		this.CmbShipToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbShipToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShipToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShipToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShipToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShipToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShipToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShipToParty).Location = new System.Drawing.Point(707, 90);
		((System.Windows.Forms.Control)(object)this.CmbShipToParty).Name = "CmbShipToParty";
		((System.Windows.Forms.Control)(object)this.CmbShipToParty).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbShipToParty).TabIndex = 8;
		this.CmbDeliveryToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Location = new System.Drawing.Point(707, 62);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Name = "CmbDeliveryToParty";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).TabIndex = 7;
		this.CmbBuyer.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyer).Location = new System.Drawing.Point(707, 34);
		((System.Windows.Forms.Control)(object)this.CmbBuyer).Name = "CmbBuyer";
		((System.Windows.Forms.Control)(object)this.CmbBuyer).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyer).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(604, 96);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(105, 15);
		this.label6.TabIndex = 523;
		this.label6.Text = "Deliver To Address\r\n";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(604, 68);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(90, 15);
		this.label5.TabIndex = 521;
		this.label5.Text = "Deliver To Party";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(981, 34);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 9;
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(981, 90);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 11;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label4.Location = new System.Drawing.Point(907, 88);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(73, 30);
		this.label4.TabIndex = 519;
		this.label4.Text = "Approval      \r\nStatus        ";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(604, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 517;
		this.label2.Text = "Buyer Name";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(981, 62);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 10;
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(403, 90);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 5;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(315, 88);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 30);
		this.label14.TabIndex = 515;
		this.label14.Text = "Commission\r\nAgent";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(907, 68);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(40, 15);
		this.label30.TabIndex = 221;
		this.label30.Text = "Status";
		this.CmbParentItem.AllowNull = (DefaultableBoolean)1;
		this.CmbParentItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Location = new System.Drawing.Point(80, 90);
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Name = "CmbParentItem";
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbParentItem).Leave += new System.EventHandler(CmbParentItem_Leave);
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(403, 34);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 3;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1284, 31);
		this.panel5.TabIndex = 109;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(156, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Sale Order Report";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(907, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "Report Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(315, 68);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 99;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(403, 62);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 96);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 96;
		this.label7.Text = "Parent Item";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(315, 40);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Item Category";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 40);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date From,To";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1126, 90);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(196, 36);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(115, 22);
		this.txtDateTo.TabIndex = 1;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1284, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNewHistory, this.btnRefresh, this.btnPrint, this.tsDrowdownSaleCmgt, this.btnshortcutkeys, this.btnOpenOrders, this.btnCompleteOrders, this.btnCancelOrders });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1284, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tsDrowdownSaleCmgt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDrowdownSaleCmgt.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDrowdownSaleCmgt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDrowdownSaleCmgt.Name = "tsDrowdownSaleCmgt";
		this.tsDrowdownSaleCmgt.Size = new System.Drawing.Size(70, 24);
		this.tsDrowdownSaleCmgt.Text = "Print";
		this.tsDrowdownSaleCmgt.Visible = false;
		this.tsDrowdownSaleCmgt.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDrowdownSaleCmgt_DropDownItemClicked);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnOpenOrders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnOpenOrders.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOpenOrders.Image = (System.Drawing.Image)resources.GetObject("btnOpenOrders.Image");
		this.btnOpenOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOpenOrders.Name = "btnOpenOrders";
		this.btnOpenOrders.Size = new System.Drawing.Size(45, 24);
		this.btnOpenOrders.Text = "Open";
		this.btnOpenOrders.Visible = false;
		this.btnOpenOrders.Click += new System.EventHandler(btnOpenOrders_Click);
		this.btnCompleteOrders.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCompleteOrders.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnCompleteOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCompleteOrders.Name = "btnCompleteOrders";
		this.btnCompleteOrders.Size = new System.Drawing.Size(92, 24);
		this.btnCompleteOrders.Text = "Complete";
		this.btnCompleteOrders.Visible = false;
		this.btnCompleteOrders.Click += new System.EventHandler(btnCompleteOrders_Click);
		this.btnCancelOrders.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelOrders.Image = Architecture.WinApp.Properties.Resources.close_line;
		this.btnCancelOrders.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCancelOrders.Name = "btnCancelOrders";
		this.btnCancelOrders.Size = new System.Drawing.Size(75, 24);
		this.btnCancelOrders.Text = "Cancel";
		this.btnCancelOrders.Visible = false;
		this.btnCancelOrders.Click += new System.EventHandler(btnCancelOrders_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1186, 69);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 609;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1186, 52);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 608;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1186, 35);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 607;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 561);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSaleOrderCmagtReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleOrderCmagtReport_FormClosing);
		base.Load += new System.EventHandler(frmStockReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderCmagtReport_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
