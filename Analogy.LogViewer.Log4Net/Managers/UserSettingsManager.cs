using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
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
                    string data = File.ReadAllText(SettingFile);
                    Settings = System.Text.Json.JsonSerializer.Deserialize<UserSettings>(data);
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
                File.WriteAllText(SettingFile, System.Text.Json.JsonSerializer.Serialize(Settings));
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogCritical("", $"Unable to save file {SettingFile}: {ex}");
            }
        }
    }
}