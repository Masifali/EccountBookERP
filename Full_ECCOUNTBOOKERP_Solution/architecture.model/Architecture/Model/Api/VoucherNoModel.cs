using System;
using System.Collections.Generic;

namespace Architecture.Model.Api;

public class VoucherNoModel
{
	public int VoucherCode { get; set; }

	public DateTime VoucherDate { get; set; }

	public string VoucherType { get; set; } = string.Empty;


	public List<VoucherDetailModel> Data { get; set; } = new List<VoucherDetailModel>();

}
