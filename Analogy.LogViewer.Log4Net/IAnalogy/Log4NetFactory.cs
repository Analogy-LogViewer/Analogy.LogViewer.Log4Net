using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetFactory : IAnalogyFactory
    {
        internal static Guid Log4NetFactoryId = new Guid("DBC17C75-5212-46E3-B98A-539E779000E3");
        public Guid FactoryId { get; set; } = Log4NetFactoryId;
        public string Title { get; set; } = "Log4Net Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Log4Net Parser for Analogy Log Viewer";

    }
}
