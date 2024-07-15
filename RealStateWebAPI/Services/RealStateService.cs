
using Nethereum.Hex.HexTypes;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace RealStateWebAPI.Services
{
	public class RealStateService : IRealStateService
	{
		private Nethereum.Web3.Web3 _web3 = new("HTTP://127.0.0.1:7545");
		private static string _accountAddress;
		private static string _accountPassword;
		private string _contractAddress = "0x2F627A1AA95c4A576AcA25D1d8b50e058664809c";
		private string _contractABI = @"[{""constant"": false,""inputs"": [{""name"": ""ecCode"",""type"": ""uint256""}],""name"": ""accept"",""outputs"": [{""name"": """",""type"": ""bool""}],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""function""},{""constant"": false,""inputs"": [{""name"": ""ecCode"",""type"": ""uint256""},{""name"": ""buyer"",""type"": ""address""},{""name"": ""seller"",""type"": ""address""},{""name"": ""amount"",""type"": ""uint256""}],""name"": ""addEstateContract"",""outputs"": [{""name"": """",""type"": ""bool""}],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""function""},{""constant"": false,""inputs"": [{""name"": ""sender"",""type"": ""address""},{""name"": ""receiver"",""type"": ""address""},{""name"": ""transferAmount"",""type"": ""uint256""}],""name"": ""transferFrom"",""outputs"": [{""name"": """",""type"": ""bool""}],""payable"": false,""stateMutability"": ""nonpayable"",""type"": ""function""},{""inputs"": [],""payable"": true,""stateMutability"": ""payable"",""type"": ""constructor""},{""constant"": true,""inputs"": [{""name"": ""accountToCheck"",""type"": ""address""}],""name"": ""customerExist"",""outputs"": [{""name"": """",""type"": ""bool""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [{""name"": """",""type"": ""uint256""}],""name"": ""EsContractStrsCodes"",""outputs"": [{""name"": """",""type"": ""uint256""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [{""name"": ""tokenOwner"",""type"": ""address""}],""name"": ""getBalance"",""outputs"": [{""name"": """",""type"": ""uint256""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""},{""constant"": true,""inputs"": [{""name"": ""ecCode"",""type"": ""uint256""}],""name"": ""getOwner"",""outputs"": [{""name"": ""owner"",""type"": ""address""}],""payable"": false,""stateMutability"": ""view"",""type"": ""function""}]";
		private string _contractName = "RealEstateToken";


		public async Task<bool> Accept(uint ecCode)
		{
			Contract contract = await GetContract(_contractName);
			var method = contract.GetFunction("accept");

			var transactionHash = await method.SendTransactionAsync(_accountAddress, new HexBigInteger(900000), null, ecCode);
			var receipt = await GetReceiptAsync(transactionHash);
			// var result = await method.CallDeserializingToObjectAsync<MyRealEstate.Services.Services.OutputDocument>(ecCode);
			return true;
		}

		public async Task<string> Transfer(string recipient, BigInteger transferwei)
		{

			bool result = await _web3.Personal.UnlockAccount.SendRequestAsync(_accountAddress, _accountPassword, 60);
			if (result)
			{
				Account ac1 = new("bdcb760866de79f2071c58bc82037346a3daecfe6f74aea6ed800a6e66246a9a");
				string ac2 = recipient;

				TransactionReceipt receipt = SendEther(ac1, ac2, _web3, transferwei).Result;
				return receipt.TransactionHash;
			}
			return null;
		}

		private static async Task<TransactionReceipt> SendEther(Account account, string recipient, Web3 web3, BigInteger transferwei)
		{
			var transactionPolling = web3.TransactionManager.TransactionReceiptService;
			HexBigInteger nonce = web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.Address).Result;

			return await transactionPolling.SendRequestAndWaitForReceiptAsync(() =>
			{
				TransactionInput transactionInput = new()
				{
					From = account.Address,
					//Gas = new HexBigInteger(25000),
					GasPrice = new HexBigInteger(10 ^ 10),
					To = recipient,
					Value = new HexBigInteger(transferwei),
					Nonce = nonce

				};
				var txSigned = new Nethereum.Signer.TransactionSigner();
				var signedTx = txSigned.SignTransaction(account.PrivateKey, transactionInput.To, transactionInput.Value, transactionInput.Nonce);
				Nethereum.RPC.Eth.Transactions.EthSendRawTransaction transaction = new(web3.Client);
				return transaction.SendRequestAsync(signedTx);
			});
		}

		public async Task<TransactionReceipt> GetReceiptAsync(string transactionHash)
		{
			TransactionReceipt? receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
			while (receipt == null)
			{
				Thread.Sleep(1000);
				receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
			}
			return receipt;
		}

		public async Task<string> AddAccount(string password)
		{
			string account = await _web3.Personal.NewAccount.SendRequestAsync(password);
			return account.ToString();
		}

		public async Task<bool> AddEstateContract(uint ecCode, string addressBuyer, string addressSeller, uint amount)
		{
			Contract contract = await GetContract(_contractName);
			var method = contract.GetFunction("addEstateContract");


			var estimate = await method.EstimateGasAsync(ecCode, addressBuyer, addressSeller, amount);
			var estimateGas = estimate.Value;

			// Transfer
			var trans = await Transfer(_accountAddress, Web3.Convert.ToWei(new HexBigInteger(2) * estimateGas, UnitConversion.EthUnit.Gwei));
			//


			var transactionHash = await method.SendTransactionAsync(_accountAddress, new HexBigInteger(3000000),
				new HexBigInteger(0), ecCode, addressBuyer, addressSeller, amount);

			BigInteger gasUsed = GetReceiptAsync(transactionHash).Result.GasUsed.Value;


			var result = await method.CallDeserializingToObjectAsync<MyRealEstate.Services.Services.OutputDocument>(_accountAddress, new HexBigInteger(3000000),
				new HexBigInteger(0), ecCode, addressBuyer, addressSeller, amount);

			return result.Result;



		}

		public async Task<int> GetBalance(string address)
		{
			Contract contract = await GetContract(_contractName);
			var method = contract.GetFunction("getBalance");

			var result = await method.CallAsync<int>(address);
			return result;
		}

		public async Task<Contract> GetContract(string name)
		{
			bool result = await _web3.Personal.UnlockAccount.SendRequestAsync(_accountAddress, _accountPassword, 60);
			return result ? _web3.Eth.GetContract(_contractABI, _contractAddress) : (Contract?)null;
		}

		public async Task<string> GetOwner(uint ecCode)
		{
			Contract contract = await GetContract(_contractName);
			var method = contract.GetFunction("getOwner");

			var result = await method.CallAsync<string>(ecCode);
			return result;
		}

		public void SetAccount(string accountAddress, string accountPassword)
		{
			_accountAddress = accountAddress;
			_accountPassword = accountPassword;
		}
	}
}
