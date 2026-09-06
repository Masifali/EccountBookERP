using System;

namespace Architecture.Model.Production;

public class ProductionManualCostingInputDetail
{
	public decimal FinalAmount { get; set; }

	public decimal InputAmount { get; set; }

	public decimal InputRate { get; set; }

	public decimal Weight { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ActionTypeId { get; set; }

	public int ByProductDetailId { get; set; }

	public int Id { get; set; }

	public int InvFoodProductionId { get; set; }

	public int ItemId { get; set; }

	public int ProductionManualCostingId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual DateTime? RefDocDate { get; set; }

	public virtual DateTime? ProductionDate { get; set; }

	public virtual int ProductionNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }
}
