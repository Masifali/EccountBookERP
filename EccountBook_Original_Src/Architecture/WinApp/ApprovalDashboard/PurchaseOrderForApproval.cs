using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PurchaseOrderForApproval : Form
{
	public DataTable grdlst = new DataTable();

	public DataTable dtPending = new DataTable();

	private DataTable dtBranch = new DataTable();

	public int BranchesId;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private bool BranchImplemented;

	private bool AllowEditOnVoucherNoinReports;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label2;

	private Label label1;

	internal UltraCombo CmbItem;

	internal UltraCombo CmbSupplier;

	private Label label3;

	private ToolStripButton btnPurchsaeOrderDetail;

	private TextBox txtSrTo;

	private TextBox txtSrFrom;

	private Label label4;

	private Label label6;

	private ToolStripButton toolStripButton2;

	internal UltraCombo CmbApproved;

	internal UltraCombo CmbStatus;

	private CtrlGrdBar ctrlGrdBar3;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX grdPendingViewVoucher;

	private ToolStripButton btnApprovedOrder;

	private ToolStripButton btnPendingForView;

	internal GridEX GridDetail;

	private Panel panel7;

	private Label label7;

	private Panel panel8;

	internal GridEX GridDetailForPendingView;

	private Label label5;

	private Button btnshow;

	private ToolStripButton BtnCancel;

	private ToolStripButton Btnrefresh;

	public PurchaseOrderForApproval(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchaseOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseOrderBranchWise"));
			if (BranchImplemented)
			{
				UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			AllowEditOnVoucherNoinReports = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowEditOnVoucherNoinReports"));
			AllDropDownBind();
			ComboStatusFill();
			ComboApprovedfill();
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			((UltraGridBase)CmbApproved).Rows[1].Activate();
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				fromdate.Value = Convert.ToDateTime(RequestedFromDate);
				todate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				todate.Value = DateTime.Today;
			}
			fromdate.Focus();
			GetDataForApproval();
			GetPendingPurchaseOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			DataTable dtAll = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "41",
				BranchesIds = (BranchImplemented ? UserAccount.BranchesId.ToString() : "")
			});
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Item"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDL(dtParty, CmbSupplier, "Id", "ReferenceName", "Supplier Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dtItem, CmbItem, "Id", "ReferenceName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDL(dt, CmbStatus, "Id", "Name", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboApprovedfill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Approved");
			dt.Rows.Add(2, "Not Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbApproved, "Id", "Name", "Status", ZeroIndex: false);
			((UltraGridBase)CmbApproved).Rows[0].Activate();
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
			GetDataForApproval();
			GetPendingPurchaseOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataForApproval()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			obj.BranchesIds = (BranchImplemented ? UserAccount.BranchesId.ToString() : "");
			if (((Control)(object)CmbApproved).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			obj.ReqType = "False";
			obj.ActionTypeId = false;
			grdlst = PurchaseOrder.PurchaseOrderHistory(obj);
			DataTable dataTable = grdlst;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId");
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("BranchSrNo", typeof(int));
				dt.Columns.Add("BranchName", typeof(string));
				dt.Columns.Add("OrderSupCustId");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("CommissionAgent");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("AvgRate40Kg", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("NoOfAttachments");
				dt.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				var TotalsValuesById = (from row in grdlst.AsEnumerable()
					group row by row.Field<int>("Id")).ToDictionary((IGrouping<int, DataRow> group) => group.Key, (IGrouping<int, DataRow> group) => new
				{
					Amount = group.Sum((DataRow row) => row.Field<double>("Amount")),
					OrderItemQty = group.Sum((DataRow row) => row.Field<double>("OrderItemQty")),
					NetWeight = group.Sum((DataRow row) => row.Field<double>("NetWeight"))
				});
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					int id = Conversion.ToInt(grdlst.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						var obj2 = (TotalsValuesById.ContainsKey(id) ? TotalsValuesById[id] : new
						{
							Amount = 0.0,
							OrderItemQty = 0.0,
							NetWeight = 0.0
						});
						double totalAmount = obj2.Amount;
						double totalQty = obj2.OrderItemQty;
						double totalWeight = obj2.NetWeight;
						double AvgRate40kg = totalAmount / totalWeight * 40.0;
						dt.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["DocumentTypeId"], grdlst.Rows[i]["DocNo"], grdlst.Rows[i]["DocDate"], grdlst.Rows[i]["BranchSrNo"], grdlst.Rows[i]["BranchName"], grdlst.Rows[i]["OrderSupCustId"], grdlst.Rows[i]["SupplierName"], grdlst.Rows[i]["CompanyNameSpCAgent"], grdlst.Rows[i]["DeliveryTerm"], grdlst.Rows[i]["OrderExpiryDate"], totalQty, totalWeight, AvgRate40kg, totalAmount, grdlst.Rows[i]["EntryDate"], grdlst.Rows[i]["UserNameEusr"], grdlst.Rows[i]["ModifyDate"], grdlst.Rows[i]["UserNameMusr"], grdlst.Rows[i]["NoOfAttachments"]);
						existingIds.Add(id);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grd.DataSource = null;
				GridDetail.DataSource = null;
			}
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["OrderSupCustId"].Visible = false;
			grd.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			grd.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2);
			grd.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["OrderExpiryDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grd.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grd.RootTable.Columns["OrderExpiryDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grd.RootTable.Columns["OrderQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["OrderWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AvgRate40Kg"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			if (AllowEditOnVoucherNoinReports)
			{
				grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			}
			grd.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(grd, "Slip", "Slip203", 60, 0);
			GridEX_Helper.AddButton(grd, "Detail", "Detail", 60, 1);
			if (((Control)(object)CmbApproved).Text == "Approved" && ((Control)(object)CmbStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					GridEX_Helper.AddButton(grd, "Complete", "Complete", 80, 1);
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					GridEX_Helper.AddButton(grd, "Cancel", "Cancel", 60, 2);
				}
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved" && ((Control)(object)CmbStatus).Text == "Open")
			{
				GridEX_Helper.AddSelectButton(grd);
				GridEX_Helper.AddButton(grd, "IsApproved", "Approve", 60, 1);
				GridEX_Helper.AddButton(grd, "ApprovedAndCancel", "Approved & Cancel", 120, 1);
			}
			if (grd.RootTable.Columns.Contains("Select"))
			{
				grd.RootTable.Columns["Select"].Position = grd.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grd.RootTable.Columns.Contains("Slip"))
			{
				grd.RootTable.Columns["Slip"].Position = grd.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grd.RootTable.Columns.Contains("Detail"))
			{
				grd.RootTable.Columns["Detail"].Position = grd.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grd.RootTable.Columns.Contains("IsApproved"))
			{
				grd.RootTable.Columns["IsApproved"].Position = grd.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grd.RootTable.Columns.Contains("ApprovedAndCancel"))
			{
				grd.RootTable.Columns["ApprovedAndCancel"].Position = grd.RootTable.Columns["DocNo"].Position - 1;
			}
			grd.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
			}
			if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["OrderSupCustId"].Value), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick_1(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grd.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (e.Column.Key == "Detail")
			{
				new ActivityDetails(UserAccount, Id).Show();
			}
			else if (e.Column.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			if (e.Column.Key == "Complete")
			{
				ApproveOrder(Id, "Status");
			}
			if (e.Column.Key == "Cancel")
			{
				ApproveOrder(Id, "Cancel");
			}
			if (e.Column.Key == "ApprovedAndCancel")
			{
				ApproveOrder(Id, "ApproveAndCancel");
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
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (grd.CurrentColumn.Key == "DocNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
				}
			}
			else if (grd.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["OrderSupCustId"].Value), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "ItemName")
			{
				Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, todate.Value);
			}
			else if (grd.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grd.CurrentColumn.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (grd.CurrentColumn.Key == "Detail")
			{
				new ActivityDetails(UserAccount, Id).Show();
			}
			else if (grd.CurrentColumn.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			else if (grd.CurrentColumn.Key == "Complete")
			{
				ApproveOrder(Id, "Status");
			}
			else if (grd.CurrentColumn.Key == "Cancel")
			{
				ApproveOrder(Id, "Cancel");
			}
			else if (grd.CurrentColumn.Key == "ApprovedAndCancel")
			{
				ApproveOrder(Id, "ApproveAndCancel");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseGLAC");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("PPBagPrice");
				dt.Columns.Add("JuteBagPrice");
				dt.Columns.Add("PPBagWeight");
				dt.Columns.Add("JuteBagWeight");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("LabSampleId");
				dt.Columns.Add("LabSampleNo");
				dt.Columns.Add("CityArea");
				DataTable resultRows = (from row in grdlst.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["Crop"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["PPBagRate"], resultRows.Rows[i]["JuteBagRate"], resultRows.Rows[i]["PPBagWeight"], resultRows.Rows[i]["JuteBagWeight"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["InvLabSampleAnalysisHeaderId"], resultRows.Rows[i]["LabSampleNo"], resultRows.Rows[i]["CityArea"]);
				}
				GridDetail.DataSource = dt;
				GridDetail.RetrieveStructure();
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
			GridDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridDetail.RootTable.Columns["LabSampleId"].Visible = false;
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 2);
			GridDetail.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GridDetail.RootTable.Columns["LabSampleNo"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridDetail.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, todate.Value);
			}
			if (e.Column.Key == "LabSampleNo")
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = Conversion.ToInt(item.Cells["LabSampleId"].Value);
				DataTable dt = new DataTable();
				dt = InvLabSampleAnalysisHeader.GetPrintSlipAndReport(obj);
				if (dt.Rows.Count == 0 || dt == null)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvLabSampleAnalysisHeader.GetDataForLabSampleAnalysisSubReport(obj.Id), "LabSampleAnalysisSubParameterReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "657-RptInvLabSampleAnalysisSlipA.rpt", lstSubReports);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null && GridDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetPendingPurchaseOrder()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesIds = (BranchImplemented ? UserAccount.BranchesId.ToString() : "");
			obj.DocumentTypeId = 41;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			obj.IsApproved = false;
			obj.ReqType = "True";
			obj.ActionTypeId = true;
			dtPending = PurchaseOrder.PurchaseOrderHistory(obj);
			if (dtPending.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("CommissionAgent");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("AvgRate40Kg", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				int i;
				for (i = 0; i < dtPending.Rows.Count; i++)
				{
					bool num = dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"]));
					double totalAmount = (from row in grdlst.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("Amount"));
					double totalQty = (from row in dtPending.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("OrderItemQty"));
					double totalWeight = (from row in dtPending.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(dtPending.Rows[i]["Id"])
						select row).Sum((DataRow row) => row.Field<double>("NetWeight"));
					double AvgRate40kg = totalAmount / totalWeight * 40.0;
					if (!num)
					{
						dt.Rows.Add(dtPending.Rows[i]["Id"], dtPending.Rows[i]["DocumentTypeId"], dtPending.Rows[i]["DocNo"], dtPending.Rows[i]["DocDate"], dtPending.Rows[i]["OrderSupCustId"], dtPending.Rows[i]["SupplierName"], dtPending.Rows[i]["CompanyNameSpCAgent"], dtPending.Rows[i]["DeliveryTerm"], dtPending.Rows[i]["OrderExpiryDate"], totalQty, totalWeight, AvgRate40kg, totalAmount, dtPending.Rows[i]["EntryDate"], dtPending.Rows[i]["UserNameEusr"], dtPending.Rows[i]["ModifyDate"], dtPending.Rows[i]["UserNameMusr"], dtPending.Rows[i]["NoOfAttachments"]);
					}
				}
				grdPendingViewVoucher.DataSource = dt;
				grdPendingViewVoucher.RetrieveStructure();
				GridSettingsForPending();
			}
			else
			{
				grdPendingViewVoucher.DataSource = null;
				GridDetailForPendingView.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingsForPending()
	{
		try
		{
			grdPendingViewVoucher.RootTable.Columns["Id"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdPendingViewVoucher, 3, 2);
			grdPendingViewVoucher.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdPendingViewVoucher.RootTable.Columns["OrderExpiryDate"].FormatString = "dd-MMM-yyyy";
			grdPendingViewVoucher.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdPendingViewVoucher.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			if (AllowEditOnVoucherNoinReports)
			{
				grdPendingViewVoucher.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			}
			grdPendingViewVoucher.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdPendingViewVoucher.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdPendingViewVoucher.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdPendingViewVoucher.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdPendingViewVoucher.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdPendingViewVoucher.RootTable.Columns["OrderExpiryDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdPendingViewVoucher.RootTable.Columns["OrderQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdPendingViewVoucher.RootTable.Columns["OrderWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdPendingViewVoucher.RootTable.Columns["AvgRate40Kg"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPendingViewVoucher.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPendingViewVoucher.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdPendingViewVoucher.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdPendingViewVoucher.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdPendingViewVoucher.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GridEX_Helper.AddButton(grd, "Slip", "Slip203", 60, 0);
			GridEX_Helper.AddButton(grd, "Detail", "Detail", 60, 1);
			if (((Control)(object)CmbApproved).Text == "Approved" && ((Control)(object)CmbStatus).Text == "Open")
			{
				if (formright.CanChangeOrderStatusToComplete)
				{
					GridEX_Helper.AddButton(grd, "Complete", "Complete", 80, 1);
				}
				if (formright.CanChangeOrderStatusToCancel)
				{
					GridEX_Helper.AddButton(grd, "Cancel", "Cancel", 60, 2);
				}
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved" && ((Control)(object)CmbStatus).Text == "Open")
			{
				GridEX_Helper.AddSelectButton(grd);
				GridEX_Helper.AddButton(grd, "IsApproved", "Approve", 60, 1);
				GridEX_Helper.AddButton(grd, "ApprovedAndCancel", "Approve & Cancel", 120, 1);
			}
			if (grdPendingViewVoucher.RootTable.Columns.Contains("Select"))
			{
				grdPendingViewVoucher.RootTable.Columns["Select"].Position = grdPendingViewVoucher.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdPendingViewVoucher.RootTable.Columns.Contains("Slip"))
			{
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = grdPendingViewVoucher.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdPendingViewVoucher.RootTable.Columns.Contains("Detail"))
			{
				grdPendingViewVoucher.RootTable.Columns["Detail"].Position = grdPendingViewVoucher.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdPendingViewVoucher.RootTable.Columns.Contains("IsApproved"))
			{
				grdPendingViewVoucher.RootTable.Columns["IsApproved"].Position = grdPendingViewVoucher.RootTable.Columns["DocNo"].Position - 1;
			}
			if (grdPendingViewVoucher.RootTable.Columns.Contains("ApprovedAndCancel"))
			{
				grdPendingViewVoucher.RootTable.Columns["ApprovedAndCancel"].Position = grdPendingViewVoucher.RootTable.Columns["DocNo"].Position - 1;
			}
			grdPendingViewVoucher.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grdPendingViewVoucher.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (e.Column.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			if (e.Column.Key == "Complete")
			{
				ApproveOrder(Id, "Status");
			}
			if (e.Column.Key == "Cancel")
			{
				ApproveOrder(Id, "Cancel");
			}
			if (e.Column.Key == "ApprovedAndCancel")
			{
				ApproveOrder(Id, "ApproveAndCancel");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
			}
			else if (e.Column.Key == "NoOfAttachments")
			{
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
			else if (e.Column.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["SupplierCustomerId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingViewVoucher.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingViewVoucher.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (grdPendingViewVoucher.CurrentColumn.Key == "DocNo")
			{
				if (AllowEditOnVoucherNoinReports && clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					CommonServices.EditMethodFromLinked(DocumentTypeId, Id);
				}
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Slip")
			{
				GenerateReport(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "SupplierName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["SupplierCustomerId"].Value), fromdate.Value, todate.Value);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "IsApproved")
			{
				ApproveOrder(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Complete")
			{
				ApproveOrder(Id, "Status");
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Cancel")
			{
				ApproveOrder(Id, "Cancel");
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "ApprovedAndCancel")
			{
				ApproveOrder(Id, "ApproveAndCancel");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingViewVoucher_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseGLAC");
				dt.Columns.Add("ItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("PPBagPrice");
				dt.Columns.Add("JuteBagPrice");
				dt.Columns.Add("PPBagWeight");
				dt.Columns.Add("JuteBagWeight");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CityArea");
				DataTable resultRows = (from row in dtPending.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["Crop"], resultRows.Rows[i]["UOMCodeItm"], resultRows.Rows[i]["OrderItemQty"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["OrderItemRate"], resultRows.Rows[i]["UOMCodeRate"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["PPBagRate"], resultRows.Rows[i]["JuteBagRate"], resultRows.Rows[i]["PPBagWeight"], resultRows.Rows[i]["JuteBagWeight"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityArea"]);
				}
				GridDetailForPendingView.DataSource = dt;
				GridDetailForPendingView.RetrieveStructure();
				GridDetailSettingForPendingRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettingForPendingRecord()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridDetailForPendingView.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GridDetailForPendingView.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			GridDetailForPendingView.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GridDetailForPendingView.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			GridDetailForPendingView.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GridDetailForPendingView.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GridDetailForPendingView.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetailForPendingView.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GridDetailForPendingView.RootTable.Columns["PPBagPrice"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetailForPendingView.RootTable.Columns["JuteBagPrice"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetailForPendingView.RootTable.Columns["PPBagWeight"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetailForPendingView.RootTable.Columns["JuteBagWeight"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetailForPendingView.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			GridDetailForPendingView.RootTable.Columns["CityArea"].Width = Constants.InventoryConstants.CityName;
			GridDetailForPendingView.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemRate"].FormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetailForPendingView.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetailForPendingView.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null && GridDetailForPendingView.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(r.Cells["ItemId"].Value), fromdate.Value, todate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void reset()
	{
		((Control)(object)CmbSupplier).Text = string.Empty;
		((Control)(object)CmbItem).Text = string.Empty;
		txtSrFrom.Clear();
		txtSrTo.Clear();
		fromdate.Focus();
	}

	private void Btnrefresh_Click(object sender, EventArgs e)
	{
		AllDropDownBind();
	}

	public void ShowReport()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			if (fromdate.Value.ToString() != string.Empty && todate.Value.ToString() != string.Empty)
			{
				Reporting rpt = new Reporting();
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@OrderId", grd.CurrentRow.Cells[0].Value.ToString());
				rpt.RptPerameter("@StartOrderDate", (string)null);
				rpt.RptPerameter("@EndOrderDate", (string)null);
				if (txtSrFrom.Text.Trim().ToString() == string.Empty)
				{
					rpt.RptPerameter("@PoSrFrom", (string)null);
				}
				else
				{
					rpt.RptPerameter("@PoSrFrom", txtSrFrom.Text.Trim().ToString());
				}
				if (txtSrTo.Text.Trim().ToString() == string.Empty)
				{
					rpt.RptPerameter("@PoSrTo", (string)null);
				}
				else
				{
					rpt.RptPerameter("@PoSrTo", txtSrTo.Text.Trim().ToString());
				}
				if (((Control)(object)CmbSupplier).Text.ToString() == string.Empty)
				{
					rpt.RptPerameter("@SupplierCustomerId", (string)null);
				}
				else
				{
					rpt.RptPerameter("@SupplierCustomerId", CmbSupplier.Value.ToString());
				}
				if (((Control)(object)CmbItem).Text.ToString() == string.Empty)
				{
					rpt.RptPerameter("@ItemId", (string)null);
				}
				else
				{
					rpt.RptPerameter("@ItemId", CmbItem.Value.ToString());
				}
				rpt.RptPerameter("@IsApproved", "false");
				rpt.RptPerameter("@Status", (string)null);
				rpt.RptPerameter("@PurchaseOrderId", grd.CurrentRow.Cells[0].Value.ToString());
				rpt.ShowReport("RptPurchaseOrderSlipWithReceivingDetail.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int Id)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			if (((Control)(object)CmbApproved).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "206-RptPurchaseOrderRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			obj.FromDate = fromdate.Value;
			obj.ToDate = todate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtSrFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtSrTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			obj.Status = ((Control)(object)CmbStatus).Text.Trim();
			if (((Control)(object)CmbApproved).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			DataTable dt = PurchaseOrderReports.PurchaseOrderDetailRegisterReport206(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "205-InvRptPurchaseOrderDetailRegisterRice.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveOrder(int Id, string RequesType = "Approve")
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to " + RequesType + " Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.Id = Id;
				item.ReqType = RequesType;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(val);
				MessageBox.Show("Record " + RequesType + " Successfully");
				GetDataForApproval();
				GetPendingPurchaseOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovedOrder_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.Id = Conversion.ToInt(r.Cells[0].Value);
						item.ReqType = "Approve";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
					MessageBox.Show("Record Approve Successfully");
					GetDataForApproval();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved" && ((Control)(object)CmbStatus).Text == "Open" && MessageBox.Show("Are you sure to Approve & cancel Purchase Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					item.ReqType = "Cancel";
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
				MessageBox.Show("Record Approve & Cancel Successfully");
				GetDataForApproval();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbApproved).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Move PurchaseOrder in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grd.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.Id = Conversion.ToInt(r.Cells[0].Value);
						item.ReqType = "PendingForView";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					PurchaseOrder.UpdateStatusandIsApprovedbyOrderId(obj);
					MessageBox.Show("Record Updated Successfully");
					GetDataForApproval();
					GetPendingPurchaseOrder();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
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

	private void PurchaseOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			tabControl1.SelectedIndex = 0;
			fromdate.Focus();
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			print_Click_1(null, null);
		}
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
			GetDataForApproval();
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			btnApprovedOrder_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.B)
		{
			btnPendingForView_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				tabControl1.SelectedIndex = 1;
			}
			else
			{
				tabControl1.SelectedIndex = 0;
			}
		}
		if (!e.Control || e.KeyCode != Keys.Down)
		{
			return;
		}
		if (tabControl1.SelectedIndex == 0)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
			{
				((Control)(object)GridDetail).Focus();
			}
			else
			{
				((Control)(object)grd).Focus();
			}
		}
		else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGridPendingView")
		{
			((Control)(object)GridDetailForPendingView).Focus();
		}
		else
		{
			((Control)(object)grdPendingViewVoucher).Focus();
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
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_0ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Expected O, but got Unknown
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e06: Expected O, but got Unknown
		//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f45: Expected O, but got Unknown
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PurchaseOrderForApproval));
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GridDetailForPendingView = new GridEX();
		this.grdPendingViewVoucher = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbApproved = new UltraCombo();
		this.CmbStatus = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.txtSrTo = new System.Windows.Forms.TextBox();
		this.txtSrFrom = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.CmbSupplier = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnPurchsaeOrderDetail = new System.Windows.Forms.ToolStripButton();
		this.btnApprovedOrder = new System.Windows.Forms.ToolStripButton();
		this.btnPendingForView = new System.Windows.Forms.ToolStripButton();
		this.BtnCancel = new System.Windows.Forms.ToolStripButton();
		this.Btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1138, 35);
		this.panel3.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(144, 24);
		this.label23.TabIndex = 1;
		this.label23.Text = "Purchase Order";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1124, 284);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).Tag = "MasterGrid";
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick_1);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 64);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1138, 685);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 65);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1138, 620);
		this.panel6.TabIndex = 110;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1138, 620);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.ctrlGrdBar3);
		this.tabPage1.Controls.Add(this.label7);
		this.tabPage1.Location = new System.Drawing.Point(4, 23);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1130, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Po_History";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 314);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1124, 215);
		this.panel8.TabIndex = 23;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1124, 215);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 2;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1124, 284);
		this.panel7.TabIndex = 22;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1086, 5);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Location = new System.Drawing.Point(3, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(1124, 27);
		this.label7.TabIndex = 1;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.tabPage2.Location = new System.Drawing.Point(4, 23);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1130, 593);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pending Order For View";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GridDetailForPendingView.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailForPendingView).BackColor = System.Drawing.Color.White;
		this.GridDetailForPendingView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetailForPendingView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailForPendingView.DesignTimeLayout = GridDetailForPendingView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailForPendingView.FilterMode = (FilterMode)2;
		this.GridDetailForPendingView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailForPendingView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Location = new System.Drawing.Point(3, 329);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Name = "GridDetailForPendingView";
		((ScrollableBase)this.GridDetailForPendingView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(1124, 261);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 2;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingViewVoucher).BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(1124, 326);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Tag = "MasterGridPendingView";
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApproved);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtSrTo);
		this.panel5.Controls.Add(this.txtSrFrom);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1138, 65);
		this.panel5.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(192, 40);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 17;
		this.label5.Text = "Po Sr To";
		this.CmbApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApproved).Location = new System.Drawing.Point(732, 34);
		((System.Windows.Forms.Control)(object)this.CmbApproved).Name = "CmbApproved";
		((System.Windows.Forms.Control)(object)this.CmbApproved).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbApproved).TabIndex = 16;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(732, 5);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 14;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(882, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(663, 40);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(68, 15);
		this.label6.TabIndex = 15;
		this.label6.Text = "IsApproved";
		this.txtSrTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrTo.Location = new System.Drawing.Point(254, 36);
		this.txtSrTo.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrTo.Name = "txtSrTo";
		this.txtSrTo.Size = new System.Drawing.Size(100, 23);
		this.txtSrTo.TabIndex = 8;
		this.txtSrTo.Tag = "int";
		this.txtSrFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSrFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSrFrom.Location = new System.Drawing.Point(254, 7);
		this.txtSrFrom.Margin = new System.Windows.Forms.Padding(2);
		this.txtSrFrom.Name = "txtSrFrom";
		this.txtSrFrom.Size = new System.Drawing.Size(100, 23);
		this.txtSrFrom.TabIndex = 7;
		this.txtSrFrom.Tag = "int";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(448, 34);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 12;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(448, 5);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(358, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 11;
		this.label2.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(191, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(66, 15);
		this.label4.TabIndex = 6;
		this.label4.Text = "Po Sr From";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(358, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 9;
		this.label1.Text = "Supplier Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(663, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 15);
		this.label3.TabIndex = 13;
		this.label3.Text = "Status";
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(68, 36);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(118, 23);
		this.todate.TabIndex = 5;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(68, 7);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(118, 23);
		this.fromdate.TabIndex = 3;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 11);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 2;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(5, 40);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 4;
		this.label33.Text = "To Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1138, 749);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1138, 29);
		this.panel2.TabIndex = 3;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.Btnrefresh, this.toolStripButton2, this.btnPurchsaeOrderDetail, this.BtnCancel, this.btnApprovedOrder, this.btnPendingForView });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1138, 27);
		this.toolStrip3.TabIndex = 0;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(168, 24);
		this.toolStripButton2.Text = "205-PoDetailRegister";
		this.toolStripButton2.Visible = false;
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnPurchsaeOrderDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPurchsaeOrderDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchsaeOrderDetail.Name = "btnPurchsaeOrderDetail";
		this.btnPurchsaeOrderDetail.Size = new System.Drawing.Size(115, 24);
		this.btnPurchsaeOrderDetail.Text = "206-PoDetail";
		this.btnPurchsaeOrderDetail.Visible = false;
		this.btnPurchsaeOrderDetail.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnApprovedOrder.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApprovedOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprovedOrder.Name = "btnApprovedOrder";
		this.btnApprovedOrder.Size = new System.Drawing.Size(93, 24);
		this.btnApprovedOrder.Text = "Approved";
		this.btnApprovedOrder.Click += new System.EventHandler(btnApprovedOrder_Click);
		this.btnPendingForView.Image = Architecture.WinApp.Properties.Resources.eye_fill;
		this.btnPendingForView.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(144, 24);
		this.btnPendingForView.Text = "Pending For View";
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.BtnCancel.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnCancel.Name = "BtnCancel";
		this.BtnCancel.Size = new System.Drawing.Size(75, 24);
		this.BtnCancel.Text = "Cancel";
		this.BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
		this.Btnrefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Btnrefresh.Image = (System.Drawing.Image)resources.GetObject("Btnrefresh.Image");
		this.Btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Btnrefresh.Name = "Btnrefresh";
		this.Btnrefresh.Size = new System.Drawing.Size(80, 24);
		this.Btnrefresh.Text = "&Refresh";
		this.Btnrefresh.Click += new System.EventHandler(Btnrefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1138, 749);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchaseOrderForApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchaseOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchaseOrderHistory_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
