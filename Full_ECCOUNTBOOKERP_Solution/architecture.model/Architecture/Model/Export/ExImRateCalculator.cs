namespace Architecture.Model.Export;

public class ExImRateCalculator
{
	public double BPAmount { get; set; }

	public double BpPrcnt { get; set; }

	public double BPWeight { get; set; }

	public double BrokeryComm { get; set; }

	public double BrokeryCommAmount { get; set; }

	public double ExportCost { get; set; }

	public double ExportRateUsd { get; set; }

	public double FgAmount { get; set; }

	public double FgWeight { get; set; }

	public double FreightInwardAmount { get; set; }

	public double FreightInwardRate { get; set; }

	public double MarketRate { get; set; }

	public double NetWeight { get; set; }

	public double PackingCost { get; set; }

	public double ProcessingAmount { get; set; }

	public double ProcessingRate { get; set; }

	public double RmAmount { get; set; }

	public double UsdExcngRate { get; set; }

	public double XFactoryRateRs { get; set; }

	public double BPRate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int Qty { get; set; }

	public string VarietyBrandName { get; set; }
}
