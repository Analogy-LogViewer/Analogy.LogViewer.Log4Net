using System;
using System.Collections.Generic;
using System.Text;

namespace Analogy.LogViewer.Log4Net
{
    public class UserSettings
    {
        public string FileOpenDialogFilters { get; set; }
        public string FileSaveDialogFilters { get; } = string.Empty;
        public List<string> SupportFormats { get; set; } 
        public RegExPattern RegExPattern { get; set; }

        public UserSettings()
        {
            FileOpenDialogFilters = "Plain log text file (*.log)|*.log";
            SupportFormats = new List<string> { "*.log" };
            RegExPattern = new RegExPattern();
        }
    }
}
