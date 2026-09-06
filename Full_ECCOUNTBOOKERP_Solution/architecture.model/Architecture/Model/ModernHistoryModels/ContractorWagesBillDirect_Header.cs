using System;

namespace Architecture.Model.ModernHistoryModels;

public class ContractorWagesBillDirect_Header
{
	public int Id { get; set; }

	public DateTime DocDate { get; set; }

	public int DocNo { get; set; }

	public string RemarksHeader { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public string EntryUserProfileImageUrl { get; set; }

	public string ModifyUserProfileImageUrl { get; set; }

	public double TotalBags { get; set; }

	public double TotalAmount { get; set; }

	public int AttachmentsCount { get; set; }
}
