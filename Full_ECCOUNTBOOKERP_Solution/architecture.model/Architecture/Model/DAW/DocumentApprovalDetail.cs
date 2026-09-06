namespace Architecture.Model.DAW;

public class DocumentApprovalDetail
{
	public bool IsApproved { get; set; }

	public bool IsFinalApprover { get; set; }

	public bool IsMandatory { get; set; }

	public bool IsRejected { get; set; }

	public int ApprovalLevelId { get; set; }

	public int DocumentApprovalPolicyId { get; set; }

	public int Id { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int UserId { get; set; }

	public string UserComments { get; set; }
}
