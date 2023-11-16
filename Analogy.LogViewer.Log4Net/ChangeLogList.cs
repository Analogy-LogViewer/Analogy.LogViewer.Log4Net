using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogy.LogViewer.Log4Net
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial commit (template)", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 12, 08), "");
        }
    }
}