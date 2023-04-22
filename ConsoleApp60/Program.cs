using KestrelTcp.EchoServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ConsoleApp60
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {   // This shows how a custom framework could plug in an experience without using Kestrel APIs directly
                    services.ConfigureMessageParser(new IPEndPoint(IPAddress.Loopback, 8009));
                })
                .UseKestrel(options =>
                {   // Setup a ConnectionHandler through UseKestrel
                    options.ListenLocalhost(8007, builder => { builder.UseConnectionHandler<EchoConnectionHandler>(); });
                })
                .UseStartup<Startup>();
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // There is no HTTP request pipeline, so who is going to use this? Ah, its a kestrel thing
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }

}
