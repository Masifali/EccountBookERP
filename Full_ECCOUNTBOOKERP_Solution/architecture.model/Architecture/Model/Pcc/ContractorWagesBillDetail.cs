namespace Architecture.Model.pcc;

public class ContractorWagesBillDetail
{
	public bool FreeOfCost { get; set; }

	public decimal Amount { get; set; }

	public decimal AddLess { get; set; }

	public decimal NetAmount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal ItemNetWeight { get; set; }

	public decimal VarientEquivalent { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public int CurrencyId { get; set; }

	public int ActionTypeId { get; set; }

	public int ContractorId { get; set; }

	public int ContractorWagesActivityId { get; set; }

	public int ContractorWagesBillId { get; set; }

	public int ContractorWagesRateScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int ParentUomId { get; set; }

	public int WareHouseFromId { get; set; }

	public int WareHouseToId { get; set; }

	public string RemarksDetail { get; set; }

	public int RefDocNo { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int WagesTypeId { get; set; }

	public virtual string ContractorName { get; set; }

	public virtual string ContractorWagesActivity { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string WareHouseFrom { get; set; }

	public virtual string WareHouseTo { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string ContractorWagesScheduleActivity { get; set; }
}
