using System;
using System.Collections.Generic;

namespace Architecture.Model.FeedMill.Production;

public class BomHeader
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public bool IsDefault { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BomBatchQty { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int DocumentTypeId { get; set; }

	public int ProjectsId { get; set; }

	public string BomLevelType { get; set; }

	public string BomName { get; set; }

	public string BomType { get; set; }

	public string RemarksHeader { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual string PackUomEquivelent { get; set; }

	public virtual List<BomConsumptionDetail> BomConsumptionDetailList { get; set; }

	public virtual List<BomFohDetail> BomFohDetailList { get; set; }

	public virtual List<BomInputDetail> BomInputDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
