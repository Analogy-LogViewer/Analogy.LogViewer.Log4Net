using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetDataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; set; } = Log4NetFactory.Log4NetFactoryId;
        public string Title { get; set; } = "Log4Net Data Providers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider>
            {
                new OfflineDataProvider()
            };
    }
}
