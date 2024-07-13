using Newtonsoft.Json;

internal record Data
{
	public string Sender { get; set; } = null!;
	public string Receiver { get; set; } = null!;
	public Double Amount { get; set; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}