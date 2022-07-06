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
