using System;

namespace Architecture.Model.TaxProject.Export;

public class ExImInvoicePackingListDetailCustom
{
	public DateTime? ExpiryDate { get; set; }

	public DateTime? PackageDate { get; set; }

	public double GrossWeight { get; set; }

	public double NetWeight { get; set; }

	public double OuterQty { get; set; }

	public int ContainerId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int InspectionId { get; set; }

	public int ItemId { get; set; }

	public string CommodityDetail { get; set; }

	public string LotNo { get; set; }

	public string PmDetail { get; set; }

	public string Remarks { get; set; }

	public string SubLotNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string InspectionLotNo { get; set; }

	public virtual string ContainerNo { get; set; }
}
