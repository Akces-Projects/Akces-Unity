using System;
using System.Collections.Generic;

namespace Akces.Unity.Models
{
    public class Harmonogram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool WorkerEnabled { get; set; }
        public string IsActive { get => Active ? "Aktywny" : "Nieaktywny"; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public virtual List<HarmonogramPosition> Positions { get; set; }

        public Harmonogram()
        {
            Positions = new List<HarmonogramPosition>();
        }
    }
}
