using System;

namespace Architecture.Model;

public class ReportConfig
{
	public byte ActionTypeId { get; set; }

	public bool IsActive { get; set; }

	public bool IsSubReport { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ReportConfigId { get; set; }

	public int ReportSeqNo { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public string ReportFileName { get; set; }

	public string ReportFolder { get; set; }

	public string ReportIconURL { get; set; }

	public string ReportProcedureName { get; set; }

	public string ReportShortName { get; set; }

	public string ReportTitle { get; set; }

	public string TargetFunctionName { get; set; }

	public string TargetSource { get; set; }
}
