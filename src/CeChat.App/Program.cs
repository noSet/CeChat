using CeChat.Grains.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeChat.App
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            IServiceProvider serviceProvider = ConfigureServices(new ServiceCollection());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(serviceProvider.GetRequiredService<CeChatApp>());
        }

        public static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddSerilog(new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger());
            });

            services.AddTransient<CeChatApp>();
            services.AddTransient<IClusterClient>(sp =>
            {
                var looger = sp.GetRequiredService<ILogger<ClientBuilder>>();

                var client = new ClientBuilder()
                    .ConfigureApplicationParts(manager => manager.AddApplicationPart(typeof(ICeChatRoomGrain).Assembly).WithReferences())
                    .UseLocalhostClustering()
                    .AddSimpleMessageStreamProvider("SMS")
                    .Build();

                client.Connect(async error =>
                {
                    looger.LogError(error, error.Message);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return true;
                });

                return client;
            });

            return services.BuildServiceProvider();
        }
    }
}
