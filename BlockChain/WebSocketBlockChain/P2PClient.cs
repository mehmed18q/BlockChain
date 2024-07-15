using BlockChainConsole.Entities;
using Newtonsoft.Json;
using WebSocketSharp.NetCore;

namespace BlockChainConsole.WebSocketBlockChain
{
	public class P2PClient
	{
		private readonly IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

		public void Connect(string url)
		{
			if (!wsDict.ContainsKey(url))
			{
				WebSocket ws = new(url);
				ws.OnMessage += (sender, e) =>
				{
					if (e.Data.EndsWith(": Hi "))
					{
						Console.WriteLine($"+ {e.Data}{Program.Name}");
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
					}
				};
				ws.Connect();
				ws.Send($"{Program.Name}: Hi ");
				ws.Send(JsonConvert.SerializeObject(Program.TopCoin));
				wsDict.Add(url, ws);
			}
		}

		public void Send(string url, string data)
		{
			foreach (KeyValuePair<string, WebSocket> item in wsDict)
			{
				if (item.Key == url)
				{
					item.Value.Send(data);
				}
			}
		}

		public void Broadcast(string data)
		{
			foreach (KeyValuePair<string, WebSocket> item in wsDict)
			{
				item.Value.Send(data);
			}
		}

		public IList<string> GetServers()
		{
			IList<string> servers = new List<string>();
			foreach (KeyValuePair<string, WebSocket> item in wsDict)
			{
				servers.Add(item.Key);
			}
			return servers;
		}

		public void Close()
		{
			foreach (KeyValuePair<string, WebSocket> item in wsDict)
			{
				item.Value.Close();
			}
		}
	}
}
