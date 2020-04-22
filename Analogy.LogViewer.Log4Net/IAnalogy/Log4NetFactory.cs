using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetFactory : IAnalogyFactory
    {
        public Guid FactoryId { get; } = new Guid("DBC17C75-5212-46E3-B98A-539E779000E3");
        public string Title { get; } = "Log4Net Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Log4Net Parser for Analogy Log Viewer";

    }
}
