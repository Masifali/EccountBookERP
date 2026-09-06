using System;

namespace Architecture.Model.Inventory;

public class InventoryBidding
{
	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public int BiddingNumber { get; set; }

	public DateTime BiddingDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int GrainMarketId { get; set; }

	public int SupplierId { get; set; }

	public int BrokerId { get; set; }

	public int ItemId { get; set; }

	public int CropYearId { get; set; }

	public double Moisture { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public int PostUser { get; set; }

	public bool IsApproved { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchId { get; set; }

	public int ProjectId { get; set; }

	public int FinancialYearId { get; set; }

	public string Status { get; set; }

	public virtual string GrainMarketName { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string BrokerName { get; set; }

	public virtual string CropYear { get; set; }
}
