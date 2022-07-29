using System;

namespace Akces.Unity.Models
{
    public class WorkerStatus 
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public bool Enabled { get; set; }
    }
}
