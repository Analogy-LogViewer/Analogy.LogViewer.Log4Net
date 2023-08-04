using Analogy.LogViewer.Log4Net.Managers;
using Analogy.LogViewer.Log4Net.Properties;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Interfaces;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.Log4Net.IAnalogy
{
    public class Log4NetDataProviderSettings : Template.TemplateUserSettingsFactory
    {
        public override string Title { get; set; } = "Log4Net settings";
        public override UserControl DataProviderSettings { get; set; }
        public override Image? SmallImage { get; set; } = Resources.log4net16x16;
        public override Image? LargeImage { get; set; } = Resources.log4net32x32;
        public override Guid FactoryId { get; set; } = Log4NetFactory.Log4NetFactoryId;
        public override Guid Id { get; set; } = new Guid("2D09F7E7-C55E-41B0-8068-A474D2361F85");

        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new UserSetttingsUC();
        }

        public override Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}
