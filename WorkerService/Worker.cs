using KestrelTcp.EchoServer;
using Microsoft.AspNetCore.Connections;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            IWebHostBuilder builder = new WebHostBuilder()
            .UseKestrel(options =>
            {
                options.ListenLocalhost(8007, builder =>
                {
                    builder.UseConnectionHandler<EchoConnectionHandler>();
                });
            })
            .UseStartup<MyStartup>();
            var it = builder.Build();
            await it.RunAsync();

        }
    }
}
