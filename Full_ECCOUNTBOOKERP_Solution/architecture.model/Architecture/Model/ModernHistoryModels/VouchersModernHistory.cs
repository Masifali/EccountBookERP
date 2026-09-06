using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class VouchersModernHistory
{
	public bool IsChecked { get; set; }

	public bool IsSelected { get; set; }

	public VouchersHistory_Header VoucherHistoryHeader { get; set; }

	public List<VoucherHistory_Detail> VoucherHistoryDetail { get; set; }
}
