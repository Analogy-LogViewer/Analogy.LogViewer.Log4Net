using Analogy.Interfaces;
using Analogy.LogViewer.Log4Net.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.Log4Net
{
    public class OfflineDataProvider : Template.OfflineDataProvider
    {
        public override Guid Id { get; set; } = new Guid("E1696270-97BE-489F-9440-453BEA1AB7B8");
        public override string OptionalTitle { get; set; } = string.Empty;
        public override bool UseCustomColors { get; set; }
        public override bool CanSaveToLogFile { get; set; }
        public override string FileOpenDialogFilters => UserSettingsManager.UserSettings.Settings.FileOpenDialogFilters;
        public override string FileSaveDialogFilters { get; set; } = string.Empty;
        public override IEnumerable<string> SupportFormats => UserSettingsManager.UserSettings.Settings.SupportFormats;
        public override string InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? SmallImage { get; set; }
        public override Image? LargeImage { get; set; }


        public override bool DisableFilePoolingOption { get; set; }
        private RegexParser Parser { get; set; }
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {

            List<IAnalogyLogMessage> messages = await Parser.ParseLog(fileName, token, messagesHandler);
            return messages;
        }

        public override Task SaveAsync(List<IAnalogyLogMessage> messages, string fileName) => Task.CompletedTask;

        public override bool CanOpenFile(string fileName)
        {
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                if (PatternMatcher.StrictMatchPattern(pattern, fileName))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);

        protected override List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (string pattern in UserSettingsManager.UserSettings.Settings.SupportFormats)
            {
                files.AddRange(dirInfo.GetFiles(pattern).ToList());
            }

            if (!recursive)
            {
                return files;
            }

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

        public override Task InitializeDataProvider(ILogger logger)
        {
            Parser = new RegexParser(UserSettingsManager.UserSettings.Settings, true, logger);
            return base.InitializeDataProvider(logger);
        }
    }
}