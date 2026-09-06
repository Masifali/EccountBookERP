using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Architecture.PSDSMS;

public sealed class ClientUtil<T> where T : class
{
	private static ClientUtil<T> clientUtilInstance;

	private string url = string.Empty;

	private string apikey = string.Empty;

	private HttpClient client;

	public static ClientUtil<T> ClientUtilInstance
	{
		get
		{
			if (clientUtilInstance == null)
			{
				clientUtilInstance = new ClientUtil<T>();
			}
			return clientUtilInstance;
		}
	}

	private ClientUtil()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		url = "";
		client = new HttpClient();
		client.BaseAddress = new Uri(url.ToString());
		client.DefaultRequestHeaders.Accept.Clear();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public object PostAsJsonData(string postUrl, T data)
	{
		try
		{
			HttpResponseMessage result = client.PostAsJsonAsync(postUrl, data).Result;
			result.EnsureSuccessStatusCode();
			return result.Content.ReadAsStringAsync().Result;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
