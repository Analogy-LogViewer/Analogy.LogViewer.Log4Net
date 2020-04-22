using System;
using System.Collections.Generic;
using System.Text;

namespace Analogy.LogViewer.Log4Net
{
    public class RegExPattern
    {
        public string Pattern { get; }
        public string DateTimeFormat { get; }

        public RegExPattern(string pattern, string dateTimeFormat)
        {
            Pattern = pattern;
            DateTimeFormat = dateTimeFormat;
        }
    }
}
