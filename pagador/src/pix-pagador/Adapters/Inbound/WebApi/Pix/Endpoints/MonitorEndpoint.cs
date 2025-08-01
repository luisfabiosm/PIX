﻿using System.Diagnostics;

namespace Adapters.Inbound.WebApi.Pix.Endpoints
{
    public static partial class MonitorEndpoint
    {

        public static void AddMonitorEndpoints(this WebApplication app)
        {
            var monitoringGroup = app.MapGroup("pix/api/monitoring")
                               .WithTags("Monitoramento")
                               .AllowAnonymous();


            monitoringGroup.MapGet("/health/detailed", async (
            IServiceProvider serviceProvider) =>
                    {
                        var checks = new Dictionary<string, object>
                        {
                            ["timestamp"] = DateTime.UtcNow,
                            ["version"] = "2.0.0",
                            ["environment"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                            ["machineName"] = Environment.MachineName,
                            ["processId"] = Environment.ProcessId
                        };

                        // Adicionar checks específicos se necessário
                        // var dbCheck = await CheckDatabaseConnection(serviceProvider);
                        // checks["database"] = dbCheck;

                        return Results.Ok(checks);
                    })
        .WithName("DetailedHealthCheck")
        .WithSummary("Health check detalhado da aplicação")
        .CacheOutput("NoCache");



        monitoringGroup.MapGet("/metrics", () =>
        {
            var metrics = new
            {
                memoryUsage = GC.GetTotalMemory(false) / 1024 / 1024, // MB
                gcCollections = new
                {
                    gen0 = GC.CollectionCount(0),
                    gen1 = GC.CollectionCount(1),
                    gen2 = GC.CollectionCount(2)
                },
                threadCount = System.Threading.ThreadPool.ThreadCount,
                uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()
            };

            return Results.Ok(metrics);
        })
        .WithName("GetMetrics")
        .WithSummary("Métricas básicas da aplicação")
        .CacheOutput("PixCache");
        }
    }
}
