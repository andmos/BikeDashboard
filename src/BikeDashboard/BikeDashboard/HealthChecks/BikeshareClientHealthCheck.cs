using System;
using System.Threading;
using System.Threading.Tasks;
using BikeshareClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BikeDashboard.HealthChecks
{
    public class BikeshareClientHealthCheck : IHealthCheck 
    {
        private readonly IBikeshareClient _bikeShareClient; 

        public BikeshareClientHealthCheck(IBikeshareClient bikeShareClient)
        {
            _bikeShareClient = bikeShareClient; 
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var systemInformation = await _bikeShareClient.GetSystemInformationAsync();
                if (string.IsNullOrEmpty(systemInformation.Id)) 
                {
                    return HealthCheckResult.Unhealthy("BikeShare client Id is empty.");
                }
                return HealthCheckResult.Healthy($"{nameof(IBikeshareClient)}, {systemInformation}");
            }
            catch (Exception ex) 
            {
                return HealthCheckResult.Unhealthy(nameof(IBikeshareClient), ex);
            }
        }
    }
}
