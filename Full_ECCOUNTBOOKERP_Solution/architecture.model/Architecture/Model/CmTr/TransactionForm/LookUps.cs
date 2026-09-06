namespace Architecture.Model.CmTr.TransactionForm;

public class LookUps
{
	public int GLAccountId { get; set; }

	public int Id { get; set; }

	public int InvLookupTypeId { get; set; }

	public bool IsActive { get; set; }

	public string Code { get; set; }

	public string LookupName { get; set; }

	public string LookupNameOtherLingo { get; set; }

	public virtual string LookUpTypeName { get; set; }

	public virtual string AccountTitle { get; set; }
}
