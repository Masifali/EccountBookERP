using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Inventory.FromWise_Helper_Methods;

public class frmPurchaseInvoiceStoreManagement_Helper
{
	public static void InitializeDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderNo", typeof(int));
			dtGrid.Columns.Add("PurchaseDemandDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("PurchaseDemandId", typeof(int));
			dtGrid.Columns.Add("PurchaseDemandNo", typeof(int));
			dtGrid.Columns.Add("PreBillDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("PreBillId", typeof(int));
			dtGrid.Columns.Add("PreBillNo", typeof(int));
			dtGrid.Columns.Add("DeliveryChallanDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("DeliveryChallanId", typeof(int));
			dtGrid.Columns.Add("DeliveryChallanNo", typeof(int));
			dtGrid.Columns.Add("InvGrnDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("InvGrnId", typeof(int));
			dtGrid.Columns.Add("InvGrnDetailId", typeof(int));
			dtGrid.Columns.Add("GrnNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("WareHouseName");
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("ItemUomId", typeof(int));
			dtGrid.Columns.Add("UOMCodeItem");
			dtGrid.Columns.Add("ItemConditionId", typeof(int));
			dtGrid.Columns.Add("ItemCondition", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("ItemAmountWithoutDiscount", typeof(decimal));
			dtGrid.Columns.Add("DiscountAmount", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("TaxNameId", typeof(int));
			dtGrid.Columns.Add("TaxName");
			dtGrid.Columns.Add("TaxPercent", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Freights", typeof(double));
			dtGrid.Columns.Add("ExpenseAmount", typeof(double));
			dtGrid.Columns.Add("PoAttachments", typeof(int));
			dtGrid.Columns.Add("DemandAttachments", typeof(int));
			dtGrid.Columns.Add("PreBillAttachments", typeof(int));
			dtGrid.Columns.Add("DeliveryChallanAttachments", typeof(int));
			dtGrid.Columns.Add("GrnAttachments", typeof(int));
			dtGrid.Columns.Add("RackId", typeof(int));
			dtGrid.Columns.Add("RackName", typeof(string));
		}
	}

	public static void FillDetailFromListCommonForReadById(InvPurchaseInvoice invPI, DataTable dtGrid, bool AddExtraColumns = false)
	{
		if (invPI != null && dtGrid != null && invPI.invPurchaseInvoiceDetailList != null)
		{
			for (int i = 0; i < invPI.invPurchaseInvoiceDetailList.Count; i++)
			{
				InvPurchaseInvoiceDetail item = invPI.invPurchaseInvoiceDetailList[i];
				DataRow row = dtGrid.NewRow();
				row["Id"] = item.Id;
				row["InvGrnId"] = item.InvGrnId;
				row["InvGrnDetailId"] = item.InvGrnDetailId;
				row["InvGrnDocumentTypeId"] = item.GrnDocumentTypeId;
				row["GrnNo"] = item.GrnNo;
				row["PurchaseOrderDocumentTypeId"] = item.PurchaseOrderDocumentTypeId;
				row["PurchaseOrderId"] = item.PurchaseOrderId;
				row["PurchaseOrderNo"] = item.PurchaseOrder;
				row["PurchaseDemandDocumentTypeId"] = item.PurchaseDemandDocumentTypeId;
				row["PurchaseDemandId"] = item.PurchaseDemandId;
				row["PurchaseDemandNo"] = item.PurchaseDemandNo;
				row["PreBillDocumentTypeId"] = item.PreBillDocumentTypeId;
				row["PreBillId"] = item.PreBillId;
				row["PreBillNo"] = item.PreBillNo;
				row["DeliveryChallanDocumentTypeId"] = item.DeliveryChallanDocumentTypeId;
				row["DeliveryChallanId"] = item.DeliveryChallanId;
				row["DeliveryChallanNo"] = item.DeliveryChallanNo;
				row["ItemId"] = item.ItemId;
				row["ItemName"] = item.ItemName;
				row["ItemUomId"] = item.ItemUOMId;
				row["UOMCodeItem"] = item.UOMCodeItem;
				row["ItemQty"] = item.ItemQty;
				row["WarehouseId"] = item.WarehouseId;
				row["WareHouseName"] = item.WareHouseName;
				row["ItemConditionId"] = item.ItemConditionId;
				row["ItemCondition"] = item.ItemCondition;
				row["RackId"] = item.RackId;
				row["RackName"] = item.rackName;
				row["Rate"] = Math.Round(item.ItemRate, 2);
				row["ItemAmountWithoutDiscount"] = Math.Round(item.ItemAmountWithoutDiscount, 2);
				row["DiscountAmount"] = Math.Round(item.DiscountAmount, 2);
				row["ItemAmount"] = Math.Round(item.ItemAmount, 2);
				row["TaxNameId"] = item.TaxNameId;
				row["TaxName"] = item.TaxDescriptions;
				row["TaxPercent"] = item.TaxPercent;
				row["TaxAmount"] = item.TaxAmount;
				row["BillAmount"] = item.BillAmount;
				row["Freights"] = item.FreightAmount;
				row["ExpenseAmount"] = item.ExpenseAmount;
				row["PoAttachments"] = item.NoOfAttachmentsPO;
				row["DemandAttachments"] = item.NoOfAttachmentsPD;
				row["PreBillAttachments"] = item.NoOfAttachmentsPreBill;
				row["DeliveryChallanAttachments"] = item.NoOfAttachmentsDC;
				row["GrnAttachments"] = item.NoOfAttachmentsGrn;
				dtGrid.Rows.Add(row);
			}
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null, int BaseDocumentTypeId = 0)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "PurchaseOrderDocumentTypeId", "PurchaseOrderId", "PurchaseDemandDocumentTypeId", "PurchaseDemandId", "PreBillDocumentTypeId", "PreBillId", "DeliveryChallanDocumentTypeId", "DeliveryChallanId", "InvGrnDocumentTypeId",
				"InvGrnId", "InvGrnDetailId", "ItemId", "ItemUomId", "WarehouseId", "DemondId", "Journal", "TaxNameId", "ItemConditionId", "RackId",
				"ExpenseAmount"
			};
			switch (BaseDocumentTypeId)
			{
			case 1:
				hiddenColumns.AddRange(new string[6] { "PurchaseDemandNo", "PreBillNo", "DeliveryChallanNo", "DemandAttachments", "PreBillAttachments", "DeliveryChallanAttachments" });
				break;
			case 2:
				hiddenColumns.AddRange(new string[6] { "PurchaseOrderNo", "PreBillNo", "DeliveryChallanNo", "PoAttachments", "PreBillAttachments", "DeliveryChallanAttachments" });
				break;
			case 3:
				hiddenColumns.AddRange(new string[2] { "PurchaseOrderNo", "PoAttachments" });
				break;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["UOMCodeItem"].Caption = "UOM";
			gridEX.RootTable.Columns["PoAttachments"].ColumnType = (ColumnType)5;
			gridEX.RootTable.Columns["DemandAttachments"].ColumnType = (ColumnType)5;
			gridEX.RootTable.Columns["PreBillAttachments"].ColumnType = (ColumnType)5;
			gridEX.RootTable.Columns["DeliveryChallanAttachments"].ColumnType = (ColumnType)5;
			gridEX.RootTable.Columns["GrnAttachments"].ColumnType = (ColumnType)5;
			gridEX.RootTable.Columns["WareHouseName"].Width = 150;
			gridEX.RootTable.Columns["ItemName"].Width = 180;
			gridEX.RootTable.Columns["WareHouseName"].Position = gridEX.RootTable.Columns["ItemName"].Position + 1;
			gridEX.RootTable.Columns["RackName"].Position = gridEX.RootTable.Columns["WareHouseName"].Position + 1;
			gridEX.RootTable.Columns["UOMCodeItem"].Width = 70;
			gridEX.RootTable.Columns["ItemCondition"].Width = 100;
			gridEX.RootTable.Columns["RackName"].Width = 120;
			gridEX.RootTable.Columns["ItemQty"].Width = 70;
			gridEX.RootTable.Columns["Rate"].Width = 70;
			gridEX.RootTable.Columns["ItemAmount"].Width = 100;
			gridEX.RootTable.Columns["TaxName"].Width = 120;
			gridEX.RootTable.Columns["TaxPercent"].Width = 65;
			gridEX.RootTable.Columns["TaxAmount"].Width = 90;
			gridEX.RootTable.Columns["BillAmount"].Width = 110;
			gridEX.RootTable.Columns["Freights"].Width = 100;
			gridEX.RootTable.Columns["ExpenseAmount"].Width = 100;
			gridEX.RootTable.Columns["GrnNo"].Width = 70;
			gridEX.RootTable.Columns["PreBillNo"].Width = 70;
			gridEX.RootTable.Columns["DeliveryChallanNo"].Width = 70;
			gridEX.RootTable.Columns["PurchaseDemandNo"].Width = 70;
			gridEX.RootTable.Columns["PurchaseOrderNo"].Width = 70;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}
