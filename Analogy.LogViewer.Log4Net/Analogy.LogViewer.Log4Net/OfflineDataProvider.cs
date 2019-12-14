using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Log4Net
{
    public class OfflineDataProvider : IAnalogyOfflineDataProvider
    {

        public Guid ID { get; } = new Guid("E1696270-97BE-489F-9440-453BEA1AB7B8");
        public string OptionalTitle { get; } = string.Empty;
        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "All supported log file types|*.log;*.json|Plain Analogy XML log file (*.log)|*.log|Analogy JSON file (*.json)|*.json";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.log", "*.json" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;

        public Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
            => GetSupportedFilesInternal(dirInfo, recursiveLoad);

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName) => Task.CompletedTask;

        public bool CanOpenFile(string fileName)

            => fileName.EndsWith(".log", StringComparison.InvariantCultureIgnoreCase) ||
               fileName.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase);


        public bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);

        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.log")
                .Concat(dirInfo.GetFiles("*.json"))
                .ToList();
            if (!recursive)
                return files;
            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //to nothing with information tat the messages was opened/read
        }

    }
}