using System;

namespace Architecture.PSDSMS;

public class Message : EventArgs
{
	public int MessageId { get; set; }

	public DateTime? MessageLogDate { get; set; }

	public string MessageText { get; set; }

	public string Msg { get; set; }

	public string Mobile { get; set; }

	public string Status { get; set; }

	public string PassCode { get; set; }
}
