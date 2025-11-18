using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetDataProvidersFactory : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = Log4NetFactory.Log4NetFactoryId;
        public override string Title { get; set; } = "Log4Net Data Providers";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider>
            {
                new OfflineDataProvider(),
            };
    }
}