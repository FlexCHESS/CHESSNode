
// Main program entry point for the CHESS adapter
// tim.farnham@toshiba-bril.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace smaBESSadapter
{
    public class Program
    {

        public static String urlprefix = "https://preprodapim.umbrellaiot.com:9095/aas/3.0";
        public static void Main(string[] args)
        {

            String prefix = System.Environment.GetEnvironmentVariable("AAS_URL");
            if (prefix != null) urlprefix = prefix;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
