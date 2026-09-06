using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseInvoiceReverseActivityDetail
{
	public List<PurchaseInvoiceReverseActivityDetail_InvoiceInfo> InvoiceInfo { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_GrnInfo> GrnInfoList { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_WagesInfo> WagesInfoList { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_LabInfo> LabInfoList { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_WbId> WbInfoList { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_GpInfo> GpInfoList { get; set; }

	public List<PurchaseInvoiceReverseActivityDetail_PoInfo> PoInfoList { get; set; }
}
