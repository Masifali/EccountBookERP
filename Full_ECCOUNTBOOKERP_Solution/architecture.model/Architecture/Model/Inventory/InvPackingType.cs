namespace Architecture.Model.Inventory;

public class InvPackingType
{
	public int Id { get; set; }

	public string PackTypeCode { get; set; }

	public string PackTypeDesc { get; set; }

	public bool IsActive { get; set; }

	public double MinEbWeight { get; set; }

	public double MaxEbWeight { get; set; }

	public double MinEbStockWeight { get; set; }

	public double MaxEbStockWeight { get; set; }
}
