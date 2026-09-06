namespace Architecture.Model.PartyProcessing;

public class StoreIssueReceiveHeaderDetailPartyProcessing
{
	public decimal QtyInReceived { get; set; }

	public decimal QtyOutIssued { get; set; }

	public decimal RequestedQty { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int SortNo { get; set; }

	public int StoreIssueReceiveHeaderPartyProcessingId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefEntryNoId { get; set; }

	public int RefEntryNoDetailId { get; set; }

	public string RemarksSub { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }
}
