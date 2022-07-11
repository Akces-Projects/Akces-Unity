using System;

namespace Akces.Unity.Models
{
    public class HarmonogramPosition 
    {
        private int repeatAfterMinutes = 15;

        public int Id { get; set; }
        public bool Active { get; set; }
        public bool Repeat { get; set; }
        public int RepeatAfterMinutes { get => repeatAfterMinutes; set { if (value > 0) { repeatAfterMinutes = value; } else { repeatAfterMinutes = 1; } } }
        public string Description { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now.AddMinutes(15);
        public DateTime? LastLaunchTime { get; set; }
        public TaskType HarmonogramOperation { get; set; }
        public virtual Account Account { get; set; }

        public bool ShouldRun() 
        {
            if (!Active)
                return false;

            if (StartTime > DateTime.Now)
                return false;

            if (LastLaunchTime == null)
                return true;

            if (Repeat && LastLaunchTime.Value.AddMinutes(RepeatAfterMinutes) <= DateTime.Now)
                return true;

            return false;
        }
    }

    public enum TaskType
    {
        ImportZamowien,
        PobranieStatusowZamowien,
        WyslanieCen
    }
}
