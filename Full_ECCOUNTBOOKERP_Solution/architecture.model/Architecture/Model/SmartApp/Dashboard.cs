namespace Architecture.Model.SmartApp;

public class Dashboard
{
	public virtual int SortingNo { get; set; }

	public string AccountDescription { get; set; }

	public decimal Opening { get; set; }

	public decimal CurrDr { get; set; }

	public decimal CurrCr { get; set; }

	public decimal DiffAmount { get; set; }

	public decimal Closing { get; set; }

	public int CompanyId { get; set; }

	public string Company { get; set; }

	public string DiffStatus { get; set; }
}
