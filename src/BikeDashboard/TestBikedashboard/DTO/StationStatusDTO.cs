﻿using System;
using System.Collections.Generic;
using BikeshareClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestBikedashboard.DTO
{
    public class StationStatusDTO
    {
        [JsonConstructor]
        public StationStatusDTO([JsonProperty("last_updated"), JsonConverter(typeof(UnixDateTimeConverter))]DateTime lastUpdated,
                                    [JsonProperty("ttl")] int timeToLive,
                                    [JsonProperty("data")]StationStatusData stationStatusData)
        {
            LastUpdated = lastUpdated;
            TimeToLive = timeToLive;
            StationsStatusData = stationStatusData;
        }


        public DateTime LastUpdated { get; }


        public int TimeToLive { get; }


        public StationStatusData StationsStatusData { get; }

    }
    public class StationStatusData
    {
        [JsonConstructor]
        public StationStatusData(IEnumerable<StationStatus> stations)
        {
            StationsStatus = stations;
        }

        public IEnumerable<StationStatus> StationsStatus { get; }
    }
}
