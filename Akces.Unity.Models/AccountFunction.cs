using System;
using System.Collections.Generic;

namespace Akces.Unity.Models
{
    public class AccountFunction 
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
        public Account Account { get; set; }
        public AccountFunctionType AccountFunctionType { get; set; }
    }

    public class AccountFunctionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DefaultScript { get; set; }

        static AccountFunctionType()
        {
            MatchAssormentFunction = new AccountFunctionType()
            {
                Id = new Guid("a35ce9cd-31dc-4412-b7dc-b8a1b27701cc"),
                Name = "Funkcja dopasowania asortymentu do produktu",
                DefaultScript = "return Assortment.Symbol == Product.Symbol;"
            };

            ConcludeProductSymbolFunction = new AccountFunctionType()
            {
                Id = new Guid("844a822a-3f61-4835-892a-0c3133a1ac52"),
                Name = "Funkcja określająca symbol produktu",
                DefaultScript = "return Product.Symbol;"
            };

            CalculateOrderPositionQuantityFunction = new AccountFunctionType()
            {
                Id = new Guid("0379f3b4-762d-453e-afc0-317cd353919a"),
                Name = "Funkcja wyliczająca ilość na pozycji",
                DefaultScript = "return Product.Quantity;"
            };

            AccountFunctionTypes = new List<AccountFunctionType>() 
            {
                ConcludeProductSymbolFunction,
                CalculateOrderPositionQuantityFunction
            };
        }

        public static AccountFunctionType MatchAssormentFunction { get; private set; }
        public static AccountFunctionType ConcludeProductSymbolFunction { get; private set; }
        public static AccountFunctionType CalculateOrderPositionQuantityFunction { get; private set; }
        public static List<AccountFunctionType> AccountFunctionTypes { get; private set; }
    }
}
