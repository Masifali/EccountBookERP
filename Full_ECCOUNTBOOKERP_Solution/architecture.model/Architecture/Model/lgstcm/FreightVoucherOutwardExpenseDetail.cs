namespace Architecture.Model.lgstcm;

public class FreightVoucherOutwardExpenseDetail
{
	public double AddAmount { get; set; }

	public double LessAmount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int FreightVoucherOutwardExpenseDetailId { get; set; }

	public int FreightVoucherOutwardId { get; set; }

	public int LogiticOtherChargesItemsId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherChargesItemDescription { get; set; }
}
