using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class fcyExRateForwardBooking
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovadDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FinalMaturityDate { get; set; }

	public DateTime FirstMaturityDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OptionalPeriod { get; set; }

	public DateTime DateOfPurchase { get; set; }

	public decimal BookingRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal HcyAmount { get; set; }

	public int ActionId { get; set; }

	public int ApprovedById { get; set; }

	public int BankId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int FcyId { get; set; }

	public int FinancialYearId { get; set; }

	public int FixedDays { get; set; }

	public int HcyId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OptionalDays { get; set; }

	public int OrganizationId { get; set; }

	public int TotalDays { get; set; }

	public string BankContractNo { get; set; }

	public string ScreenName { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
