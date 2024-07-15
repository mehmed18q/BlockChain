using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateWebAPI.Services;

namespace RealStateWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealStateController : ControllerBase
    {
        private IRealStateService service;

        public RealStateController(IRealStateService realStateService)
        {
            service = realStateService;
        }

        [HttpGet]
        [Route("getBalance/{accountAddress}")]
        public async Task<decimal> GetBalance([FromRoute] string accountAddress)
        {
            return await service.GetBalance(accountAddress);
        }

        [HttpGet]
        [Route("AddAccount/{password}")]
        public async Task<string> AddAccount([FromRoute] string password)
        {
            return await service.AddAccount(password);
        }

        [HttpGet]
        [Route("RunContract/{name}/{contractMethod}/{value}")]
        public async Task<string> RunContract([FromRoute] string name,[FromRoute] string ContractMethod, [FromRoute] int Value)
        {
            var contract = await service.GetContract(name);
            var Method = contract.GetFunction(ContractMethod);
            try
            {
                var result = await Method.CallAsync<int>(Value);
                return result.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
        }


    }
}