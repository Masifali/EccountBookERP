using System;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeRevenueExpenseScheduleDetail
{
	public DateTime EffectiveDate { get; set; }

	public decimal Rate { get; set; }

	public decimal RateUom { get; set; }

	public int ActionTypeId { get; set; }

	public int CalculationParameterId { get; set; }

	public int ChargesIncentiveId { get; set; }

	public int CommTradeRevenueExpenseScheduleId { get; set; }

	public int DaysFrom { get; set; }

	public int DaysTo { get; set; }

	public int Id { get; set; }

	public int IsRevenueExpense { get; set; }

	public int PaymentTermId { get; set; }

	public int SortNo { get; set; }

	public int TransactionTypeId { get; set; }
}
