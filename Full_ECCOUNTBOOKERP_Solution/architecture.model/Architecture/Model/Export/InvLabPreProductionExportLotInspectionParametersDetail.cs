namespace Architecture.Model.Export;

public class InvLabPreProductionExportLotInspectionParametersDetail
{
	public int Id { get; set; }

	public int InvLabPreProductionExportLotInspectionHeaderId { get; set; }

	public int ParameterId { get; set; }

	public int RequiredAnalysisGroupId { get; set; }

	public string Remarks { get; set; }

	public virtual string ParameterName { get; set; }

	public virtual string RequiredAnalysisGroup { get; set; }
}
