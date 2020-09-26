using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CeChat.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Serilog;

namespace CeChat.Silo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime(options =>
                {
                    options.SuppressStatusMessages = true;
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddSerilog(new LoggerConfiguration().WriteTo.Console().CreateLogger());
                })
                .UseOrleans(builder =>
                {
                    builder.ConfigureApplicationParts(options =>
                    {
                        options.AddApplicationPart(typeof(CeChatRoomGrain).Assembly).WithReferences();
                    });

                    builder.UseLocalhostClustering();
                    builder.AddSimpleMessageStreamProvider("SMS");
                    builder.AddMemoryGrainStorageAsDefault();
                    builder.AddMemoryGrainStorage("PubSubStore");
                });
    }
}
