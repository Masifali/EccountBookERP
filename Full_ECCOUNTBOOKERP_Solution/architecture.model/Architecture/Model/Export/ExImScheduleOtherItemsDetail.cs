using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImScheduleOtherItemsDetail
{
	public double Qty { get; set; }

	public int ContractId { get; set; }

	public int ContractOtherItemId { get; set; }

	public int ContractScheduleId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public string Remarks { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public virtual List<ExImScheduleOtherItemsDetail> ExImScheduleOtherItemsDetailList { get; set; }
}
