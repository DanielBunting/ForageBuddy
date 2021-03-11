using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Windows.Forms;

namespace ForageBuddy
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));
                   services.AddScoped<MainForm>();
                   services.AddScoped<IForageCalculator, ForageCalculator>();
                   services.AddScoped<IImageParser, ImageParser>();
               });

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                Application.Run(services.GetRequiredService<MainForm>());
            }
        }
    }
}
