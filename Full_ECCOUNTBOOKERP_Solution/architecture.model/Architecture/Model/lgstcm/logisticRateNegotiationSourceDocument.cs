namespace Architecture.Model.lgstcm;

public class logisticRateNegotiationSourceDocument
{
	public int ActionTypeId { get; set; }

	public int BrokerAgentNameId { get; set; }

	public int logisticRateNegotiationHeaderId { get; set; }

	public int logisticRateNegotiationSourceDocumentId { get; set; }

	public int ServiceProviderId { get; set; }

	public int SourceDocumentId { get; set; }

	public int SourceDocumentTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string SourceDocumentType { get; set; }

	public virtual string SourceDocumentNo { get; set; }

	public virtual string BrokerAgentName { get; set; }

	public virtual string ServiceProvider { get; set; }

	public virtual int SourceDocumentNoOfContainers { get; set; }

	public virtual double SourceDocumentMTon { get; set; }
}
