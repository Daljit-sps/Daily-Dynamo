using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyDynamo.Shared.Common;

namespace DailyDynamo.Shared.Utilities
{
    public static class AppSettingsUtility
    {
        public static AppSettingsModel Settings { get; set; }

        public static void SetSettings(AppSettingsModel appSettingsModel)
        {
            Settings = appSettingsModel;
        }
    }
}
