using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Log4Net
{
    public class OnlineDataProvider : IAnalogyRealTimeDataProvider
    {

        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }=new OfflineDataProvider();
        public bool IsConnected { get; }
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;

        public Guid ID { get; } = new Guid("BC02F81A-F102-4894-84B1-9BDEE1251A8C");
        public string OptionalTitle { get; } = string.Empty;

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //to nothing with information tat the messages was opened/read
        }

        public Task<bool> CanStartReceiving()
        {
            return Task.FromResult(true);
        }

        public void StartReceiving()
        {
            //start sending messages to analogy via OnMessageReady or OnManyMessagesReady events
        }

        public void StopReceiving()
        {
           //stop operation
        }

    }
}
