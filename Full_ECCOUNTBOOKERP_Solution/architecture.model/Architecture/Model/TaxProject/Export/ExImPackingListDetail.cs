using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.Model.TaxProject.Export;

public class ExImPackingListDetail
{
	public int Id { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ExImInvoicePackingDetailId { get; set; }

	public int ItemId { get; set; }

	public int InvCropYearId { get; set; }

	public int JobLotid { get; set; }

	public int ExImPackMaterilaTypeId { get; set; }

	public double InnerQty { get; set; }

	public int UOMScheduleIdInner { get; set; }

	public double OuterQty { get; set; }

	public int UOMScheduleIdOuter { get; set; }

	public int ActionTypeId { get; set; }

	public double GrossWeight { get; set; }

	public double NetWeight { get; set; }

	public double PackingWeight { get; set; }

	public double PackingWeightTotal { get; set; }

	public string ContainerNo { get; set; }

	public string SealNo { get; set; }

	public int WarehouseId { get; set; }

	public int SortNo { get; set; }

	public int RateUomId { get; set; }

	public double RatePrice { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual double OuterUomEquivalent { get; set; }

	public virtual string OuterUomCode { get; set; }

	public virtual string PackingMaterial { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CropYear { get; set; }

	public virtual List<Architecture.Model.Export.ExImPackingListDetail> ExImPackingListDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
