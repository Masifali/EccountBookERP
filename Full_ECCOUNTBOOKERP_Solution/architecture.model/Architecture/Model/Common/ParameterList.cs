namespace Architecture.Model.Common;

public class ParameterList
{
	public string ParameterName { get; set; }

	public object ParameterValue { get; set; }

	public ParameterList()
	{
	}

	public ParameterList(string ParameterName, object ParameterValue)
	{
		this.ParameterName = ParameterName;
		this.ParameterValue = ParameterValue;
	}
}
