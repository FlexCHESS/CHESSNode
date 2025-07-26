using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Azure.DigitalTwins.Core;
using System.Net.Sockets;
using System;
using Azure.Identity;

namespace IO.Swagger
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        public static DigitalTwinsClient dtClient;
        public static string subjectAttributes = "";

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            string _adtServiceUrl = System.Environment.GetEnvironmentVariable("adtServiceUrl", EnvironmentVariableTarget.Process);
            string _adtClientId = System.Environment.GetEnvironmentVariable("adtClientId", EnvironmentVariableTarget.Process);
            string _adtClientSecret = System.Environment.GetEnvironmentVariable("adtClientSecret", EnvironmentVariableTarget.Process);
            string _adtTenantId = System.Environment.GetEnvironmentVariable("adtTenantId", EnvironmentVariableTarget.Process);
            subjectAttributes = System.Environment.GetEnvironmentVariable("CONF", EnvironmentVariableTarget.Process);


            ClientSecretCredential credential = new ClientSecretCredential(_adtTenantId, _adtClientId, _adtClientSecret);

            dtClient = new DigitalTwinsClient(new Uri(_adtServiceUrl), credential);


            Console.WriteLine($"Service client created – ready to go");
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the web host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
