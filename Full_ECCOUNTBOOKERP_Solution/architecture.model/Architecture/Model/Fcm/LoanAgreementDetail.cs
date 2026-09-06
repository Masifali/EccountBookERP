namespace Architecture.Model.Fcm;

public class LoanAgreementDetail
{
	public decimal fAmount { get; set; }

	public decimal fStockQty { get; set; }

	public decimal QtyRate { get; set; }

	public int ActionTypeId { get; set; }

	public int CollateralTypeId { get; set; }

	public int LoanSecutiryTypeId { get; set; }

	public int SortNo { get; set; }

	public long LoanAgreementDetailId { get; set; }

	public long LoanAgreementHeaderId { get; set; }

	public string LoanSecurityDescription { get; set; }

	public virtual string LoanSecutiryType { get; set; }

	public virtual string CollateralType { get; set; }
}
