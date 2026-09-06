namespace Architecture.Model.HRM.EmployeeManagement;

public class tblEmployeeAddressDetail
{
	public int EmployeeId { get; set; }

	public int AddressTypeId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int CountryId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ProvinceId { get; set; }

	public string AddressDetail { get; set; }

	public virtual string CountryName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string StateProvince { get; set; }

	public virtual string AddressType { get; set; }
}
