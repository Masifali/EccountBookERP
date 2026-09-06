using System.Data;

namespace Architecture.WebHelper.Entity;

public class SubReportObject
{
	public DataTable dtSubReport { get; set; }

	public string SubReportName { get; set; }

	public SubReportObject(DataTable dtSubReport, string SubReportName)
	{
		this.dtSubReport = dtSubReport;
		this.SubReportName = SubReportName;
	}
}
