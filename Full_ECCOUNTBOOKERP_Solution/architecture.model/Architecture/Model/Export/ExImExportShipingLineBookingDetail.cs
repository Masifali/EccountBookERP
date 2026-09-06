namespace Architecture.Model.Export;

public class ExImExportShipingLineBookingDetail
{
	public int ExImExportShipingLineBookingId { get; set; }

	public int Id { get; set; }

	public int ContainerTypeId { get; set; }

	public string ContainerType { get; set; }

	public string Container { get; set; }

	public string ContainerSize { get; set; }

	public string SealNo { get; set; }
}
