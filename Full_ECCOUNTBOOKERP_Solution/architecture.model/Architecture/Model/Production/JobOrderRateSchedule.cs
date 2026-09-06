using System;

namespace Architecture.Model.Production;

public class JobOrderRateSchedule
{
	public DateTime EffectiveDate { get; set; }

	public double ItemRate { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int JobOrderId { get; set; }

	public int TransTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }
}
