using BlockChainConsole.Entities;
using Newtonsoft.Json;
using WebSocketSharp.NetCore;
using WebSocketSharp.NetCore.Server;

namespace BlockChainConsole.WebSocketBlockChain
{
	public class P2PServer : WebSocketBehavior
	{
		private bool chainSynched = false;
		private WebSocketServer? wss = null;

		public void Start()
		{
			wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
			wss.AddWebSocketService<P2PServer>("/Blockchain");
			wss.Start();
			Console.WriteLine($"Started Server at ws://127.0.0.1:{Program.Port}");
		}

		protected override void OnMessage(MessageEventArgs e)
		{
			if (e.Data.EndsWith(": Hi "))
			{
				Console.WriteLine($"+ {e.Data}{Program.Name}");
				Send($"{Program.Name}: Hi ");
			}
			else
			{
				BlockChain? newChain = JsonConvert.DeserializeObject<BlockChain>(e.Data);

				if (newChain is not null && newChain.IsValid() && newChain.Chain.Count > Program.TopCoin.Chain.Count)
				{
					List<Transaction> newTransactions = [.. newChain.PendingTransactions, .. Program.TopCoin.PendingTransactions];

					newChain.PendingTransactions = newTransactions;
					Program.TopCoin = newChain;
				}

				if (!chainSynched)
				{
					Send(JsonConvert.SerializeObject(Program.TopCoin));
					chainSynched = true;
				}
			}
		}
	}
}
