using System;
using System.Collections.Generic;

namespace Akces.Unity.Models
{
    public class TaskReport 
    {
        public int Id { get; set; }
        public int? HarmonogramPositionId { get; set; }
        public string Description { get; set; }
        public OperationType OperationType { get; set; }
        public DateTime Created { get; set; }
        public int PositionsCount { get; set; }
        public int InfosCount { get; set; }
        public int WarningsCount { get; set; }
        public int ErrorsCount { get; set; }
        public virtual List<TaskReportPosition> Positions { get; set; }

        public TaskReport()
        {
            Positions = new List<TaskReportPosition>();
        }
    }
}
