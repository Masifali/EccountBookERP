using System.Collections.Generic;

namespace Architecture.Model.MultiLanguages;

public class SupplierCustomerMultilingo
{
	public int Id { get; set; }

	public int MultilanguagesId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string CompanyName { get; set; }

	public virtual List<SupplierCustomerMultilingo> SupplierCustomerMultilingoList { get; set; }
}
