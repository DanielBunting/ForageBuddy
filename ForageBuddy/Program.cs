using System;
using System.IO;
using System.Windows.Forms;
using Serilog;

namespace ForageBuddy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logger = CreateLogger();
            var imageParser = CreateImageParser(logger);
            var forageCalculator = CreateForageCalculator(imageParser, logger);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(forageCalculator, logger));
        }
        
        private static ILogger CreateLogger()
        {
            var logFolder = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"));
            
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            
            return new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolder, $"ForageBuddy_{DateTime.Now:yyyy-MM-dd_hh-mm-ss}_log.txt"))
                .CreateLogger();
        }

        private static IImageParser CreateImageParser(ILogger logger) => new ImageParser(logger);

        private static IForageCalculator CreateForageCalculator(IImageParser imageParser, ILogger logger) => new ForageCalculator(imageParser, logger);
    }
}
