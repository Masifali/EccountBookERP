using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Steel.Production;

public class InvFoodProduction
{
	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public DateTime DocDate { get; set; }

	public int DocCode { get; set; }

	public int ProductionJobOrderId { get; set; }

	public string ProductionJobOrderNo { get; set; }

	public int WIPAccountId { get; set; }

	public int WIPItemId { get; set; }

	public string MainRemarks { get; set; }

	public int PlantId { get; set; }

	public int ApprovedUserId { get; set; }

	public DateTime ApprovedDate { get; set; }

	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public int FinancialYearId { get; set; }

	public int ActionId { get; set; }

	public bool? PendingForView { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual List<InvFoodProductionDetail> invFoodProductionDetails { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
