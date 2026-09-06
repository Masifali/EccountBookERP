using System.Collections.Generic;

namespace Architecture.Model.Common;

public static class ConstantData
{
	public static List<AccountCategoryData> AccountCategory()
	{
		return new List<AccountCategoryData>
		{
			new AccountCategoryData
			{
				AccountCategory = "Staff"
			},
			new AccountCategoryData
			{
				AccountCategory = "Supplier"
			},
			new AccountCategoryData
			{
				AccountCategory = "COA"
			}
		};
	}
}
