using System;
using System.Collections.Generic;
using System.Text;

namespace Akces.Unity.Models
{
    public class UnityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Admin { get; set; }
        public bool CanOpenTasks { get; set; }
        public bool CanOpenHarmonograms { get; set; }
        public bool CanOpenPricesUpdate { get; set; }
    }
}
