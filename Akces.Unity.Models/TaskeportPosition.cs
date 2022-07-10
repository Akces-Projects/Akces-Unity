namespace Akces.Unity.Models
{
    public class TaskReportPosition 
    {
        public int Id { get; set; }
        public string ObjectName { get; set; }
        public string Description { get; set; }
        public ReportPositionType Type { get; set; }
    }
    public enum ReportPositionType
    {
        Info, Warn, Error
    }
}
