using System;

namespace Akces.Unity.Models
{
    public class HarmonogramPosition 
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool Repeat { get; set; }
        public int RepeatAfterMinutes { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? LastLaunchTime { get; set; }
        public OperationType HarmonogramOperation { get; set; }
        public virtual Account Account { get; set; }

        public bool ShouldRun() 
        {
            if (Active)
                return false;

            if (LastLaunchTime == null)
                return true;

            if (Repeat && LastLaunchTime.HasValue && LastLaunchTime.Value.AddMinutes(RepeatAfterMinutes) <= DateTime.Now)
                return true;

            return false;
        }
    }

    public enum OperationType
    {
        ImportZamowien,
        PobranieStatusowZamowien,
        WyslanieCen
    }
}
