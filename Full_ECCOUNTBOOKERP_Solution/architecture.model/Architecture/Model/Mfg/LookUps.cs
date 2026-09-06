namespace Architecture.Model.Mfg;

public class LookUps
{
	public int GLAccountId { get; set; }

	public int Id { get; set; }

	public int LookUpTypeId { get; set; }

	public string Code { get; set; }

	public string LookupName { get; set; }

	public virtual string AccountTitle { get; set; }
}
