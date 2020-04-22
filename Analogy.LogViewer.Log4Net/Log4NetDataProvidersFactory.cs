using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetDataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = new Guid("DBC17C75-5212-46E3-B98A-539E779000E3");
        public string Title { get; } = "Log4Net Data Providers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; }
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public Log4NetDataProvidersFactory()
        {
            //get some data provider
            List<IAnalogyDataProvider> dataProviders = new List<IAnalogyDataProvider>();
            dataProviders.Add(new OfflineDataProvider());
            dataProviders.Add(new OnlineDataProvider());
            DataProviders = dataProviders;
        }
    }
}
