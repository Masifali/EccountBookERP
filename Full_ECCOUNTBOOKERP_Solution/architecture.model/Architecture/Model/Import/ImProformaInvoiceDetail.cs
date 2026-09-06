namespace Architecture.Model.Import;

public class ImProformaInvoiceDetail
{
	public double FcAmount { get; set; }

	public double ItemRate { get; set; }

	public double QtyInner { get; set; }

	public double QtyOuter { get; set; }

	public int ExImPackMaterialTypeId { get; set; }

	public int ImProformaInvoiceHeaderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int NetWeightKgs { get; set; }

	public int UOMScheduleIdInner { get; set; }

	public int UOMScheduleIdOuter { get; set; }

	public int UOMScheduleIdRate { get; set; }

	public string Description { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ExImPackMaterialTypeName { get; set; }

	public virtual double UOMScheduleIdOuterName { get; set; }

	public virtual double UOMScheduleIdInnerName { get; set; }

	public virtual double UOMScheduleIdRateName { get; set; }

	public virtual string ItemName { get; set; }
}
