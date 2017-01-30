using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNethereum1._3
{
    class Program
    {
        private static Contract contract;
        private static Web3 web3;

        static void Main(string[] args)
        {
             web3 = new Web3();

            var text = File.ReadAllLines("../../Files/info.txt");

            string abi = text[0];
            string bytecode = text[1];
            string contractAddress = text[4];
            string deployerAddress = text[2];
            string deployerPassword = text[3];
        

            var task = UnlockAccount(deployerAddress, deployerPassword);

            task.Wait();

            if (task.Result)
            {
                contract = web3.Eth.GetContract(abi, contractAddress);
                var chargers = contract.GetFunction("getChargers");
                var result = chargers.CallAsync<object>(); //error here , (if I call Send Transcation result is same)
                result.Wait();
            }
        }
        public static async Task<bool> UnlockAccount(string deployerAddress , string deployerPassword)
        {
            try
            {
                var unlockAccountResult = await web3.Personal.UnlockAccount.SendRequestAsync(
                deployerAddress, deployerPassword, new HexBigInteger(120));
                return unlockAccountResult;
            }
            catch (RpcResponseException ex)
            {
                //log
                return false;
            }

        }
    }
}
