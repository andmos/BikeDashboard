using System;
using System.Collections.Generic;

namespace TestBikedashboard.DTO
{

    public class HealthCheckDTO
    {
        public string Status { get; set; }
        public List<Check> Checks { get; set; }
    }

    public class Check
    {
        public string Service { get; set; }
        public string Status { get; set; }
    }
}
