namespace BlockChainConsole.Entities
{
	internal record BlockChain
	{
		public IList<Block> Chain { get; set; } = null!;
		public int Difficulty { get; set; } = 2;
		public int Reward { get; set; } = 1;

		public IList<Transaction> PendingTransactions { get; set; } = new List<Transaction>();

		public void CreateTransaction(Transaction transaction)
		{
			PendingTransactions.Add(transaction);
		}

		public void ProcessPendingTransaction(string minerAddress)
		{
			Block block = new(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
			AddBlock(block);
			PendingTransactions = new List<Transaction>();
			CreateTransaction(new Transaction(null, minerAddress, Reward));
		}

		public void InitializeChain()
		{
			Chain = new List<Block>();
			AddGenesisBlock();

		}
		public void AddGenesisBlock()
		{
			Chain.Add(CreateGenesisBlock());
		}

		public Block CreateGenesisBlock()
		{
			Block block = new(DateTime.Now, null, PendingTransactions);
			block.Mine(Difficulty);
			PendingTransactions = new List<Transaction>();
			return block;
		}

		public Block GetLatestBlock()
		{
			return Chain[Chain.Count - 1];
		}

		public void AddBlock(Block block)
		{
			Block latestBlock = GetLatestBlock();
			block.Index = latestBlock.Index + 1;
			block.PreviousHash = latestBlock.Hash;
			block.Mine(Difficulty);
			Chain.Add(block);
		}

		public bool IsValid()
		{
			for (int i = 1; i < Chain.Count; i++)
			{
				Block currentBlock = Chain[i];
				Block previousBlock = Chain[i - 1];

				if (currentBlock.Hash != currentBlock.CalculateHash())
				{
					return false;
				}

				if (currentBlock.PreviousHash != previousBlock.Hash)
				{
					return false;
				}
			}
			return true;
		}

		public int GetBalance(string address)
		{
			int balance = 0;
			foreach (Block item in Chain)
			{
				foreach (Transaction transaction in item.Transactions.Where(t => t.FromAddress == address || t.ToAddress == address))
				{
					int type = transaction.FromAddress == address ? -1 : 1;

					balance += type * transaction.Amount;
				}
			}

			return balance;
		}
	}
}
