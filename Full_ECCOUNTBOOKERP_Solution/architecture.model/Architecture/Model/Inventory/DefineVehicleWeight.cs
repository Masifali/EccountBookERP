using System;

namespace Architecture.Model.Inventory;

public class DefineVehicleWeight
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double VehicleNetWeight { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string Remarks { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }
}
