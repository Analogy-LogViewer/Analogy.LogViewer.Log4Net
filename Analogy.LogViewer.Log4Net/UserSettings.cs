﻿using System.Collections.Generic;

namespace Analogy.LogViewer.Log4Net
{
    public class UserSettings
    {
        public string FileOpenDialogFilters { get; set; }
        public string FileSaveDialogFilters { get; } = string.Empty;
        public List<string> SupportFormats { get; set; }
        public string LogsLocation { get; set; }
        public List<RegexPattern> RegexPatterns { get; set; }

        public UserSettings()
        {
            LogsLocation = string.Empty;
            FileOpenDialogFilters = "Plain log text file (*.1)|*.1";
            SupportFormats = new List<string> { "*.1" };
            RegexPatterns = new List<RegexPattern>();
            RegexPatterns.Add(new RegexPattern(@"\$(?<Date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})+\|+(?<Thread>\d+)+\|(?<Level>\w+)+\|+(?<Source>.*)\|(?<Text>.*)", "yyyy-MM-dd HH:mm:ss,fff", ""));
        }
    }
}