using System.IO;
using Microsoft.Extensions.Configuration;
// Install NuGet packages:
// Microsoft.Extensions.Configuration;
// Microsoft.Extensions.Configuration.Json;
// Microsoft.Extensions.Configuration.FileExtensions;
namespace Config
{
    public static class AppConfig
    {
        private static IConfigurationRoot _configuration;

        static AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetAdminEmail() => _configuration["DefaultAdmin:Email"];

        public static string GetAdminPassword() => _configuration["DefaultAdmin:Password"];

        public static string GetConnectionString() => _configuration.GetConnectionString("HotelDb");
    }
}

