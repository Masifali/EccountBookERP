using System;
using System.Collections.Generic;

namespace Architecture.Model.Import;

public class ImLcOrderSchedulePackingDetail
{
	public decimal NetWeight { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int ImItemId { get; set; }

	public int ImLcOrderId { get; set; }

	public int ImLcOrderScheduleId { get; set; }

	public int NoOfBags { get; set; }

	public int PackingTypeId { get; set; }

	public int ActionTypeId { get; set; }

	public int PackUomId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual DateTime AttentiveLoadingDate { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual List<ImLcOrderSchedulePackingDetail> ImLcOrderSchedulePackingDetailList { get; set; }
}
