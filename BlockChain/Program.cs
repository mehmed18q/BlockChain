using BlockChainConsole.Entities;
using Newtonsoft.Json;

DateTime startTime = DateTime.Now;
BlockChain TopCoin = new();
TopCoin.AddBlock(new Block(DateTime.Now, null, new Data { Sender = "Hamed", Receiver = "Arash", Amount = 5 }.ToString()));
TopCoin.AddBlock(new Block(DateTime.Now, null, new Data { Sender = "Arash", Receiver = "v", Amount = 5 }.ToString()));
TopCoin.AddBlock(new Block(DateTime.Now, null, new Data { Sender = "Pedram", Receiver = "Iman", Amount = 15 }.ToString()));
DateTime endTime = DateTime.Now;

//TopCoin.Chain[3].Data = "sender:Pedram,receiver:Sepehr,amount:15";

Console.WriteLine(JsonConvert.SerializeObject(TopCoin, Formatting.Indented));
Console.WriteLine($"Duration: {endTime - startTime}");

//Console.WriteLine($"Is Chain Valid: {TopCoin.IsValid()}");
//Console.WriteLine("Update receiver to Sepehr");
//TopCoin.Chain[1].Data = new Data { Sender = "Hamed", Receiver = "Sepehr", Amount = 5 }.ToString();
//TopCoin.Chain[1].Hash = TopCoin.Chain[1].CalculateHash();
//Console.WriteLine($"Is Chain Valid: {TopCoin.IsValid()}");
//Console.WriteLine($"Update the entrie chain");
//TopCoin.Chain[2].PreviousHash = TopCoin.Chain[1].Hash;
//TopCoin.Chain[2].Hash = TopCoin.Chain[2].CalculateHash();
//TopCoin.Chain[3].PreviousHash = TopCoin.Chain[2].Hash;
//TopCoin.Chain[3].Hash = TopCoin.Chain[3].CalculateHash();
//Console.WriteLine($"Is Chain Valid: {TopCoin.IsValid()}");

Console.Read();

