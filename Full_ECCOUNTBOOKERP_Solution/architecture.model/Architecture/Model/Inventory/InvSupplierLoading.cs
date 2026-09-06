using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvSupplierLoading
{
	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime ArrivalDate { get; set; }

	public DateTime DepartureDate { get; set; }

	public decimal Freight { get; set; }

	public decimal Quantity { get; set; }

	public decimal SupplierWbWeight { get; set; }

	public int AgentBrokerId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int SupplierId { get; set; }

	public string BiltyNo { get; set; }

	public string DriverName { get; set; }

	public string Driver1CellNo { get; set; }

	public string Driver2CellNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleImage { get; set; }

	public int CityId { get; set; }

	public int TransitDays { get; set; }

	public int FinancialYearId { get; set; }

	public virtual List<InvSupplierLoadingDetail> InvSupplierLoadingDetailList { get; set; }
}
