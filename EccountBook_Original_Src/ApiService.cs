using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class ApiService
{
	private static readonly HttpClient client = new HttpClient();

	private static string _token = "";

	public static string ApiBaseUrl { get; set; } = "http://5.161.113.248:825/";


	public static void SetToken(string token)
	{
		_token = token;
		client.DefaultRequestHeaders.Clear();
		client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
	}

	public static async Task<T> GetAsync<T>(string endpoint)
	{
		string url = ApiBaseUrl + endpoint;
		HttpResponseMessage response = await client.GetAsync(url);
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("API GET failed: " + response.ReasonPhrase);
		}
		return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
	}

	public static async Task<T> PostAsync<T>(string endpoint, object data)
	{
		string url = ApiBaseUrl + endpoint;
		StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
		HttpResponseMessage response = await client.PostAsync(url, content);
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("API POST failed: " + response.ReasonPhrase);
		}
		return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
	}
}
