using System;
using System.Collections.Generic;
using System.Text;

namespace Analogy.LogViewer.Log4Net
{
    [Serializable]
    public class RegExPattern
    {
        public string Pattern { get; set; }
        public string DateTimeFormat { get; set; }
        public bool IsSet => !string.IsNullOrEmpty(Pattern) && !string.IsNullOrEmpty(DateTimeFormat);
        public RegExPattern()
        {
            Pattern = string.Empty;
            DateTimeFormat = string.Empty;

        }
        public RegExPattern(string pattern, string dateTimeFormat)
        {
            Pattern = pattern;
            DateTimeFormat = dateTimeFormat;
        }
    }
}
