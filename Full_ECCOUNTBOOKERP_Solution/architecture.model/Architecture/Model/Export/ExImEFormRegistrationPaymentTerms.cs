namespace Architecture.Model.Export;

public class ExImEFormRegistrationPaymentTerms
{
	public double FcAmount { get; set; }

	public double PaymentPercent { get; set; }

	public int DaDays { get; set; }

	public int ExImEFormRegistrationId { get; set; }

	public int ExmLcPaymentTermId { get; set; }

	public int Id { get; set; }

	public virtual string PaymentTermName { get; set; }
}
