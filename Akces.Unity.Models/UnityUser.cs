using System;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.Models
{
    public class UnityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
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
        public static string Tasks { get; } = nameof(Tasks);
        public static string Harmonograms { get; } = nameof(Harmonograms);
        public static string Reports { get; } = nameof(Reports);
        public static string Prizes { get; } = nameof(Prizes);
        public static string Accounts { get; } = nameof(Accounts);
        public static string Users { get; } = nameof(Users);
        public static List<string> List { get; }

        static Modules()
        {
            List = typeof(Modules).GetProperties().Select(x => x.GetValue(x, null)).Cast<string>().ToList();
        }
    }
}
