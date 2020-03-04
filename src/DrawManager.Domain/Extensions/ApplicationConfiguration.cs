using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain.Extensions
{
    public static class ApplicationConfiguration
    {
        public static IConfigurationRoot GetConfiguration(string path, string environmentName = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(Constants.APP_SETTINGS_JSON_FILE, optional: true, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                var filePath = string.Format(Constants.FORMAT_ENVIRONMENT_APP_SETTINGS_FILE, environmentName);
                builder = builder.AddJsonFile(filePath, optional: true, reloadOnChange: true);
            }

            builder = builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
