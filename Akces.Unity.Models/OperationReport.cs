using System;
using System.Collections.Generic;

namespace Akces.Unity.Models
{
    public class OperationReport 
    {
        public int Id { get; set; }
        public int HarmonogramPositionId { get; set; }
        public string Description { get; set; }
        public OperationType OperationType { get; set; }
        public DateTime Created { get; set; }
        public int PositionsCount { get; set; }
        public int InfosCount { get; set; }
        public int WarningsCount { get; set; }
        public int ErrorsCount { get; set; }
        public virtual List<OperationReportPosition> Positions { get; set; }

        public OperationReport()
        {
            Positions = new List<OperationReportPosition>();
        }
    }
}
