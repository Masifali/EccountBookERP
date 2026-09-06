using System;

namespace Architecture.Model.Service;

public class ExImClearingAgentBillDetail
{
	public DateTime DocDate { get; set; }

	public DateTime EffectedDateD { get; set; }

	public DateTime ValidUpTo { get; set; }

	public double Amount { get; set; }

	public double OtherChargesAmount { get; set; }

	public double FcyAmount { get; set; }

	public double ChargesRate { get; set; }

	public double ExhangeRate { get; set; }

	public double ItemRate { get; set; }

	public double Qty { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double TotalAmount { get; set; }

	public int BillHeaderId { get; set; }

	public int CurrencyId { get; set; }

	public int DebitAccountId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int TaxNameId { get; set; }

	public int UomId { get; set; }

	public string Description { get; set; }

	public string SupplierRefNo { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CurrencyName { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string UomName { get; set; }
}
