using Serilog.Core;
using Serilog.Events;

namespace BlazorWithSqlInTheCloud2025.Logging
{
    public class CustomEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // Example: flag business-hours vs after-hours
            var hour = DateTimeOffset.Now.Hour;
            var isBusinessHours = hour >= 8 && hour <= 18;
            var prop = propertyFactory.CreateProperty("IsBusinessHours", isBusinessHours);
            logEvent.AddPropertyIfAbsent(prop);
        }
    }

}
