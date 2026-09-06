namespace Architecture.Model.WeightBridge;

public class WeighbridgeScalesList
{
	public bool IsActive { get; set; }

	public double AcceptedPlusMins { get; set; }

	public double BaudRate { get; set; }

	public double WbsCapacityMax { get; set; }

	public double WbsCapacityMin { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DataBits { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int StopBits { get; set; }

	public string MacAddress { get; set; }

	public string ModelName { get; set; }

	public string Parity { get; set; }

	public string PortName { get; set; }

	public string ReadTimeOutWb { get; set; }

	public string WbsNameDescription { get; set; }

	public string WriteTimeOutWb { get; set; }
}
