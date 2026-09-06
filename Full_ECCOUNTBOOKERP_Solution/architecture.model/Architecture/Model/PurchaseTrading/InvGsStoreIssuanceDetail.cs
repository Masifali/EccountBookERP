namespace Architecture.Model.PurchaseTrading;

public class InvGsStoreIssuanceDetail
{
	public double IssueQty { get; set; }

	public int AssetsId { get; set; }

	public int IssuanceTypeId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int DepartmentId { get; set; }

	public int Id { get; set; }

	public int LineId { get; set; }

	public int InvGsStoreIssuanceHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomSchId { get; set; }

	public int ItemConditionId { get; set; }

	public int Warehouseid { get; set; }

	public int SupplierCustomerId { get; set; }

	public int EmployeeId { get; set; }

	public int DrAccountId { get; set; }

	public int BagTypeId { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public virtual double QtyBal { get; set; }

	public string ReamarksDetail { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocDetailId { get; set; }

	public int ActionTypeId { get; set; }

	public int DepartmentToId { get; set; }

	public int DepartRequestDetailId { get; set; }

	public int DepartRequestId { get; set; }

	public int WorkOrderIdId { get; set; }

	public int WorkStationFromId { get; set; }

	public int WorkStationToId { get; set; }

	public int ContractScheduleId { get; set; }

	public int RackId { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string IssuanceType { get; set; }

	public virtual string DepartmentName { get; set; }

	public virtual string ToDepartmentName { get; set; }

	public virtual int DepartmentRequestNo { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double RowBalanceQty { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string UomCode { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string DebitAc { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string WorkStationFrom { get; set; }

	public virtual string WorkStationTo { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string ContractScheduleNo { get; set; }

	public virtual string rackName { get; set; }
}
