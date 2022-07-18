using System;

namespace Akces.Unity.Models
{
    public abstract class AccountFunction 
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
        public int Id { get; set; }
        public string Name { get; set; }
        public string DefaultScript { get; set; }
    }
}
