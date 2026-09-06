using System.Collections.Generic;

namespace Architecture.Model.Export;

public class GdContainerBreakUp
{
	public decimal NetWeight { get; set; }

	public decimal NoOfBags { get; set; }

	public int ExImInvoiceId { get; set; }

	public int GdId { get; set; }

	public int Id { get; set; }

	public string ContainerNo { get; set; }

	public string ContainerSize { get; set; }

	public virtual List<GdContainerBreakUp> GdContainerBreakUpList { get; set; }
}
