namespace Architecture.Model.Export;

public class ThirdPartyInspectionLotSamplingDetail
{
	public double SampleWeight_Kg { get; set; }

	public double TotalWeight_Kg { get; set; }

	public int Id { get; set; }

	public int InvLabPreProductionExportLotInspectionHeaderId { get; set; }

	public int NoOfSample { get; set; }

	public int ThirdPartyTypeId { get; set; }

	public string Remarks { get; set; }

	public string SealNo { get; set; }

	public virtual string PartyTypeName { get; set; }
}
