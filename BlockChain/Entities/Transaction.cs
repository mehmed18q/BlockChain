using Newtonsoft.Json;

namespace BlockChainConsole.Entities
{
	internal record Transaction
	{
		public string? FromAddress { get; set; }
		public string ToAddress { get; set; } = null!;
		public int Amount { get; set; }

		public Transaction(string? fromAddress, string toAddress, int amount)
		{
			FromAddress = fromAddress;
			ToAddress = toAddress;
			Amount = amount;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
