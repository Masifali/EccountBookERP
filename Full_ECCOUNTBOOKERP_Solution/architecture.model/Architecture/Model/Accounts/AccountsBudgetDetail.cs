using System;

namespace Architecture.Model.Accounts;

public class AccountsBudgetDetail
{
	public decimal BudgetAmount { get; set; }

	public decimal BudgetPrcnt { get; set; }

	public int AccountsBudgetHeaderId { get; set; }

	public DateTime BudgetMonth { get; set; }

	public int Id { get; set; }
}
