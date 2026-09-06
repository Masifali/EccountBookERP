using System.Collections.Generic;

namespace Architecture.Model.Api;

public class AccountTitlesModel
{
	public string AccountTitle { get; set; } = string.Empty;


	public List<VoucherNoModel> Data { get; set; } = new List<VoucherNoModel>
	{
		new VoucherNoModel()
	};

}
