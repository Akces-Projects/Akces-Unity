using System;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.Models
{
    public class WorkerStatus 
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public bool Enabled { get; set; }
    }

    public class UnityUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public bool IsWorker { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public List<Authorisation> Authorisations { get; set; }
    }

    public class Authorisation 
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public AuthorisationType AuthorisationType { get; set; }
    }

    public enum AuthorisationType 
    {
        Deny, AllowRead, AllowReadWrite
    }

    public static class Modules
    {
        public const string Tasks = nameof(Tasks);
        public const string Harmonograms = nameof(Harmonograms);
        public const string Reports = nameof(Reports);
        public const string Prizes  = nameof(Prizes);
        public const string Accounts  = nameof(Accounts);
        public const string Users  = nameof(Users);
        public static List<string> List { get; }

        static Modules()
        {
            List = typeof(Modules).GetProperties()
                .Select(x => x.GetValue(x, null))
                .Where(x => x != null)  
                .Cast<string>()
                .ToList();
        }
    }
}
