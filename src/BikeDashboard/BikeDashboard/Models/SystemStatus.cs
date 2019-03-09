using System;
namespace BikeDashboard.Models
{
    public class SystemStatus
    {
        public string SystemName { get; set; }
        public string SystemOperator { get; set; }
        public int Stations { get; set; }
        public int StationsWithAvailableBikes { get; set; }
        public int StationsWithAvailableLocks { get; set; }
    }
}
