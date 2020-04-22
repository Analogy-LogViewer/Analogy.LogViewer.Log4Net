using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.Log4Net.Managers;
using Analogy.LogViewer.Log4Net.Properties;

namespace Analogy.LogViewer.Log4Net.IAnalogy
{
    public class Log4NetDataProviderSettingss : IAnalogyDataProviderSettings
    {
        public string Title { get; } = "Log4Net settings";
        public UserControl DataProviderSettings { get; } = new UserSetttingsUC();
        public Image SmallImage { get; } = Resources.log4net16x16;
        public Image LargeImage { get; } = Resources.log4net32x32;
        public Guid FactoryId { get; set; } = Log4NetFactory.Log4NetFactoryId;
        public Guid ID { get; set; } = new Guid("2D09F7E7-C55E-41B0-8068-A474D2361F85");

        public Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}
