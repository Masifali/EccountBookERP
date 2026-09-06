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
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class PendingApprovalStockTransfer : Form
{
	public int DocumentTypeId;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label lblHeading;

	private Panel panel4;

	private Button btnApproved;

	private Button btnshow;

	internal UltraCombo cmbItem;

	private Label label6;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Label label27;

	private Label label33;

	internal UltraCombo CmbWarehouseTo;

	private Label label3;

	internal UltraCombo CmbWarehouseFrom;

	private Label label2;

	internal UltraCombo CmbTransType;

	private Label label1;

	private Panel panel2;

	private Panel panel5;

	private Label label4;

	private Panel panel6;

	private Panel panel7;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridMain;

	internal GridEX DataGridDetail;

	private ToolStripButton btnRefresh;

	public PendingApprovalStockTransfer(UserAccount Puser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void PendingApprovalStockTransfer_Load(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 68)
			{
				lblHeading.Text = "Stock Transfer Approval";
			}
			else if (DocumentTypeId == 806)
			{
				lblHeading.Text = "Stock Transfer Manual Approval";
			}
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				datFromDate.Value = Convert.ToDateTime(RequestedFromDate);
				datToDate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				datFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				datToDate.Value = DateTime.Now;
			}
			TransferTypeFill();
			ComboBinds();
			GetPendingRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TransferTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TransferType");
			dt.Rows.Add(1, "Inward");
			dt.Rows.Add(2, "Outward");
			dt.Rows.Add(3, "MoveOrder");
			DDL.BindDDLNew(dt, CmbTransType, "Id", "TransferType", "Transfer Type", false);
			((UltraGridBase)CmbTransType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBinds()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvStockTransferHeader.AllComboAgainstStockTransfer(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = Conversion.ToString((object)DocumentTypeId)
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtWarehouseFrom = new DataTable();
			dtWarehouseFrom.Columns.Add("Id");
			dtWarehouseFrom.Columns.Add("Name");
			DataTable dtWarehouseTo = new DataTable();
			dtWarehouseTo.Columns.Add("Id");
			dtWarehouseTo.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Name");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemName")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "WareHouseFrom")
				{
					dtWarehouseFrom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "WareHouseTo")
				{
					dtWarehouseTo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtWarehouseFrom.Rows.Count > 0)
			{
				DDL.BindDDL(dtWarehouseFrom, CmbWarehouseFrom, "Id", "Name", "Warehosue From", ZeroIndex: false);
				((UltraGridBase)CmbWarehouseFrom).DisplayLayout.Bands[0].Columns[1].Width = 250;
			}
			else
			{
				((Control)(object)CmbWarehouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWarehouseFrom).DataSource = null;
			}
			if (dtWarehouseTo.Rows.Count > 0)
			{
				DDL.BindDDL(dtWarehouseTo, CmbWarehouseTo, "Id", "Name", "Warehosue To", ZeroIndex: false);
				((UltraGridBase)CmbWarehouseTo).DisplayLayout.Bands[0].Columns[1].Width = 250;
			}
			else
			{
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((UltraGridBase)CmbWarehouseTo).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "Name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 250;
			}
			else
			{
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridMain).Name);
			if (fs != null)
			{
				DataGridMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridDetail).Name);
			if (fs != null)
			{
				DataGridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetPendingRecords()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.Activity = ((Control)(object)CmbTransType).Text;
			obj.FromWarehouseId = Conversion.ToInt(CmbWarehouseFrom.Value);
			obj.ToWarehouseId = Conversion.ToInt(CmbWarehouseTo.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			dtHistory = InvStockTransferHeader.StockTransferHistoryForApproval(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("DocumentTypeId", typeof(int));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("TransferType", typeof(string));
				dtMain.Columns.Add("GpNo", typeof(int));
				dtMain.Columns.Add("TicketNo", typeof(int));
				dtMain.Columns.Add("FactoryWeight", typeof(double));
				dtMain.Columns.Add("OtherWeight", typeof(double));
				dtMain.Columns.Add("DiffWeight", typeof(double));
				dtMain.Columns.Add("EntryUser");
				dtMain.Columns.Add("EntryDate", typeof(DateTime));
				dtMain.Columns.Add("Remarks");
				dtMain.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dtHistory.Rows[i]["Id"]);
					if (!existingIds.Contains(id))
					{
						dtMain.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocumentTypeId"], dtHistory.Rows[i]["DocNo"], Convert.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["TransferType"], dtHistory.Rows[i]["GpNo"], dtHistory.Rows[i]["TicketNo"], dtHistory.Rows[i]["WbNetWeight"], dtHistory.Rows[i]["OtherWeight"], dtHistory.Rows[i]["DiffWeight"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["RemarksHeader"]);
						existingIds.Add(id);
					}
				}
				DataGridMain.DataSource = dtMain;
				DataGridMain.RetrieveStructure();
				GridSettings();
			}
			else
			{
				DataGridMain.ClearStructure();
				DataGridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			DataGridMain.RootTable.Columns["Id"].Visible = false;
			DataGridMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			if (DocumentTypeId == 806)
			{
				DataGridMain.RootTable.Columns["OtherWeight"].Visible = false;
				DataGridMain.RootTable.Columns["DiffWeight"].Visible = false;
			}
			else if (DocumentTypeId == 68)
			{
				DataGridMain.RootTable.Columns["OtherWeight"].Visible = true;
				DataGridMain.RootTable.Columns["DiffWeight"].Visible = true;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridMain, 2, 3);
			DataGridMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataGridMain.RootTable.Columns["TransferType"].Width = Constants.InventoryConstants.StringConstant;
			DataGridMain.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridMain.RootTable.Columns["TicketNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridMain.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridMain.RootTable.Columns["OtherWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridMain.RootTable.Columns["DiffWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridMain.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridMain.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridMain.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridMain.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["OtherWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["OtherWeight"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["OtherWeight"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["OtherWeight"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["DiffWeight"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns.Add("Select");
			DataGridMain.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridMain.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridMain.RootTable.Columns["Select"].Width = 25;
			DataGridMain.RootTable.Columns["Select"].Position = 0;
			if (DocumentTypeId == 68)
			{
				DataGridMain.RootTable.Columns.Add("Slip");
				DataGridMain.RootTable.Columns["Slip"].Caption = "Slip406";
				DataGridMain.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridMain.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				DataGridMain.RootTable.Columns["Slip"].ButtonText = "Slip406";
				DataGridMain.RootTable.Columns["Slip"].Width = 60;
			}
			else if (DocumentTypeId == 806)
			{
				DataGridMain.RootTable.Columns.Add("Slip");
				DataGridMain.RootTable.Columns["Slip"].Caption = "Slip407";
				DataGridMain.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridMain.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				DataGridMain.RootTable.Columns["Slip"].ButtonText = "Slip407";
				DataGridMain.RootTable.Columns["Slip"].Width = 60;
			}
			DataGridMain.RootTable.Columns["Slip"].Position = 1;
			DataGridMain.RootTable.Columns.Add("IsApproved");
			DataGridMain.RootTable.Columns["IsApproved"].Caption = "Approve";
			DataGridMain.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridMain.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
			DataGridMain.RootTable.Columns["IsApproved"].ButtonText = "Approve";
			DataGridMain.RootTable.Columns["IsApproved"].Width = 60;
			DataGridMain.RootTable.Columns["IsApproved"].Position = 2;
			DataGridMain.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("WarehouseFrom");
				dt.Columns.Add("WarehouseTo");
				dt.Columns.Add("JobLotFrom");
				dt.Columns.Add("JobLotTo");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("EbUnit", typeof(double));
				dt.Columns.Add("EbTotal", typeof(double));
				dt.Columns.Add("AdLsWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				DataTable resultRows = (from row in dtHistory.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["FromWareHouseName"], resultRows.Rows[i]["ToWareHouseName"], resultRows.Rows[i]["JobLotFrom"], resultRows.Rows[i]["JobLotTo"], resultRows.Rows[i]["CropYear"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackingType"], resultRows.Rows[i]["PackUomCode"], resultRows.Rows[i]["Qty"], resultRows.Rows[i]["GrossWeight"], resultRows.Rows[i]["EbUnit"], resultRows.Rows[i]["EbTotal"], resultRows.Rows[i]["AdLsWeight"], resultRows.Rows[i]["NetWeight"]);
				}
				DataGridDetail.DataSource = dt;
				DataGridDetail.RetrieveStructure();
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
			DataGridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GetPendingRecords();
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			if (DataGridMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridMain.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvStockTransferHeader.ApproveStockTransfer(obj);
				MessageBox.Show("Record Approve Successfully");
				GetPendingRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				if (DocumentTypeId == 68)
				{
					GenerateReportStockTransferAuto(Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value));
				}
				else if (DocumentTypeId == 806)
				{
					GenerateReportStockTransferManual(Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value));
				}
			}
			else if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveRecord(int Id)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.Id = Id;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvStockTransferHeader.ApproveStockTransfer(val);
				MessageBox.Show("Record Approve Successfully");
				GetPendingRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportStockTransferAuto(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvStockTransferHeader.StockTransferSlipandRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 68,
				Id = Id
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "406-InvStockTransferSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateReportStockTransferManual(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvStockTransferHeader.StockTransferSlipandRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 806,
				Id = Id
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "407-InvStockTransferSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datToDate.Value = DateTime.Now;
			((Control)(object)CmbTransType).Text = string.Empty;
			((Control)(object)CmbWarehouseFrom).Text = string.Empty;
			((Control)(object)CmbWarehouseTo).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			GetPendingRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBinds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		GridEXLayout DataGridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.DataGridMain = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbWarehouseTo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbWarehouseFrom = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbTransType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.btnApproved = new System.Windows.Forms.Button();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lblHeading = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridMain).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(901, 703);
		this.panel1.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridDetail);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 468);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(901, 235);
		this.panel6.TabIndex = 13;
		this.DataGridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridDetail).BackColor = System.Drawing.Color.White;
		this.DataGridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridDetail.DesignTimeLayout = DataGridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridDetail.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Name = "DataGridDetail";
		((ScrollableBase)this.DataGridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Size = new System.Drawing.Size(901, 209);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).TabIndex = 13;
		this.DataGridDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridDetail).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(901, 26);
		this.panel7.TabIndex = 12;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(861, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar2.MyGrid = this.DataGridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 27;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(7, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(353, 19);
		this.label5.TabIndex = 4;
		this.label5.Text = "Detail Information of Above Selected Row";
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridMain);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 161);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(901, 307);
		this.panel2.TabIndex = 12;
		this.DataGridMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridMain).BackColor = System.Drawing.Color.White;
		this.DataGridMain.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridMain.DesignTimeLayout = DataGridMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridMain.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridMain).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.DataGridMain).Name = "DataGridMain";
		((ScrollableBase)this.DataGridMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Size = new System.Drawing.Size(901, 283);
		((System.Windows.Forms.Control)(object)this.DataGridMain).TabIndex = 12;
		this.DataGridMain.TotalRow = (InheritableBoolean)1;
		this.DataGridMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridMain).VisualStyle = (VisualStyle)4;
		this.DataGridMain.ColumnButtonClick += new ColumnActionEventHandler(DataGridMain_ColumnButtonClick);
		this.DataGridMain.SelectionChanged += new System.EventHandler(DataGridMain_SelectionChanged);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(901, 24);
		this.panel5.TabIndex = 11;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(861, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar1.MyGrid = this.DataGridMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 26;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(7, 2);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(150, 19);
		this.label4.TabIndex = 4;
		this.label4.Text = "Main Information";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseTo);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.btnApproved);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(901, 102);
		this.panel4.TabIndex = 11;
		this.CmbWarehouseTo.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Location = new System.Drawing.Point(354, 37);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Name = "CmbWarehouseTo";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(258, 43);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(83, 15);
		this.label3.TabIndex = 153;
		this.label3.Text = "Warehouse To";
		this.CmbWarehouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Location = new System.Drawing.Point(354, 9);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Name = "CmbWarehouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(258, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 15);
		this.label2.TabIndex = 151;
		this.label2.Text = "Warehouse From";
		this.CmbTransType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTransType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransType).Location = new System.Drawing.Point(88, 65);
		((System.Windows.Forms.Control)(object)this.CmbTransType).Name = "CmbTransType";
		((System.Windows.Forms.Control)(object)this.CmbTransType).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransType).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(15, 71);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 149;
		this.label1.Text = "Trans.Type";
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(631, 64);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(77, 26);
		this.btnApproved.TabIndex = 7;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(567, 64);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(354, 65);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(258, 71);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 147;
		this.label6.Text = "Item Name";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yyyy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(88, 39);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(161, 23);
		this.datToDate.TabIndex = 1;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yyyy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(88, 11);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(161, 23);
		this.datFromDate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(15, 15);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(15, 43);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "To Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.lblHeading);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(901, 32);
		this.panel3.TabIndex = 10;
		this.lblHeading.AutoSize = true;
		this.lblHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeading.ForeColor = System.Drawing.Color.White;
		this.lblHeading.Location = new System.Drawing.Point(10, 6);
		this.lblHeading.Name = "lblHeading";
		this.lblHeading.Size = new System.Drawing.Size(128, 19);
		this.lblHeading.TabIndex = 4;
		this.lblHeading.Text = "Stock Transfer";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnRefresh });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(901, 27);
		this.toolStrip3.TabIndex = 9;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(69, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(901, 703);
		base.Controls.Add(this.panel1);
		base.Name = "PendingApprovalStockTransfer";
		this.Text = "Pending Approval Stock Transfer";
		base.Load += new System.EventHandler(PendingApprovalStockTransfer_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridMain).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
