using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class RiceOrdersHistory
{
	public OrderHeaderModel OrderHeader { get; set; }

	public List<OrderDetailModel> OrderDetail { get; set; }

	public List<OrderEmptyBagsModel> OrderEmptyBags { get; set; }

	public List<OrderExpensesModel> OrderExpenses { get; set; }
}
