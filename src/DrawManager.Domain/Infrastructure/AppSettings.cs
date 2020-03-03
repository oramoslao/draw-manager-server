using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Infrastructure
{
    public static class AppSettings
    {
        public static IConfigurationRoot GetConfiguration(string path, string environmentName = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(Constants.AppSettingsJsonFile, optional: true, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                var filePath = string.Format(Constants.FormatEnvironmentFileAppSettings, environmentName);
                builder = builder.AddJsonFile(filePath, optional: true, reloadOnChange: true);
            }

            builder = builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
