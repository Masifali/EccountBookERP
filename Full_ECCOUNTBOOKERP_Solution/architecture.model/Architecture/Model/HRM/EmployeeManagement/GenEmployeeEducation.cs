using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class GenEmployeeEducation
{
	public bool PostState { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime PassingYear { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime StartYear { get; set; }

	public double CGPA { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int DegreeId { get; set; }

	public int EmployeeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string DegreeTitle { get; set; }

	public string Institute { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string DegreeName { get; set; }
}
