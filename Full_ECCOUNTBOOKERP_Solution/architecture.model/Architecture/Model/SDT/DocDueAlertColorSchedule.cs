using System;
using System.Collections.Generic;

namespace Architecture.Model.SDT;

public class DocDueAlertColorSchedule
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BeforeRangeFrom { get; set; }

	public int BeforeRangeTo { get; set; }

	public int ChartOfDocumentId { get; set; }

	public int ColorGroupId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int LevelNameId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SeqNo { get; set; }

	public long DocDueAlertColorScheduleId { get; set; }

	public string ColorCode { get; set; }

	public virtual string ColorGroup { get; set; }

	public virtual string levelName { get; set; }

	public virtual string documentName { get; set; }

	public virtual List<DocDueAlertColorSchedule> DocDueAlertColorScheduleList { get; set; }
}
