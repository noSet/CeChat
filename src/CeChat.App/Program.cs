using CeChat.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddTransient<CeChatApp>();
            services.AddTransient<ICeChatRoomService>(sp =>
            {
                // todo 创建远程对象
                return null;
            });

            return services.BuildServiceProvider();
        }
    }
}
