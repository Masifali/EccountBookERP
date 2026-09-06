using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class BoilerConsumption
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OffTime { get; set; }

	public DateTime OnTime { get; set; }

	public decimal DrierSteamUse { get; set; }

	public decimal EndReading { get; set; }

	public decimal MoisturePaddy { get; set; }

	public decimal QtyBags { get; set; }

	public decimal StartReading { get; set; }

	public decimal SteamWeight { get; set; }

	public decimal TotalWeight { get; set; }

	public decimal TotalWeightMann { get; set; }

	public decimal WaterWeight { get; set; }

	public decimal WtPerVehcile { get; set; }

	public double NoOfVehicle { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int Uom { get; set; }

	public int BoilerId { get; set; }

	public string Remarks { get; set; }

	public int VehicleType { get; set; }

	public virtual List<BoilerConsumption> BoilerConsumptionsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
