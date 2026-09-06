namespace Architecture.WebHelper.Entity;

public class ReportSectionObject
{
	public string sectionName { get; set; }

	public decimal sectionheight { get; set; }

	public ReportSectionObject(string sectionName, decimal sectionheight)
	{
		this.sectionName = sectionName;
		this.sectionheight = sectionheight;
	}
}
