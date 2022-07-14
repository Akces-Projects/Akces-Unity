using System;

namespace Akces.Unity.Models
{
    public abstract class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public AccountType AccountType { get; protected set; }
        public virtual NexoConfiguration NexoConfiguration { get; set; }

        public string MatchAssormentScript { get; set; } = "if (Product.Symbol == Assortment.Name) return true; else return false;";
        public string ConcludeProductSymbolScript { get; set; } = "var t = new string (Product.Symbol.Replace('a', 'b').Skip(3).ToArray()); return t + 24.ToString();";
        public string CalculateOrderPositionQuantityScript { get; set; } = "Math.Abs(-Product.Quantity + 15)";

        public Account()
        {
            NexoConfiguration = new NexoConfiguration();
        }        
    }

    public enum AccountType
    {
        Shoper, shopGold, Baselinker, Allegro, Olx
    }
}
