namespace Architecture.Model.Export;

public class ExImLcOrderDocumentList
{
	public int Copies { get; set; }

	public int DocumentId { get; set; }

	public int Id { get; set; }

	public int LcOrderId { get; set; }

	public int Original { get; set; }

	public virtual string exImDocName { get; set; }
}
