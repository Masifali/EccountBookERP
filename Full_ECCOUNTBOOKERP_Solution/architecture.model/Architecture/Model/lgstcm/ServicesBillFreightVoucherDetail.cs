namespace Architecture.Model.lgstcm;

public class ServicesBillFreightVoucherDetail
{
	public int ServicesBillFreightVoucherDetailId { get; set; }

	public int ServicesBillHeaderId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int FreightVoucherOutwardId { get; set; }

	public int GatePassOutwardId { get; set; }

	public int DeliveryOrderId { get; set; }

	public double Qty { get; set; }

	public double Weight { get; set; }

	public double FreightRate { get; set; }

	public int RateBaseUomId { get; set; }

	public double QtyForRate { get; set; }

	public double BiltyFreight { get; set; }

	public double OtherCharges { get; set; }

	public double TotalBiltyFreight { get; set; }

	public double ThisRowFreightAmount { get; set; }

	public string Remarks { get; set; }

	public virtual string RateBaseUom { get; set; }

	public virtual double RateBaseUomEquivalent { get; set; }
}
