using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.FeedMill.Production;

public class ProductionHeader
{
	public bool IsApproved { get; set; }

	public bool AutoUpdate { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int WIPWarehouseId { get; set; }

	public int WIPGLAccountID { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int PlantId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int WorkOrderId { get; set; }

	public int ProductionTypeId { get; set; }

	public int ShiftId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string ProductionNo { get; set; }

	public string RemarksHeader { get; set; }

	public bool IsUploaded { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual List<ProductionOutPutDetail> ProductionOutPutDetailList { get; set; }

	public virtual List<ProductionOverHeadsDetail> ProductionOverHeadsDetailList { get; set; }

	public virtual List<ProductionInputDetail> ProductionInputDetailList { get; set; }

	public virtual List<ProductionConsumptionDetail> ProductionConsumptionDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
