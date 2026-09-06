using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class InvManufacturingBOMHeader
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public bool IsDefault { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BomBatchQty { get; set; }

	public int ApprovedUserId { get; set; }

	public int BomItemId { get; set; }

	public int CropYearId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int DocNo { get; set; }

	public int ItemUomId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string BomName { get; set; }

	public string BomType { get; set; }

	public virtual List<InvManufacturingBOMMaterialDetail> InvManufacturingBOMMaterialsDetails { get; set; }

	public virtual List<InvManufacturingBOMOverHeadsDetail> InvManufacturingOverHeadDetails { get; set; }

	public virtual List<InvManufacturingBOMBiProductDetail> InvManufacturingBOMProductsDetails { get; set; }

	public virtual List<ManufacturingBOMPackingMaterialDetail> ManufacturingBOMPackingMaterialDetail { get; set; }
}
