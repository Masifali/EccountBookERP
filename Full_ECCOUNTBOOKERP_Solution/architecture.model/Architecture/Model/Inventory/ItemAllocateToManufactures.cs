namespace Architecture.Model.Inventory;

public class ItemAllocateToManufactures
{
	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ManufactureId { get; set; }

	public bool IsActive { get; set; }
}
