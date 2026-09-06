namespace Architecture.Model.Export;

public class InvLabPreProductionExportLotInspectionDetail
{
	public decimal Weight { get; set; }

	public int ContractId { get; set; }

	public int ContractScheduleId { get; set; }

	public int InvoiceId { get; set; }

	public int Id { get; set; }

	public int InvLabPreProductionExportLotInspectionHeaderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string SealNoBuyer { get; set; }

	public string SealNoShipper { get; set; }

	public string Status { get; set; }

	public string SubLot { get; set; }

	public string SubRemarks { get; set; }

	public virtual string SupplierCustomer { get; set; }

	public virtual string ContractNo { get; set; }

	public virtual string ContractScheduleNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual int ReferredStatusId { get; set; }
}
