using BlockChainConsole.Entities;
using BlockChainConsole.WebSocketBlockChain;
using Newtonsoft.Json;

namespace BlockChainConsole
{
	internal class Program
	{
		public static int Port = 501;
		public static P2PServer? Server = null;
		public static P2PClient Client = new();
		public static BlockChain TopCoin = new();
		public static string Name = "Sadeq";

		private static void Main(string[] args)
		{
			TopCoin.InitializeChain();

			if (args.Length >= 1)
			{
				Port = int.Parse(args[0]);
			}

			if (args.Length >= 2)
			{
				Name = args[1];
			}

			if (Port > 0)
			{
				Server = new P2PServer();
				Server.Start();
			}

			bool _while = true;
			while (_while)
			{
				switch (Statics.ShowMenu())
				{
					case 1:
						Console.Write("Please Enter the Server URL: ");
						string? serverURL = Console.ReadLine();
						Client.Connect($"{serverURL}/Blockchain");
						break;
					case 2:
						Console.Write("Please Enter the Receiver Name: ");
						string? receiverName = Console.ReadLine();
						Console.Write("Please Enter the Amount: ");
						string? amount = Console.ReadLine();
						if (amount is not null && !string.IsNullOrEmpty(receiverName))
						{
							TopCoin.CreateTransaction(new Transaction(Name, receiverName, int.Parse(amount)));
							TopCoin.ProcessPendingTransaction(Name);
							Client.Broadcast(JsonConvert.SerializeObject(TopCoin));
						}
						break;
					case 3:
						Console.WriteLine("Blockchain:");
						Console.WriteLine(JsonConvert.SerializeObject(TopCoin, Formatting.Indented));
						break;
					case 4:
						Console.Write("Please Enter the User Name: ");
						Name = Console.ReadLine() ?? Name;
						break;
					default:
						_while = false;
						break;
				}

			}

			Client.Close();
		}
	}
}
