using System;

namespace Architecture.Model.Import;

public class SeaAirport
{
	public DateTime CreatedOn { get; set; }

	public DateTime LastModifiedOn { get; set; }

	public int AppActionId { get; set; }

	public int portTypeId { get; set; }

	public int seaAirPortId { get; set; }

	public long AppUserLogId { get; set; }

	public long CreatedUserId { get; set; }

	public long LastModifiedUserId { get; set; }

	public string countryName { get; set; }

	public string portName { get; set; }

	public virtual string PortType { get; set; }
}
