using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainConsole.Entities
{
	internal record Block
	{
		public int Index { get; set; }
		public DateTime TimeStamp { get; set; }
		public string? PreviousHash { get; set; }
		public string? Hash { get; set; }
		public IList<Transaction> Transactions { get; set; }
		public int Nonce { get; set; } = 0;

		public Block(DateTime timeStamp, string? previousHash, IList<Transaction> transactions)
		{
			Index = 0;
			TimeStamp = timeStamp;
			PreviousHash = previousHash;
			Transactions = transactions;
			Hash = CalculateHash();
		}

		public string CalculateHash()
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
			byte[] outputBytes = SHA256.HashData(inputBytes);
			return Convert.ToBase64String(outputBytes);
		}

		public void Mine(int difficulty)
		{
			string leadingZeroes = new('0', difficulty);
			while (Hash is null || Hash[..difficulty] != leadingZeroes)
			{
				Nonce++;
				Hash = CalculateHash();
			}
		}
	}
}
