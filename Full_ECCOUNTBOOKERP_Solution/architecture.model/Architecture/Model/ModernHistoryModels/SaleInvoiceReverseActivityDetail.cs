using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class SaleInvoiceReverseActivityDetail
{
	public List<SaleInvoiceReverseActivityDetail_InvoiceInfo> InvoiceInfo { get; set; }

	public List<SaleInvoiceReverseActivityDetail_GdnInfo> GdnInfoList { get; set; }

	public List<SaleInvoiceReverseActivityDetail_WagesInfo> WagesInfoList { get; set; }

	public List<SaleInvoiceReverseActivityDetail_WbInfo> WbInfoList { get; set; }

	public List<SaleInvoiceReverseActivityDetail_GpInfo> GpInfoList { get; set; }

	public List<SaleInvoiceReverseActivityDetail_DoInfo> DoInfoList { get; set; }

	public List<SaleInvoiceReverseActivityDetail_SoInfo> SoInfoList { get; set; }
}
