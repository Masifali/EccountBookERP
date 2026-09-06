using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.StoreManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.StoreManagement
{

public class frmPendingDeliveryChallanLoader : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	private DataTable dtHistory = new DataTable();

	private bool FormInitialized;

	private int DocumentTypeId = 148;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private Panel panel3;

	internal GridEX grd;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	private TextBox txtDocNoTo;

	private Label label1;

	private TextBox txtDocNoFrom;

	private Label label4;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label6;

	internal UltraCombo CmbItemName;

	private ToolStripButton BtnLoadSelectedRows;

	internal GridEX grdDetail;

	private Panel panel4;

	private Label label5;

	internal UltraCombo CmbBillToParty;

	public frmPendingDeliveryChallanLoader(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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
			base.AutoScrollMinSize = new Size(980, 650);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentMethod()
	{
		try
		{
			CombosFill((await Task.Run(delegate
			{
				DataTable comboData = ComboDbCall();
				PendingDataDbCall(Intilization: true);
				return new
				{
					ComboData = comboData
				};
			})).ComboData);
			GrdDataBind();
			FromDate.Value = DateTime.Now.AddDays(-7.0);
			FromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error occurred during database call: " + ex.Message);
		}
	}

	private DataTable ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return DeliveryChallanHeader.GetDataForDropDownFromDeliveryChallanHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BillToPartyName")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBillToParty, dtST, "Id", "Name", "BillTo Party Name", AllColumns: true, Conversion.ToInt(CmbBillToParty.Value), insertDefaultRow: true, null, null, ActivateRow: false);
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

	private void PendingDataDbCall(bool Intilization = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = DocumentTypeId;
			Obj.FromDate = (Intilization ? DateTime.Now.AddDays(-7.0) : Conversion.ToDateTime((object)FromDate.Value));
			Obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			Obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			Obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			Obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			Obj.BillToPartyId = Conversion.ToInt(CmbBillToParty.Value);
			dtHistory = DeliveryChallanHeader.DeliveryChallanHeader_PendingDataLoader(Obj);
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
			if (dtHistory != null && dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("GatePassId", typeof(int));
				dt.Columns.Add("GpSrNo", typeof(string));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpStatus", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				HashSet<int> addedIds = new HashSet<int>();
				foreach (DataRow r in dtHistory.Rows)
				{
					int id = Conversion.ToInt(r["DeliveryChallanHeaderId"]);
					if (addedIds.Add(id))
					{
						dt.Rows.Add(r["RecordNo"], r["DeliveryChallanHeaderId"], r["DocumentTypeId"], r["DocNo"], r["DocDate"], r["ReferenceNo"], r["DeliveryTerm"], r["CityName"], r["VehicleNo"], r["BiltyNo"], r["GatePassId"], r["GpSrNo"], r["GpDate"], r["GpStatus"], r["RemarksHeader"], r["EntryDate"], r["EntryUserName"], r["ModifyDate"], r["ModifyUserName"]);
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
			List<string> hiddenColumns = new List<string> { "RecordNo", "Id", "DocumentTypeId", "GatePassId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			GridEX_Helper.SetColumnWidth(grd, "DocNo", Constants.InventoryConstants.DocNoConstant + 10);
			GridEX_Helper.SetColumnWidth(grd, "DocDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "ReferenceNo", 120);
			GridEX_Helper.SetColumnWidth(grd, "DeliveryTerm", 120);
			GridEX_Helper.SetColumnWidth(grd, "CityName", 120);
			GridEX_Helper.SetColumnWidth(grd, "VehicleNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "BiltyNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "GpSrNo", 100);
			GridEX_Helper.SetColumnWidth(grd, "GpDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "GpStatus", 100);
			GridEX_Helper.SetColumnWidth(grd, "RemarksHeader", Constants.InventoryConstants.RemarksConstants);
			GridEX_Helper.SetColumnWidth(grd, "EntryUserName", Constants.InventoryConstants.EntryUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "EntryDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "ModifyUserName", Constants.InventoryConstants.ModifyUserConstants);
			GridEX_Helper.SetColumnWidth(grd, "ModifyDate", Constants.InventoryConstants.DateConstant);
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 1;
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
				else
				{
					CrystalReportPrint_Helper.StoreDeliveryChallanSlip148(Id);
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
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows != null && checkedRows.Count() > 0)
			{
				HashSet<int> selectedIds = new HashSet<int>(checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
				DetailGridBind(selectedIds);
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows != null && checkedRows.Count() > 0)
			{
				HashSet<int> selectedIds = new HashSet<int>(checkedRows.Select((GridEXRow r) => Conversion.ToInt(r.Cells["Id"].Value)));
				DetailGridBind(selectedIds);
			}
			else
			{
				grdDetail.DataSource = null;
			}
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
			dt.Columns.Add("PurchaseDemandHeaderId", typeof(int));
			dt.Columns.Add("PurchaseDemandDetailId", typeof(int));
			dt.Columns.Add("PurchaseDemandNo", typeof(int));
			dt.Columns.Add("PurchaseDemandDate", typeof(string));
			dt.Columns.Add("PurchaseOrderHeaderId", typeof(int));
			dt.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dt.Columns.Add("PurchaseOrderNo", typeof(int));
			dt.Columns.Add("PurchaseOrderDate", typeof(string));
			dt.Columns.Add("PurchasePreBillNo", typeof(int));
			dt.Columns.Add("PurchasePreBillDate", typeof(string));
			dt.Columns.Add("BillToPartyId", typeof(int));
			dt.Columns.Add("BillToPartyName", typeof(string));
			dt.Columns.Add("GpStatus", typeof(string));
			dt.Columns.Add("WareHouseName", typeof(string));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemCode", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("Uom", typeof(string));
			dt.Columns.Add("ItemCondition", typeof(string));
			dt.Columns.Add("ChallanQty", typeof(decimal));
			dt.Columns.Add("VendorSupplierId", typeof(int));
			dt.Columns.Add("VendorSupplierName", typeof(string));
			dt.Columns.Add("ReferencePartyId", typeof(int));
			dt.Columns.Add("ReferencePartyName", typeof(string));
			dt.Columns.Add("PartyBillNo", typeof(string));
			dt.Columns.Add("PartyBillDate", typeof(DateTime));
			dt.Columns.Add("PurchaseDemandQty", typeof(decimal));
			dt.Columns.Add("PurchaseOrderQty", typeof(decimal));
			dt.Columns.Add("PurchasePreBillQty", typeof(decimal));
			dt.Columns.Add("ChallanQtyUsedInGrn", typeof(decimal));
			dt.Columns.Add("BalChallanQty", typeof(decimal));
			dt.Columns.Add("RemarksDetail", typeof(string));
			foreach (DataRow r2 in from r in dtHistory.AsEnumerable()
				where selectedIds.Contains(Conversion.ToInt(r["DeliveryChallanHeaderId"]))
				select r)
			{
				DataRow newRow = dt.NewRow();
				newRow["Id"] = r2["DeliveryChallanHeaderId"];
				newRow["DetailId"] = r2["DetailId"];
				newRow["PurchaseDemandHeaderId"] = r2["PurchaseDemandHeaderId"];
				newRow["PurchaseDemandDetailId"] = r2["PurchaseDemandDetailId"];
				newRow["PurchaseDemandNo"] = r2["PurchaseDemandDocNo"];
				newRow["PurchaseDemandDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchaseDemandDocDate"])) ? "" : Conversion.ToDateTime(r2["PurchaseDemandDocDate"]).ToString("d"));
				newRow["PurchaseOrderHeaderId"] = r2["PurchaseOrderHeaderId"];
				newRow["PurchaseOrderDetailId"] = r2["PurchaseOrderDetailId"];
				newRow["PurchaseOrderNo"] = r2["PurchaseOrderDocNo"];
				newRow["PurchaseOrderDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchaseOrderDocDate"])) ? "" : Conversion.ToDateTime(r2["PurchaseOrderDocDate"]).ToString("d"));
				newRow["PurchasePreBillNo"] = r2["PurchasePreBillDocNo"];
				newRow["PurchasePreBillDate"] = (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2["PurchasePreBillDocDate"])) ? "" : Conversion.ToDateTime(r2["PurchasePreBillDocDate"]).ToString("d"));
				newRow["BillToPartyId"] = r2["BillToPartyId"];
				newRow["BillToPartyName"] = r2["BillToPartyName"];
				newRow["GpStatus"] = r2["GpStatus"];
				newRow["WareHouseName"] = r2["WareHouseName"];
				newRow["ItemId"] = r2["ItemId"];
				newRow["ItemCode"] = r2["ItemCode"];
				newRow["ItemName"] = r2["ItemName"];
				newRow["Uom"] = r2["Uom"];
				newRow["ItemCondition"] = r2["ItemCondition"];
				newRow["ChallanQty"] = r2["ChallanQty"];
				newRow["VendorSupplierId"] = r2["VendorSupplierId"];
				newRow["VendorSupplierName"] = r2["VendorSupplierName"];
				newRow["ReferencePartyId"] = r2["ReferencePartyId"];
				newRow["ReferencePartyName"] = r2["ReferencePartyName"];
				newRow["PartyBillNo"] = r2["PartyBillNo"];
				newRow["PartyBillDate"] = r2["PartyBillDate"];
				newRow["PurchaseDemandQty"] = r2["PurchaseDemandQty"];
				newRow["PurchaseOrderQty"] = r2["PurchaseOrderQty"];
				newRow["PurchasePreBillQty"] = r2["PurchasePreBillQty"];
				newRow["ChallanQtyUsedInGrn"] = r2["ChallanQtyUsedInGrn"];
				newRow["BalChallanQty"] = r2["BalChallanQty"];
				newRow["RemarksDetail"] = r2["RemarksDetail"];
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
				"Id", "DetailId", "PurchaseDemandHeaderId", "PurchaseDemandDetailId", "PurchaseOrderHeaderId", "PurchaseOrderDetailId", "VendorSupplierId", "ReferencePartyId", "ItemId", "PurchaseOrderNo",
				"PurchaseOrderDate", "BillToPartyId", "GpStatus"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			GridEX_Helper.SetColumnWidth(grdDetail, "WareHouseName", 130);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCode", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemName", Constants.InventoryConstants.ItemName - 20);
			GridEX_Helper.SetColumnWidth(grdDetail, "Uom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "ChallanQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "ChallanQtyUsedInGrn", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "BalChallanQty", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "VendorSupplierName", Constants.InventoryConstants.SupplierName);
			GridEX_Helper.SetColumnWidth(grdDetail, "ReferencePartyName", Constants.InventoryConstants.SupplierName);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseDemandDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchaseOrderDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillNo", 70);
			GridEX_Helper.SetColumnWidth(grdDetail, "PurchasePreBillDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "BillToPartyName", Constants.InventoryConstants.SupplierName);
			GridEX_Helper.SetColumnWidth(grdDetail, "PartyBillNo", 65);
			GridEX_Helper.SetColumnWidth(grdDetail, "PartyBillDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "RemarksDetail", Constants.InventoryConstants.RemarksConstants);
			GridEX_Helper.AddSelectButton(grdDetail, "Select", 30, 0);
			grdDetail.FrozenColumns = 1;
			GridEX_Helper.CheckAllRows(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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
			GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
			if (checkedRows == null || checkedRows.Length == 0)
			{
				MessageBox.Show("Check the row first in Detail Grid");
				return;
			}
			dtLoader = dtHistory.Clone();
			List<int> detailIds = new List<int>();
			int baseId = Conversion.ToInt(checkedRows[0].Cells["Id"].Value);
			GridEXRow[] array = checkedRows;
			foreach (GridEXRow row in array)
			{
				if (Conversion.ToString(row.Cells["GpStatus"].Value) != "Accepted")
				{
					MessageBox.Show("Only Accepted Gate Pass can be selected.");
					return;
				}
				if (Conversion.ToInt(row.Cells["Id"].Value) != baseId)
				{
					MessageBox.Show("Sorry! You can Only Check rows which have the same DocNo");
					return;
				}
				int detailId = Conversion.ToInt(row.Cells["DetailId"].Value);
				detailIds.Add(detailId);
			}
			if (detailIds.Count == 0)
			{
				MessageBox.Show("No valid rows selected.");
				return;
			}
			List<DataRow> rows = (from r in dtHistory.AsEnumerable()
				where detailIds.Contains(r.Field<int>("DetailId"))
				select r).ToList();
			if (rows.Any())
			{
				dtLoader = rows.CopyToDataTable();
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

	private void frmPendingDeliveryChallanLoader_KeyDown(object sender, KeyEventArgs e)
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

	private void txtDocNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmPendingDeliveryChallanLoader));
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadSelectedRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbBillToParty = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(973, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.BtnLoadSelectedRows, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(973, 25);
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
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(213, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Delivery Challan";
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(973, 190);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_SelectionChanged);
		this.grd.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grd_RowCheckStateChanged);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToParty);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel2.Controls.Add(this.txtDocNoTo);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.txtDocNoFrom);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.ToDate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(973, 66);
		this.panel2.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(337, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 36;
		this.label5.Text = "Bill To Party";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(337, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 36;
		this.label6.Text = "Item Name";
		this.CmbBillToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Location = new System.Drawing.Point(407, 6);
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
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
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
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(407, 35);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 5;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(257, 37);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(199, 41);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "To Doc";
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(257, 8);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrom_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(199, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "From Doc";
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(69, 37);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(125, 22);
		this.ToDate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(69, 8);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(125, 22);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 41);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(620, 35);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(57, 26);
		this.btngrnlod.TabIndex = 6;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 116);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(973, 25);
		this.panel3.TabIndex = 31;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 354);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(973, 257);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 331);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(973, 23);
		this.panel4.TabIndex = 35;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(973, 611);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmPendingDeliveryChallanLoader";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPendingDeliveryChallanLoader_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
