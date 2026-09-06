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
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalGatePassOutward : Form
{
	public int ActionId;

	public int BranchesId;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Label lblGp;

	private DateTimePicker ToDate;

	internal UltraCombo CmbSupplier;

	private Label label1;

	private DateTimePicker fromdate;

	private Label label27;

	private Label label33;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo cmbgatetype;

	private Label label2;

	private ToolStripButton btnApproved;

	public PendingApprovalGatePassOutward(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGPOutward_Load(object sender, EventArgs e)
	{
		try
		{
			if (ActionId == 1)
			{
				lblGp.Text = "GatePass Inward";
			}
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				fromdate.Value = Convert.ToDateTime(RequestedFromDate);
				ToDate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			SupplierCustomer();
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void reset()
	{
		SupplierCustomer();
	}

	private void SupplierCustomer()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		try
		{
			if (ActionId == 1)
			{
				DataTable dt = GatePassInward.GetDataForDropDownFromGPI(UserAccount.OrganizationId, UserAccount.CompanyId, "Supplier", Conversion.ToString((object)UserAccount.BranchesId));
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, CmbSupplier, "Id", "ReferenceName", "Party Name", false);
					((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 400;
				}
				return;
			}
			DataTable dt2 = SupplierCustomer.SupplierCustomerAgainstOutwarGP(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dt2.Rows.Count > 0)
			{
				DDL.BindDDL(dt2, CmbSupplier, "Id", "CompanyName", "Party Name", ZeroIndex: false);
				((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gatetypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("GateType");
			dt.Rows.Add(1, "GatePass Outward");
			dt.Rows.Add(2, "GatePass General Outward");
			DDL.BindDDL(dt, cmbgatetype, "Id", "GateType", "Gate Type", ZeroIndex: false);
			((UltraGridBase)cmbgatetype).Rows[0].Activate();
			((UltraGridBase)cmbgatetype).DisplayLayout.Bands[0].Columns[1].Width = 190;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHisory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassOutward.PendingGatePassForApproval(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FromDate = Conversion.ToDateTime((object)fromdate.Value),
				ToDate = Conversion.ToDateTime((object)ToDate.Value),
				SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value),
				ActionId = ActionId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocumentTypeId");
				dtGrid.Columns.Add("GpDate", typeof(DateTime));
				dtGrid.Columns.Add("GpSrNo", typeof(int));
				dtGrid.Columns.Add("CustomerName");
				dtGrid.Columns.Add("OrderType");
				dtGrid.Columns.Add("OrderNo", typeof(int));
				dtGrid.Columns.Add("VehicleType");
				dtGrid.Columns.Add("VehicleNo");
				dtGrid.Columns.Add("BiltyNo");
				dtGrid.Columns.Add("ItemName");
				dtGrid.Columns.Add("WareHouse");
				dtGrid.Columns.Add("NoOfBags", typeof(double));
				dtGrid.Columns.Add("InDateTime", typeof(DateTime));
				dtGrid.Columns.Add("OutDateTime", typeof(DateTime));
				dtGrid.Columns.Add("Remarks");
				dtGrid.Columns.Add("CityName");
				dtGrid.Columns.Add("FreightAmount", typeof(double));
				dtGrid.Columns.Add("NetPaidAmount", typeof(double));
				dtGrid.Columns.Add("DOTotalWeight", typeof(double));
				dtGrid.Columns.Add("WbNetWeight", typeof(double));
				dtGrid.Columns.Add("DiffWeight", typeof(double));
				dtGrid.Columns.Add("ContainerNo");
				dtGrid.Columns.Add("ContainerNo1");
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Columns.Add("NoOfAttachments");
				dtGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string PartyName = "";
					PartyName = ((ActionId != 1) ? Conversion.ToString(dt.Rows[i]["CustomerName"]) : Conversion.ToString(dt.Rows[i]["SupplierName"]));
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GpSrNo"], PartyName, dt.Rows[i]["OrderType"], dt.Rows[i]["OrderNo"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["ItemName"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["NoOfBags"], dt.Rows[i]["InDateTimeStamp"], dt.Rows[i]["OutDateTimeStamp"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["CityName"], dt.Rows[i]["Freight"], dt.Rows[i]["NetPaid"], dt.Rows[i]["DoTotalWeiht"], dt.Rows[i]["NetWbWeight"], dt.Rows[i]["DifferenceWeight"], dt.Rows[i]["Container"], dt.Rows[i]["Container1"], dt.Rows[i]["EntryDate"], dt.Rows[i]["EntryUser"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtGrid;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (ActionId == 1)
			{
				DataGridHistory.RootTable.Columns["DoTotalWeight"].Caption = "SupplierWeight";
			}
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["InDateTime"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["OutDateTime"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.CarrierTypeConstant;
			DataGridHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.CarrierTypeConstant;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["InDateTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["OutDateTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			DataGridHistory.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["NetPaidAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["DOTotalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["WbNetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["DiffWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["ContainerNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			DataGridHistory.RootTable.Columns["ContainerNo1"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetPaidAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetPaidAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetPaidAmount"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NetPaidAmount"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DoTotalWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DoTotalWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DoTotalWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DoTotalWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WbNetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WbNetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WbNetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["WbNetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["DiffWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns.Add("Select");
			DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridHistory.RootTable.Columns["Select"].Width = 40;
			DataGridHistory.RootTable.Columns["Select"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.RootTable.Columns["CustomerName"].Caption = "PartyName";
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void frmGPOutward_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnNew_Click(null, null);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		gridHisory();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				int Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				if (ActionId == 1)
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Id);
				}
				else
				{
					CommonServices.GatePassOutwardSlipandRegister(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "GpSrNo")
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					if (ActionId == 1)
					{
						CommonServices.EditMethodFromLinked(51, Id);
					}
					else
					{
						CommonServices.EditMethodFromLinked(91, Id);
					}
				}
			}
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve GatePass?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			ApprovalList obj = new ApprovalList();
			obj.AllApprovalLists = new List<ApprovalList>();
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				if (ActionId == 1)
				{
					detail.DocumentTypeId = 51;
				}
				else
				{
					detail.DocumentTypeId = 91;
				}
				detail.Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				detail.EntryUser = UserAccount.ID;
				obj.AllApprovalLists.Add(detail);
			}
			GatePassOutward.ApproveGatePassOutward(obj);
			MessageBox.Show("Record Approved Successfully");
			gridHisory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (DataGridHistory.CurrentColumn.Key == "Print")
			{
				if (ActionId == 1)
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Id);
				}
				else
				{
					CommonServices.GatePassOutwardSlipandRegister(Id);
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "GpSrNo")
			{
				if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					if (ActionId == 1)
					{
						CommonServices.EditMethodFromLinked(51, Id);
					}
					else
					{
						CommonServices.EditMethodFromLinked(91, Id);
					}
				}
			}
			else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
			{
				if (ActionId == 1)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 51);
				}
				else
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 91);
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.PendingApprovalGatePassOutward));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbgatetype = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.lblGp = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgatetype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(635, 551);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 106);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(635, 445);
		this.panel5.TabIndex = 1;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(635, 445);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 47;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgatetype);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.ToDate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.fromdate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 51);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(635, 55);
		this.panel4.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(726, 189);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(92, 15);
		this.label2.TabIndex = 168;
		this.label2.Text = "Gate Pass Type";
		this.label2.Visible = false;
		this.cmbgatetype.AllowNull = (DefaultableBoolean)1;
		this.cmbgatetype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgatetype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgatetype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgatetype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbgatetype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgatetype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgatetype).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbgatetype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgatetype).Location = new System.Drawing.Point(822, 183);
		((System.Windows.Forms.Control)(object)this.cmbgatetype).Name = "cmbgatetype";
		((System.Windows.Forms.Control)(object)this.cmbgatetype).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbgatetype).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbgatetype).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbgatetype).Visible = false;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(499, 22);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 3;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(130, 24);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(121, 23);
		this.ToDate.TabIndex = 1;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(253, 22);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(253, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(69, 15);
		this.label1.TabIndex = 166;
		this.label1.Text = "Party Name";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(7, 24);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(121, 23);
		this.fromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(7, 4);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 164;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(130, 4);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 163;
		this.label33.Text = "To Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.lblGp);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(635, 26);
		this.panel3.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(595, 3);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.lblGp.AutoSize = true;
		this.lblGp.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblGp.ForeColor = System.Drawing.Color.White;
		this.lblGp.Location = new System.Drawing.Point(4, 3);
		this.lblGp.Name = "lblGp";
		this.lblGp.Size = new System.Drawing.Size(160, 19);
		this.lblGp.TabIndex = 4;
		this.lblGp.Text = "GatePass Outward";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(635, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnApproved });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(635, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(70, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(83, 24);
		this.btnApproved.Text = "Approved";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(635, 551);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PendingApprovalGatePassOutward";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmGPOutward_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGPOutward_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgatetype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
