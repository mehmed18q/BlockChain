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
		public string Data { get; set; }
		public int Nonce { get; set; } = 0;

		public Block(DateTime timeStamp, string? previousHash, string data)
		{
			Index = 0;
			TimeStamp = timeStamp;
			PreviousHash = previousHash;
			Data = data;
			Hash = CalculateHash();
		}

		public string CalculateHash()
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
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
