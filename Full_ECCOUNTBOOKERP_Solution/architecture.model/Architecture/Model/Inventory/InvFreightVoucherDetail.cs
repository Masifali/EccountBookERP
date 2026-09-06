namespace Architecture.Model.Inventory;

public class InvFreightVoucherDetail
{
	public decimal LessAny { get; set; }

	public decimal ScaleCharges { get; set; }

	public decimal TotalPayableAmount { get; set; }

	public decimal ShortageAmount { get; set; }

	public decimal DeductedAmount { get; set; }

	public decimal ChargeToParty { get; set; }

	public double OtherDeduction { get; set; }

	public decimal RateForShortage { get; set; }

	public double AdvanceByParty { get; set; }

	public double AdvanceByFactory { get; set; }

	public double AddLessAmount { get; set; }

	public int ActionId { get; set; }

	public int BiltyFreight { get; set; }

	public int FreightVoucherId { get; set; }

	public int Id { get; set; }

	public int SortNo { get; set; }

	public int DebitAccountId { get; set; }

	public int CustomGroupId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string AccountTitle { get; set; }
}
