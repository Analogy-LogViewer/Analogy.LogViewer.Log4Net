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
        public Guid FactoryId { get; } = Log4NetFactory.Log4NetFactoryId;
        public string Title { get; } = "Log4Net Data Providers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider>
            {
                new OfflineDataProvider()
            };
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
  
    }
}
