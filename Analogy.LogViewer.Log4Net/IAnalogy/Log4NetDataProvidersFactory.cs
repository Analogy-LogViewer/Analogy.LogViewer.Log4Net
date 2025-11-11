using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetDataProvidersFactory : IAnalogyDataProvidersFactoryWinForms
    {
        public Guid FactoryId { get; set; } = Log4NetFactory.Log4NetFactoryId;
        public string Title { get; set; } = "Log4Net Data Providers";
        IEnumerable<IAnalogyDataProvider> IAnalogyDataProvidersFactory.DataProviders => DataProviders;

        public IEnumerable<IAnalogyDataProviderWinForms> DataProviders { get; } = new List<IAnalogyDataProviderWinForms>
            {
                new OfflineDataProvider(),
            };
    }
}