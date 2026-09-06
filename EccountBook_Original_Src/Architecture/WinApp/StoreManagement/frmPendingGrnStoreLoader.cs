using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmPendingGrnStoreLoader : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	private DataTable dtFromDb = new DataTable();

	private DataTable dtGrid = new DataTable();

	public DataTable dtBranch = new DataTable();

	private bool FormInitialized;

	private int DocumentTypeId = 48;

	private bool IsTaxable;

	private bool BranchImplemented;

	private IContainer components;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbBranchName;

	private Label label2;

	internal GridEX grd;

	private Panel panel4;

	internal GridEX grdDetail;

	private Panel panel5;

	private Panel panel6;

	private Label label5;

	private Label label6;

	internal UltraCombo CmbBillToParty;

	internal UltraCombo CmbItemName;

	private TextBox txtDocNoTo;

	private Label label3;

	private TextBox txtDocNoFrom;

	private Label label4;

	private Label label7;

	private Button button1;

	private Panel panel7;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton BtnLoadSelectedRows;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	public frmPendingGrnStoreLoader(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		FormHelper.FormEventHandler(this);
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 650);
			BranchesFill();
			CombosFill(ComboDbCall());
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			FromDate.Focus();
			PendingDataDbCall();
			GrdDataBind();
			FormInitialized = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				dtBranch = InvGrn.GetBranchesAllocatedToUserFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
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
			cmbBranchName.LimitToList.ToString();
			((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)cmbBranchName).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)cmbBranchName).Text = BranchName;
				}
				else
				{
					((Control)(object)cmbBranchName).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ComboDbCall()
	{
		try
		{
			string BranchIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbBranchName).Text, dtBranch, "BranchName");
			return InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null, BranchIds, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			CombosFill(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombosFill(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtItem = dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Supplier")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBillToParty, dtST, "Id", "Name", "Supplier Name", AllColumns: true, Conversion.ToInt(CmbBillToParty.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemName.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataDbCall();
			GrdDataBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtFromDb.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = DocumentTypeId;
			string BranchIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)cmbBranchName).Text, dtBranch, "BranchName");
			if (string.IsNullOrEmpty(BranchIds))
			{
				cmbBranchName.Focus();
				throw new Exception("Select branch first");
			}
			Obj.BranchIds = BranchIds;
			Obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			Obj.ToDate = Conversion.ToDateTime((object)Todate.Value);
			Obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			Obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			Obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			Obj.BillToPartyId = Conversion.ToInt(CmbBillToParty.Value);
			dtFromDb = InvGrn.InvGrnStore_PendingDataLoader(Obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdDataBind()
	{
		try
		{
			if (dtFromDb != null && dtFromDb.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("BaseDocumentTypeId", typeof(int));
				dt.Columns.Add("BaseDocumentType", typeof(string));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("TransporterId", typeof(int));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("CarriageAmount", typeof(decimal));
				dt.Columns.Add("InwardGatePassId", typeof(int));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("GPDate", typeof(DateTime));
				dt.Columns.Add("VehicleType", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				HashSet<int> addedIds = new HashSet<int>();
				foreach (DataRow r in dtFromDb.Rows)
				{
					int id = Conversion.ToInt(r["Id"]);
					if (addedIds.Add(id))
					{
						dt.Rows.Add(r["RecordNo"], r["Id"], r["DocumentTypeId"], r["BaseDocumentTypeId"], r["BaseDocumentType"], r["DocNo"], r["DocDate"], r["SupplierCustomerId"], r["SupplierName"], r["TransporterId"], r["TransporterName"], r["CarriageAmount"], r["InwardGatePassId"], r["GpNo"], r["GPDate"], r["VehicleType"], r["VehicleNo"], r["BiltyNo"], r["EntryDate"], r["EntryUserName"], r["ModifyDate"], r["ModifyUserName"], r["RemarksHeader"], r["NoOfAttachments"]);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
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

	public void grdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "RecordNo", "Id", "DocumentTypeId", "BaseDocumentTypeId", "SupplierCustomerId", "TransporterId", "InwardGatePassId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GridEX_Helper.SetColumnWidth(grd, "DocNo", Constants.InventoryConstants.DocNoConstant + 10);
			GridEX_Helper.SetColumnWidth(grd, "DocDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "SupplierName", 150);
			GridEX_Helper.SetColumnWidth(grd, "TransporterName", 150);
			GridEX_Helper.SetColumnWidth(grd, "CarriageAmount", 100);
			GridEX_Helper.SetColumnWidth(grd, "GpNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "GPDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "VehicleType", 100);
			GridEX_Helper.SetColumnWidth(grd, "VehicleNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "BiltyNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "RemarksHeader", Constants.InventoryConstants.RemarksConstants);
			GridEX_Helper.SetColumnWidth(grd, "NoOfAttachments", 80);
			GridEX_Helper.SetColumnWidth(grd, "EntryUserName", Constants.InventoryConstants.EntryUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "EntryDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "ModifyUserName", Constants.InventoryConstants.ModifyUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "ModifyDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
				}
				else if (e.Column.Key == "DocNo")
				{
					CommonServices.GrnSlipReport212(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			HashSet<int> selectedIds;
			if (checkedRows != null && checkedRows.Any())
			{
				selectedIds = new HashSet<int>(checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
			}
			else
			{
				GridEXRow r2 = grd.CurrentRow;
				if (r2 == null || (int)r2.RowType != 0 || r2.Cells.Count == 0)
				{
					grdDetail.DataSource = null;
					return;
				}
				selectedIds = new HashSet<int> { Conversion.ToInt(r2.Cells["Id"].Value) };
			}
			DetailGridBind(selectedIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			HashSet<int> selectedIds;
			if (checkedRows != null && checkedRows.Any())
			{
				selectedIds = new HashSet<int>(checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
			}
			else
			{
				GridEXRow r2 = grd.CurrentRow;
				if (r2 == null || (int)r2.RowType != 0 || r2.Cells.Count == 0)
				{
					grdDetail.DataSource = null;
					return;
				}
				selectedIds = new HashSet<int> { Conversion.ToInt(r2.Cells["Id"].Value) };
			}
			DetailGridBind(selectedIds);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(HashSet<int> selectedIds)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DetailId", typeof(int));
			dt.Columns.Add("SupplierCustomerId", typeof(int));
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("PurchaseOrderDocumentTypeId", typeof(int));
			dt.Columns.Add("PurchaseOrderHeaderId", typeof(int));
			dt.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dt.Columns.Add("PurchaseOrderNo", typeof(int));
			dt.Columns.Add("PurchaseOrderDate", typeof(string));
			dt.Columns.Add("PurchaseDemandDocumentTypeId", typeof(int));
			dt.Columns.Add("PurchaseDemandHeaderId", typeof(int));
			dt.Columns.Add("PurchaseDemandDetailId", typeof(int));
			dt.Columns.Add("PurchaseDemandNo", typeof(int));
			dt.Columns.Add("PurchaseDemandDate", typeof(string));
			dt.Columns.Add("PurchasePreBillDocumentTypeId", typeof(int));
			dt.Columns.Add("PurchasePreBillId", typeof(int));
			dt.Columns.Add("PurchasePreBillDetailId", typeof(int));
			dt.Columns.Add("PurchasePreBillNo", typeof(int));
			dt.Columns.Add("PurchasePreBillDate", typeof(string));
			dt.Columns.Add("DeliveryChallanDocumentTypeId", typeof(int));
			dt.Columns.Add("DeliveryChallanId", typeof(int));
			dt.Columns.Add("DeliveryChallanDetailId", typeof(int));
			dt.Columns.Add("DeliveryChallanNo", typeof(int));
			dt.Columns.Add("DeliveryChallanDate", typeof(string));
			dt.Columns.Add("WareHouseId", typeof(int));
			dt.Columns.Add("WareHouseName", typeof(string));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemCode", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("Uom", typeof(string));
			dt.Columns.Add("ItemCondition", typeof(string));
			dt.Columns.Add("ItemQty", typeof(decimal));
			dt.Columns.Add("PurchaseOrderQty", typeof(decimal));
			dt.Columns.Add("PurchaseDemandQty", typeof(decimal));
			dt.Columns.Add("PurchasePreBillQty", typeof(decimal));
			dt.Columns.Add("DeliveryChallanQty", typeof(decimal));
			dt.Columns.Add("RemarksDetail", typeof(string));
			dt.Columns.Add("NoOfAttachmentsPO", typeof(int));
			dt.Columns.Add("NoOfAttachmentsPD", typeof(int));
			dt.Columns.Add("NoOfAttachmentsPreBill", typeof(int));
			dt.Columns.Add("NoOfAttachmentsDC", typeof(int));
			foreach (DataRow r2 in from r in dtFromDb.AsEnumerable()
				where selectedIds.Contains(Conversion.ToInt(r["Id"]))
				select r)
			{
				DataRow newRow = dt.NewRow();
				newRow["Id"] = r2["Id"];
				newRow["DetailId"] = r2["DetailId"];
				newRow["SupplierCustomerId"] = r2["SupplierCustomerId"];
				newRow["DocNo"] = r2["DocNo"];
				newRow["PurchaseOrderDocumentTypeId"] = r2["PurchaseOrderDocumentTypeId"];
				newRow["PurchaseOrderHeaderId"] = r2["PurchaseOrderHeaderId"];
				newRow["PurchaseOrderHeaderId"] = r2["PurchaseOrderHeaderId"];
				newRow["PurchaseOrderDetailId"] = r2["PurchaseOrderDetailId"];
				newRow["PurchaseOrderNo"] = r2["PurchaseOrderNo"];
				newRow["PurchaseOrderDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchaseOrderDate"])) ? "" : Conversion.ToDateTime(r2["PurchaseOrderDate"]).ToString("d"));
				newRow["PurchaseDemandDocumentTypeId"] = r2["PurchaseDemandDocumentTypeId"];
				newRow["PurchaseDemandHeaderId"] = r2["PurchaseDemandHeaderId"];
				newRow["PurchaseDemandDetailId"] = r2["PurchaseDemandDetailId"];
				newRow["PurchaseDemandNo"] = r2["PurchaseDemandNo"];
				newRow["PurchaseDemandDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchaseDemandDate"])) ? "" : Conversion.ToDateTime(r2["PurchaseDemandDate"]).ToString("d"));
				newRow["PurchasePreBillDocumentTypeId"] = r2["PurchasePreBillDocumentTypeId"];
				newRow["PurchasePreBillId"] = r2["PurchasePreBillId"];
				newRow["PurchasePreBillDetailId"] = r2["PurchasePreBillDetailId"];
				newRow["PurchasePreBillNo"] = r2["PurchasePreBillNo"];
				newRow["PurchasePreBillDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchasePreBillDate"])) ? "" : Conversion.ToDateTime(r2["PurchasePreBillDate"]).ToString("d"));
				newRow["DeliveryChallanDocumentTypeId"] = r2["DeliveryChallanDocumentTypeId"];
				newRow["DeliveryChallanId"] = r2["DeliveryChallanId"];
				newRow["DeliveryChallanDetailId"] = r2["DeliveryChallanDetailId"];
				newRow["DeliveryChallanNo"] = r2["DeliveryChallanNo"];
				newRow["DeliveryChallanDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["DeliveryChallanDate"])) ? "" : Conversion.ToDateTime(r2["DeliveryChallanDate"]).ToString("d"));
				newRow["WareHouseId"] = r2["WareHouseId"];
				newRow["WareHouseName"] = r2["WareHouseName"];
				newRow["ItemId"] = r2["ItemId"];
				newRow["ItemCode"] = r2["ItemCode"];
				newRow["ItemName"] = r2["ItemName"];
				newRow["Uom"] = r2["Uom"];
				newRow["ItemCondition"] = r2["ItemCondition"];
				newRow["ItemQty"] = r2["ItemQty"];
				newRow["PurchaseOrderQty"] = r2["PurchaseOrderQty"];
				newRow["PurchaseDemandQty"] = r2["PurchaseDemandQty"];
				newRow["PurchasePreBillQty"] = r2["PurchasePreBillQty"];
				newRow["DeliveryChallanQty"] = r2["DeliveryChallanQty"];
				newRow["RemarksDetail"] = r2["RemarksDetail"];
				newRow["NoOfAttachmentsPO"] = r2["NoOfAttachmentsPO"];
				newRow["NoOfAttachmentsPD"] = r2["NoOfAttachmentsPD"];
				newRow["NoOfAttachmentsPreBill"] = r2["NoOfAttachmentsPreBill"];
				newRow["NoOfAttachmentsDC"] = r2["NoOfAttachmentsDC"];
				dt.Rows.Add(newRow);
			}
			grdDetail.DataSource = dt;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "DetailId", "PurchaseOrderHeaderId", "PurchaseOrderDetailId", "PurchaseDemandHeaderId", "PurchaseOrderDocumentTypeId", "PurchaseDemandDocumentTypeId", "PurchasePreBillDocumentTypeId", "DeliveryChallanDocumentTypeId", "PurchaseDemandDetailId",
				"PurchasePreBillId", "PurchasePreBillDetailId", "DeliveryChallanId", "DeliveryChallanDetailId", "WareHouseId", "ItemId", "SupplierCustomerId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			grdDetail.RootTable.Columns["NoOfAttachmentsPO"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["NoOfAttachmentsPD"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["NoOfAttachmentsPreBill"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["NoOfAttachmentsDC"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["NoOfAttachmentsPO"].Caption = "PO Attachtments";
			grdDetail.RootTable.Columns["NoOfAttachmentsPD"].Caption = "Demand Attachtments";
			grdDetail.RootTable.Columns["NoOfAttachmentsPreBill"].Caption = "PreBill Attachtments";
			grdDetail.RootTable.Columns["NoOfAttachmentsDC"].Caption = "DC Attachtments";
			GridEX_Helper.SetColumnWidth(grdDetail, "WareHouseName", 130);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCode", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemName", Constants.InventoryConstants.ItemName - 20);
			GridEX_Helper.SetColumnWidth(grdDetail, "Uom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCondition", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "DeliveryChallanQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "DeliveryChallanNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "DeliveryChallanDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "RemarksDetail", Constants.InventoryConstants.RemarksConstants);
			grdDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (e.Column.Key == "NoOfAttachmentsPO")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PurchaseOrderHeaderId"].Value), Conversion.ToInt(r.Cells["PurchaseOrderDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "NoOfAttachmentsPD")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PurchaseDemandHeaderId"].Value), Conversion.ToInt(r.Cells["PurchaseDemandDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "NoOfAttachmentsPreBill")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PurchasePreBillId"].Value), Conversion.ToInt(r.Cells["PurchasePreBillDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "NoOfAttachmentsDC")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["DeliveryChallanId"].Value), Conversion.ToInt(r.Cells["DeliveryChallanDocumentTypeId"].Value));
				}
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDepartmentRequest_FormClosed(object sender, FormClosedEventArgs e)
	{
		dtLoader = null;
		Hide();
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtDocNoFrom.Clear();
			txtDocNoTo.Clear();
			((Control)(object)CmbItemName).Text = string.Empty;
			PendingDataDbCall();
			GrdDataBind();
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
			BranchesFill();
			CombosFill(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows == null || checkedRows.Length == 0)
			{
				MessageBox.Show("Please select at least one record from the grid.");
				return;
			}
			HashSet<int> selectedMasterIds = new HashSet<int>(checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
			List<DataRow> selectedDetailRows = (from r in dtFromDb.AsEnumerable()
				where selectedMasterIds.Contains(Conversion.ToInt(r["Id"]))
				select r).ToList();
			if (!selectedDetailRows.Any())
			{
				MessageBox.Show("No detail records found for selected rows.");
				return;
			}
			int? basePartyId = null;
			int? baseDocumentTypeId = null;
			foreach (DataRow item in selectedDetailRows)
			{
				int partyId = Conversion.ToInt(item["SupplierCustomerId"]);
				int documentTypeId = Conversion.ToInt(item["BaseDocumentTypeId"]);
				if (!basePartyId.HasValue)
				{
					basePartyId = partyId;
				}
				if (!baseDocumentTypeId.HasValue)
				{
					baseDocumentTypeId = documentTypeId;
				}
				if (partyId != basePartyId.Value)
				{
					MessageBox.Show("Sorry! You can only load records of the same Party.");
					return;
				}
				if (documentTypeId != baseDocumentTypeId.Value)
				{
					MessageBox.Show("Sorry! You can only load records of the same Base Document.");
					return;
				}
			}
			dtLoader = dtFromDb.Clone();
			HashSet<int> addedDetailIds = new HashSet<int>();
			foreach (DataRow r2 in selectedDetailRows)
			{
				int detailId = Conversion.ToInt(r2["DetailId"]);
				if (addedDetailIds.Add(detailId))
				{
					dtLoader.ImportRow(r2);
				}
			}
			Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add("Ctrl+S", "For Search");
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

	private void InvfrmLoadSupplySchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
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
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Expected O, but got Unknown
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Expected O, but got Unknown
		Appearance appearance5 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
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
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.cmbBranchName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbBillToParty = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadSelectedRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label9 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1146, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 22);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(84, 6);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 150;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 149;
		this.label2.Text = "Branch Name";
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9f);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(216, 37);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(130, 22);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(84, 37);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(130, 22);
		this.FromDate.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1193, 203);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 38;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grd_RowCheckStateChanged);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 344);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1193, 23);
		this.panel4.TabIndex = 41;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 367);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1193, 230);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 39;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 116);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1193, 25);
		this.panel5.TabIndex = 40;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.label6);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToParty);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel6.Controls.Add(this.txtDocNoTo);
		this.panel6.Controls.Add(this.Todate);
		this.panel6.Controls.Add(this.FromDate);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add(this.txtDocNoFrom);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.label7);
		this.panel6.Controls.Add(this.button1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 50);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1193, 66);
		this.panel6.TabIndex = 36;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(492, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 36;
		this.label5.Text = "Supplier";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(492, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 36;
		this.label6.Text = "Item Name";
		this.CmbBillToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Location = new System.Drawing.Point(562, 6);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Name = "CmbBillToParty";
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).TabIndex = 4;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(562, 35);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 5;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(412, 37);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.Tag = "int";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(354, 41);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 34;
		this.label3.Text = "To Doc";
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(412, 8);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.Tag = "int";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(354, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "From Doc";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 41);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 15;
		this.label7.Text = "From,To Date";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(775, 35);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(57, 26);
		this.button1.TabIndex = 6;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btngrnlod_Click);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip1);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1193, 50);
		this.panel7.TabIndex = 37;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.BtnLoadSelectedRows, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1193, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.BtnLoadSelectedRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadSelectedRows.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadSelectedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadSelectedRows.Name = "BtnLoadSelectedRows";
		this.BtnLoadSelectedRows.Size = new System.Drawing.Size(58, 22);
		this.BtnLoadSelectedRows.Text = "Load";
		this.BtnLoadSelectedRows.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 27);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(158, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "Pending Grn Store";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1193, 597);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel6);
		base.Controls.Add(this.panel7);
		base.Name = "frmPendingGrnStoreLoader";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(LoadDepartmentRequest_FormClosed);
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmLoadSupplySchedule_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
