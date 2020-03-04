using System;
using System.Collections.Generic;
using System.Text;

namespace DrawManager.Domain
{
    public class Constants
    {
        public const string APP_SETTINGS_JSON_FILE = "appsettings.json";
        public const string FORMAT_ENVIRONMENT_APP_SETTINGS_FILE = "appsettings.{0}.json";

        public const string CONNECTION_STRING_NAME_SQL_SERVER = "SqlDbConnection";
        public const string SQL_SERVER_ENTITY_FRAMEWORK_ASSEMBLY_NAME = "DrawManager.Database.SqlServer";
    }
}
