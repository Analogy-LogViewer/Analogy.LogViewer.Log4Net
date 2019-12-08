using System;
using System.Collections.Generic;
using System.Text;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetDataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public string Title { get; } = "Log$Net Data Providers";
        public IEnumerable<IAnalogyDataProvider> Items { get; }

        public Log4NetDataProvidersFactory()
        {
            //get some data provider
            List<IAnalogyDataProvider> dataProviders = new List<IAnalogyDataProvider>();
            dataProviders.Add(new OfflineDataProvider());
            dataProviders.Add(new OnlineDataProvider());
        }
    }
}
