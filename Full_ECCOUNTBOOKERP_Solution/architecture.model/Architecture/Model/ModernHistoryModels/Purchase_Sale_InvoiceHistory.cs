using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class Purchase_Sale_InvoiceHistory
{
	public Prucahse_Slae_InvoiceHistoryHeader InvoiceHeader { get; set; }

	public List<Puchase_Sale_InvoiceHistoryDetail> InvoiceDetail { get; set; }

	public bool ChargeToProductDetailPresent { get; set; }

	public List<ChargeToProductDetailForInvoiceHistory> ChargeToProductDetail { get; set; }

	public bool Customer_Supplier_AddLessDetailPresent { get; set; }

	public List<Customer_Supplier_AddLessDetailForInvoiceHistory> Customer_Supplier_AddLessDetail { get; set; }

	public bool OtherExpensesDetailPresent { get; set; }

	public List<OtherExpensedDetailForInvoiceHistory> OtherExpensesDetail { get; set; }

	public bool EmptyBagsDetailPresent { get; set; }

	public List<EmtpyBagsDetailForInvoiceHistory> EmptyBagsDetail { get; set; }
}
