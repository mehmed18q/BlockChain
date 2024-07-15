using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealStateWebAPI.Services
{
    public interface IRealStateService
    {
        Task<decimal> GetBalance(string address);
        Task<string> AddAccount(string password);
        Task<Contract> GetContract(string name);
    }
}
