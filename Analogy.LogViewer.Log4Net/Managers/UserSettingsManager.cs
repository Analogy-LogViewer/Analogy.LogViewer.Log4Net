using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Analogy.LogViewer.Log4Net.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        private string SettingFile { get; } = "AnalogyLog4net.Settings";
        public UserSettings Settings { get; }

        public UserSettingsManager()
        {
            Settings = new UserSettings();
            if (File.Exists(SettingFile))
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                    };
                    string data = File.ReadAllText(SettingFile);
                    Settings = JsonConvert.DeserializeObject<UserSettings>(data, settings);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogCritical(ex, "", $"Unable to read file {SettingFile}: {ex}");
                }
            }
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(SettingFile, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogCritical("", $"Unable to save file {SettingFile}: {ex}");
            }
        }
    }
}