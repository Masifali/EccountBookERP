using System;
using System.Collections.Generic;

namespace Architecture.Model.Mfg;

public class productionRouting
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public string operationTime { get; set; }

	public decimal CapacityQty { get; set; }

	public decimal operatingCost { get; set; }

	public int ActionId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int productionRoutingId { get; set; }

	public int RevisionNo { get; set; }

	public int sequenceNo { get; set; }

	public int WorkStationOperationId { get; set; }

	public int RoutingNameId { get; set; }

	public string routingName { get; set; }

	public virtual string WorkStationName { get; set; }

	public virtual string OperationName { get; set; }

	public virtual List<productionRouting> ProductionRoutingList { get; set; }
}
