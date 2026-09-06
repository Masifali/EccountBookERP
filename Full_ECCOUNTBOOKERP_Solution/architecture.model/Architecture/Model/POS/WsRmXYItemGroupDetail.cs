namespace Architecture.Model.POS;

public class WsRmXYItemGroupDetail
{
	public double yItemQty { get; set; }

	public double xMinQty { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int WsRmXYItemGroupHeaderId { get; set; }

	public virtual string ItemName { get; set; }
}
