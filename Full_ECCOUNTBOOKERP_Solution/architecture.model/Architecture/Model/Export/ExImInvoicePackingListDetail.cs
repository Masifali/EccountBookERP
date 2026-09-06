using System;

namespace Architecture.Model.Export;

public class ExImInvoicePackingListDetail
{
	public DateTime? EntryDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime PackageDate { get; set; }

	public double GrossWeight { get; set; }

	public double InnerQty { get; set; }

	public double NetWeight { get; set; }

	public double OuterQty { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocId { get; set; }

	public int RefDocSubId { get; set; }

	public int ActionTypeId { get; set; }

	public int ContainerId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int InnerUomId { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OuterUomId { get; set; }

	public int ThirdPartyAnalysisId { get; set; }

	public int ThirdPartyAnalysisSubId { get; set; }

	public string CommodityDetail { get; set; }

	public string LotNo { get; set; }

	public string PmDetail { get; set; }

	public string Remarks { get; set; }

	public string SubLotNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ThirdPartyAnalysisNo { get; set; }

	public virtual string InnerUom { get; set; }

	public virtual string OuterUom { get; set; }

	public virtual double InnerUomEquivalent { get; set; }

	public virtual double OuterUomEquivalent { get; set; }

	public virtual string ContainerNo { get; set; }
}
