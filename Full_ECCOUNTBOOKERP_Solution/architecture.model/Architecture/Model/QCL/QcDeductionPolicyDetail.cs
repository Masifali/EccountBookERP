namespace Architecture.Model.QCL;

public class QcDeductionPolicyDetail
{
	public decimal dedOnRateRs { get; set; }

	public decimal dedWeight { get; set; }

	public decimal RangeFrom { get; set; }

	public decimal RangeTo { get; set; }

	public double dedOnQtyKg { get; set; }

	public int ActionTypeId { get; set; }

	public int AnalysisParameterId { get; set; }

	public int dedQtyUom { get; set; }

	public int DeductionParameterId { get; set; }

	public int dedWtUom { get; set; }

	public int QcDeductionPolicyDetailId { get; set; }

	public int QcDeductionPolicyHeaderId { get; set; }

	public int SortNo { get; set; }

	public string DeductionParameterName { get; set; }

	public virtual string AnalysisParameter { get; set; }
}
