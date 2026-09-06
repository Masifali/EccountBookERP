using System;

namespace Architecture.Model.Export;

public class FIOpening
{
	public DateTime EntryDate { get; set; }

	public DateTime FIDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OpeningDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FIAmount { get; set; }

	public decimal LcyAmount { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FcyId { get; set; }

	public int BankId { get; set; }

	public int ConsigneeId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string FINumber { get; set; }

	public string Remarks { get; set; }

	public virtual string FcyCode { get; set; }

	public virtual string EntryUser { get; set; }

	public virtual string ModifyUser { get; set; }
}
