using System;

namespace Architecture.Model.Inventory;

public class GrainMarket
{
	public int Id { get; set; }

	public string GrainMarketName { get; set; }

	public string GrainMarketDescription { get; set; }

	public int StateProvinceId { get; set; }

	public int CityId { get; set; }

	public string Address { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchId { get; set; }

	public int ProjectId { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }
}
