using System;
using System.Collections.Generic;
using System.Text;

namespace Analogy.LogViewer.Log4Net
{
    [Serializable]
    public class RegExPattern
    {
        public string Pattern { get; }
        public string DateTimeFormat { get; }
        public bool IsSet { get; set; }
        public RegExPattern()
        {
            Pattern = string.Empty;
            DateTimeFormat = string.Empty;
            IsSet = false;

        }
        public RegExPattern(string pattern, string dateTimeFormat)
        {
            Pattern = pattern;
            DateTimeFormat = dateTimeFormat;
            IsSet = true;
        }
    }
}
