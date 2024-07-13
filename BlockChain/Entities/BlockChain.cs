namespace BlockChainConsole.Entities
{
	internal record BlockChain
	{
		public IList<Block> Chain { get; set; } = null!;
		public int Difficulty { get; set; } = 3;

		public BlockChain()
		{
			InitializeChain();
			AddGenesisBlock();
		}

		public void InitializeChain()
		{
			Chain = new List<Block>();

		}
		public void AddGenesisBlock()
		{
			Chain.Add(CreateGenesisBlock());
		}

		public static Block CreateGenesisBlock()
		{
			return new Block(DateTime.Now, null, "{}");
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
	}
}
