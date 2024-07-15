using Newtonsoft.Json;
using System.Net;

string url = "https://www.etherchain.org/api/gasPriceOracle/";


if (WebRequest.Create(url) is HttpWebRequest webRequest)
{
	webRequest.ContentType = "application/json";
	webRequest.UserAgent = "Nothing";

	using Stream s = webRequest.GetResponse().GetResponseStream();
	if (s != null)
	{
		using StreamReader sr = new(s);
		string ethergaspriceAsJson = sr.ReadToEnd();
		EtherGasPrice? ethergasprice = JsonConvert.DeserializeObject<EtherGasPrice>(ethergaspriceAsJson);

		Console.WriteLine(JsonConvert.SerializeObject(ethergasprice, Formatting.Indented));
	}
}

Console.ReadLine();