using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabSampleLogRegister
{
	public bool IsAttachment { get; set; }

	public DateTime SampleDate { get; set; }

	public double Weight { get; set; }

	public int BranchId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int InvLabSampleAnalysisId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int ItemUomId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int Qty { get; set; }

	public int ReferencePartyId { get; set; }

	public int SampleNo { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string Crop { get; set; }

	public string LotDesc { get; set; }

	public string OtherRemarks { get; set; }

	public string SampleCookingPic { get; set; }

	public string SamplePic { get; set; }

	public int DocumentTypeId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }
}
