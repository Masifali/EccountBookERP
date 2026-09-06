namespace Architecture.Model.CommissionAgent;

public class purchaseOrderCommissionDetail
{
	public decimal commissionAmount { get; set; }

	public decimal commissionRate { get; set; }

	public int agentTypeId { get; set; }

	public int commissionAgentId { get; set; }

	public int commissionTypeId { get; set; }

	public int purchaseOrderCommissionDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int rateUomId { get; set; }

	public int sortNo { get; set; }

	public string commissionRemarks { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual string AgentType { get; set; }

	public virtual string CommissionType { get; set; }

	public virtual double RateUom { get; set; }
}
