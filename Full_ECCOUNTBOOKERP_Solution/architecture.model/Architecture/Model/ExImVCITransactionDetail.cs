namespace Architecture.Model;

public class ExImVCITransactionDetail
{
	public int ExImVCITransactionId { get; set; }

	public int Id { get; set; }

	public int InspectionStatus { get; set; }

	public int VCICategoryId { get; set; }

	public int VCIParameter { get; set; }

	public string Remarks { get; set; }

	public virtual string VciParameterDescription { get; set; }

	public virtual string VciCategoryDescription { get; set; }
}
