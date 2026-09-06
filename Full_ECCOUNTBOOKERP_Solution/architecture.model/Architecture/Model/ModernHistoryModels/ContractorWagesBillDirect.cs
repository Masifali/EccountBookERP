using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class ContractorWagesBillDirect
{
	public ContractorWagesBillDirect_Header BillHeader { get; set; }

	public List<ContractorWagesBillDirect_Detail> BillDetaiList { get; set; }
}
