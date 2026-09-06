using System;

namespace Architecture.Model.Accounts;

public class InvInLandFreightAgreementDetail
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal FregihtAmount { get; set; }

	public decimal FreightRate { get; set; }

	public decimal MinAmount { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int DestinationLocationId { get; set; }

	public int EnteryUserId { get; set; }

	public int FreightTypeId { get; set; }

	public int FreightUom { get; set; }

	public int Id { get; set; }

	public int InvInLandFreightAgreement { get; set; }

	public int LoadingDistrictId { get; set; }

	public int LoadingLocationId { get; set; }

	public int ModifyUserId { get; set; }

	public int PackSizeTo { get; set; }

	public string RemarksDetail { get; set; }

	public string SubAgreementCode { get; set; }

	public virtual string DistrictName { get; set; }

	public virtual string LoadingLocationName { get; set; }

	public virtual string UnLoadingLocationName { get; set; }

	public virtual string FreightType { get; set; }
}
